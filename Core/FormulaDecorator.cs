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
            return formula.Calculate(variables);
        }

        public override void Accept(FormulaVisitor v)
        {
            v.Visit(this);
            if (!v.ToLeave)
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
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                val = value;
            }
        }

        public bool IsConst { get; set; }
        public bool IsVar
        {
            get { return !IsConst; }
        }

        public ConstVar(Const value)
        {
            if (value == null)
                throw new ArgumentNullException();
            val = value.Value;
            IsConst = true;
        }

        public ConstVar(Var value)
        {
            if (value == null)
                throw new ArgumentNullException();
            val = value.Value;
            IsConst = false;
        }
    }

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
        public ConstVar ConcatValue { get; set; }
        
        public ConcatDecorator(Const value)
        {
            ConcatValue = new ConstVar(value);
        }

        public ConcatDecorator(Var value)
        {
            ConcatValue = new ConstVar(value);
        }

        public override string Calculate(Dictionary<string, string> variables = null)
        {
            if (ConcatValue.IsConst)
                return base.Calculate(variables) + ConcatValue.Value;
            else if (variables != null && variables.ContainsKey(ConcatValue.Value))
                return base.Calculate(variables) + variables[ConcatValue.Value];
            else
                throw new ApplicationException("Uninitialized variable");
        }
    }

    // Замена в строке F пробельных символов на символ a. F # a
    public class ReplaceSpaceDecorator : FormulaDecorator
    {
        private char symbol;
        public char Symbol
        {
            get
            {
                return symbol;
            }
        }

        public ReplaceSpaceDecorator(char symb)
        {
            symbol = symb;
        }

        public override string Calculate(Dictionary<string, string> variables = null)
        {
            return base.Calculate(variables).Replace(' ', Symbol);
        }
    }

    // Замена в строке F всех вхождений подстроки c1/v1 на c2/v2.
    // F @ (c1, c2), F @ (c1, v2), F @ (v1, c2), F @ (v1, v2)
    public class ReplaceSubstringDecorator : FormulaDecorator
    {
        private ReplaceVisitorAdapter rv;

        public ReplaceSubstringDecorator(Const source, Const dest)
        {
            rv = new ReplaceVisitorAdapter(source, dest);
        }

        public ReplaceSubstringDecorator(Const source, Var dest)
        {
            rv = new ReplaceVisitorAdapter(source, dest);
        }

        public ReplaceSubstringDecorator(Var source, Const dest)
        {
            rv = new ReplaceVisitorAdapter(source, dest);
        }

        public ReplaceSubstringDecorator(Var source, Var dest)
        {
            rv = new ReplaceVisitorAdapter(source, dest);
        }

        public override string Calculate(Dictionary<string, string> variables = null)
        {
            base.Calculate(variables);
            Accept(rv);
            return base.Calculate(variables);
        }
    }

    // Круглые скобки. (F)
    public class ParenthesesDecorator : FormulaDecorator
    {
        public override string Calculate(Dictionary<string, string> variables = null)
        {
            return string.Format("({0})", base.Calculate(variables));
        }
    }

    // Удаление из F всех вхождений самого частого
    // и самого редкого символов из строки c. F ! c
    public class CharsFreqRemoveAdapter : FormulaDecorator
    {
        private CharsFreqRemover cfr;

        public CharsFreqRemoveAdapter(Const value)
        {
            cfr = new CharsFreqRemover(value.Value);
        }

        public override string Calculate(Dictionary<string, string> variables = null)
        {
            return cfr.Remove(base.Calculate(variables));
        }
    }
}
