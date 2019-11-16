namespace Quirk.AST
{
    public class ConstInt : ProgObj
    {
        public readonly int Value;


        public ConstInt(int value)
        {
            Value = value;
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}