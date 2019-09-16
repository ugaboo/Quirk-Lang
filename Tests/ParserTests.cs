using System.Diagnostics;
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
            Assert.AreEqual("x", ((AST.NamedObj)assignment.Left).Name);
            Assert.AreEqual(0, ((AST.ConstInt)assignment.Right).Value);

            assignment = (AST.Assignment)module.Statements[2];
            Assert.AreEqual("z", ((AST.NamedObj)assignment.Left).Name);
            Assert.AreEqual(1, ((AST.ConstInt)assignment.Right).Value);

            var z = assignment.Left;

            assignment = (AST.Assignment)module.Statements[3];
            Assert.AreEqual("y", ((AST.NamedObj)assignment.Left).Name);
            Assert.AreEqual(z, assignment.Right);

            var y = assignment.Left;

            assignment = (AST.Assignment)module.Statements[4];
            Assert.AreEqual("x", ((AST.NamedObj)assignment.Left).Name);       // NamedObj for 'x' was created anew in the Atom method
            Assert.AreEqual(y, assignment.Right);
        }

        [TestMethod()]
        public void OrTest()
        {
            new Parser("Code/Parser/OrTest.qk", "OrTest", out var module);
            Assert.AreEqual(1, module.Statements.Count);

            var evaluation = (AST.Evaluation)module.Statements[0];

            var expr1 = (AST.BinaryExpression)evaluation.Expr;
            Assert.AreEqual(AST.BinaryExpressionType.Or, expr1.Type);

            var expr2 = (AST.BinaryExpression)expr1.Left;
            Assert.AreEqual(AST.BinaryExpressionType.Or, expr2.Type);

            Assert.AreEqual(true, ((AST.ConstBool)expr2.Left).Value);
            Assert.AreEqual(false, ((AST.ConstBool)expr2.Right).Value);
            Assert.AreEqual(true, ((AST.ConstBool)expr1.Right).Value);
        }

        [TestMethod()]
        public void AndTest()
        {
            new Parser("Code/Parser/AndTest.qk", "AndTest", out var module);
            Assert.AreEqual(1, module.Statements.Count);

            var evaluation = (AST.Evaluation)module.Statements[0];

            var expr1 = (AST.BinaryExpression)evaluation.Expr;
            Assert.AreEqual(AST.BinaryExpressionType.And, expr1.Type);

            var expr2 = (AST.BinaryExpression)expr1.Left;
            Assert.AreEqual(AST.BinaryExpressionType.And, expr2.Type);

            Assert.AreEqual(false, ((AST.ConstBool)expr2.Left).Value);
            Assert.AreEqual(true, ((AST.ConstBool)expr2.Right).Value);
            Assert.AreEqual(false, ((AST.ConstBool)expr1.Right).Value);
        }

        [TestMethod()]
        public void NotTest()
        {
            new Parser("Code/Parser/NotTest.qk", "NotTest", out var module);
            Assert.AreEqual(1, module.Statements.Count);

            var evaluation = (AST.Evaluation)module.Statements[0];

            var expr1 = (AST.UnaryExpression)evaluation.Expr;
            Assert.AreEqual(AST.UnaryExpressionType.Not, expr1.Type);

            var expr2 = (AST.UnaryExpression)expr1.Expr;
            Assert.AreEqual(AST.UnaryExpressionType.Not, expr2.Type);

            Assert.AreEqual(true, ((AST.ConstBool)expr2.Expr).Value);
        }

        [TestMethod()]
        public void CompOp()
        {
            new Parser("Code/Parser/CompOp.qk", "CompOp", out var module);
            Assert.AreEqual(6, module.Statements.Count);

            var evaluation = (AST.Evaluation)module.Statements[0];
            var expr = (AST.BinaryExpression)evaluation.Expr;
            Assert.AreEqual(AST.BinaryExpressionType.Less, expr.Type);
            Assert.AreEqual(1, ((AST.ConstInt)expr.Left).Value);
            Assert.AreEqual(2, ((AST.ConstInt)expr.Right).Value);

            evaluation = (AST.Evaluation)module.Statements[1];
            expr = (AST.BinaryExpression)evaluation.Expr;
            Assert.AreEqual(AST.BinaryExpressionType.Greater, expr.Type);
            Assert.AreEqual(2, ((AST.ConstInt)expr.Left).Value);
            Assert.AreEqual(1, ((AST.ConstInt)expr.Right).Value);

            evaluation = (AST.Evaluation)module.Statements[2];
            expr = (AST.BinaryExpression)evaluation.Expr;
            Assert.AreEqual(AST.BinaryExpressionType.Equal, expr.Type);
            Assert.AreEqual(1, ((AST.ConstInt)expr.Left).Value);
            Assert.AreEqual(1, ((AST.ConstInt)expr.Right).Value);

            evaluation = (AST.Evaluation)module.Statements[3];
            expr = (AST.BinaryExpression)evaluation.Expr;
            Assert.AreEqual(AST.BinaryExpressionType.LessOrEqual, expr.Type);
            Assert.AreEqual(1, ((AST.ConstInt)expr.Left).Value);
            Assert.AreEqual(2, ((AST.ConstInt)expr.Right).Value);

            evaluation = (AST.Evaluation)module.Statements[4];
            expr = (AST.BinaryExpression)evaluation.Expr;
            Assert.AreEqual(AST.BinaryExpressionType.GreaterOrEqual, expr.Type);
            Assert.AreEqual(2, ((AST.ConstInt)expr.Left).Value);
            Assert.AreEqual(1, ((AST.ConstInt)expr.Right).Value);

            evaluation = (AST.Evaluation)module.Statements[5];
            expr = (AST.BinaryExpression)evaluation.Expr;
            Assert.AreEqual(AST.BinaryExpressionType.NotEqual, expr.Type);
            Assert.AreEqual(1, ((AST.ConstInt)expr.Left).Value);
            Assert.AreEqual(2, ((AST.ConstInt)expr.Right).Value);
        }

        [TestMethod()]
        public void Comparison()
        {
            new Parser("Code/Parser/Comparison.qk", "Comparison", out var module);
            Assert.AreEqual(1, module.Statements.Count);

            var evaluation = (AST.Evaluation)module.Statements[0];

            var rightAnd = (AST.BinaryExpression)evaluation.Expr;
            Assert.AreEqual(AST.BinaryExpressionType.And, rightAnd.Type);

            var leftAnd = (AST.BinaryExpression)rightAnd.Left;
            Assert.AreEqual(AST.BinaryExpressionType.And, leftAnd.Type);

            var left = (AST.BinaryExpression)leftAnd.Left;
            Assert.AreEqual(AST.BinaryExpressionType.Less, left.Type);
            Assert.AreEqual("x", ((AST.NamedObj)left.Left).Name);
            Assert.AreEqual("y", ((AST.NamedObj)left.Right).Name);

            var center = (AST.BinaryExpression)leftAnd.Right;
            Assert.AreEqual(AST.BinaryExpressionType.LessOrEqual, center.Type);
            Assert.AreEqual("y", ((AST.NamedObj)center.Left).Name);
            Assert.AreEqual("z", ((AST.NamedObj)center.Right).Name);

            var right = (AST.BinaryExpression)rightAnd.Right;
            Assert.AreEqual(AST.BinaryExpressionType.Greater, right.Type);
            Assert.AreEqual("z", ((AST.NamedObj)right.Left).Name);
            Assert.AreEqual("w", ((AST.NamedObj)right.Right).Name);
        }

        [TestMethod()]
        public void Expr()
        {
            new Parser("Code/Parser/Expr.qk", "Expr", out var module);
            Assert.AreEqual(1, module.Statements.Count);

            var evaluation = (AST.Evaluation)module.Statements[0];

            var expr1 = (AST.BinaryExpression)evaluation.Expr;
            Assert.AreEqual(AST.BinaryExpressionType.BitOr, expr1.Type);

            var expr2 = (AST.BinaryExpression)expr1.Left;
            Assert.AreEqual(AST.BinaryExpressionType.BitOr, expr2.Type);

            Assert.AreEqual("a", ((AST.NamedObj)expr2.Left).Name);
            Assert.AreEqual("b", ((AST.NamedObj)expr2.Right).Name);
            Assert.AreEqual("c", ((AST.NamedObj)expr1.Right).Name);
        }

        [TestMethod()]
        public void XorExpr()
        {
            new Parser("Code/Parser/XorExpr.qk", "XorExpr", out var module);
            Assert.AreEqual(1, module.Statements.Count);

            var evaluation = (AST.Evaluation)module.Statements[0];

            var expr1 = (AST.BinaryExpression)evaluation.Expr;
            Assert.AreEqual(AST.BinaryExpressionType.BitXor, expr1.Type);

            var expr2 = (AST.BinaryExpression)expr1.Left;
            Assert.AreEqual(AST.BinaryExpressionType.BitXor, expr2.Type);

            Assert.AreEqual("a", ((AST.NamedObj)expr2.Left).Name);
            Assert.AreEqual("b", ((AST.NamedObj)expr2.Right).Name);
            Assert.AreEqual("c", ((AST.NamedObj)expr1.Right).Name);
        }

        [TestMethod()]
        public void AndExpr()
        {
            new Parser("Code/Parser/AndExpr.qk", "AndExpr", out var module);
            Assert.AreEqual(1, module.Statements.Count);

            var evaluation = (AST.Evaluation)module.Statements[0];

            var expr1 = (AST.BinaryExpression)evaluation.Expr;
            Assert.AreEqual(AST.BinaryExpressionType.BitAnd, expr1.Type);

            var expr2 = (AST.BinaryExpression)expr1.Left;
            Assert.AreEqual(AST.BinaryExpressionType.BitAnd, expr2.Type);

            Assert.AreEqual("a", ((AST.NamedObj)expr2.Left).Name);
            Assert.AreEqual("b", ((AST.NamedObj)expr2.Right).Name);
            Assert.AreEqual("c", ((AST.NamedObj)expr1.Right).Name);
        }

        [TestMethod()]
        public void ShiftExpr()
        {
            new Parser("Code/Parser/ShiftExpr.qk", "ShiftExpr", out var module);
            Assert.AreEqual(1, module.Statements.Count);

            var evaluation = (AST.Evaluation)module.Statements[0];

            var expr1 = (AST.BinaryExpression)evaluation.Expr;
            Assert.AreEqual(AST.BinaryExpressionType.RightShift, expr1.Type);

            var expr2 = (AST.BinaryExpression)expr1.Left;
            Assert.AreEqual(AST.BinaryExpressionType.LeftShift, expr2.Type);

            Assert.AreEqual("a", ((AST.NamedObj)expr2.Left).Name);
            Assert.AreEqual("b", ((AST.NamedObj)expr2.Right).Name);
            Assert.AreEqual("c", ((AST.NamedObj)expr1.Right).Name);
        }
        
        [TestMethod()]
        public void ArithExpr()
        {
            new Parser("Code/Parser/ArithExpr.qk", "ArithExpr", out var module);
            Assert.AreEqual(1, module.Statements.Count);

            var evaluation = (AST.Evaluation)module.Statements[0];

            var expr1 = (AST.BinaryExpression)evaluation.Expr;
            Assert.AreEqual(AST.BinaryExpressionType.Sub, expr1.Type);

            var expr2 = (AST.BinaryExpression)expr1.Left;
            Assert.AreEqual(AST.BinaryExpressionType.Add, expr2.Type);

            Assert.AreEqual("a", ((AST.NamedObj)expr2.Left).Name);
            Assert.AreEqual("b", ((AST.NamedObj)expr2.Right).Name);
            Assert.AreEqual("c", ((AST.NamedObj)expr1.Right).Name);
        }

        [TestMethod()]
        public void Term()
        {
            new Parser("Code/Parser/Term.qk", "Term", out var module);
            Assert.AreEqual(1, module.Statements.Count);

            var evaluation = (AST.Evaluation)module.Statements[0];

            var expr1 = (AST.BinaryExpression)evaluation.Expr;
            Assert.AreEqual(AST.BinaryExpressionType.FloorDiv, expr1.Type);

            var expr2 = (AST.BinaryExpression)expr1.Left;
            Assert.AreEqual(AST.BinaryExpressionType.Mod, expr2.Type);

            var expr3 = (AST.BinaryExpression)expr2.Left;
            Assert.AreEqual(AST.BinaryExpressionType.Div, expr3.Type);

            var expr4 = (AST.BinaryExpression)expr3.Left;
            Assert.AreEqual(AST.BinaryExpressionType.Mul, expr4.Type);

            Assert.AreEqual("a", ((AST.NamedObj)expr4.Left).Name);
            Assert.AreEqual("b", ((AST.NamedObj)expr4.Right).Name);
            Assert.AreEqual("c", ((AST.NamedObj)expr3.Right).Name);
            Assert.AreEqual("d", ((AST.NamedObj)expr2.Right).Name);
            Assert.AreEqual("e", ((AST.NamedObj)expr1.Right).Name);
        }

        [TestMethod()]
        public void Factor()
        {
            new Parser("Code/Parser/Factor.qk", "Factor", out var module);
            Assert.AreEqual(3, module.Statements.Count);

            var evaluation = (AST.Evaluation)module.Statements[0];
            var expr = (AST.UnaryExpression)evaluation.Expr;
            Assert.AreEqual(AST.UnaryExpressionType.Plus, expr.Type);
            Assert.AreEqual("x", ((AST.NamedObj)expr.Expr).Name);

            evaluation = (AST.Evaluation)module.Statements[1];
            expr = (AST.UnaryExpression)evaluation.Expr;
            Assert.AreEqual(AST.UnaryExpressionType.Minus, expr.Type);
            Assert.AreEqual("y", ((AST.NamedObj)expr.Expr).Name);

            evaluation = (AST.Evaluation)module.Statements[2];
            expr = (AST.UnaryExpression)evaluation.Expr;
            Assert.AreEqual(AST.UnaryExpressionType.BitNot, expr.Type);
            Assert.AreEqual("z", ((AST.NamedObj)expr.Expr).Name);
        }

        [TestMethod()]
        public void Power()
        {
            new Parser("Code/Parser/Power.qk", "Power", out var module);
            Assert.AreEqual(1, module.Statements.Count);

            var evaluation = (AST.Evaluation)module.Statements[0];

            var expr = (AST.BinaryExpression)evaluation.Expr;
            Assert.AreEqual(AST.BinaryExpressionType.Power, expr.Type);

            Assert.AreEqual("x", ((AST.NamedObj)expr.Left).Name);
            Assert.AreEqual(3, ((AST.ConstInt)expr.Right).Value);
        }

        [TestMethod()]
        public void AtomSimple()
        {
            new Parser("Code/Parser/AtomSimple.qk", "AtomSimple", out var module);
            Assert.AreEqual(6, module.Statements.Count);

            var evaluation = (AST.Evaluation)module.Statements[0];
            Assert.AreEqual("some_id", ((AST.NamedObj)evaluation.Expr).Name);

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

            var center = (AST.BinaryExpression)evaluation.Expr;
            Assert.AreEqual(AST.BinaryExpressionType.Mul, center.Type);

            var left = (AST.BinaryExpression)center.Left;
            Assert.AreEqual("a", ((AST.NamedObj)left.Left).Name);
            Assert.AreEqual("b", ((AST.NamedObj)left.Right).Name);

            var right = (AST.BinaryExpression)center.Right;
            Assert.AreEqual("c", ((AST.NamedObj)right.Left).Name);
            Assert.AreEqual("d", ((AST.NamedObj)right.Right).Name);
        }

        [TestMethod()]
        public void Trailer()
        {
            new Parser("Code/Parser/Trailer.qk", "Trailer", out var module);
            Assert.AreEqual(3, module.Statements.Count);

            var evaluation = (AST.Evaluation)module.Statements[0];
            var call = (AST.FuncCall)evaluation.Expr;
            Assert.AreEqual("f", ((AST.NamedObj)call.Func).Name);
            Assert.AreEqual(0, call.Args.Count);

            evaluation = (AST.Evaluation)module.Statements[1];
            call = (AST.FuncCall)evaluation.Expr;
            Assert.AreEqual("g", ((AST.NamedObj)call.Func).Name);
            Assert.AreEqual(1, call.Args.Count);
            Assert.AreEqual("x", ((AST.NamedObj)call.Args[0]).Name);

            evaluation = (AST.Evaluation)module.Statements[2];
            call = (AST.FuncCall)evaluation.Expr;
            Assert.AreEqual("h", ((AST.NamedObj)call.Func).Name);
            Assert.AreEqual(2, call.Args.Count);
            Assert.AreEqual("y", ((AST.NamedObj)call.Args[0]).Name);
            Assert.AreEqual(1, ((AST.ConstInt)call.Args[1]).Value);
        }

        [TestMethod()]
        public void FuncDef()
        {
            new Parser("Code/Parser/FuncDef.qk", "FuncDef", out var module);
            Assert.AreEqual(0, module.Statements.Count);

            var func = (AST.Func)module.NameTable["a"];
            Assert.AreEqual(0, func.Parameters.Count);
            Assert.AreEqual(0, func.Statements.Count);
            Assert.AreEqual(0, func.NameTable.Count);

            func = (AST.Func)module.NameTable["b"];
            Assert.AreEqual(0, func.Parameters.Count);
            Assert.AreEqual(0, func.Statements.Count);
            Assert.AreEqual(1, func.NameTable.Count);
            var inner = (AST.Func)func.NameTable["c"];
            Assert.AreEqual(0, inner.Parameters.Count);
            Assert.AreEqual(0, inner.Statements.Count);
            Assert.AreEqual(0, inner.NameTable.Count);

            func = (AST.Func)module.NameTable["c"];
            Assert.AreEqual(1, func.Parameters.Count);
            Assert.AreEqual(0, func.Statements.Count);
            Assert.AreEqual(0, func.NameTable.Count);
            var param = (AST.Variable)func.Parameters[0];
            Assert.AreEqual("x", param.Name);

            func = (AST.Func)module.NameTable["d"];
            Assert.AreEqual(2, func.Parameters.Count);
            Assert.AreEqual(0, func.Statements.Count);
            Assert.AreEqual(0, func.NameTable.Count);
            param = (AST.Variable)func.Parameters[0];
            Assert.AreEqual("x", param.Name);
            param = (AST.Variable)func.Parameters[1];
            Assert.AreEqual("y", param.Name);
        }

        [TestMethod()]
        public void Suite()
        {
            new Parser("Code/Parser/Suite.qk", "Suite", out var module);
            Assert.AreEqual(0, module.Statements.Count);

            var func = (AST.Func)module.NameTable["a"];
            Assert.AreEqual(0, func.Parameters.Count);
            Assert.AreEqual(0, func.Statements.Count);
            Assert.AreEqual(0, func.NameTable.Count);

            func = (AST.Func)module.NameTable["b"];
            Assert.AreEqual(0, func.Parameters.Count);
            Assert.AreEqual(2, func.Statements.Count);
            Assert.AreEqual(0, func.NameTable.Count);
            var expr = (AST.Assignment)func.Statements[0];
            Assert.AreEqual("x", ((AST.NamedObj)expr.Left).Name);
            Assert.AreEqual(1, ((AST.ConstInt)expr.Right).Value);
            expr = (AST.Assignment)func.Statements[1];
            Assert.AreEqual("y", ((AST.NamedObj)expr.Left).Name);
            Assert.AreEqual(2, ((AST.ConstInt)expr.Right).Value);
        }
    }
}