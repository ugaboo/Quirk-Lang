namespace Quirk.AST
{
    public class NamedObj : ProgObj
    {
        public readonly string Name;


        public NamedObj(string name)
        {
            Name = name;
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);
        }
    }
}