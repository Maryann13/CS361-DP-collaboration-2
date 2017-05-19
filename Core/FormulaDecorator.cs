using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CharsFrequency;

namespace Core
{
    // Абстрактный декоратор формулы
    public abstract class FormulaDecorator : Formula
    {
        protected Formula formula;

        public Formula Subformula
        {
            get { return formula; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                formula = value;
            }
        }

        public FormulaDecorator()
        {
            formula = new Const("");
        }

        public override string Calculate(Dictionary<string, string> variables = null)
        {
            return formula.Calculate();
        }

        public override void Accept(FormulaVisitor v)
        {
            v.Visit(this);
            formula.Accept(v);
        }
    }

    // Режим для декораторов, выполняющих операции с константами и переменными
    public enum Mode { Const, Var };

    // Удаление начальных и конечных пробельных символов. ^ F
    public class RemoveSpacesDecorator : FormulaDecorator
    {
        public override string Calculate(Dictionary<string, string> variables = null)
        {
            return base.Calculate(variables).Trim();
        }
    }

    // Конкатенация строк. F && c и F && v
    public class ConcatDecorator : FormulaDecorator
    {
        public string ConcatString { get; }
        public Mode ConcatMode { get; set; }
        
        public ConcatDecorator(string str, Mode mode)
        {
            if (str == null)
                throw new ArgumentNullException();
            ConcatString = str;
            ConcatMode = mode;
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
    }

    // Замена в строке F пробельных символов на символ a. F # a
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

    // Замена в строке F всех вхождений подстроки c1/v1 на c2/v2.
    // F @ (c1, c2), F @ (c1, v2), F @ (v1, c2), F @ (v1, v2)
    public class ReplaceSubstringDecorator : FormulaDecorator
    {
        private ReplaceFormulaVisitor rv;

        ReplaceSubstringDecorator(string source, string dest, Mode sMode, Mode dMode)
        {
            rv = new ReplaceFormulaVisitor(source, dest, sMode, dMode);
        }

        public override string Calculate(Dictionary<string, string> variables = null)
        {
            Accept(rv);
            return formula.Calculate();
        }
    }

    // Круглые скобки. (F)
    public class ParenthesesDecorator : FormulaDecorator
    {
        public override string Calculate(Dictionary<string, string> variables = null)
        {
            return string.Format("({0})", formula.Calculate());
        }
    }

    // TODO: Декоратор формулы F ! c и адаптер для CharsFreqRemover
}
