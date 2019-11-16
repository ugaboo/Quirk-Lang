namespace Quirk.AST
{
    public class Variable : ProgObj
    {
        public readonly string Name;
        public ProgObj Type;


        public Variable(string name)
        {
            Name = name;
        }

        public Variable(Variable other)
        {
            Name = other.Name;
            Type = other.Type;
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}