namespace Quirk.AST
{
    public class Parameter : ProgObj
    {
        public readonly string Name;
        public ProgObj Type;


        public Parameter(string name)
        {
            Name = name;
        }

        public Parameter(int index, TypeObj type)
        {
            Name = ((char)('a' + index)).ToString();
            Type = type;
        }

        public Parameter(Parameter other)
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