using System.Collections.Generic;

namespace Quirk.AST
{
    public class ReturnStmnt : ProgObj
    {
        public readonly List<ProgObj> Values = new List<ProgObj>();


        public ReturnStmnt() { }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}