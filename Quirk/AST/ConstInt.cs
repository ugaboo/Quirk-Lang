namespace Quirk.AST
{
    public class ConstInt : IProgObj
    {
        public readonly int Value;

        public ConstInt(int value)
        {
            Value = value;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}