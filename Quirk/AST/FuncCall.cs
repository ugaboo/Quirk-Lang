using System.Collections.Generic;

namespace Quirk.AST
{
    public class FuncCall : IProgObj
    {
        public IProgObj Func;
        public readonly List<IProgObj> Args = new List<IProgObj>();


        public FuncCall(IProgObj func)
        {
            Func = func;
        }

        public FuncCall(IProgObj func, List<IProgObj> args)
        {
            Func = func;
            Args.AddRange(args);
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
