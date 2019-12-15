using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quirk.AST;

namespace Quirk.Tests
{
    [TestClass()]
    public class NameVisitorTests
    {
        [TestMethod()]
        public void Params()
        {
            new Parser("Code/NameVisitor/Params.qk", "Params", out var module);
            new Visitors.NameVisitor(module);

            var def = (FuncDef)module.Statements[0];
            var assignment = (Assignment)def.Func.Statements[0];
            Assert.AreEqual(def.Func.Parameters[0], assignment.Left);
            assignment = (Assignment)def.Func.Statements[1];
            Assert.AreEqual(def.Func.Parameters[1], assignment.Left);
            assignment = (Assignment)def.Func.Statements[2];
            Assert.AreEqual(def.Func.Parameters[2], assignment.Left);
        }

        [TestMethod()]
        public void RetType()
        {
            new Parser("Code/NameVisitor/RetType.qk", "RetType", out var module);
            new Visitors.NameVisitor(module);

            var def = (FuncDef)module.Statements[0];
            Assert.AreEqual(Helpers.BuiltIns.Int, def.Func.RetType);
        }

        [TestMethod()]
        public void Overload()
        {
            new Parser("Code/NameVisitor/Overload.qk", "Overload", out var module);
            new Visitors.NameVisitor(module);

            var def1 = (FuncDef)module.Statements[0];
            var def2 = (FuncDef)module.Statements[1];
            var eval = (Evaluation)module.Statements[2];

            var call = (FuncCall)eval.Expr;
            var overload = (Overload)call.Func;

            Assert.AreEqual("f", overload.Name);
            Assert.AreEqual(2, overload.Funcs.Count);
            Assert.AreEqual(def1.Func, overload.Funcs[0]);
            Assert.AreEqual(def2.Func, overload.Funcs[1]);

            var def3 = (FuncDef)def2.Func.Statements[0];
            var eval2 = (Evaluation)def2.Func.Statements[1];
            var call2 = (FuncCall)eval2.Expr;
            var overload2 = (Overload)call2.Func;

            Assert.AreEqual("f", overload2.Name);
            Assert.AreEqual(3, overload2.Funcs.Count);
            Assert.AreEqual(def1.Func, overload2.Funcs[0]);
            Assert.AreEqual(def2.Func, overload2.Funcs[1]);
            Assert.AreEqual(def3.Func, overload2.Funcs[2]);
        }

        [TestMethod()]
        public void Assignment()
        {
            new Parser("Code/NameVisitor/Assignment.qk", "Assignment", out var module);
            new Visitors.NameVisitor(module);

            var A = (Assignment)module.Statements[0];
            var x = (Variable)A.Left;
            Assert.AreEqual("x", x.Name);
            var zero = (ConstInt)A.Right;
            Assert.AreEqual(10, zero.Value);

            A = (Assignment)module.Statements[1];
            var add = (FuncCall)A.Left;
            Assert.AreEqual("__add__", ((Overload)add.Func).Name);
            Assert.AreEqual(x, add.Args[0]);

            var sub = (FuncCall)A.Right;
            Assert.AreEqual("__sub__", ((Overload)sub.Func).Name);
            Assert.AreEqual(x, sub.Args[1]);
        }

        [TestMethod()]
        public void Evaluation()
        {
            new Parser("Code/NameVisitor/Evaluation.qk", "Evaluation", out var module);
            new Visitors.NameVisitor(module);

            var evaluation = (Evaluation)module.Statements[1];
            var x = (Variable)evaluation.Expr;
            Assert.AreEqual("x", x.Name);
        }

        [TestMethod()]
        public void FuncCall()
        {
            new Parser("Code/NameVisitor/FuncCall.qk", "FuncCall", out var module);
            new Visitors.NameVisitor(module);

            var def = (FuncDef)module.Statements[0];
            var f = (Function)def.Func;

            var assignment = (Assignment)module.Statements[1];
            var a = (Variable)assignment.Left;

            assignment = (Assignment)module.Statements[2];
            var b = (Variable)assignment.Left;

            var evaluation = (Evaluation)module.Statements[3];
            var call = (FuncCall)evaluation.Expr;

            Assert.AreEqual(f, ((Overload)call.Func).Funcs[0]);
            Assert.AreEqual(a, (Variable)call.Args[0]);
            Assert.AreEqual(b, (Variable)call.Args[1]);
        }

        [TestMethod()]
        public void Return()
        {
            new Parser("Code/NameVisitor/Return.qk", "Return", out var module);
            new Visitors.NameVisitor(module);

            var assignmentY = (Assignment)module.Statements[0];
            var y = (Variable)assignmentY.Left;

            var assignmentX = (Assignment)module.Statements[1];
            var x = (Variable)assignmentX.Left;

            var returnStmnt = (ReturnStmnt)module.Statements[2];
            Assert.AreEqual(2, returnStmnt.Values.Count);
            Assert.AreEqual(x, returnStmnt.Values[0]);
            Assert.AreEqual(y, returnStmnt.Values[1]);
        }

        [TestMethod()]
        public void NestedFuncs()
        {
            void AssertCall(Function func, ProgObj statement)
            {
                var eval = (Evaluation)statement;
                var call = (FuncCall)eval.Expr;
                var overload = (Overload)call.Func;
                foreach (var f in overload.Funcs) {
                    if (f == func) {
                        return;
                    }
                }
                throw new Exception("Failed");
            }

            new Parser("Code/NameVisitor/NestedFuncs.qk", "NestedFuncs", out var module);
            new Visitors.NameVisitor(module);

            var x1 = ((Assignment)module.Statements[0]).Left;
            var f1 = ((FuncDef)module.Statements[1]).Func;

            var x2 = ((Assignment)f1.Statements[0]).Left;
            var f2 = ((FuncDef)f1.Statements[1]).Func;

            var x3 = ((Assignment)f2.Statements[0]).Left;
            var f3 = ((FuncDef)f2.Statements[1]).Func;

            Assert.AreEqual(x1, ((Assignment)f3.Statements[0]).Left);
            Assert.AreEqual(x2, ((Assignment)f3.Statements[1]).Left);
            Assert.AreEqual(x3, ((Assignment)f3.Statements[2]).Left);

            AssertCall(f1, f3.Statements[3]);
            AssertCall(f2, f3.Statements[4]);
            AssertCall(f3, f3.Statements[5]);

            AssertCall(f3, f2.Statements[2]);
        }

        [TestMethod()]
        public void If()
        {
            new Parser("Code/NameVisitor/If.qk", "If", out var module);
            new Visitors.NameVisitor(module);

            var A = (Assignment)module.Statements[0];
            var x = (Variable)A.Left;

            var IF = (IfStmnt)module.Statements[1];
            Assert.AreEqual(x, IF.Condition);

            var A0 = (Assignment)IF.Then[0];
            Assert.AreEqual(x, A0.Right);

            var ELIF = (IfStmnt)IF.Else[0];
            Assert.AreEqual(x, ELIF.Condition);

            var A1 = (Assignment)ELIF.Then[0];
            Assert.AreEqual(x, A1.Right);

            var A2 = (Assignment)ELIF.Else[0];
            Assert.AreEqual(x, A2.Right);
        }

        [TestMethod()]
        public void DeferedDef1() {
            new Parser("Code/NameVisitor/DeferedDef1.qk", "DeferedDef1", out var module);
            new Visitors.NameVisitor(module);

            var def0 = (FuncDef)module.Statements[0];
            var first = def0.Func;
            var firstRet = (ReturnStmnt)first.Statements[0];

            var def1 = (FuncDef)module.Statements[1];
            var second = def1.Func;
            var secondRet = (ReturnStmnt)second.Statements[0];

            var call = (FuncCall)firstRet.Values[0];
            var overload = (Overload)call.Func;
            Assert.AreEqual(second, overload.Funcs[0]);

            call = (FuncCall)secondRet.Values[0];
            overload = (Overload)call.Func;
            Assert.AreEqual(first, overload.Funcs[0]);
        }

        [TestMethod()]
        public void DeferedDef2() {
            new Parser("Code/NameVisitor/DeferedDef2.qk", "DeferedDef2", out var module);
            new Visitors.NameVisitor(module);

            var firstDef = (FuncDef)module.Statements[0];
            var first = firstDef.Func;
            var firstRet = (ReturnStmnt)first.Statements[0];
            var firstRetCall = (FuncCall)firstRet.Values[0];

            var secondDef = (FuncDef)module.Statements[1];
            var second = secondDef.Func;
            var secondRet = (ReturnStmnt)second.Statements[0];
            var secondRetCall = (FuncCall)secondRet.Values[0];

            var thirdDef = (FuncDef)module.Statements[2];
            var third = thirdDef.Func;
            var thirdRet = (ReturnStmnt)third.Statements[0];
            var thirdRetCall = (FuncCall)thirdRet.Values[0];

            var fourthDef = (FuncDef)module.Statements[4];
            var fourth = fourthDef.Func;
            var fourthRet = (ReturnStmnt)fourth.Statements[0];
            var fourthRetCall = (FuncCall)fourthRet.Values[0];

            Assert.AreEqual(second, ((Overload)firstRetCall.Func).Funcs[0]);
            Assert.AreEqual(first, ((Overload)secondRetCall.Func).Funcs[0]);
            Assert.AreEqual(fourth, ((Overload)thirdRetCall.Func).Funcs[0]);
            Assert.AreEqual(third, ((Overload)fourthRetCall.Func).Funcs[0]);

            var eval = (Evaluation)module.Statements[3];
            var call = (FuncCall)eval.Expr;
            Assert.AreEqual(first, ((Overload)call.Func).Funcs[0]);
        }

        [TestMethod()]
        public void BuiltIns()
        {
            new Parser("Code/NameVisitor/BuiltIns.qk", "BuiltIns", out var module);
            new Visitors.NameVisitor(module);

            var E = (Evaluation)module.Statements[0];
            Assert.AreEqual(Helpers.BuiltIns.Int, E.Expr);

            E = (Evaluation)module.Statements[1];
            Assert.AreEqual(Helpers.BuiltIns.Float, E.Expr);

            E = (Evaluation)module.Statements[2];
            Assert.AreEqual(Helpers.BuiltIns.Bool, E.Expr);

            E = (Evaluation)module.Statements[3];
            Assert.AreEqual("print", ((Overload)E.Expr).Name);

            E = (Evaluation)module.Statements[4];
            var call = (FuncCall)E.Expr;
            Assert.AreEqual("__add__", ((Overload)call.Func).Name);
        }
    }
}