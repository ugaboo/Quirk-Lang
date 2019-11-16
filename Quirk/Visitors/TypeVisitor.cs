﻿using System;
using System.Collections.Generic;
using Quirk.AST;
using static Quirk.ErrorType;

namespace Quirk.Visitors
{
    public partial class TypeVisitor : IVisitor
    {
        readonly SpecFuncs specFuncs = new SpecFuncs();
        readonly Stack<Function> hierarchy = new Stack<Function>();
        readonly Stack<TypeObj> result = new Stack<TypeObj>();


        public TypeVisitor(Module module)
        {
            new NameVisitor(module);
            module.Accept(this);
            if (result.Count > 0) {
                throw new InvalidOperationException();
            }
        }

        public void Visit(Module module)
        {
            foreach (var statement in module.Statements) {
                statement.Accept(this);
            }
        }

        public void Visit(Overload overload) { throw new InvalidOperationException(); }

        public void Visit(Function func)
        {
            hierarchy.Push(func);

            foreach (var statement in func.Statements) {
                statement.Accept(this);
            }
            if (func.RetType != null) {
                result.Push((TypeObj)func.RetType);     // TODO: return statement + RetType calculation
            } else {
                result.Push(null);
            }

            hierarchy.Pop();
        }

        public void Visit(Variable variable)
        {
            result.Push((TypeObj)variable.Type);
        }

        public void Visit(AST.Tuple tuple) { throw new Exception("Not implemented"); }

        public void Visit(FuncDef funcDef) { }

        public void Visit(Assignment assignment)
        {
            assignment.Right.Accept(this);
            var type = result.Pop();
            if (type == null) {
                throw new CompilationError(CantDetermineType);
            }
            if (assignment.Left is Variable variable) {
                if (variable.Type == null) {
                    variable.Type = type;
                } else if (variable.Type != type) {
                    throw new Exception("Not implemented");     // type generalization
                }
            } else {
                throw new CompilationError(AssignmentIsNotPossible);
            }
        }

        public void Visit(Evaluation evaluation)
        {
            evaluation.Expr.Accept(this);
            result.Pop();
        }

        public void Visit(FuncCall funcCall)
        {
            var overload = funcCall.Func as Overload;
            if (overload == null) {
                throw new CompilationError(ObjectIsNotCallable);
            }
            var types = new List<TypeObj>();
            foreach (var arg in funcCall.Args) {
                arg.Accept(this);
                var type = result.Pop();
                if (type == null) {
                    throw new CompilationError(CantDetermineType);
                }
                types.Add(type);
            }
            var func = overload.Find(types);
            if (func == null) {
                throw new CompilationError(ObjectIsNotDefined);
            }
            if (func.TemplateParamsCount > 0) {
                var spec = specFuncs.Find(func, types);
                if (spec == null) {
                    new TemplateVisitor(func, out spec);
                    for (var i = 0; i < types.Count; i += 1) {
                        spec.Parameters[i].Type = types[i];
                    }
                }
                funcCall.Func = spec;
            } else {
                funcCall.Func = func;
            }
            funcCall.Func.Accept(this);
        }

        public void Visit(ReturnStmnt returnStmnt)
        {
            if (hierarchy.Count == 0) {
                throw new CompilationError(ReturnOutsideFunction);
            }
            var func = hierarchy.Peek();
            if (returnStmnt.Values.Count == 0) {
                if (func.RetType != null) {
                    throw new CompilationError(ObjectRequired);
                }
            } else if (returnStmnt.Values.Count == 1) {
                returnStmnt.Values[0].Accept(this);
                if (func.RetType != null) {
                    throw new Exception("Not implemented");     // type generaization or type conversion
                } else {
                    func.RetType = result.Pop();
                }
            } else {
                throw new Exception("Not implemented");         // tuple
            }
        }

        public void Visit(NameObj nameObj) { throw new InvalidOperationException(); }

        public void Visit(ConstInt constInt)
        {
            result.Push(TypeObj.Int);
        }

        public void Visit(ConstFloat constFloat)
        {
            result.Push(TypeObj.Float);
        }

        public void Visit(ConstBool constBool)
        {
            result.Push(TypeObj.Bool);
        }

        public void Visit(TypeObj typeObj) { throw new InvalidOperationException(); }
    }
}