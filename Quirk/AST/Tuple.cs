namespace Quirk.AST
{
    public class Tuple : ProgObj
    {
        public Tuple()
        {
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);
        }
    }
}