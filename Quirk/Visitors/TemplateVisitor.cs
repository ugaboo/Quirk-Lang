using System;
using System.Collections.Generic;
using Quirk.AST;

namespace Quirk.Visitors
{
    public partial class TemplateVisitor : IVisitor
    {
        readonly Dictionary<ProgObj, ProgObj> clones = new Dictionary<ProgObj, ProgObj>();
        readonly Stack<ProgObj> result = new Stack<ProgObj>();


        public TemplateVisitor(Function template, out Function spec)
        {
            template.Accept(this);
            spec = (Function)result.Pop();

            if (result.Count > 0) {
                throw new InvalidOperationException();
            }
        }

        public void Visit(Module module) { throw new InvalidOperationException(); }

        public void Visit(Overload overload)
        {
            var overloadClone = new Overload(overload.Name);

            foreach (var func in overload.Funcs) {
                if (clones.TryGetValue(func, out var funcClone) == true) {
                    overloadClone.Funcs.Add((Function)funcClone);
                } else {
                    overloadClone.Funcs.Add(func);
                }
            }

            result.Push(overloadClone);
        }

        public void Visit(Function func)
        {
            var clone = new Function(func.Name);

            clone.RetType = func.RetType;

            foreach (var param in func.Parameters) {
                param.Accept(this);
                var p = (Parameter)result.Pop();
                clone.Parameters.Add(p);
                clones[param] = p;
            }

            foreach (var stmnt in func.Statements) {
                stmnt.Accept(this);
                clone.Statements.Add(result.Pop());
            }

            result.Push(clone);
        }

        public void Visit(Variable variable)
        {
            if (clones.TryGetValue(variable, out var clone) == true) {
                result.Push(clone);
            } else {
                if (variable.Type == null) {
                    // local variable
                    var v = new Variable(variable);
                    result.Push(v);
                    clones[variable] = v;
                } else {
                    // global variable / closure
                    result.Push(variable);
                }
            }
        }

        public void Visit(Parameter parameter)
        {
            if (clones.TryGetValue(parameter, out var clone) == true) {
                result.Push(clone);
            } else {
                var p = new Parameter(parameter);
                result.Push(p);
                clones[parameter] = p;
            }
        }

        public void Visit(AST.Tuple tuple) { throw new Exception("Not implemented"); }

        public void Visit(FuncDef funcDef)
        {
            funcDef.Func.Accept(this);
            var func = (Function)result.Pop();
            clones[funcDef.Func] = func;
            result.Push(new FuncDef(func));
        }

        public void Visit(Assignment assignment)
        {
            assignment.Left.Accept(this);
            var left = result.Pop();

            assignment.Right.Accept(this);
            var right = result.Pop();

            result.Push(new Assignment(left, right));
        }

        public void Visit(Evaluation evaluation)
        {
            evaluation.Expr.Accept(this);
            var expr = result.Pop();

            result.Push(new Evaluation(expr));
        }

        public void Visit(FuncCall funcCall)
        {
            funcCall.Func.Accept(this);

            var clone = new FuncCall(result.Pop());

            foreach (var arg in funcCall.Args) {
                arg.Accept(this);
                clone.Args.Add(result.Pop());
            }

            result.Push(clone);
        }

        public void Visit(IfStmnt ifStmnt)
        {
            var clone = new IfStmnt();

            foreach (var tuple in ifStmnt.IfThen) {
                tuple.condition.Accept(this);
                var condition = result.Pop();

                var statements = new List<ProgObj>();
                foreach (var stmnt in tuple.statements) {
                    stmnt.Accept(this);
                    statements.Add(result.Pop());
                }

                clone.IfThen.Add((condition, statements));
            }
            foreach (var stmnt in ifStmnt.ElseStatements) {
                stmnt.Accept(this);
                clone.ElseStatements.Add(result.Pop());
            }

            result.Push(clone);
        }

        public void Visit(ReturnStmnt returnStmnt)
        {
            var clone = new ReturnStmnt();

            foreach (var val in returnStmnt.Values) {
                val.Accept(this);
                clone.Values.Add(result.Pop());
            }

            result.Push(clone);
        }

        public void Visit(NameObj namedObj) { throw new InvalidOperationException(); }

        public void Visit(ConstInt constInt)
        {
            result.Push(constInt);
        }

        public void Visit(ConstFloat constFloat)
        {
            result.Push(constFloat);
        }

        public void Visit(ConstBool constBool)
        {
            result.Push(constBool);
        }

        public void Visit(TypeObj typeObj) { throw new Exception("Not implemented"); }
    }
}