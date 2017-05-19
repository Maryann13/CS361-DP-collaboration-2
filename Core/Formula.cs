using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    // Абстрактный класс формулы
    public abstract class Formula
    {
        /// <summary>
        /// Вычислить значение формулы
        /// </summary>
        /// <param name="variables">Словарь со значениями переменных</param>
        public abstract string Calculate(Dictionary<string, string> variables = null);

        public abstract void Accept(FormulaVisitor v);
    }

    // Константа
    public class Const : Formula
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

        public Const(string c)
        {
            Value = c;
        }

        public override void Accept(FormulaVisitor v)
        { }

        public override string Calculate(Dictionary<string, string> variables = null)
        {
            return Value;
        }
    }

    // Переменная
    public class Var : Formula
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

        public Var(string v)
        {
            Value = v;
        }

        public override void Accept(FormulaVisitor v)
        { }

        public override string Calculate(Dictionary<string, string> variables = null)
        {
            if (variables.ContainsKey(Value))
                return variables[Value];
            else
                throw new ApplicationException("Uninitialized variable");
        }
    }
}
