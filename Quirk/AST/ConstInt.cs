namespace Quirk.AST
{
    public class ConstInt : ProgObj
    {
        public readonly int Value;

        public ConstInt(int value)
        {
            Value = value;
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);
        }
    }
}