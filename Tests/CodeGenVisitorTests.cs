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
    }
}