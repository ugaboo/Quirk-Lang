using System;
using System.Collections.Generic;
using NameTable = System.Collections.Generic.Dictionary<string, Quirk.AST.IProgObj>;

namespace Quirk.Visitors
{
    public partial class NameFinder : AST.IVisitor
    {
        Stack<NameTable> nameTables = new Stack<NameTable>();


        AST.IProgObj Find(string name)
        {
            foreach (var table in nameTables) {
                if (table.TryGetValue(name, out var obj)) {
                    return obj;
                }
            }
            return null;
        }

        void Replace(ref AST.IProgObj obj)
        {
            if (obj is AST.NamedObj named) {
                obj = Find(named.Name);
                if (obj == null) {
                    throw new Exception("Object not found");
                }
            } else {
                obj.Accept(this);
            }
        }


        public void Visit(AST.Module module)
        {
            nameTables.Push(module.NameTable);

            foreach (var statement in module.Statements) {
                statement.Accept(this);
            }

            nameTables.Pop();
        }

        public void Visit(AST.Func func)
        {
            nameTables.Push(func.NameTable);

            foreach (var statement in func.Statements) {
                statement.Accept(this);
            }

            nameTables.Pop();
        }

        public void Visit(AST.Variable variable) { }

        public void Visit(AST.Intrinsic intrinsic) { }

        public void Visit(AST.Tuple tuple)
        {
            throw new Exception();
        }


        public void Visit(AST.Assignment assignment)
        {
            if (assignment.Left is AST.NamedObj named) {
                assignment.Left = Find(named.Name);
                if (assignment.Left == null) {
                    var variable = new AST.Variable(named.Name);
                    nameTables.Peek()[variable.Name] = variable;
                    assignment.Left = variable;
                }
            } else {
                assignment.Left.Accept(this);
            }

            Replace(ref assignment.Right);
        }

        public void Visit(AST.Evaluation evaluation)
        {
            Replace(ref evaluation.Expr);
        }


        public void Visit(AST.BinaryExpression expression)
        {
            Replace(ref expression.Left);
            Replace(ref expression.Right);
        }

        public void Visit(AST.UnaryExpression expression)
        {
            Replace(ref expression.Expr);
        }

        public void Visit(AST.FuncCall funcCall)
        {
            Replace(ref funcCall.Func);

            var args = funcCall.Args;
            for (var i = 0; i < args.Count; i += 1) {
                var arg = args[i];
                Replace(ref arg);
                args[i] = arg;
            }
        }

        public void Visit(AST.NamedObj namedObj)
        {
            throw new Exception();
        }

        public void Visit(AST.ConstBool constBool) { }

        public void Visit(AST.ConstInt constInt) { }

        public void Visit(AST.ConstFloat constFloat) { }
    }
}