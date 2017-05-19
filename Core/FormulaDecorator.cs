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

    // Обёртка над строкой для различения констант и переменных
    public class ConstVar
    {
        private string val;

        public string Value
        {
            get { return val; }
        }

        public bool IsConst { get; set; }
        public bool IsVar
        {
            get { return !IsConst; }
        }

        public ConstVar(string value, Mode mode)
        {
            if (value == null)
                throw new ArgumentNullException();
            val = value;
            IsConst = mode == Mode.Const;
        }
    }

    // Режим константы/переменной
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
        public ConstVar ConcatValue { get; }
        
        public ConcatDecorator(ConstVar value)
        {
            if (value == null)
                throw new ArgumentNullException();
            ConcatValue = value;
        }

        public override string Calculate(Dictionary<string, string> variables = null)
        {
            if (ConcatValue.IsConst)
                return Calculate() + ConcatValue;
            else if (variables != null && variables.ContainsKey(ConcatValue.Value))
                return Calculate() + variables[ConcatValue.Value];
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

        ReplaceSubstringDecorator(ConstVar source, ConstVar dest)
        {
            if (source == null || dest == null)
                throw new ArgumentNullException();
            rv = new ReplaceFormulaVisitor(source, dest);
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
