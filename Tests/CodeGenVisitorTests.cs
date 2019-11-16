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
    }
}