using System.Collections.Generic;

namespace Quirk.AST
{
    public class FuncDef : ProgObj
    {
        public Function Func;
        public readonly List<Function> Specs = new List<Function>();


        public FuncDef(Function func)
        {
            Func = func;
            Func.Def = this;
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
