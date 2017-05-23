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
        bool ToLeave { get; set; }

        void Visit(RemoveSpacesDecorator formula);
        void Visit(ReplaceSpaceDecorator formula);
        void Visit(ReplaceSubstringDecorator formula);
        void Visit(CharsFreqRemoveAdapter formula);
        void Visit(ParenthesesDecorator formula);
        void Visit(ConcatDecorator formula);
    }

    // Визитор на замену подстрок в формуле
    public class ReplaceFormulaVisitor : FormulaVisitor
    {
        public ConstVar Substring { get; set; }
        public ConstVar Replacement { get; set; }

        public bool ToLeave { get; set; }

        public ReplaceFormulaVisitor(ConstVar source, ConstVar dest)
        {
            if (source == null || dest == null)
                throw new ArgumentNullException();
            Substring = source;
            Replacement = dest;
            ToLeave = false;
        }

        public void Visit(RemoveSpacesDecorator f)
        {
            Replace(f);
        }

        public void Visit(ReplaceSpaceDecorator f)
        {
            Replace(f);
        }

        public void Visit(ReplaceSubstringDecorator f)
        {
            Replace(f);
        }

        public void Visit(CharsFreqRemoveAdapter f)
        {
            Replace(f);
        }

        public void Visit(ParenthesesDecorator f)
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
                    f.ConcatValue.Value
                        = f.ConcatValue.Value.Replace(Substring.Value, Replacement.Value);
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
                    (f.Subformula as Var).Value
                        = (f.Subformula as Var).Value.Replace(Substring.Value, Replacement.Value);
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
                    (f.Subformula as Const).Value
                        = (f.Subformula as Const).Value.Replace(Substring.Value, Replacement.Value);
                    if (Replacement.IsVar)
                    {
                        var fVar = new Var((f.Subformula as Const).Value);
                        f.Subformula = fVar;
                    }
                }
                else if (Substring.IsConst && Replacement.IsVar)
                {
                    string value = (f.Subformula as Const).Value;
                    f.Subformula = new Const("");
                    ReplaceEntries(f, value);
                }
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
                var concatC = new ConcatDecorator(new Const(strs[i]));
                concatC.Subformula = formula.Subformula;
                formula.Subformula = concatC;

                if (i < strs.Length - 1 || emptyLast)
                {
                    Assert.IsTrue(Replacement.IsVar);
                    var concatV = new ConcatDecorator(new Var(Replacement.Value));
                    concatV.Subformula = formula.Subformula;
                    formula.Subformula = concatV;
                }
            }
            ToLeave = true;
        }
    }
}
