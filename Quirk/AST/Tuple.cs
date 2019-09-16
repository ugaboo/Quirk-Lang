namespace Quirk.AST
{
    public class Tuple : IProgObj
    {
        public Tuple()
        {
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}