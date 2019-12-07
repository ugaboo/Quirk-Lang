using Quirk.AST;
using Quirk.Helpers;
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
            Assert.AreEqual(BuiltIns.Int, ((Variable)assignment.Left).Type);

            assignment = (Assignment)module.Statements[1];
            Assert.AreEqual(BuiltIns.Float, ((Variable)assignment.Left).Type);

            assignment = (Assignment)module.Statements[2];
            Assert.AreEqual(BuiltIns.Bool, ((Variable)assignment.Left).Type);
        }

        [TestMethod()]
        public void RetType()
        {
            new Parser("Code/TypeVisitor/RetType.qk", "RetType", out var module);
            new Visitors.TypeVisitor(module);

            var def = (FuncDef)module.Statements[0];
            Assert.AreEqual(BuiltIns.Int, def.Func.RetType);
        }

        [TestMethod()]
        public void OperatorOverloads()
        {
            new Parser("Code/TypeVisitor/OperatorOverloads.qk", "OperatorOverloads", out var module);
            new Visitors.TypeVisitor(module);

            var assignment = (Assignment)module.Statements[0];
            Assert.AreEqual(BuiltIns.Int, ((Variable)assignment.Left).Type);

            assignment = (Assignment)module.Statements[1];
            Assert.AreEqual(BuiltIns.Float, ((Variable)assignment.Left).Type);

            assignment = (Assignment)module.Statements[2];
            Assert.AreEqual(BuiltIns.Float, ((Variable)assignment.Left).Type);
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
                Assert.AreEqual(BuiltIns.Int, a.Type);
            }

            {
                // template f
                var def = (FuncDef)module.Statements[1];        // def f(x): ...
                var f = def.Func;
                Assert.AreEqual(null, f.Parameters[0].Type);

                var a2 = (Assignment)f.Statements[0];           // a = 2
                Assert.AreEqual(a, a2.Left);
                Assert.AreEqual(BuiltIns.Int, a.Type);           // no changes

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
                Assert.AreEqual(BuiltIns.Int, ((Variable)a3.Left).Type);
                Assert.AreEqual(null, ((Variable)by.Left).Type);
                Assert.AreEqual(null, ((Variable)cy.Left).Type);
            }
            {
                var eval = (Evaluation)module.Statements[2];
                var call = (FuncCall)eval.Expr;
                // spec f(Int)
                var f = (Function)call.Func;
                Assert.AreEqual(BuiltIns.Int, f.Parameters[0].Type);
                var a2 = (Assignment)f.Statements[0];
                Assert.AreEqual(BuiltIns.Int, ((Variable)a2.Left).Type);
                var bx = (Assignment)f.Statements[1];
                Assert.AreEqual(BuiltIns.Int, ((Variable)bx.Left).Type);
                var defG = (FuncDef)f.Statements[2];
                {
                    // template g
                    var g = defG.Func;
                    Assert.AreEqual(null, g.Parameters[0].Type);
                    var a3 = (Assignment)g.Statements[0];
                    var by = (Assignment)g.Statements[1];
                    var cy = (Assignment)g.Statements[2];
                    Assert.AreEqual(BuiltIns.Int, ((Variable)a3.Left).Type);     // global
                    Assert.AreEqual(BuiltIns.Int, ((Variable)by.Left).Type);     // closure
                    Assert.AreEqual(null, ((Variable)cy.Left).Type);
                }
                var evalG = (Evaluation)f.Statements[3];
                {
                    // spec g(Int)
                    var callG = (FuncCall)evalG.Expr;
                    var g = (Function)callG.Func;
                    Assert.AreEqual(BuiltIns.Int, g.Parameters[0].Type);
                    var a3 = (Assignment)g.Statements[0];
                    var by = (Assignment)g.Statements[1];
                    var cy = (Assignment)g.Statements[2];
                    Assert.AreEqual(BuiltIns.Int, ((Variable)a3.Left).Type);
                    Assert.AreEqual(BuiltIns.Int, ((Variable)by.Left).Type);
                    Assert.AreEqual(BuiltIns.Int, ((Variable)cy.Left).Type);
                }
            }
            {
                var eval = (Evaluation)module.Statements[3];
                var call = (FuncCall)eval.Expr;
                // spec f(Float)
                var f = (Function)call.Func;
                Assert.AreEqual(BuiltIns.Float, f.Parameters[0].Type);
                var a2 = (Assignment)f.Statements[0];
                Assert.AreEqual(BuiltIns.Int, ((Variable)a2.Left).Type);
                var bx = (Assignment)f.Statements[1];
                Assert.AreEqual(BuiltIns.Float, ((Variable)bx.Left).Type);
                var defG = (FuncDef)f.Statements[2];
                {
                    // template g
                    var g = defG.Func;
                    Assert.AreEqual(null, g.Parameters[0].Type);
                    var a3 = (Assignment)g.Statements[0];
                    var by = (Assignment)g.Statements[1];
                    var cy = (Assignment)g.Statements[2];
                    Assert.AreEqual(BuiltIns.Int, ((Variable)a3.Left).Type);
                    Assert.AreEqual(BuiltIns.Float, ((Variable)by.Left).Type);
                    Assert.AreEqual(null, ((Variable)cy.Left).Type);
                }
                var evalG = (Evaluation)f.Statements[3];
                {
                    // spec g(Float)
                    var callG = (FuncCall)evalG.Expr;
                    var g = (Function)callG.Func;
                    Assert.AreEqual(BuiltIns.Float, g.Parameters[0].Type);
                    var a3 = (Assignment)g.Statements[0];
                    var by = (Assignment)g.Statements[1];
                    var cy = (Assignment)g.Statements[2];
                    Assert.AreEqual(BuiltIns.Int, ((Variable)a3.Left).Type);
                    Assert.AreEqual(BuiltIns.Float, ((Variable)by.Left).Type);
                    Assert.AreEqual(BuiltIns.Float, ((Variable)cy.Left).Type);
                }
            }
        }

        [TestMethod()]
        public void If()
        {
            new Parser("Code/TypeVisitor/If.qk", "If", out var module);
            new Visitors.TypeVisitor(module);

            Assert.AreEqual(4, module.Statements.Count);

            var ifStmnt = (IfStmnt)module.Statements[3];
            var assign = (Assignment)ifStmnt.Then[0];
            var x = (Variable)assign.Left;
            Assert.AreEqual(BuiltIns.Bool, x.Type);

            var elif = (IfStmnt)ifStmnt.Else[0];
            var call = (FuncCall)elif.Condition;
            Assert.AreEqual(BuiltIns.Gt_Int_Int, call.Func);

            assign = (Assignment)elif.Then[0];
            var y = (Variable)assign.Left;
            Assert.AreEqual(BuiltIns.Int, y.Type);

            assign = (Assignment)elif.Else[0];
            var z = (Variable)assign.Left;
            Assert.AreEqual(BuiltIns.Float, z.Type);
        }

        [TestMethod()]
        public void TemplateIf()
        {
            new Parser("Code/TypeVisitor/TemplateIf.qk", "TemplateIf", out var module);
            new Visitors.TypeVisitor(module);

            IfStmnt ifT, ifI, ifF;
            {
                var def = (FuncDef)module.Statements[0];
                var template = def.Func;
                ifT = (IfStmnt)template.Statements[0];
            }
            {
                var eval = (Evaluation)module.Statements[1];
                var call = (FuncCall)eval.Expr;
                var intFunc = (Function)call.Func;
                ifI = (IfStmnt)intFunc.Statements[0];
            }
            {
                var eval = (Evaluation)module.Statements[2];
                var call = (FuncCall)eval.Expr;
                var floatFunc = (Function)call.Func;
                ifF = (IfStmnt)floatFunc.Statements[0];
            }
            Assert.AreNotEqual(ifT, ifI);
            Assert.AreNotEqual(ifI, ifF);

            Assert.AreNotEqual(ifT.Condition, ifF.Condition);
            Assert.AreNotEqual(ifT.Then[0], ifF.Then[0]);

            Assert.AreEqual(BuiltIns.Gt_Float_Int, ((FuncCall)ifF.Condition).Func);
            var assign = (Assignment)ifF.Then[0];
            Assert.AreEqual(BuiltIns.Float, ((Variable)assign.Left).Type);

            var elifT = (IfStmnt)ifT.Else[0];
            var elifF = (IfStmnt)ifF.Else[0];

            Assert.AreNotEqual(elifT.Condition, elifF.Condition);
            Assert.AreNotEqual(elifT.Then[0], elifF.Then[0]);

            Assert.AreEqual(BuiltIns.Lt_Float_Int, ((FuncCall)elifF.Condition).Func);
            assign = (Assignment)elifF.Then[0];
            Assert.AreEqual(BuiltIns.Float, ((Variable)assign.Left).Type);

            assign = (Assignment)elifF.Else[0];
            Assert.AreEqual(BuiltIns.Float, ((Variable)assign.Left).Type);
        }
    }
}