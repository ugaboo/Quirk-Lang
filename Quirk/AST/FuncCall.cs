using System.Collections.Generic;

namespace Quirk.AST
{
    public class FuncCall : ProgObj
    {
        public ProgObj Func;
        public readonly List<ProgObj> Args = new List<ProgObj>();


        public FuncCall(ProgObj func)
        {
            Func = func;
        }

        public FuncCall(ProgObj func, List<ProgObj> args)
        {
            Func = func;
            Args.AddRange(args);
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
