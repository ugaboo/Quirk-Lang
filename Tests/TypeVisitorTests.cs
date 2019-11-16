using Quirk.AST;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Quirk.Tests
{
    [TestClass()]
    public class TypeVisitorTests
    {
        [TestMethod()]
        public void Constants()
        {
            new Parser("Code/TypeVisitor/Constants.qk", "Constants", out var module);
            new Visitors.TypeVisitor(module);

            var assignment = (Assignment)module.Statements[0];
            Assert.AreEqual(TypeObj.Int, ((Variable)assignment.Left).Type);

            assignment = (Assignment)module.Statements[1];
            Assert.AreEqual(TypeObj.Float, ((Variable)assignment.Left).Type);

            assignment = (Assignment)module.Statements[2];
            Assert.AreEqual(TypeObj.Bool, ((Variable)assignment.Left).Type);
        }

        [TestMethod()]
        public void RetType()
        {
            new Parser("Code/TypeVisitor/RetType.qk", "RetType", out var module);
            new Visitors.TypeVisitor(module);

            var def = (FuncDef)module.Statements[0];
            Assert.AreEqual(TypeObj.Int, def.Func.RetType);
        }

        [TestMethod()]
        public void OperatorOverloads()
        {
            new Parser("Code/TypeVisitor/OperatorOverloads.qk", "OperatorOverloads", out var module);
            new Visitors.TypeVisitor(module);

            var assignment = (Assignment)module.Statements[0];
            Assert.AreEqual(TypeObj.Int, ((Variable)assignment.Left).Type);

            assignment = (Assignment)module.Statements[1];
            Assert.AreEqual(TypeObj.Float, ((Variable)assignment.Left).Type);

            assignment = (Assignment)module.Statements[2];
            Assert.AreEqual(TypeObj.Float, ((Variable)assignment.Left).Type);
        }

        [TestMethod()]
        public void FuncCall()
        {
            new Parser("Code/TypeVisitor/FuncCall.qk", "FuncCall", out var module);
            new Visitors.TypeVisitor(module);

            var funcDef = (FuncDef)module.Statements[0];
            var eval = (Evaluation)module.Statements[1];
            var funcCall = (FuncCall)eval.Expr;
            //Assert.AreEqual(TypeObj.Int, ((Variable)assignment.Left).Type);

        }

        [TestMethod()]
        public void TemplateFunc()
        {
            new Parser("Code/TypeVisitor/TemplateFunc.qk", "TemplateFunc", out var module);
            new Visitors.TypeVisitor(module);

            Variable a;
            {
                var a1 = (Assignment)module.Statements[0];      // a = 1
                a = (Variable)a1.Left;
                Assert.AreEqual(TypeObj.Int, a.Type);
            }

            {
                // template f
                var def = (FuncDef)module.Statements[1];        // def f(x): ...
                var f = def.Func;
                Assert.AreEqual(null, f.Parameters[0].Type);

                var a2 = (Assignment)f.Statements[0];           // a = 2
                Assert.AreEqual(a, a2.Left);
                Assert.AreEqual(TypeObj.Int, a.Type);           // no changes

                var bx = (Assignment)f.Statements[1];           // b = x
                var b = (Variable)bx.Left;
                Assert.AreEqual(null, b.Type);

                // template g
                var defG = (FuncDef)f.Statements[2];            // def g(y): ...
                var g = defG.Func;
                Assert.AreEqual(null, g.Parameters[0].Type);
                var a3 = (Assignment)g.Statements[0];           // a = 3
                var by = (Assignment)g.Statements[1];           // b = y
                var cy = (Assignment)g.Statements[2];           // c = y
                Assert.AreEqual(TypeObj.Int, ((Variable)a3.Left).Type);
                Assert.AreEqual(null, ((Variable)by.Left).Type);
                Assert.AreEqual(null, ((Variable)cy.Left).Type);
            }
            {
                var eval = (Evaluation)module.Statements[2];
                var call = (FuncCall)eval.Expr;
                // spec f(Int)
                var f = (Function)call.Func;
                Assert.AreEqual(TypeObj.Int, f.Parameters[0].Type);
                var a2 = (Assignment)f.Statements[0];
                Assert.AreEqual(TypeObj.Int, ((Variable)a2.Left).Type);
                var bx = (Assignment)f.Statements[1];
                Assert.AreEqual(TypeObj.Int, ((Variable)bx.Left).Type);
                var defG = (FuncDef)f.Statements[2];
                {
                    // template g
                    var g = defG.Func;
                    Assert.AreEqual(null, g.Parameters[0].Type);
                    var a3 = (Assignment)g.Statements[0];
                    var by = (Assignment)g.Statements[1];
                    var cy = (Assignment)g.Statements[2];
                    Assert.AreEqual(TypeObj.Int, ((Variable)a3.Left).Type);     // global
                    Assert.AreEqual(TypeObj.Int, ((Variable)by.Left).Type);     // closure
                    Assert.AreEqual(null, ((Variable)cy.Left).Type);
                }
                var evalG = (Evaluation)f.Statements[3];
                {
                    // spec g(Int)
                    var callG = (FuncCall)evalG.Expr;
                    var g = (Function)callG.Func;
                    Assert.AreEqual(TypeObj.Int, g.Parameters[0].Type);
                    var a3 = (Assignment)g.Statements[0];
                    var by = (Assignment)g.Statements[1];
                    var cy = (Assignment)g.Statements[2];
                    Assert.AreEqual(TypeObj.Int, ((Variable)a3.Left).Type);
                    Assert.AreEqual(TypeObj.Int, ((Variable)by.Left).Type);
                    Assert.AreEqual(TypeObj.Int, ((Variable)cy.Left).Type);
                }
            }
            {
                var eval = (Evaluation)module.Statements[3];
                var call = (FuncCall)eval.Expr;
                // spec f(Float)
                var f = (Function)call.Func;
                Assert.AreEqual(TypeObj.Float, f.Parameters[0].Type);
                var a2 = (Assignment)f.Statements[0];
                Assert.AreEqual(TypeObj.Int, ((Variable)a2.Left).Type);
                var bx = (Assignment)f.Statements[1];
                Assert.AreEqual(TypeObj.Float, ((Variable)bx.Left).Type);
                var defG = (FuncDef)f.Statements[2];
                {
                    // template g
                    var g = defG.Func;
                    Assert.AreEqual(null, g.Parameters[0].Type);
                    var a3 = (Assignment)g.Statements[0];
                    var by = (Assignment)g.Statements[1];
                    var cy = (Assignment)g.Statements[2];
                    Assert.AreEqual(TypeObj.Int, ((Variable)a3.Left).Type);
                    Assert.AreEqual(TypeObj.Float, ((Variable)by.Left).Type);
                    Assert.AreEqual(null, ((Variable)cy.Left).Type);
                }
                var evalG = (Evaluation)f.Statements[3];
                {
                    // spec g(Float)
                    var callG = (FuncCall)evalG.Expr;
                    var g = (Function)callG.Func;
                    Assert.AreEqual(TypeObj.Float, g.Parameters[0].Type);
                    var a3 = (Assignment)g.Statements[0];
                    var by = (Assignment)g.Statements[1];
                    var cy = (Assignment)g.Statements[2];
                    Assert.AreEqual(TypeObj.Int, ((Variable)a3.Left).Type);
                    Assert.AreEqual(TypeObj.Float, ((Variable)by.Left).Type);
                    Assert.AreEqual(TypeObj.Float, ((Variable)cy.Left).Type);
                }
            }
        }
    }
}