namespace Quirk.AST
{
    public class ConstBool : ProgObj
    {
        public readonly bool Value;

        public ConstBool(bool value)
        {
            Value = value;
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);
        }
    }
}