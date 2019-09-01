namespace Quirk.AST
{
    public enum BinaryExpressionType {
        None, Mul, Power, Div, Mod, FloorDiv, Add, Sub, LeftShift, RightShift, BitAnd, BitOr, BitXor,
        Less, Greater, Equal, LessOrEqual, GreaterOrEqual, NotEqual, And, Or,
    }

    public class BinaryExpression : IProgObj
    {
        public readonly BinaryExpressionType Type;
        public IProgObj Left;
        public IProgObj Right;


        public BinaryExpression(BinaryExpressionType type, IProgObj left, IProgObj right)
        {
            Type = type;
            Left = left;
            Right = right;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}