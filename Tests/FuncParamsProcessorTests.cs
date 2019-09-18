using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Quirk.Tests
{
    [TestClass()]
    public class FuncParamsProcessorTests
    {
        [TestMethod()]
        public void CopyParamsToNameTable()
        {
            new Parser("Code/FuncParamsProcessor/CopyParamsToNameTable.qk", "CopyParamsToNameTable", out var module);
            new Visitors.FuncParamsProcessor().Visit(module);

            var overload = (AST.Overload)module.NameTable["f"];
            var func = overload.Funcs[0];

            Assert.AreEqual(true, func.NameTable["x"] is AST.Variable);
            Assert.AreEqual(true, func.NameTable["y"] is AST.Variable);
            Assert.AreEqual(true, func.NameTable["z"] is AST.Variable);
        }

        [TestMethod()]
        public void ParameterOverlap()
        {
            new Parser("Code/FuncParamsProcessor/ParameterOverlap.qk", "ParameterOverlap", out var module);
            new Visitors.FuncParamsProcessor().Visit(module);

            var overload = (AST.Overload)module.NameTable["f"];
            var func = overload.Funcs[0];

            Assert.AreEqual(true, func.Parameters[0] is AST.Variable);
            Assert.AreEqual(true, func.NameTable["x"] is AST.Overload);
        }
    }
}