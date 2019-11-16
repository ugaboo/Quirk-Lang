namespace Quirk.AST
{
    public partial interface IVisitor
    {
        void Visit(Module module);
        void Visit(Overload overload);
        void Visit(Function func);
        void Visit(Variable variable);
        void Visit(Tuple tuple);
        void Visit(FuncDef funcDef);
        void Visit(Assignment assignment);
        void Visit(Evaluation evaluation);
        void Visit(FuncCall funcCall);
        void Visit(ReturnStmnt returnStmnt);
        void Visit(NameObj namedObj);
        void Visit(ConstInt constInt);
        void Visit(ConstFloat constFloat);
        void Visit(ConstBool constBool);
        void Visit(TypeObj type);
    }
}