using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    // Абстрактный декоратор операций
    public abstract class FormulaDecorator : Formula
    {
        protected Formula formula;

        public void SetFormula(Formula f)
        {
            if (f == null)
                throw new ArgumentNullException();
            formula = f;
        }

        public override string Calculate(Dictionary<string, string> variables = null)
        {
            return formula.Calculate();
        }
    }

    // Режим для декораторов, выполняющих операции с константами и переменными
    public enum Mode { Const, Var };

    // Удаление начальных и конечных пробельных символов ^ F
    public class RemoveSpacesDecorator : FormulaDecorator
    {
        public override string Calculate(Dictionary<string, string> variables = null)
        {
            return formula.Calculate(variables).Trim();
        }
    }

    // Конкатенация строк F && c и F && v
    public class ConcatDecorator : FormulaDecorator
    {
        public string ConcatString { get; }
        public Mode ConcatMode { get; }
        
        public ConcatDecorator(string str, Mode mode)
        {
            ConcatString = str;
            ConcatMode = mode;
        }

        public override string Calculate(Dictionary<string, string> variables = null)
        {
            if (ConcatMode == Mode.Const)
                return formula.Calculate() + ConcatString;
            else if (variables.ContainsKey(ConcatString))
                return formula.Calculate() + variables[ConcatString];
            else
                throw new ApplicationException("Uninitialized variable");
        }
    }

    //todo остальные декораторы
}
