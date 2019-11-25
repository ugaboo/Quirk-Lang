using Quirk.AST;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Quirk.Tests
{
    [TestClass()]
    public class CodeGenVisitorTests
    {
        [TestMethod()]
        public void Function()
        {
            new Parser("Code/CodeGenVisitor/Function.qk", "Function", out var module);
            new Visitors.CodeGenVisitor(module);
        }

        [TestMethod()]
        public void Variables()
        {
            new Parser("Code/CodeGenVisitor/Variables.qk", "Variables", out var module);
            new Visitors.CodeGenVisitor(module);
        }

        [TestMethod()]
        public void Return()
        {
            new Parser("Code/CodeGenVisitor/Return.qk", "Return", out var module);
            new Visitors.CodeGenVisitor(module);
        }

        [TestMethod()]
        public void Add()
        {
            new Parser("Code/CodeGenVisitor/Add.qk", "Add", out var module);
            new Visitors.CodeGenVisitor(module);
        }

        [TestMethod()]
        public void Sub()
        {
            new Parser("Code/CodeGenVisitor/Sub.qk", "Sub", out var module);
            new Visitors.CodeGenVisitor(module);
        }

        [TestMethod()]
        public void Mul()
        {
            new Parser("Code/CodeGenVisitor/Mul.qk", "Mul", out var module);
            new Visitors.CodeGenVisitor(module);
        }

        [TestMethod()]
        public void TrueDiv()
        {
            new Parser("Code/CodeGenVisitor/TrueDiv.qk", "TrueDiv", out var module);
            new Visitors.CodeGenVisitor(module);
        }

        [TestMethod()]
        public void FloorDiv()
        {
            new Parser("Code/CodeGenVisitor/FloorDiv.qk", "FloorDiv", out var module);
            new Visitors.CodeGenVisitor(module);
        }

        [TestMethod()]
        public void Mod()
        {
            new Parser("Code/CodeGenVisitor/Mod.qk", "Mod", out var module);
            new Visitors.CodeGenVisitor(module);
        }

        [TestMethod()]
        public void Pow()
        {
            new Parser("Code/CodeGenVisitor/Pow.qk", "Pow", out var module);
            new Visitors.CodeGenVisitor(module);
        }

        [TestMethod()]
        public void Pos()
        {
            new Parser("Code/CodeGenVisitor/Pos.qk", "Pos", out var module);
            new Visitors.CodeGenVisitor(module);
        }

        [TestMethod()]
        public void Neg()
        {
            new Parser("Code/CodeGenVisitor/Neg.qk", "Neg", out var module);
            new Visitors.CodeGenVisitor(module);
        }

        [TestMethod()]
        public void Lt()
        {
            new Parser("Code/CodeGenVisitor/Lt.qk", "Lt", out var module);
            new Visitors.CodeGenVisitor(module);
        }

        [TestMethod()]
        public void Gt()
        {
            new Parser("Code/CodeGenVisitor/Gt.qk", "Gt", out var module);
            new Visitors.CodeGenVisitor(module);
        }

        [TestMethod()]
        public void Le()
        {
            new Parser("Code/CodeGenVisitor/Le.qk", "Le", out var module);
            new Visitors.CodeGenVisitor(module);
        }

        [TestMethod()]
        public void Ge()
        {
            new Parser("Code/CodeGenVisitor/Ge.qk", "Ge", out var module);
            new Visitors.CodeGenVisitor(module);
        }

        [TestMethod()]
        public void Eq()
        {
            new Parser("Code/CodeGenVisitor/Eq.qk", "Eq", out var module);
            new Visitors.CodeGenVisitor(module);
        }

        [TestMethod()]
        public void Ne()
        {
            new Parser("Code/CodeGenVisitor/Ne.qk", "Ne", out var module);
            new Visitors.CodeGenVisitor(module);
        }

        [TestMethod()]
        public void Not()
        {
            new Parser("Code/CodeGenVisitor/Not.qk", "Not", out var module);
            new Visitors.CodeGenVisitor(module);
        }
    }
}