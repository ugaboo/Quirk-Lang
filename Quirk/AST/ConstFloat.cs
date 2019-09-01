namespace Quirk.AST
{
    public class ConstFloat : IProgObj
    {
        public readonly float Value;

        public ConstFloat(float value)
        {
            Value = value;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}