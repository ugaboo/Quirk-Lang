namespace Quirk.AST
{
    public class Variable : IProgObj
    {
        public readonly string Name;


        public Variable(string name)
        {
            Name = name;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}