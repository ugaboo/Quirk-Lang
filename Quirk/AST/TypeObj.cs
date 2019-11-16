namespace Quirk.AST
{
    public class TypeObj : ProgObj
    {
        public static readonly TypeObj Int = new TypeObj("Int");
        public static readonly TypeObj Float = new TypeObj("Float");
        public static readonly TypeObj Bool = new TypeObj("Bool");

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