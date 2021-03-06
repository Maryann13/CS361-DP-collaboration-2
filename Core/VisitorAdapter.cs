﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Core
{
    public class ReplaceVisitorAdapter : FormulaVisitor
    {
        private ReplaceFormulaVisitor rv;

        public bool ToLeave
        {
            get { return rv.ToLeave; }
            set { rv.ToLeave = value; }
        }

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

        public void Visit(RemoveSpacesDecorator f)
        {
            rv.Visit(f);
        }

        public void Visit(ReplaceSpaceDecorator f)
        {
            rv.Visit(f);
        }

        public void Visit(ReplaceSubstringDecorator f)
        {
            rv.Visit(f);
        }

        public void Visit(CharsFreqRemoveAdapter f)
        {
            rv.Visit(f);
        }

        public void Visit(ParenthesesDecorator f)
        {
            rv.Visit(f);
        }

        public void Visit(ConcatDecorator f)
        {
            rv.Visit(f);
        }
    }
}
