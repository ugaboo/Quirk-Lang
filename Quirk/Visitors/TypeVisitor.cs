using System;
using System.Collections.Generic;
using Quirk.AST;
using static Quirk.ErrorType;
using Quirk.Helpers;

namespace Quirk.Visitors
{
    public partial class TypeVisitor : IVisitor
    {
        readonly SpecFuncs specFuncs = new SpecFuncs();
        readonly Stack<Function> hierarchy = new Stack<Function>();
        readonly Stack<TypeObj> result = new Stack<TypeObj>();

        bool ignoreErrors;


        public TypeVisitor(Module module)
        {
            new NameVisitor(module);

            ignoreErrors = true;
            module.Accept(this);

            if (result.Count > 0) { throw new InvalidOperationException(); }

            ignoreErrors = false;
            module.Accept(this);
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

        public void Visit(Parameter parameter)
        {
            result.Push((TypeObj)parameter.Type);
        }

        public void Visit(AST.Tuple tuple) { throw new Exception("Not implemented"); }

        public void Visit(FuncDef funcDef)
        {
            if (funcDef.Func.TemplateParamsCount == 0) {
                funcDef.Func.Accept(this);
                result.Pop();
            }
        }

        public void Visit(Assignment assignment)
        {
            assignment.Right.Accept(this);
            var type = result.Pop();
            if (type == null) {
                if (ignoreErrors) {
                    return;
                } else {
                    throw new CompilationError(CantDetermineType);
                }
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
            if (funcCall.Func is Function cf) {
                if (hierarchy.Contains(cf)) {
                    result.Push(cf.RetType as TypeObj);
                } else {
                    funcCall.Func.Accept(this);
                }
            } else if (funcCall.Func is Overload overload) {
                var types = new List<TypeObj>();
                foreach (var arg in funcCall.Args) {
                    arg.Accept(this);
                    var type = result.Pop();
                    if (type == null) {
                        if (ignoreErrors) {
                            result.Push(null);
                            return;
                        } else {
                            throw new CompilationError(CantDetermineType);
                        }
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
                        specFuncs.Add(func, spec);
                        overload.Funcs.Add(spec);
                        func.Def.Specs.Add(spec);
                        spec.Accept(this);
                        result.Pop();
                    }
                    if (!ignoreErrors) {
                        funcCall.Func = spec;
                    }
                    result.Push((TypeObj)spec.RetType);
                } else {
                    if (!ignoreErrors) {
                        funcCall.Func = func;
                        func.Accept(this);
                    } else {
                        result.Push((TypeObj)func.RetType);
                    }
                }
            } else {
                throw new CompilationError(ObjectIsNotCallable);
            }
        }

        public void Visit(IfStmnt ifStmnt)
        {
            ifStmnt.Condition.Accept(this);
            var type = result.Pop();
            if (type != BuiltIns.Bool) {
                throw new Exception("Not implemented");
            }

            foreach (var stmnt in ifStmnt.Then) {
                stmnt.Accept(this);
            }
            foreach (var stmnt in ifStmnt.Else) {
                stmnt.Accept(this);
            }
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
                var type = result.Pop();
                if (func.RetType != null && func.RetType != type) {
                    throw new Exception("Not implemented");     // type generaization or type conversion
                } else {
                    func.RetType = type;
                }
            } else {
                throw new Exception("Not implemented");         // tuple
            }
        }

        public void Visit(NameObj nameObj) { throw new InvalidOperationException(); }

        public void Visit(ConstInt constInt)
        {
            result.Push(BuiltIns.Int);
        }

        public void Visit(ConstFloat constFloat)
        {
            result.Push(BuiltIns.Float);
        }

        public void Visit(ConstBool constBool)
        {
            result.Push(BuiltIns.Bool);
        }

        public void Visit(TypeObj typeObj) { throw new InvalidOperationException(); }
    }
}