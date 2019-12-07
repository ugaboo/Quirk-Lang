using System.Collections.Generic;

namespace Quirk.AST
{
    public class IfStmnt : ProgObj
    {
        public ProgObj Condition;

        public readonly List<ProgObj> Then = new List<ProgObj>();
        public readonly List<ProgObj> Else = new List<ProgObj>();

        public IfStmnt() { }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}