namespace Quirk.AST
{
    public enum UnaryExpressionType { Plus, Minus, BitNot, Not, }

    public class UnaryExpression : IProgObj
    {
        public readonly UnaryExpressionType Type;
        public IProgObj Expr;


        public UnaryExpression(UnaryExpressionType type, IProgObj expr)
        {
            Type = type;
            Expr = expr;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}