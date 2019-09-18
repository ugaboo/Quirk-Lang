namespace Quirk.AST
{
    public enum BinaryExpressionType {
        None, Mul, Power, Div, Mod, FloorDiv, Add, Sub, LeftShift, RightShift, BitAnd, BitOr, BitXor,
        Less, Greater, Equal, LessOrEqual, GreaterOrEqual, NotEqual, And, Or,
    }

    public class BinaryExpression : ProgObj
    {
        public readonly BinaryExpressionType Type;
        public ProgObj Left;
        public ProgObj Right;


        public BinaryExpression(BinaryExpressionType type, ProgObj left, ProgObj right)
        {
            Type = type;
            Left = left;
            Right = right;
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);
        }
    }
}