namespace Quirk.AST
{
    public class Evaluation : IProgObj
    {
        public IProgObj Expr;


        public Evaluation(IProgObj expr)
        {
            Expr = expr;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}