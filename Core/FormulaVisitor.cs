using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        public ConstVar Substring { get; }
        public ConstVar Replacement { get; }

        public ReplaceFormulaVisitor(ConstVar source, ConstVar dest)
        {
            if (source == null || dest == null)
                throw new ArgumentNullException();
            Substring = source;
            Replacement = dest;
        }

        public void Visit(FormulaDecorator f)
        {
            Replace(f);
        }

        public void Visit(ConcatDecorator f)
        {
            Replace(f);
            if (f.ConcatValue.IsConst == Substring.IsConst)
            {
                if (Substring.IsConst && Replacement.IsConst || f.ConcatValue.Value == Substring.Value)
                {
                    f.ConcatValue.Value.Replace(Substring.Value, Replacement.Value);
                    f.ConcatValue.IsConst = Replacement.IsConst;
                }
                else if (Substring.IsConst)
                    ReplaceEntries(f, f.ConcatValue.Value);
            }
        }

        private void Replace(FormulaDecorator f)
        {
            if (f.Subformula is Var)
            {
                if (Substring.IsVar && (f.Subformula as Var).Value == Substring.Value)
                {
                    (f.Subformula as Var).Value.Replace(Substring.Value, Replacement.Value);
                    if (Replacement.IsConst)
                    {
                        var fConst = new Const((f.Subformula as Var).Value);
                        f.Subformula = fConst;
                    }
                }
            }
            else if (f.Subformula is Const)
            {
                if (Substring.IsConst && (Replacement.IsConst ||
                    (f.Subformula as Const).Value == Substring.Value))
                {
                    (f.Subformula as Const).Value.Replace(Substring.Value, Replacement.Value);
                    if (Replacement.IsVar)
                    {
                        var fVar = new Var((f.Subformula as Const).Value);
                        f.Subformula = fVar;
                    }
                }
                else if (Substring.IsConst && Replacement.IsVar)
                    ReplaceEntries(f, (f.Subformula as Const).Value);
            }
        }

        private void ReplaceEntries(FormulaDecorator formula, string str)
        {
            string[] sep = { Substring.Value };
            var strs = str.Split(sep, StringSplitOptions.None);

            bool emptyLast = strs.Last().Length == 0;
            strs = strs.Where(s => s.Length > 0).ToArray();

            for (int i = 0; i < strs.Length; ++i)
            {
                var concatC = new ConcatDecorator(new ConstVar(strs[i], Mode.Const));
                concatC.Subformula = formula.Subformula;
                formula.Subformula = concatC;

                if (i < strs.Length - 1 || emptyLast)
                {
                    Assert.IsTrue(Replacement.IsVar);
                    var concatV = new ConcatDecorator(Replacement);
                    concatV.Subformula = formula.Subformula;
                    formula.Subformula = concatV;
                }
            }
        }
    }
}
