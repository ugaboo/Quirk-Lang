using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Quirk.Tests
{
    [TestClass()]
    public class ParserTests
    {
        [TestMethod()]
        public void Test()
        {
            var parser = new Parser("Code/Parser/Test.qk", "Test", out var module);
            module.Accept(new Visitors.NameFinder());
            module.Accept(new Visitors.CodeGenerator());

            var process = new Process();
            process.StartInfo = new ProcessStartInfo() {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "clang",
                Arguments = "Test.bc -o test.exe",
            };
            process.Start();
        }

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

            var s = module.Statements;
            Assert.AreEqual(4, s.Count);
        }

        [TestMethod()]
        public void ExprStmnt()
        {
            new Parser("Code/Parser/ExprStmnt.qk", "ExprStmnt", out var module);
            Assert.AreEqual(5, module.Statements.Count);

            var evaluation = module.Statements[0] as AST.Evaluation;
            var two = evaluation.Expr as AST.ConstInt;
            Assert.AreEqual(2, two.Value);

            var assignment = module.Statements[1] as AST.Assignment;
            var x = assignment.Left as AST.NamedObj;
            Assert.AreEqual("x", x.Name);
            var zero = assignment.Right as AST.ConstInt;
            Assert.AreEqual(0, zero.Value);

            assignment = module.Statements[2] as AST.Assignment;
            var z = assignment.Left as AST.NamedObj;
            Assert.AreEqual("z", z.Name);
            var one = assignment.Right as AST.ConstInt;
            Assert.AreEqual(1, one.Value);

            assignment = module.Statements[3] as AST.Assignment;
            var y = assignment.Left as AST.NamedObj;
            Assert.AreEqual("y", y.Name);
            Assert.AreEqual(z, assignment.Right);

            assignment = module.Statements[4] as AST.Assignment;
            x = assignment.Left as AST.NamedObj;        // NamedObj was created anew in the Atom method
            Assert.AreEqual("x", x.Name);
            Assert.AreEqual(y, assignment.Right);
        }

        [TestMethod()]
        public void OrTest()
        {
            new Parser("Code/Parser/OrTest.qk", "OrTest", out var module);
            Assert.AreEqual(1, module.Statements.Count);

            var evaluation = module.Statements[0] as AST.Evaluation;

            var expr1 = evaluation.Expr as AST.BinaryExpression;
            Assert.AreEqual(AST.BinaryExpressionType.Or, expr1.Type);

            var expr2 = expr1.Left as AST.BinaryExpression;
            Assert.AreEqual(AST.BinaryExpressionType.Or, expr2.Type);

            var const1 = expr2.Left as AST.ConstBool;
            Assert.AreEqual(true, const1.Value);

            var const2 = expr2.Right as AST.ConstBool;
            Assert.AreEqual(false, const2.Value);

            var const3 = expr1.Right as AST.ConstBool;
            Assert.AreEqual(true, const3.Value);
        }

        [TestMethod()]
        public void AndTest()
        {
            new Parser("Code/Parser/AndTest.qk", "AndTest", out var module);
            Assert.AreEqual(1, module.Statements.Count);

            var evaluation = module.Statements[0] as AST.Evaluation;

            var expr1 = evaluation.Expr as AST.BinaryExpression;
            Assert.AreEqual(AST.BinaryExpressionType.And, expr1.Type);

            var expr2 = expr1.Left as AST.BinaryExpression;
            Assert.AreEqual(AST.BinaryExpressionType.And, expr2.Type);

            var const1 = expr2.Left as AST.ConstBool;
            Assert.AreEqual(false, const1.Value);

            var const2 = expr2.Right as AST.ConstBool;
            Assert.AreEqual(true, const2.Value);

            var const3 = expr1.Right as AST.ConstBool;
            Assert.AreEqual(false, const3.Value);
        }

        [TestMethod()]
        public void NotTest()
        {
            new Parser("Code/Parser/NotTest.qk", "NotTest", out var module);
            Assert.AreEqual(1, module.Statements.Count);

            var evaluation = module.Statements[0] as AST.Evaluation;

            var expr1 = evaluation.Expr as AST.UnaryExpression;
            Assert.AreEqual(AST.UnaryExpressionType.Not, expr1.Type);

            var expr2 = expr1.Expr as AST.UnaryExpression;
            Assert.AreEqual(AST.UnaryExpressionType.Not, expr2.Type);

            var const1 = expr2.Expr as AST.ConstBool;
            Assert.AreEqual(true, const1.Value);
        }

        [TestMethod()]
        public void Functions()
        {
            new Parser("Code/Parser/Functions.qk", "Functions", out var module);
        }
    }
}