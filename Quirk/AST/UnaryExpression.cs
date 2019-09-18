namespace Quirk.AST
{
    public enum UnaryExpressionType { Plus, Minus, BitNot, Not, }

    public class UnaryExpression : ProgObj
    {
        public readonly UnaryExpressionType Type;
        public ProgObj Expr;


        public UnaryExpression(UnaryExpressionType type, ProgObj expr)
        {
            Type = type;
            Expr = expr;
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);
        }
    }
}