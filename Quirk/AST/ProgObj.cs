namespace Quirk.AST
{
    public abstract class ProgObj
    {
        public abstract void Accept(Visitor visitor);
    }
}