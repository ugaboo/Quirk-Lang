using System.Collections.Generic;

namespace Quirk.AST
{
    public class Module : ProgObj
    {
        public readonly string Name;
        public readonly List<ProgObj> Statements = new List<ProgObj>();


        public Module(string name)
        {
            Name = name;
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}