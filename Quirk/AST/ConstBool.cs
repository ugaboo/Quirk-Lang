namespace Quirk.AST
{
    public class ConstBool : IProgObj
    {
        public readonly bool Value;

        public ConstBool(bool value)
        {
            Value = value;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}