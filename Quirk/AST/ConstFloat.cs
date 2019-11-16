namespace Quirk.AST
{
    public class ConstFloat : ProgObj
    {
        public readonly float Value;


        public ConstFloat(float value)
        {
            Value = value;
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}