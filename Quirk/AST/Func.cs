using System.Collections.Generic;

namespace Quirk.AST
{
    public class Func : IProgObj
    {
        public readonly Dictionary<string, IProgObj> NameTable = new Dictionary<string, IProgObj>();
        public readonly List<IProgObj> Statements = new List<IProgObj>();
        public readonly List<IProgObj> Parameters = new List<IProgObj>();

        public readonly string Name;


        public Func(string name)
        {
            Name = name;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}