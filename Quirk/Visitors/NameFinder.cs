using Quirk.AST;
using System;
using System.Collections.Generic;
using NameTable = System.Collections.Generic.Dictionary<string, Quirk.AST.IProgObj>;

namespace Quirk.Visitors
{
    public partial class NameFinder : IVisitor
    {
        Stack<NameTable> nameTables = new Stack<NameTable>();


        IProgObj Find(string name)
        {
            foreach (var table in nameTables) {
                if (table.TryGetValue(name, out var obj)) {
                    return obj;
                }
            }
            return null;
        }

        void Replace(ref IProgObj obj)
        {
            if (obj is NamedObj named) {
                obj = Find(named.Name);
                if (obj == null) {
                    throw new Exception("Object not found");
                }
            } else {
                obj.Accept(this);
            }
        }


        public void Visit(Module module)
        {
            nameTables.Push(module.NameTable);

            foreach (var statement in module.Statements) {
                statement.Accept(this);
            }

            nameTables.Pop();
        }

        public void Visit(Func func)
        {
            nameTables.Push(func.NameTable);

            foreach (var statement in func.Statements) {
                statement.Accept(this);
            }

            nameTables.Pop();
        }

        public void Visit(Variable variable) { }

        public void Visit(Intrinsic intrinsic) { }


        public void Visit(Assignment assignment)
        {
            if (assignment.Left is NamedObj named) {
                assignment.Left = Find(named.Name);
                if (assignment.Left == null) {
                    var variable = new Variable(named.Name);
                    nameTables.Peek()[variable.Name] = variable;
                    assignment.Left = variable;
                }
            } else {
                assignment.Left.Accept(this);
            }

            Replace(ref assignment.Right);
        }

        public void Visit(Evaluation evaluation)
        {
            Replace(ref evaluation.Expr);
        }


        public void Visit(BinaryExpression expression)
        {
            Replace(ref expression.Left);
            Replace(ref expression.Right);
        }

        public void Visit(UnaryExpression expression)
        {
            Replace(ref expression.Expr);
        }

        public void Visit(FuncCall funcCall)
        {
            Replace(ref funcCall.Func);

            var args = funcCall.Args;
            for (var i = 0; i < args.Count; i += 1) {
                var arg = args[i];
                Replace(ref arg);
                args[i] = arg;
            }
        }

        public void Visit(NamedObj namedObj)
        {
            throw new Exception();
        }

        public void Visit(ConstBool constBool) { }

        public void Visit(ConstInt constInt) { }

        public void Visit(ConstFloat constFloat) { }
    }
}