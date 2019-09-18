namespace Quirk.AST
{
    public class ConstFloat : ProgObj
    {
        public readonly float Value;

        public ConstFloat(float value)
        {
            Value = value;
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);
        }
    }
}