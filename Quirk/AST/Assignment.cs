namespace Quirk.AST
{
    public class Assignment : IProgObj
    {
        public IProgObj Left;
        public IProgObj Right;


        public Assignment(IProgObj left, IProgObj right)
        {
            Left = left;
            Right = right;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}