using System.Collections.Generic;

namespace Quirk.AST
{
    public class Overload : ProgObj
    {
        public readonly List<Func> Funcs = new List<Func>();

        public readonly string Name;


        public Overload(Func func)
        {
            Name = func.Name;
            Funcs.Add(func);
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);
        }
    }
}