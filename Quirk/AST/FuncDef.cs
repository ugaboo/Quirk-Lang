namespace Quirk.AST
{
    public class FuncDef : ProgObj
    {
        public Function Func;


        public FuncDef(Function func)
        {
            Func = func;
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
