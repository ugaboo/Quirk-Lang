namespace Quirk.AST
{
    public enum IntrinsicType { Print, }

    public class Intrinsic : IProgObj
    {
        public readonly string Name;
        public readonly IntrinsicType Type;


        public Intrinsic(string name, IntrinsicType type)
        {
            Name = name;
            Type = type;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}