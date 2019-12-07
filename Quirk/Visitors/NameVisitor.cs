using System;
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

        public void Visit(IfStmnt ifStmnt)
        {
            Replace(ref ifStmnt.Condition);

            for (var i = 0; i < ifStmnt.Then.Count; i += 1) {
                ifStmnt.Then[i].Accept(this);
            }
            for (var i = 0; i < ifStmnt.Else.Count; i += 1) {
                ifStmnt.Else[i].Accept(this);
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

            var pos = new Overload("__pos__");
            pos.Funcs.Add(BuiltIns.Pos_Int);
            pos.Funcs.Add(BuiltIns.Pos_Float);
            pos.Funcs.Add(BuiltIns.Pos_Bool);
            table["__pos__"] = pos;

            var neg = new Overload("__neg__");
            neg.Funcs.Add(BuiltIns.Neg_Int);
            neg.Funcs.Add(BuiltIns.Neg_Float);
            neg.Funcs.Add(BuiltIns.Neg_Bool);
            table["__neg__"] = neg;

            var lt = new Overload("__lt__");
            lt.Funcs.Add(BuiltIns.Lt_Int_Int);
            lt.Funcs.Add(BuiltIns.Lt_Int_Float);
            lt.Funcs.Add(BuiltIns.Lt_Float_Int);
            lt.Funcs.Add(BuiltIns.Lt_Float_Float);
            lt.Funcs.Add(BuiltIns.Lt_Int_Bool);
            lt.Funcs.Add(BuiltIns.Lt_Bool_Int);
            lt.Funcs.Add(BuiltIns.Lt_Bool_Bool);
            lt.Funcs.Add(BuiltIns.Lt_Float_Bool);
            lt.Funcs.Add(BuiltIns.Lt_Bool_Float);
            table["__lt__"] = lt;

            var gt = new Overload("__gt__");
            gt.Funcs.Add(BuiltIns.Gt_Int_Int);
            gt.Funcs.Add(BuiltIns.Gt_Int_Float);
            gt.Funcs.Add(BuiltIns.Gt_Float_Int);
            gt.Funcs.Add(BuiltIns.Gt_Float_Float);
            gt.Funcs.Add(BuiltIns.Gt_Int_Bool);
            gt.Funcs.Add(BuiltIns.Gt_Bool_Int);
            gt.Funcs.Add(BuiltIns.Gt_Bool_Bool);
            gt.Funcs.Add(BuiltIns.Gt_Float_Bool);
            gt.Funcs.Add(BuiltIns.Gt_Bool_Float);
            table["__gt__"] = gt;

            var le = new Overload("__le__");
            le.Funcs.Add(BuiltIns.Le_Int_Int);
            le.Funcs.Add(BuiltIns.Le_Int_Float);
            le.Funcs.Add(BuiltIns.Le_Float_Int);
            le.Funcs.Add(BuiltIns.Le_Float_Float);
            le.Funcs.Add(BuiltIns.Le_Int_Bool);
            le.Funcs.Add(BuiltIns.Le_Bool_Int);
            le.Funcs.Add(BuiltIns.Le_Bool_Bool);
            le.Funcs.Add(BuiltIns.Le_Float_Bool);
            le.Funcs.Add(BuiltIns.Le_Bool_Float);
            table["__le__"] = le;

            var ge = new Overload("__ge__");
            ge.Funcs.Add(BuiltIns.Ge_Int_Int);
            ge.Funcs.Add(BuiltIns.Ge_Int_Float);
            ge.Funcs.Add(BuiltIns.Ge_Float_Int);
            ge.Funcs.Add(BuiltIns.Ge_Float_Float);
            ge.Funcs.Add(BuiltIns.Ge_Int_Bool);
            ge.Funcs.Add(BuiltIns.Ge_Bool_Int);
            ge.Funcs.Add(BuiltIns.Ge_Bool_Bool);
            ge.Funcs.Add(BuiltIns.Ge_Float_Bool);
            ge.Funcs.Add(BuiltIns.Ge_Bool_Float);
            table["__ge__"] = ge;

            var eq = new Overload("__eq__");
            eq.Funcs.Add(BuiltIns.Eq_Int_Int);
            eq.Funcs.Add(BuiltIns.Eq_Int_Float);
            eq.Funcs.Add(BuiltIns.Eq_Float_Int);
            eq.Funcs.Add(BuiltIns.Eq_Float_Float);
            eq.Funcs.Add(BuiltIns.Eq_Int_Bool);
            eq.Funcs.Add(BuiltIns.Eq_Bool_Int);
            eq.Funcs.Add(BuiltIns.Eq_Bool_Bool);
            eq.Funcs.Add(BuiltIns.Eq_Float_Bool);
            eq.Funcs.Add(BuiltIns.Eq_Bool_Float);
            table["__eq__"] = eq;

            var ne = new Overload("__ne__");
            ne.Funcs.Add(BuiltIns.Ne_Int_Int);
            ne.Funcs.Add(BuiltIns.Ne_Int_Float);
            ne.Funcs.Add(BuiltIns.Ne_Float_Int);
            ne.Funcs.Add(BuiltIns.Ne_Float_Float);
            ne.Funcs.Add(BuiltIns.Ne_Int_Bool);
            ne.Funcs.Add(BuiltIns.Ne_Bool_Int);
            ne.Funcs.Add(BuiltIns.Ne_Bool_Bool);
            ne.Funcs.Add(BuiltIns.Ne_Float_Bool);
            ne.Funcs.Add(BuiltIns.Ne_Bool_Float);
            table["__ne__"] = ne;

            var not = new Overload("__not__");
            not.Funcs.Add(BuiltIns.Not_Int);
            not.Funcs.Add(BuiltIns.Not_Bool);
            not.Funcs.Add(BuiltIns.Not_Float);
            table["__not__"] = not;

            var bitand = new Overload("__bitand__");
            bitand.Funcs.Add(BuiltIns.BitAnd_Int_Int);
            bitand.Funcs.Add(BuiltIns.BitAnd_Int_Bool);
            bitand.Funcs.Add(BuiltIns.BitAnd_Bool_Int);
            bitand.Funcs.Add(BuiltIns.BitAnd_Bool_Bool);
            table["__bitand__"] = bitand;

            var bitor = new Overload("__bitor__");
            bitor.Funcs.Add(BuiltIns.BitOr_Int_Int);
            bitor.Funcs.Add(BuiltIns.BitOr_Int_Bool);
            bitor.Funcs.Add(BuiltIns.BitOr_Bool_Int);
            bitor.Funcs.Add(BuiltIns.BitOr_Bool_Bool);
            table["__bitor__"] = bitor;

            var bitxor = new Overload("__bitxor__");
            bitxor.Funcs.Add(BuiltIns.BitXor_Int_Int);
            bitxor.Funcs.Add(BuiltIns.BitXor_Int_Bool);
            bitxor.Funcs.Add(BuiltIns.BitXor_Bool_Int);
            bitxor.Funcs.Add(BuiltIns.BitXor_Bool_Bool);
            table["__bitxor__"] = bitxor;

            var invert = new Overload("__invert__");
            invert.Funcs.Add(BuiltIns.Invert_Int);
            invert.Funcs.Add(BuiltIns.Invert_Bool);
            table["__invert__"] = invert;

            var lshift = new Overload("__lshift__");
            lshift.Funcs.Add(BuiltIns.LShift_Int_Int);
            lshift.Funcs.Add(BuiltIns.LShift_Int_Bool);
            lshift.Funcs.Add(BuiltIns.LShift_Bool_Int);
            lshift.Funcs.Add(BuiltIns.LShift_Bool_Bool);
            table["__lshift__"] = lshift;

            var rshift = new Overload("__rshift__");
            rshift.Funcs.Add(BuiltIns.RShift_Int_Int);
            rshift.Funcs.Add(BuiltIns.RShift_Int_Bool);
            rshift.Funcs.Add(BuiltIns.RShift_Bool_Int);
            rshift.Funcs.Add(BuiltIns.RShift_Bool_Bool);
            table["__rshift__"] = rshift;
        }
    }
}