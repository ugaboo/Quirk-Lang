namespace Quirk.AST
{
    public class NameObj : ProgObj
    {
        public readonly string Name;


        public NameObj(string name)
        {
            Name = name;
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}