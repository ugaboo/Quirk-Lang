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
        public void Module_InvalidSyntax()
        {
            try {
                new Parser("Code/Parser/Module_InvalidSyntax.qk", "Module_InvalidSyntax", out var module);
            } catch (CompilationError exc) {
                Assert.AreEqual(ErrorType.InvalidSyntax, exc.Type);
                return;
            }
            throw new Exception("Failed");
        }

        [TestMethod()]
        public void SimpleStmnt()
        {
            new Parser("Code/Parser/SimpleStmnt.qk", "SimpleStmnt", out var module);

            var s = module.Statements;
            Assert.AreEqual(4, s.Count);
        }

        [TestMethod()]
        public void SimpleStmnt_InvalidSyntax()
        {
            try {
                new Parser("Code/Parser/SimpleStmnt_InvalidSyntax.qk", "SimpleStmnt_InvalidSyntax", out var module);
            } catch (CompilationError exc) {
                Assert.AreEqual(ErrorType.InvalidSyntax, exc.Type);
                return;
            }
            throw new Exception("Failed");
        }

        [TestMethod()]
        public void ExprStmnt()
        {
            new Parser("Code/Parser/ExprStmnt.qk", "ExprStmnt", out var module);
            Assert.AreEqual(5, module.Statements.Count);

            var evaluation = module.Statements[0] as AST.Evaluation;
            Assert.AreNotEqual(null, evaluation);
            var two = evaluation.Expr as AST.ConstInt;
            Assert.AreEqual(2, two.Value);

            var assignment = module.Statements[1] as AST.Assignment;
            Assert.AreNotEqual(null, assignment);
            var x = assignment.Left as AST.NamedObj;
            Assert.AreEqual("x", x.Name);
            var zero = assignment.Right as AST.ConstInt;
            Assert.AreEqual(0, zero.Value);

            assignment = module.Statements[2] as AST.Assignment;
            Assert.AreNotEqual(null, assignment);
            var z = assignment.Left as AST.NamedObj;
            Assert.AreEqual("z", z.Name);
            var one = assignment.Right as AST.ConstInt;
            Assert.AreEqual(1, one.Value);

            assignment = module.Statements[3] as AST.Assignment;
            Assert.AreNotEqual(null, assignment);
            var y = assignment.Left as AST.NamedObj;
            Assert.AreEqual("y", y.Name);
            Assert.AreEqual(z, assignment.Right);

            assignment = module.Statements[4] as AST.Assignment;
            Assert.AreNotEqual(null, assignment);
            x = assignment.Left as AST.NamedObj;        // NamedObj was created anew in the Atom method
            Assert.AreEqual("x", x.Name);
            Assert.AreEqual(y, assignment.Right);
        }

        [TestMethod()]
        public void ExprStmnt_InvalidSyntax()
        {
            try {
                new Parser("Code/Parser/ExprStmnt_InvalidSyntax.qk", "ExprStmnt_InvalidSyntax", out var module);
            } catch (CompilationError exc) {
                Assert.AreEqual(ErrorType.InvalidSyntax, exc.Type);
                return;
            }
            throw new Exception("Failed");
        }

        [TestMethod()]
        public void Functions()
        {
            new Parser("Code/Parser/Functions.qk", "Functions", out var module);
        }
    }
}