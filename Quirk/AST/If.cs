using System.Collections.Generic;

namespace Quirk.AST
{
    public class IfStmnt : ProgObj
    {
        public readonly List<(ProgObj condition, List<ProgObj> statements)> IfThen = new List<(ProgObj, List<ProgObj>)>();
        public readonly List<ProgObj> ElseStatements = new List<ProgObj>();

        public IfStmnt() { }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}