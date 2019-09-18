using System;
using System.Collections.Generic;
using NameTable = System.Collections.Generic.Dictionary<string, Quirk.AST.ProgObj>;

namespace Quirk.Visitors
{
    public partial class NameFinder : AST.Visitor
    {
        Stack<NameTable> nameTables = new Stack<NameTable>();


        AST.ProgObj Find(string name)
        {
            foreach (var table in nameTables) {
                if (table.TryGetValue(name, out var obj)) {
                    return obj;
                }
            }
            return null;
        }

        void Replace(ref AST.ProgObj obj)
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


        public override void Visit(AST.Module module)
        {
            nameTables.Push(module.NameTable);

            foreach (var pair in module.NameTable) {
                pair.Value.Accept(this);
            }
            foreach (var statement in module.Statements) {
                statement.Accept(this);
            }

            nameTables.Pop();
        }

        public override void Visit(AST.Overload overload)
        {
            foreach (var func in overload.Funcs) {
                func.Accept(this);
            }
        }

        public override void Visit(AST.Func func)
        {
            nameTables.Push(func.NameTable);

            foreach (var pair in func.NameTable) {
                pair.Value.Accept(this);
            }
            foreach (var statement in func.Statements) {
                statement.Accept(this);
            }

            nameTables.Pop();
        }


        public override void Visit(AST.Assignment assignment)
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

        public override void Visit(AST.Evaluation evaluation)
        {
            Replace(ref evaluation.Expr);
        }


        public override void Visit(AST.BinaryExpression expression)
        {
            Replace(ref expression.Left);
            Replace(ref expression.Right);
        }

        public override void Visit(AST.UnaryExpression expression)
        {
            Replace(ref expression.Expr);
        }

        public override void Visit(AST.FuncCall funcCall)
        {
            Replace(ref funcCall.Func);

            var args = funcCall.Args;
            for (var i = 0; i < args.Count; i += 1) {
                var arg = args[i];
                Replace(ref arg);
                args[i] = arg;
            }
        }

        public override void Visit(AST.NamedObj namedObj)
        {
            throw new Exception();
        }
    }
}