using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quirk.Helpers;

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

            var def = (AST.FuncDef)module.Statements[0];
            var assignment = (AST.Assignment)def.Func.Statements[0];
            Assert.AreEqual(def.Func.Parameters[0], assignment.Left);
            assignment = (AST.Assignment)def.Func.Statements[1];
            Assert.AreEqual(def.Func.Parameters[1], assignment.Left);
            assignment = (AST.Assignment)def.Func.Statements[2];
            Assert.AreEqual(def.Func.Parameters[2], assignment.Left);
        }

        [TestMethod()]
        public void RetType()
        {
            new Parser("Code/NameVisitor/RetType.qk", "RetType", out var module);
            new Visitors.NameVisitor(module);

            var def = (AST.FuncDef)module.Statements[0];
            Assert.AreEqual(Helpers.BuiltIns.Int, def.Func.RetType);
        }

        [TestMethod()]
        public void Overload()
        {
            new Parser("Code/NameVisitor/Overload.qk", "Overload", out var module);
            new Visitors.NameVisitor(module);

            var def1 = (AST.FuncDef)module.Statements[0];
            var def2 = (AST.FuncDef)module.Statements[1];
            var eval = (AST.Evaluation)module.Statements[2];

            var call = (AST.FuncCall)eval.Expr;
            var overload = (AST.Overload)call.Func;

            Assert.AreEqual("f", overload.Name);
            Assert.AreEqual(2, overload.Funcs.Count);
            Assert.AreEqual(def1.Func, overload.Funcs[0]);
            Assert.AreEqual(def2.Func, overload.Funcs[1]);

            var def3 = (AST.FuncDef)def2.Func.Statements[0];
            var eval2 = (AST.Evaluation)def2.Func.Statements[1];
            var call2 = (AST.FuncCall)eval2.Expr;
            var overload2 = (AST.Overload)call2.Func;

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

            var assignment = (AST.Assignment)module.Statements[1];
            var x = (AST.Variable)assignment.Left;
            Assert.AreEqual("x", x.Name);
            Assert.AreEqual(null, x.Type);

            assignment = (AST.Assignment)module.Statements[2];
            var call = (AST.FuncCall)assignment.Left;
            Assert.AreEqual("f", ((AST.Overload)call.Func).Name);
        }


        [TestMethod()]
        public void Evaluation()
        {
            new Parser("Code/NameVisitor/Evaluation.qk", "Evaluation", out var module);
            new Visitors.NameVisitor(module);

            var evaluation = (AST.Evaluation)module.Statements[1];
            var x = (AST.Variable)evaluation.Expr;
            Assert.AreEqual("x", x.Name);
        }

        [TestMethod()]
        public void FuncCall()
        {
            new Parser("Code/NameVisitor/FuncCall.qk", "FuncCall", out var module);
            new Visitors.NameVisitor(module);

            var def = (AST.FuncDef)module.Statements[0];
            var f = (AST.Function)def.Func;

            var assignment = (AST.Assignment)module.Statements[1];
            var a = (AST.Variable)assignment.Left;

            assignment = (AST.Assignment)module.Statements[2];
            var b = (AST.Variable)assignment.Left;

            var evaluation = (AST.Evaluation)module.Statements[3];
            var call = (AST.FuncCall)evaluation.Expr;

            Assert.AreEqual(f, ((AST.Overload)call.Func).Funcs[0]);
            Assert.AreEqual(a, (AST.Variable)call.Args[0]);
            Assert.AreEqual(b, (AST.Variable)call.Args[1]);
        }

        [TestMethod()]
        public void Return()
        {
            new Parser("Code/NameVisitor/Return.qk", "Return", out var module);
            new Visitors.NameVisitor(module);

            var assignmentY = (AST.Assignment)module.Statements[0];
            var y = (AST.Variable)assignmentY.Left;

            var assignmentX = (AST.Assignment)module.Statements[1];
            var x = (AST.Variable)assignmentX.Left;

            var returnStmnt = (AST.ReturnStmnt)module.Statements[2];
            Assert.AreEqual(2, returnStmnt.Values.Count);
            Assert.AreEqual(x, returnStmnt.Values[0]);
            Assert.AreEqual(y, returnStmnt.Values[1]);
        }

        [TestMethod()]
        public void NestedFuncs()
        {
            void AssertCall(AST.Function func, AST.ProgObj statement)
            {
                var eval = (AST.Evaluation)statement;
                var call = (AST.FuncCall)eval.Expr;
                var overload = (AST.Overload)call.Func;
                foreach (var f in overload.Funcs) {
                    if (f == func) {
                        return;
                    }
                }
                throw new Exception("Failed");
            }

            new Parser("Code/NameVisitor/NestedFuncs.qk", "NestedFuncs", out var module);
            new Visitors.NameVisitor(module);

            var x1 = ((AST.Assignment)module.Statements[0]).Left;
            var f1 = ((AST.FuncDef)module.Statements[1]).Func;

            var x2 = ((AST.Assignment)f1.Statements[0]).Left;
            var f2 = ((AST.FuncDef)f1.Statements[1]).Func;

            var x3 = ((AST.Assignment)f2.Statements[0]).Left;
            var f3 = ((AST.FuncDef)f2.Statements[1]).Func;

            Assert.AreEqual(x1, ((AST.Assignment)f3.Statements[0]).Left);
            Assert.AreEqual(x2, ((AST.Assignment)f3.Statements[1]).Left);
            Assert.AreEqual(x3, ((AST.Assignment)f3.Statements[2]).Left);

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

            var assign = (AST.Assignment)module.Statements[0];
            var x = (AST.Variable)assign.Left;

            var ifStmnt = (AST.IfStmnt)module.Statements[1];
            Assert.AreEqual(x, ifStmnt.Condition);

            var assign0 = (AST.Assignment)ifStmnt.Then[0];
            Assert.AreEqual(x, assign0.Right);

            var elif = (AST.IfStmnt)ifStmnt.Else[0];
            Assert.AreEqual(x, elif.Condition);

            var assign1 = (AST.Assignment)elif.Then[0];
            Assert.AreEqual(x, assign1.Right);

            var assign2 = (AST.Assignment)elif.Else[0];
            Assert.AreEqual(x, assign2.Right);
        }

        [TestMethod()]
        public void DeferedDef()
        {
            new Parser("Code/NameVisitor/DeferedDef.qk", "DeferedDef", out var module);
            new Visitors.NameVisitor(module);
        }

        [TestMethod()]
        public void BuiltIns()
        {
            new Parser("Code/NameVisitor/BuiltIns.qk", "BuiltIns", out var module);
            new Visitors.NameVisitor(module);
        }
    }
}