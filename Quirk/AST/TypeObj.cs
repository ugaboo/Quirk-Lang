namespace Quirk.AST
{
    public class TypeObj : ProgObj
    {
        public string Name;


        public TypeObj(string name)
        {
            Name = name;
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}