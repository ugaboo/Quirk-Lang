namespace Quirk.AST
{
    public abstract class ProgObj
    {
        static int maxId = 0;

        public readonly int Id;


        public ProgObj()
        {
            maxId += 1;
            Id = maxId;
        }

        public abstract void Accept(IVisitor visitor);
    }
}