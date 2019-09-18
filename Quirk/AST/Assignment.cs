namespace Quirk.AST
{
    public class Assignment : ProgObj
    {
        public ProgObj Left;
        public ProgObj Right;


        public Assignment(ProgObj left, ProgObj right)
        {
            Left = left;
            Right = right;
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);
        }
    }
}