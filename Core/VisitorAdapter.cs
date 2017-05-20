using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Core
{
    public class ReplaceVisitorAdapter : FormulaVisitor
    {
        private ReplaceFormulaVisitor rv;

        public ReplaceVisitorAdapter(Const source, Const dest)
        {
            rv = new ReplaceFormulaVisitor
                (new ConstVar(source), new ConstVar(dest));
        }

        public ReplaceVisitorAdapter(Const source, Var dest)
        {
            rv = new ReplaceFormulaVisitor
                (new ConstVar(source), new ConstVar(dest));
        }

        public ReplaceVisitorAdapter(Var source, Const dest)
        {
            rv = new ReplaceFormulaVisitor
                (new ConstVar(source), new ConstVar(dest));
        }

        public ReplaceVisitorAdapter(Var source, Var dest)
        {
            rv = new ReplaceFormulaVisitor
                (new ConstVar(source), new ConstVar(dest));
        }

        public void Visit(FormulaDecorator f)
        {
            rv.Visit(f);
        }

        public void Visit(ConcatDecorator f)
        {
            rv.Visit(f);
        }
    }
}
