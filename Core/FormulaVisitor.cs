using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    // Интерфейс визитора для формулы
    public interface FormulaVisitor
    {
        void Visit(FormulaDecorator formula);
        void Visit(ConcatDecorator formula);
    }

    // Визитор на замену подстрок в формуле
    public class ReplaceFormulaVisitor : FormulaVisitor
    {
        public string Substring { get; }
        public string Replacement { get; }
        public Mode SourceMode { get; }
        public Mode DestMode { get; }

        public ReplaceFormulaVisitor(string source, string dest, Mode sMode, Mode dMode)
        {
            if (source == null || dest == null)
                throw new ArgumentNullException();
            Substring = source;
            Replacement = dest;
            SourceMode = sMode;
            DestMode = dMode;
        }

        public void Visit(FormulaDecorator f)
        {
            Replace(f);
        }

        public void Visit(ConcatDecorator f)
        {
            Replace(f);
            if (!(f.Subformula is Empty) && f.ConcatMode == SourceMode)
            {
                if (SourceMode == Mode.Const && SourceMode == DestMode || f.ConcatString == Substring)
                {
                    f.ConcatString.Replace(Substring, Replacement);
                    f.ConcatMode = DestMode;
                }
                else if (SourceMode == Mode.Const)
                {
                    string[] sep = { Substring };
                    var strs = f.ConcatString.Split(sep, StringSplitOptions.None);

                    ReplaceEntries(f.Subformula as FormulaDecorator, strs);
                }
            }
        }

        private void Replace(FormulaDecorator f)
        {
            if (f.Subformula is Empty)
                return;

            if (f.Subformula is Var)
            {
                if (SourceMode == Mode.Var && (f.Subformula as Var).Value == Substring)
                {
                    (f.Subformula as Var).Value.Replace(Substring, Replacement);
                    if (DestMode == Mode.Const)
                    {
                        var fConst = new Const((f.Subformula as Var).Value);
                        f.Subformula = fConst;
                    }
                }
            }
            else if (f.Subformula is Const)
            {
                if (SourceMode == Mode.Const &&
                    (SourceMode == DestMode || (f.Subformula as Const).Value == Substring))
                {
                    (f.Subformula as Const).Value.Replace(Substring, Replacement);
                    if (DestMode == Mode.Var)
                    {
                        var fVar = new Var((f.Subformula as Const).Value);
                        f.Subformula = fVar;
                    }
                }
                else if (SourceMode == Mode.Const)
                {
                    string[] sep = { Substring };
                    var strs = (f.Subformula as Const).Value.Split(sep, StringSplitOptions.None);

                    var fEmpty = new Const("");
                    ReplaceEntries(fEmpty, strs);
                    f.Subformula = fEmpty;
                }
            }
        }

        private void ReplaceEntries(FormulaDecorator formula, string[] strs)
        {
            for (int i = 0; i < strs.Length - 1; ++i)
            {
                if (strs[i].Length > 0)
                {
                    var concatC = new ConcatDecorator(strs[i], Mode.Const);
                    concatC.Subformula = formula;
                    formula.Subformula = concatC;
                }

                var concatV = new ConcatDecorator(Replacement, Mode.Var);
                concatV.Subformula = formula;
                formula.Subformula = concatV;
            }

            if (strs[strs.Length - 1].Length > 0)
            {
                var concat = new ConcatDecorator(strs[strs.Length - 1], Mode.Const);
                concat.Subformula = formula;
                formula.Subformula = concat;
            }
        }
    }
}
