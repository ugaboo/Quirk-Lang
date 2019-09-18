namespace Quirk.AST
{
    public abstract partial class Visitor
    {
        public virtual void Visit(Module module) { }
        public virtual void Visit(Overload overload) { }
        public virtual void Visit(Func func) { }
        public virtual void Visit(Variable variable) { }
        public virtual void Visit(Intrinsic intrinsic) { }
        public virtual void Visit(Tuple tuple) { }

        public virtual void Visit(Assignment assignment) { }
        public virtual void Visit(Evaluation evaluation) { }

        public virtual void Visit(BinaryExpression expression) { }
        public virtual void Visit(UnaryExpression expression) { }
        public virtual void Visit(FuncCall funcCall) { }
        public virtual void Visit(NamedObj namedObj) { }
        public virtual void Visit(ConstBool constBool) { }
        public virtual void Visit(ConstInt constInt) { }
        public virtual void Visit(ConstFloat constFloat) { }
    }
}