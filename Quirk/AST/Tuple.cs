﻿namespace Quirk.AST
{
    public class Tuple : ProgObj
    {
        public Tuple()
        {
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}