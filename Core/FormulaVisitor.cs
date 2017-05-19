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
            if (f.ConcatMode == SourceMode)
            {
                if (SourceMode == Mode.Const && SourceMode == DestMode || f.ConcatString == Substring)
                {
                    f.ConcatString.Replace(Substring, Replacement);
                    f.ConcatMode = DestMode;
                }
                else if (SourceMode == Mode.Const)
                    ReplaceEntries(f, f.ConcatString);
            }
        }

        private void Replace(FormulaDecorator f)
        {
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
                    ReplaceEntries(f, (f.Subformula as Const).Value);
            }
        }

        private void ReplaceEntries(FormulaDecorator formula, string str)
        {
            string[] sep = { Substring };
            var strs = str.Split(sep, StringSplitOptions.None);

            bool emptyLast = strs.Last().Length == 0;
            strs = strs.Where(s => s.Length > 0).ToArray();

            for (int i = 0; i < strs.Length; ++i)
            {
                var concatC = new ConcatDecorator(strs[i], Mode.Const);
                concatC.Subformula = formula.Subformula;
                formula.Subformula = concatC;

                if (i < strs.Length - 1 || emptyLast)
                {
                    var concatV = new ConcatDecorator(Replacement, Mode.Var);
                    concatV.Subformula = formula.Subformula;
                    formula.Subformula = concatV;
                }
            }
        }
    }
}
