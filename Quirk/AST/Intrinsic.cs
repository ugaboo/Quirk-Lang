namespace Quirk.AST
{
    public enum IntrinsicType { Print, }

    public class Intrinsic : ProgObj
    {
        public readonly string Name;
        public readonly IntrinsicType Type;


        public Intrinsic(string name, IntrinsicType type)
        {
            Name = name;
            Type = type;
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);
        }
    }
}