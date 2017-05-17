using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    // Абстрактный декоратор формулы
    public abstract class FormulaDecorator : Formula
    {
        protected Formula formula;

        public void SetFormula(Formula f)
        {
            if (f == null)
                throw new ArgumentNullException();
            formula = f;
        }

        public FormulaDecorator()
        {
            formula = new Empty();
        }

        public override string Calculate(Dictionary<string, string> variables = null)
        {
            return formula.Calculate();
        }
    }

    // Декоратор формулы с возможностью замены подстрок
    public abstract class FormulaReplaceDecorator : FormulaDecorator
    {
        public virtual void Replace(string source, string dest, Mode sMode, Mode dMode)
        {
            if (formula is Empty)
                return;

            if (formula is Var)
            {
                if (sMode == Mode.Var && (formula as Var).Value == source)
                {
                    (formula as Var).Value.Replace(source, dest);
                    if (dMode == Mode.Const)
                    {
                        var fConst = new Const((formula as Var).Value);
                        SetFormula(fConst);
                    }
                }
            }
            else if (formula is Const)
            {
                if (sMode == Mode.Const &&
                    (sMode == dMode || (formula as Const).Value == source))
                {
                    (formula as Const).Value.Replace(source, dest);
                    if (dMode == Mode.Var)
                    {
                        var fVar = new Var((formula as Const).Value);
                        SetFormula(fVar);
                    }
                }
                else if (sMode == Mode.Const)
                {
                    string[] sep = { source };
                    var strs = (formula as Const).Value.Split(sep, StringSplitOptions.None);

                    var fEmpty = new Const("");
                    Replacer.ReplaceEntries(fEmpty, strs, dest);
                    SetFormula(fEmpty);
                }
            }
            else
                (formula as FormulaReplaceDecorator).Replace(source, dest, sMode, dMode);
        }
    }

    public static class Replacer
    {
        public static void ReplaceEntries(FormulaDecorator formula, string[] strs, string dest)
        {
            for (int i = 0; i < strs.Length - 1; ++i)
            {
                if (strs[i].Length > 0)
                {
                    var concatC = new ConcatDecorator(strs[i], Mode.Const);
                    concatC.SetFormula(formula);
                    formula.SetFormula(concatC);
                }

                var concatV = new ConcatDecorator(dest, Mode.Var);
                concatV.SetFormula(formula);
                formula.SetFormula(concatV);
            }

            if (strs[strs.Length - 1].Length > 0)
            {
                var concat = new ConcatDecorator(strs[strs.Length - 1], Mode.Const);
                concat.SetFormula(formula);
                formula.SetFormula(concat);
            }
        }
    }

    // Режим для декораторов, выполняющих операции с константами и переменными
    public enum Mode { Const, Var };

    // Константа
    public class Const : FormulaReplaceDecorator
    {
        public string Value { get; }

        public Const(string c)
        {
            if (c == null)
                throw new ArgumentNullException();
            Value = c;
        }

        public override string Calculate(Dictionary<string, string> variables = null)
        {
            return Value;
        }
    }

    // Переменная
    public class Var : FormulaReplaceDecorator
    {
        public string Value { get; }

        public Var(string v)
        {
            Value = v;
        }

        public override string Calculate(Dictionary<string, string> variables = null)
        {
            if (variables.ContainsKey(Value))
                return variables[Value];
            else
                throw new ApplicationException("Uninitialized variable");
        }
    }

    // Удаление начальных и конечных пробельных символов ^ F
    public class RemoveSpacesDecorator : FormulaReplaceDecorator
    {
        public override string Calculate(Dictionary<string, string> variables = null)
        {
            return base.Calculate(variables).Trim();
        }
    }

    // Конкатенация строк F && c и F && v
    public class ConcatDecorator : FormulaReplaceDecorator
    {
        private Mode mode;

        public string ConcatString { get; }
        public Mode ConcatMode
        {
            get { return mode; }
        }
        
        public ConcatDecorator(string str, Mode mode)
        {
            if (str == null)
                throw new ArgumentNullException();
            ConcatString = str;
            this.mode = mode;
        }

        public override string Calculate(Dictionary<string, string> variables = null)
        {
            if (ConcatMode == Mode.Const)
                return Calculate() + ConcatString;
            else if (variables.ContainsKey(ConcatString))
                return Calculate() + variables[ConcatString];
            else
                throw new ApplicationException("Uninitialized variable");
        }

        public override void Replace(string source, string dest, Mode sMode, Mode dMode)
        {
            if (!(formula is Empty) && ConcatMode == sMode)
            {
                if (sMode == Mode.Const && sMode == dMode || ConcatString == source)
                { 
                    ConcatString.Replace(source, dest);
                    mode = dMode;
                }
                else if (sMode == Mode.Const)
                {
                    string[] sep = { source };
                    var strs = ConcatString.Split(sep, StringSplitOptions.None);

                    Replacer.ReplaceEntries(formula as FormulaDecorator, strs, dest);
                }
            }
            
            Replace(source, dest, sMode, dMode);
        }
    }

    // Замена в строке F пробельных символов на символ a
    public class ReplaceSpaceDecorator : FormulaDecorator
    {
        public char Symbol { get; }

        public ReplaceSpaceDecorator(char symb)
        {
            Symbol = symb;
        }

        public override string Calculate(Dictionary<string, string> variables = null)
        {
            return Calculate().Replace(' ', Symbol);
        }
    }

    public class ReplaceSubstringDecorator : FormulaReplaceDecorator
    {
        public string Substring { get; }
        public string Replacement { get; }
        public Mode SourceMode { get; }
        public Mode DestMode { get; }

        public ReplaceSubstringDecorator(string source, string dest, Mode sMode, Mode dMode)
        {
            if (source == null || dest == null)
                throw new ArgumentNullException();
            Substring = source;
            Replacement = dest;
            SourceMode = sMode;
            DestMode = dMode;
        }

        public override string Calculate(Dictionary<string, string> variables = null)
        {
            Replace(Substring, Replacement, SourceMode, DestMode);
            return formula.Calculate();
        }
    }

    //todo остальные декораторы
}
