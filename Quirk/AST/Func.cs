using System.Collections.Generic;

namespace Quirk.AST
{
    public class Func : ProgObj
    {
        public readonly Dictionary<string, ProgObj> NameTable = new Dictionary<string, ProgObj>();
        public readonly List<ProgObj> Statements = new List<ProgObj>();
        public readonly List<ProgObj> Parameters = new List<ProgObj>();

        public readonly string Name;


        public Func(string name)
        {
            Name = name;
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);
        }
    }
}