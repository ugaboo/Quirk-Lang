﻿using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Quirk.Tests
{
    [TestClass()]
    public class ParserTests
    {
        //[TestMethod()] public void Test()
        //{
        //    var parser = new Parser("Code/Parser/Test.qk", "Test", out var module);
        //    module.Accept(new Visitors.NameFinder());
        //    module.Accept(new Visitors.CodeGenerator());

        //    var process = new Process();
        //    process.StartInfo = new ProcessStartInfo() {
        //        WindowStyle = ProcessWindowStyle.Hidden,
        //        FileName = "clang",
        //        Arguments = "Test.bc -o test.exe",
        //    };
        //    process.Start();
        //}

        [TestMethod()]
        public void Module()
        {
            new Parser("Code/Parser/Module.qk", "Module", out var module);
            Assert.AreEqual("Module", module.Name);
            Assert.AreEqual(0, module.Statements.Count);
        }

        [TestMethod()]
        public void SimpleStmnt()
        {
            new Parser("Code/Parser/SimpleStmnt.qk", "SimpleStmnt", out var module);
            Assert.AreEqual(4, module.Statements.Count);
        }

        [TestMethod()]
        public void ExprStmnt()
        {
            new Parser("Code/Parser/ExprStmnt.qk", "ExprStmnt", out var module);
            Assert.AreEqual(5, module.Statements.Count);

            var evaluation = (AST.Evaluation)module.Statements[0];
            Assert.AreEqual(2, ((AST.ConstInt)evaluation.Expr).Value);

            var assignment = (AST.Assignment)module.Statements[1];
            Assert.AreEqual("x", ((AST.NameObj)assignment.Left).Name);
            Assert.AreEqual(0, ((AST.ConstInt)assignment.Right).Value);

            assignment = (AST.Assignment)module.Statements[2];
            Assert.AreEqual("z", ((AST.NameObj)assignment.Left).Name);
            Assert.AreEqual(1, ((AST.ConstInt)assignment.Right).Value);

            var z = assignment.Left;

            assignment = (AST.Assignment)module.Statements[3];
            Assert.AreEqual("y", ((AST.NameObj)assignment.Left).Name);
            Assert.AreEqual(z, assignment.Right);

            var y = assignment.Left;

            assignment = (AST.Assignment)module.Statements[4];
            Assert.AreEqual("x", ((AST.NameObj)assignment.Left).Name);       // NamedObj for 'x' was created anew in the Atom method
            Assert.AreEqual(y, assignment.Right);
        }

        [TestMethod()]
        public void OrTest()
        {
            new Parser("Code/Parser/OrTest.qk", "OrTest", out var module);
            Assert.AreEqual(1, module.Statements.Count);

            var evaluation = (AST.Evaluation)module.Statements[0];

            var or2 = (AST.FuncCall)evaluation.Expr;
            Assert.AreEqual("__or__", ((AST.NameObj)or2.Func).Name);

            var or1 = (AST.FuncCall)or2.Args[0];
            Assert.AreEqual("__or__", ((AST.NameObj)or1.Func).Name);

            Assert.AreEqual(true, ((AST.ConstBool)or1.Args[0]).Value);
            Assert.AreEqual(false, ((AST.ConstBool)or1.Args[1]).Value);
            Assert.AreEqual(true, ((AST.ConstBool)or2.Args[1]).Value);
        }

        [TestMethod()]
        public void AndTest()
        {
            new Parser("Code/Parser/AndTest.qk", "AndTest", out var module);
            Assert.AreEqual(1, module.Statements.Count);

            var evaluation = (AST.Evaluation)module.Statements[0];

            var and2 = (AST.FuncCall)evaluation.Expr;
            Assert.AreEqual("__and__", ((AST.NameObj)and2.Func).Name);

            var and1 = (AST.FuncCall)and2.Args[0];
            Assert.AreEqual("__and__", ((AST.NameObj)and1.Func).Name);

            Assert.AreEqual(false, ((AST.ConstBool)and1.Args[0]).Value);
            Assert.AreEqual(true, ((AST.ConstBool)and1.Args[1]).Value);
            Assert.AreEqual(false, ((AST.ConstBool)and2.Args[1]).Value);
        }

        [TestMethod()]
        public void NotTest()
        {
            new Parser("Code/Parser/NotTest.qk", "NotTest", out var module);
            Assert.AreEqual(1, module.Statements.Count);

            var evaluation = (AST.Evaluation)module.Statements[0];

            var not1 = (AST.FuncCall)evaluation.Expr;
            Assert.AreEqual("__not__", ((AST.NameObj)not1.Func).Name);

            var not2 = (AST.FuncCall)not1.Args[0];
            Assert.AreEqual("__not__", ((AST.NameObj)not2.Func).Name);

            Assert.AreEqual(true, ((AST.ConstBool)not2.Args[0]).Value);
        }

        [TestMethod()]
        public void CompOp()
        {
            new Parser("Code/Parser/CompOp.qk", "CompOp", out var module);
            Assert.AreEqual(6, module.Statements.Count);

            var evaluation = (AST.Evaluation)module.Statements[0];
            var call = (AST.FuncCall)evaluation.Expr;
            Assert.AreEqual("__lt__", ((AST.NameObj)call.Func).Name);
            Assert.AreEqual(1, ((AST.ConstInt)call.Args[0]).Value);
            Assert.AreEqual(2, ((AST.ConstInt)call.Args[1]).Value);

            evaluation = (AST.Evaluation)module.Statements[1];
            call = (AST.FuncCall)evaluation.Expr;
            Assert.AreEqual("__gt__", ((AST.NameObj)call.Func).Name);
            Assert.AreEqual(2, ((AST.ConstInt)call.Args[0]).Value);
            Assert.AreEqual(1, ((AST.ConstInt)call.Args[1]).Value);

            evaluation = (AST.Evaluation)module.Statements[2];
            call = (AST.FuncCall)evaluation.Expr;
            Assert.AreEqual("__eq__", ((AST.NameObj)call.Func).Name);
            Assert.AreEqual(1, ((AST.ConstInt)call.Args[0]).Value);
            Assert.AreEqual(1, ((AST.ConstInt)call.Args[1]).Value);

            evaluation = (AST.Evaluation)module.Statements[3];
            call = (AST.FuncCall)evaluation.Expr;
            Assert.AreEqual("__le__", ((AST.NameObj)call.Func).Name);
            Assert.AreEqual(1, ((AST.ConstInt)call.Args[0]).Value);
            Assert.AreEqual(2, ((AST.ConstInt)call.Args[1]).Value);

            evaluation = (AST.Evaluation)module.Statements[4];
            call = (AST.FuncCall)evaluation.Expr;
            Assert.AreEqual("__ge__", ((AST.NameObj)call.Func).Name);
            Assert.AreEqual(2, ((AST.ConstInt)call.Args[0]).Value);
            Assert.AreEqual(1, ((AST.ConstInt)call.Args[1]).Value);

            evaluation = (AST.Evaluation)module.Statements[5];
            call = (AST.FuncCall)evaluation.Expr;
            Assert.AreEqual("__ne__", ((AST.NameObj)call.Func).Name);
            Assert.AreEqual(1, ((AST.ConstInt)call.Args[0]).Value);
            Assert.AreEqual(2, ((AST.ConstInt)call.Args[1]).Value);
        }

        [TestMethod()]
        public void Comparison()
        {
            new Parser("Code/Parser/Comparison.qk", "Comparison", out var module);
            Assert.AreEqual(1, module.Statements.Count);

            var evaluation = (AST.Evaluation)module.Statements[0];

            var rightAnd = (AST.FuncCall)evaluation.Expr;
            Assert.AreEqual("__and__", ((AST.NameObj)rightAnd.Func).Name);

            var leftAnd = (AST.FuncCall)rightAnd.Args[0];
            Assert.AreEqual("__and__", ((AST.NameObj)leftAnd.Func).Name);

            var left = (AST.FuncCall)leftAnd.Args[0];
            Assert.AreEqual("__lt__", ((AST.NameObj)left.Func).Name);
            Assert.AreEqual("x", ((AST.NameObj)left.Args[0]).Name);
            Assert.AreEqual("y", ((AST.NameObj)left.Args[1]).Name);

            var center = (AST.FuncCall)leftAnd.Args[1];
            Assert.AreEqual("__le__", ((AST.NameObj)center.Func).Name);
            Assert.AreEqual("y", ((AST.NameObj)center.Args[0]).Name);
            Assert.AreEqual("z", ((AST.NameObj)center.Args[1]).Name);

            var right = (AST.FuncCall)rightAnd.Args[1];
            Assert.AreEqual("__gt__", ((AST.NameObj)right.Func).Name);
            Assert.AreEqual("z", ((AST.NameObj)right.Args[0]).Name);
            Assert.AreEqual("w", ((AST.NameObj)right.Args[1]).Name);
        }

        [TestMethod()]
        public void Expr()
        {
            new Parser("Code/Parser/Expr.qk", "Expr", out var module);
            Assert.AreEqual(1, module.Statements.Count);

            var evaluation = (AST.Evaluation)module.Statements[0];

            var call1 = (AST.FuncCall)evaluation.Expr;
            Assert.AreEqual("__bitor__", ((AST.NameObj)call1.Func).Name);

            var call2 = (AST.FuncCall)call1.Args[0];
            Assert.AreEqual("__bitor__", ((AST.NameObj)call2.Func).Name);

            Assert.AreEqual("a", ((AST.NameObj)call2.Args[0]).Name);
            Assert.AreEqual("b", ((AST.NameObj)call2.Args[1]).Name);
            Assert.AreEqual("c", ((AST.NameObj)call1.Args[1]).Name);
        }

        [TestMethod()]
        public void XorExpr()
        {
            new Parser("Code/Parser/XorExpr.qk", "XorExpr", out var module);
            Assert.AreEqual(1, module.Statements.Count);

            var evaluation = (AST.Evaluation)module.Statements[0];

            var call1 = (AST.FuncCall)evaluation.Expr;
            Assert.AreEqual("__bitxor__", ((AST.NameObj)call1.Func).Name);

            var call2 = (AST.FuncCall)call1.Args[0];
            Assert.AreEqual("__bitxor__", ((AST.NameObj)call2.Func).Name);

            Assert.AreEqual("a", ((AST.NameObj)call2.Args[0]).Name);
            Assert.AreEqual("b", ((AST.NameObj)call2.Args[1]).Name);
            Assert.AreEqual("c", ((AST.NameObj)call1.Args[1]).Name);
        }

        [TestMethod()]
        public void AndExpr()
        {
            new Parser("Code/Parser/AndExpr.qk", "AndExpr", out var module);
            Assert.AreEqual(1, module.Statements.Count);

            var evaluation = (AST.Evaluation)module.Statements[0];

            var call1 = (AST.FuncCall)evaluation.Expr;
            Assert.AreEqual("__bitand__", ((AST.NameObj)call1.Func).Name);

            var call2 = (AST.FuncCall)call1.Args[0];
            Assert.AreEqual("__bitand__", ((AST.NameObj)call2.Func).Name);

            Assert.AreEqual("a", ((AST.NameObj)call2.Args[0]).Name);
            Assert.AreEqual("b", ((AST.NameObj)call2.Args[1]).Name);
            Assert.AreEqual("c", ((AST.NameObj)call1.Args[1]).Name);
        }

        [TestMethod()]
        public void ShiftExpr()
        {
            new Parser("Code/Parser/ShiftExpr.qk", "ShiftExpr", out var module);
            Assert.AreEqual(1, module.Statements.Count);

            var evaluation = (AST.Evaluation)module.Statements[0];

            var call1 = (AST.FuncCall)evaluation.Expr;
            Assert.AreEqual("__rshift__", ((AST.NameObj)call1.Func).Name);

            var call2 = (AST.FuncCall)call1.Args[0];
            Assert.AreEqual("__lshift__", ((AST.NameObj)call2.Func).Name);

            Assert.AreEqual("a", ((AST.NameObj)call2.Args[0]).Name);
            Assert.AreEqual("b", ((AST.NameObj)call2.Args[1]).Name);
            Assert.AreEqual("c", ((AST.NameObj)call1.Args[1]).Name);
        }
        
        [TestMethod()]
        public void ArithExpr()
        {
            new Parser("Code/Parser/ArithExpr.qk", "ArithExpr", out var module);
            Assert.AreEqual(1, module.Statements.Count);

            var evaluation = (AST.Evaluation)module.Statements[0];

            var sub = (AST.FuncCall)evaluation.Expr;
            Assert.AreEqual("__sub__", ((AST.NameObj)sub.Func).Name);

            var add = (AST.FuncCall)sub.Args[0];
            Assert.AreEqual("__add__", ((AST.NameObj)add.Func).Name);

            Assert.AreEqual("a", ((AST.NameObj)add.Args[0]).Name);
            Assert.AreEqual("b", ((AST.NameObj)add.Args[1]).Name);
            Assert.AreEqual("c", ((AST.NameObj)sub.Args[1]).Name);
        }

        [TestMethod()]
        public void Term()
        {
            new Parser("Code/Parser/Term.qk", "Term", out var module);
            Assert.AreEqual(1, module.Statements.Count);

            var evaluation = (AST.Evaluation)module.Statements[0];

            var fdiv = (AST.FuncCall)evaluation.Expr;
            Assert.AreEqual("__floordiv__", ((AST.NameObj)fdiv.Func).Name);

            var mod = (AST.FuncCall)fdiv.Args[0];
            Assert.AreEqual("__mod__", ((AST.NameObj)mod.Func).Name);

            var div = (AST.FuncCall)mod.Args[0];
            Assert.AreEqual("__truediv__", ((AST.NameObj)div.Func).Name);

            var mul = (AST.FuncCall)div.Args[0];
            Assert.AreEqual("__mul__", ((AST.NameObj)mul.Func).Name);

            Assert.AreEqual("a", ((AST.NameObj)mul.Args[0]).Name);
            Assert.AreEqual("b", ((AST.NameObj)mul.Args[1]).Name);
            Assert.AreEqual("c", ((AST.NameObj)div.Args[1]).Name);
            Assert.AreEqual("d", ((AST.NameObj)mod.Args[1]).Name);
            Assert.AreEqual("e", ((AST.NameObj)fdiv.Args[1]).Name);
        }

        [TestMethod()]
        public void Factor()
        {
            new Parser("Code/Parser/Factor.qk", "Factor", out var module);
            Assert.AreEqual(3, module.Statements.Count);

            var evaluation = (AST.Evaluation)module.Statements[0];
            var call = (AST.FuncCall)evaluation.Expr;
            Assert.AreEqual("__pos__", ((AST.NameObj)call.Func).Name);
            Assert.AreEqual("x", ((AST.NameObj)call.Args[0]).Name);

            evaluation = (AST.Evaluation)module.Statements[1];
            call = (AST.FuncCall)evaluation.Expr;
            Assert.AreEqual("__neg__", ((AST.NameObj)call.Func).Name);
            Assert.AreEqual("y", ((AST.NameObj)call.Args[0]).Name);

            evaluation = (AST.Evaluation)module.Statements[2];
            call = (AST.FuncCall)evaluation.Expr;
            Assert.AreEqual("__invert__", ((AST.NameObj)call.Func).Name);
            Assert.AreEqual("z", ((AST.NameObj)call.Args[0]).Name);
        }

        [TestMethod()]
        public void Power()
        {
            new Parser("Code/Parser/Power.qk", "Power", out var module);
            Assert.AreEqual(1, module.Statements.Count);

            var evaluation = (AST.Evaluation)module.Statements[0];

            var pow = (AST.FuncCall)evaluation.Expr;
            Assert.AreEqual("__pow__", ((AST.NameObj)pow.Func).Name);

            Assert.AreEqual("x", ((AST.NameObj)pow.Args[0]).Name);
            Assert.AreEqual(3, ((AST.ConstInt)pow.Args[1]).Value);
        }

        [TestMethod()]
        public void AtomSimple()
        {
            new Parser("Code/Parser/AtomSimple.qk", "AtomSimple", out var module);
            Assert.AreEqual(6, module.Statements.Count);

            var evaluation = (AST.Evaluation)module.Statements[0];
            Assert.AreEqual("some_id", ((AST.NameObj)evaluation.Expr).Name);

            evaluation = (AST.Evaluation)module.Statements[1];
            Assert.AreEqual(10, ((AST.ConstInt)evaluation.Expr).Value);

            evaluation = (AST.Evaluation)module.Statements[2];
            Assert.AreEqual(1.0, ((AST.ConstFloat)evaluation.Expr).Value);

            evaluation = (AST.Evaluation)module.Statements[3];
            Assert.AreEqual(true, ((AST.ConstBool)evaluation.Expr).Value);

            evaluation = (AST.Evaluation)module.Statements[4];
            Assert.AreEqual(false, ((AST.ConstBool)evaluation.Expr).Value);

            evaluation = (AST.Evaluation)module.Statements[5];
            Assert.AreEqual(true, evaluation.Expr is AST.Tuple);
        }

        [TestMethod()]
        public void AtomParenthesis()
        {
            new Parser("Code/Parser/AtomParenthesis.qk", "AtomParenthesis", out var module);
            Assert.AreEqual(1, module.Statements.Count);

            var evaluation = (AST.Evaluation)module.Statements[0];

            var mul = (AST.FuncCall)evaluation.Expr;
            Assert.AreEqual("__mul__", ((AST.NameObj)mul.Func).Name);

            var add = (AST.FuncCall)mul.Args[0];
            Assert.AreEqual("a", ((AST.NameObj)add.Args[0]).Name);
            Assert.AreEqual("b", ((AST.NameObj)add.Args[1]).Name);

            var sub = (AST.FuncCall)mul.Args[1];
            Assert.AreEqual("c", ((AST.NameObj)sub.Args[0]).Name);
            Assert.AreEqual("d", ((AST.NameObj)sub.Args[1]).Name);
        }

        [TestMethod()]
        public void Trailer()
        {
            new Parser("Code/Parser/Trailer.qk", "Trailer", out var module);
            Assert.AreEqual(3, module.Statements.Count);

            var evaluation = (AST.Evaluation)module.Statements[0];
            var call = (AST.FuncCall)evaluation.Expr;
            Assert.AreEqual("f", ((AST.NameObj)call.Func).Name);
            Assert.AreEqual(0, call.Args.Count);

            evaluation = (AST.Evaluation)module.Statements[1];
            call = (AST.FuncCall)evaluation.Expr;
            Assert.AreEqual("g", ((AST.NameObj)call.Func).Name);
            Assert.AreEqual(1, call.Args.Count);
            Assert.AreEqual("x", ((AST.NameObj)call.Args[0]).Name);

            evaluation = (AST.Evaluation)module.Statements[2];
            call = (AST.FuncCall)evaluation.Expr;
            Assert.AreEqual("h", ((AST.NameObj)call.Func).Name);
            Assert.AreEqual(2, call.Args.Count);
            Assert.AreEqual("y", ((AST.NameObj)call.Args[0]).Name);
            Assert.AreEqual(1, ((AST.ConstInt)call.Args[1]).Value);
        }

        [TestMethod()]
        public void FuncDef()
        {
            new Parser("Code/Parser/FuncDef.qk", "FuncDef", out var module);
            Assert.AreEqual(7, module.Statements.Count);

            var defA = (AST.FuncDef)module.Statements[0];
            var A = defA.Func;
            Assert.AreEqual(defA, A.Def);
            Assert.AreEqual("a", A.Name);
            Assert.AreEqual(0, A.Parameters.Count);
            Assert.AreEqual(0, A.Statements.Count);
            Assert.AreEqual(null, A.RetType);

            var defB = (AST.FuncDef)module.Statements[1];
            var B = defB.Func;
            Assert.AreEqual(defB, B.Def);
            Assert.AreEqual("b", B.Name);
            Assert.AreEqual(0, B.Parameters.Count);
            Assert.AreEqual(1, B.Statements.Count);
            Assert.AreEqual(null, B.RetType);

            var defBC = (AST.FuncDef)B.Statements[0];
            var BC = defBC.Func;
            Assert.AreEqual(defBC, BC.Def);
            Assert.AreEqual("c", BC.Name);
            Assert.AreEqual(0, BC.Parameters.Count);
            Assert.AreEqual(0, BC.Statements.Count);
            Assert.AreEqual(null, BC.RetType);

            var defC = (AST.FuncDef)module.Statements[2];
            var C = defC.Func;
            Assert.AreEqual(defC, C.Def);
            Assert.AreEqual("c", C.Name);
            Assert.AreEqual(1, C.Parameters.Count);
            Assert.AreEqual(0, C.Statements.Count);
            var param = C.Parameters[0];
            Assert.AreEqual("x", param.Name);
            Assert.AreEqual(null, param.Type);
            Assert.AreEqual(null, C.RetType);

            var defD = (AST.FuncDef)module.Statements[3];
            var D = defD.Func;
            Assert.AreEqual(defD, D.Def);
            Assert.AreEqual("d", D.Name);
            Assert.AreEqual(2, D.Parameters.Count);
            Assert.AreEqual(0, D.Statements.Count);
            param = D.Parameters[0];
            Assert.AreEqual("x", param.Name);
            Assert.AreEqual(null, param.Type);
            param = D.Parameters[1];
            Assert.AreEqual("y", param.Name);
            Assert.AreEqual(null, param.Type);
            Assert.AreEqual(null, D.RetType);

            var defA2 = (AST.FuncDef)module.Statements[4];
            var A2 = defA2.Func;
            Assert.AreEqual(defA2, A2.Def);
            Assert.AreEqual("a", A2.Name);
            Assert.AreEqual(1, A2.Parameters.Count);
            Assert.AreEqual(0, A2.Statements.Count);
            param = A2.Parameters[0];
            Assert.AreEqual("x", param.Name);
            Assert.AreEqual(null, param.Type);
            Assert.AreEqual(null, A2.RetType);

            var defE = (AST.FuncDef)module.Statements[5];
            var E = defE.Func;
            Assert.AreEqual(defE, E.Def);
            Assert.AreEqual("e", E.Name);
            Assert.AreEqual(0, E.Parameters.Count);
            Assert.AreEqual(0, E.Statements.Count);
            Assert.AreEqual("Int", ((AST.NameObj)E.RetType).Name);

            var defF = (AST.FuncDef)module.Statements[6];
            var F = defF.Func;
            Assert.AreEqual(defF, F.Def);
            Assert.AreEqual("f", F.Name);
            Assert.AreEqual(1, F.Parameters.Count);
            Assert.AreEqual(0, F.Statements.Count);
            param = F.Parameters[0];
            Assert.AreEqual("x", param.Name);
            Assert.AreEqual("Int", ((AST.NameObj)param.Type).Name);
            Assert.AreEqual(null, F.RetType);
        }

        [TestMethod()]
        public void Suite()
        {
            new Parser("Code/Parser/Suite.qk", "Suite", out var module);
            Assert.AreEqual(3, module.Statements.Count);

            var defA = (AST.FuncDef)module.Statements[0];
            var a = defA.Func;
            Assert.AreEqual(0, a.Statements.Count);

            var defB = (AST.FuncDef)module.Statements[1];
            var b = defB.Func;
            Assert.IsInstanceOfType(b.Statements[0], typeof(AST.Assignment));
            Assert.IsInstanceOfType(b.Statements[1], typeof(AST.Assignment));

            var defC = (AST.FuncDef)module.Statements[2];
            var c = defC.Func;
            var defD = (AST.FuncDef)c.Statements[0];
            Assert.IsInstanceOfType(c.Statements[1], typeof(AST.Assignment));
            var d = defD.Func;
            Assert.IsInstanceOfType(d.Statements[0], typeof(AST.Assignment));
        }

        [TestMethod()]
        public void Return()
        {
            new Parser("Code/Parser/Return.qk", "Return", out var module);
            Assert.AreEqual(3, module.Statements.Count);

            var ret1 = (AST.ReturnStmnt)module.Statements[0];
            Assert.AreEqual(0, ret1.Values.Count);

            var ret2 = (AST.ReturnStmnt)module.Statements[1];
            Assert.AreEqual(1, ret2.Values.Count);
            Assert.AreEqual(1f, ((AST.ConstFloat)ret2.Values[0]).Value);

            var ret3 = (AST.ReturnStmnt)module.Statements[2];
            Assert.AreEqual(3, ret3.Values.Count);
            Assert.AreEqual(1, ((AST.ConstInt)ret3.Values[0]).Value);
            Assert.AreEqual(2, ((AST.ConstInt)ret3.Values[1]).Value);
            Assert.AreEqual(true, ((AST.ConstBool)ret3.Values[2]).Value);
        }

        [TestMethod()]
        public void If() {
            new Parser("Code/Parser/If.qk", "If", out var module);
            //Assert.AreEqual(3, module.Statements.Count);

            var if0 = (AST.IfStmnt)module.Statements[0];
            Assert.AreEqual("x", ((AST.NameObj)if0.Condition).Name);
            Assert.AreEqual(0, if0.Then.Count);
            Assert.AreEqual(0, if0.Else.Count);

            var if1 = (AST.IfStmnt)module.Statements[1];
            Assert.AreEqual("x", ((AST.NameObj)if1.Condition).Name);
            Assert.AreEqual(1, if1.Then.Count);
            Assert.AreEqual(1, if1.Else.Count);
            var elif1 = (AST.IfStmnt)if1.Else[0];
            Assert.AreEqual("y", ((AST.NameObj)elif1.Condition).Name);
            Assert.AreEqual(1, elif1.Then.Count);
            Assert.AreEqual(0, elif1.Else.Count);

            var if2 = (AST.IfStmnt)module.Statements[2];
            Assert.AreEqual("x", ((AST.NameObj)if2.Condition).Name);
            Assert.AreEqual(0, if2.Then.Count);
            Assert.AreEqual(1, if2.Else.Count);
            var elif2Y = (AST.IfStmnt)if2.Else[0];
            Assert.AreEqual("y", ((AST.NameObj)elif2Y.Condition).Name);
            Assert.AreEqual(0, elif2Y.Then.Count);
            Assert.AreEqual(1, elif2Y.Else.Count);
            var elif2Z = (AST.IfStmnt)elif2Y.Else[0];
            Assert.AreEqual("z", ((AST.NameObj)elif2Z.Condition).Name);
            Assert.AreEqual(0, elif2Z.Then.Count);
            Assert.AreEqual(1, elif2Z.Else.Count);

            var if3 = (AST.IfStmnt)module.Statements[3];
            Assert.AreEqual("x", ((AST.NameObj)if3.Condition).Name);
            Assert.AreEqual(0, if3.Then.Count);
            Assert.AreEqual(0, if3.Else.Count);
        }

        [TestMethod()]
        public void If2() {
            new Parser("Code/Parser/If2.qk", "If2", out var module);
            Assert.AreEqual(1, module.Statements.Count);

            var def = (AST.FuncDef)module.Statements[0];
            var f = def.Func;
            Assert.AreEqual(2, f.Statements.Count);

            var if1 = (AST.IfStmnt)f.Statements[0];
            Assert.AreEqual(1, if1.Then.Count);
            Assert.AreEqual(0, if1.Else.Count);

            var if2 = (AST.IfStmnt)if1.Then[0];
            Assert.AreEqual(1, if2.Then.Count);
            Assert.AreEqual(0, if2.Else.Count);

            Assert.IsInstanceOfType(if2.Then[0], typeof(AST.ReturnStmnt));

            Assert.IsInstanceOfType(f.Statements[1], typeof(AST.ReturnStmnt));
        }
    }
}