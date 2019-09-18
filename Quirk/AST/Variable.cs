namespace Quirk.AST
{
    public class Variable : ProgObj
    {
        public readonly string Name;


        public Variable(string name)
        {
            Name = name;
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);
        }
    }
}