namespace Quirk.AST
{
    public partial interface IVisitor
    {
        void Visit(Module module);
        void Visit(Func func);
        void Visit(Variable variable);
        void Visit(Intrinsic intrinsic);
        void Visit(Tuple tuple);

        void Visit(Assignment assignment);
        void Visit(Evaluation evaluation);

        void Visit(BinaryExpression expression);
        void Visit(UnaryExpression expression);
        void Visit(FuncCall funcCall);
        void Visit(NamedObj namedObj);
        void Visit(ConstBool constBool);
        void Visit(ConstInt constInt);
        void Visit(ConstFloat constFloat);
    }
}