namespace Quirk.AST
{
    public class Evaluation : ProgObj
    {
        public ProgObj Expr;


        public Evaluation(ProgObj expr)
        {
            Expr = expr;
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);
        }
    }
}