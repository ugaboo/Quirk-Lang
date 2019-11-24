﻿using System;
using System.Collections.Generic;
using Quirk.AST;
using Quirk.Helpers;
using NameTable = System.Collections.Generic.Dictionary<string, Quirk.AST.ProgObj>;

namespace Quirk.Visitors
{
    public partial class NameVisitor : IVisitor
    {
        Stack<NameTable> nameTables = new Stack<NameTable>();
        Stack<Function> deferedDefs = new Stack<Function>();

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

            AddBuiltIns(moduleTable);

            nameTables.Push(moduleTable);
            foreach (var statement in module.Statements) {
                statement.Accept(this);
            }
            while (deferedDefs.Count > 0) {
                deferedDefs.Pop().Accept(this);
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
            while (deferedDefs.Count > 0) {
                deferedDefs.Pop().Accept(this);
            }
            nameTables.Pop();
        }

        public void Visit(Variable variable)
        {
            if (variable.Type != null) {
                Replace(ref variable.Type);
            }
        }

        public void Visit(Parameter parameter)
        {
            if (parameter.Type != null) {
                Replace(ref parameter.Type);
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
            //func.Accept(this);
            deferedDefs.Push(func);
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

            while (deferedDefs.Count > 0) {
                deferedDefs.Pop().Accept(this);
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


        void AddBuiltIns(NameTable table)
        {
            table["Int"] = BuiltIns.Int;
            table["Float"] = BuiltIns.Float;
            table["Bool"] = BuiltIns.Bool;

            var print = new Overload("print");
            print.Funcs.Add(BuiltIns.Print_Int);
            print.Funcs.Add(BuiltIns.Print_Float);
            print.Funcs.Add(BuiltIns.Print_Bool);
            table["print"] = print;

            var add = new Overload("__add__");
            add.Funcs.Add(BuiltIns.Add_Int_Int);
            add.Funcs.Add(BuiltIns.Add_Int_Float);
            add.Funcs.Add(BuiltIns.Add_Float_Int);
            add.Funcs.Add(BuiltIns.Add_Float_Float);
            add.Funcs.Add(BuiltIns.Add_Int_Bool);
            add.Funcs.Add(BuiltIns.Add_Bool_Int);
            add.Funcs.Add(BuiltIns.Add_Bool_Bool);
            add.Funcs.Add(BuiltIns.Add_Float_Bool);
            add.Funcs.Add(BuiltIns.Add_Bool_Float);
            table["__add__"] = add;

            var sub = new Overload("__sub__");
            sub.Funcs.Add(BuiltIns.Sub_Int_Int);
            sub.Funcs.Add(BuiltIns.Sub_Int_Float);
            sub.Funcs.Add(BuiltIns.Sub_Float_Int);
            sub.Funcs.Add(BuiltIns.Sub_Float_Float);
            sub.Funcs.Add(BuiltIns.Sub_Int_Bool);
            sub.Funcs.Add(BuiltIns.Sub_Bool_Int);
            sub.Funcs.Add(BuiltIns.Sub_Bool_Bool);
            sub.Funcs.Add(BuiltIns.Sub_Float_Bool);
            sub.Funcs.Add(BuiltIns.Sub_Bool_Float);
            table["__sub__"] = sub;

            var mul = new Overload("__mul__");
            mul.Funcs.Add(BuiltIns.Mul_Int_Int);
            mul.Funcs.Add(BuiltIns.Mul_Int_Float);
            mul.Funcs.Add(BuiltIns.Mul_Float_Int);
            mul.Funcs.Add(BuiltIns.Mul_Float_Float);
            mul.Funcs.Add(BuiltIns.Mul_Int_Bool);
            mul.Funcs.Add(BuiltIns.Mul_Bool_Int);
            mul.Funcs.Add(BuiltIns.Mul_Bool_Bool);
            mul.Funcs.Add(BuiltIns.Mul_Float_Bool);
            mul.Funcs.Add(BuiltIns.Mul_Bool_Float);
            table["__mul__"] = mul;

            var truediv = new Overload("__truediv__");
            truediv.Funcs.Add(BuiltIns.TrueDiv_Int_Int);
            truediv.Funcs.Add(BuiltIns.TrueDiv_Int_Float);
            truediv.Funcs.Add(BuiltIns.TrueDiv_Float_Int);
            truediv.Funcs.Add(BuiltIns.TrueDiv_Float_Float);
            truediv.Funcs.Add(BuiltIns.TrueDiv_Int_Bool);
            truediv.Funcs.Add(BuiltIns.TrueDiv_Bool_Int);
            truediv.Funcs.Add(BuiltIns.TrueDiv_Bool_Bool);
            truediv.Funcs.Add(BuiltIns.TrueDiv_Float_Bool);
            truediv.Funcs.Add(BuiltIns.TrueDiv_Bool_Float);
            table["__truediv__"] = truediv;

            var floordiv = new Overload("__floordiv__");
            floordiv.Funcs.Add(BuiltIns.FloorDiv_Int_Int);
            floordiv.Funcs.Add(BuiltIns.FloorDiv_Int_Float);
            floordiv.Funcs.Add(BuiltIns.FloorDiv_Float_Int);
            floordiv.Funcs.Add(BuiltIns.FloorDiv_Float_Float);
            floordiv.Funcs.Add(BuiltIns.FloorDiv_Int_Bool);
            floordiv.Funcs.Add(BuiltIns.FloorDiv_Bool_Int);
            floordiv.Funcs.Add(BuiltIns.FloorDiv_Bool_Bool);
            floordiv.Funcs.Add(BuiltIns.FloorDiv_Float_Bool);
            floordiv.Funcs.Add(BuiltIns.FloorDiv_Bool_Float);
            table["__floordiv__"] = floordiv;

            var mod = new Overload("__mod__");
            mod.Funcs.Add(BuiltIns.Mod_Int_Int);
            mod.Funcs.Add(BuiltIns.Mod_Int_Float);
            mod.Funcs.Add(BuiltIns.Mod_Float_Int);
            mod.Funcs.Add(BuiltIns.Mod_Float_Float);
            mod.Funcs.Add(BuiltIns.Mod_Int_Bool);
            mod.Funcs.Add(BuiltIns.Mod_Bool_Int);
            mod.Funcs.Add(BuiltIns.Mod_Bool_Bool);
            mod.Funcs.Add(BuiltIns.Mod_Float_Bool);
            mod.Funcs.Add(BuiltIns.Mod_Bool_Float);
            table["__mod__"] = mod;

            var pow = new Overload("__pow__");
            pow.Funcs.Add(BuiltIns.Pow_Int_Int);
            pow.Funcs.Add(BuiltIns.Pow_Int_Float);
            pow.Funcs.Add(BuiltIns.Pow_Float_Int);
            pow.Funcs.Add(BuiltIns.Pow_Float_Float);
            pow.Funcs.Add(BuiltIns.Pow_Int_Bool);
            pow.Funcs.Add(BuiltIns.Pow_Bool_Int);
            pow.Funcs.Add(BuiltIns.Pow_Bool_Bool);
            pow.Funcs.Add(BuiltIns.Pow_Float_Bool);
            pow.Funcs.Add(BuiltIns.Pow_Bool_Float);
            table["__pow__"] = pow;
        }
    }
}