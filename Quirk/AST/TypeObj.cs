using System.Collections.Generic;

namespace Quirk.AST
{
    public class TypeObj : ProgObj
    {
        public string Name;
        public readonly List<Function> Initializers = new List<Function>();


        public TypeObj(string name)
        {
            Name = name;
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}