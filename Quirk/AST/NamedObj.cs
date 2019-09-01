namespace Quirk.AST
{
    public class NamedObj : IProgObj
    {
        public readonly string Name;


        public NamedObj(string name)
        {
            Name = name;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}