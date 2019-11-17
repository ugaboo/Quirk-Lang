using System;
using System.Collections.Generic;
using Quirk.AST;
using NameTable = System.Collections.Generic.Dictionary<string, Quirk.AST.ProgObj>;

namespace Quirk.Visitors
{
    public partial class NameVisitor : IVisitor
    {
        Stack<NameTable> nameTables = new Stack<NameTable>();

        ProgObj Find(string name)
        {
            foreach (var table in nameTables) {
                if (table.TryGetValue(name, out var obj)) {
                    return obj;
                }
            }
            return null;
        }

        void Replace(ref ProgObj obj)
        {
            if (obj is NameObj named) {
                var found = Find(named.Name);
                obj = found ?? throw new CompilationError(ErrorType.ObjectIsNotDefined);
            } else {
                obj.Accept(this);
            }
        }


        public NameVisitor(Module module)
        {
            module.Accept(this);
        }

        public void Visit(Module module)
        {
            var moduleTable = new NameTable();

            CreateBuiltIns(module, moduleTable);

            nameTables.Push(moduleTable);
            foreach (var statement in module.Statements) {
                statement.Accept(this);
            }
            nameTables.Pop();
        }

        public void Visit(Overload overload) { throw new InvalidOperationException(); }

        public void Visit(Function func)
        {
            var funcTable = new NameTable();

            foreach (var param in func.Parameters) {
                if (funcTable.ContainsKey(param.Name)) {
                    throw new CompilationError(ErrorType.DuplicateParameter);
                } else {
                    funcTable[param.Name] = param;
                }
                param.Accept(this);
            }

            if (func.RetType != null) {
                Replace(ref func.RetType);
            }

            nameTables.Push(funcTable);
            foreach (var statement in func.Statements) {
                statement.Accept(this);
            }
            nameTables.Pop();
        }

        public void Visit(Variable variable)
        {
            if (variable.Type != null) {
                Replace(ref variable.Type);
            }
        }

        public void Visit(AST.Tuple tuple) { throw new Exception("Not implemented"); }

        public void Visit(FuncDef funcDef)
        {
            var func = funcDef.Func;
            var name = func.Name;
            if (Find(name) is Overload overload) {
                overload = new Overload(overload);
            } else {
                overload = new Overload(name);                
            }
            overload.Funcs.Add(func);
            nameTables.Peek()[name] = overload;
            func.Accept(this);
        }

        public void Visit(Assignment assignment)
        {
            if (assignment.Left is NameObj named) {
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

        public void Visit(FuncCall funcCall)
        {
            Replace(ref funcCall.Func);
            var overload = (Overload)funcCall.Func;
            var localCopy = new Overload(overload);
            funcCall.Func = localCopy;

            var args = funcCall.Args;
            for (var i = 0; i < args.Count; i += 1) {
                var arg = args[i];
                Replace(ref arg);
                args[i] = arg;
            }
        }

        public void Visit(ReturnStmnt returnStmnt)
        {
            var vals = returnStmnt.Values;
            for (var i = 0; i < vals.Count; i += 1) {
                var val = vals[i];
                Replace(ref val);
                vals[i] = val;
            }
        }

        public void Visit(NameObj namedObj) { throw new InvalidOperationException(); }

        public void Visit(ConstInt constInt) { }

        public void Visit(ConstFloat constFloat) { }

        public void Visit(ConstBool constBool) { }

        public void Visit(TypeObj typeObj) { throw new InvalidOperationException(); }


        void CreateBuiltIns(Module module, NameTable table)
        {
            void CreateOverload(string name, ProgObj retType, params ProgObj[] paramTypes)
            {
                Overload overload;
                if (table.TryGetValue(name, out var obj)) {
                    overload = (Overload)obj;
                } else {
                    table[name] = overload = new Overload(name);
                }
                var func = new Function(name);
                func.RetType = retType;
                for (var i = 0; i < paramTypes.Length; i += 1) {
                    var v = new Variable( ( (char)('a' + i) ).ToString() );
                    v.Type = paramTypes[i];
                    func.Parameters.Add(v);
                }
                overload.Funcs.Add(func);
            }

            table["Int"] = TypeObj.Int;
            table["Float"] = TypeObj.Float;
            table["Bool"] = TypeObj.Bool;

            CreateOverload("print", null, TypeObj.Int);

            CreateOverload("__add__", TypeObj.Int, TypeObj.Int, TypeObj.Int);
            CreateOverload("__add__", TypeObj.Float, TypeObj.Float, TypeObj.Float);
            CreateOverload("__add__", TypeObj.Float, TypeObj.Int, TypeObj.Float);
            CreateOverload("__add__", TypeObj.Float, TypeObj.Float, TypeObj.Int);
        }
    }
}