namespace Quirk.AST
{
    public interface IProgObj
    {
        void Accept(IVisitor visitor);
    }
}