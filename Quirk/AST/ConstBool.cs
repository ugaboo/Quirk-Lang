namespace Quirk.AST
{
    public class ConstBool : ProgObj
    {
        public readonly bool Value;


        public ConstBool(bool value)
        {
            Value = value;
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}