using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Quirk.Tests
{
    [TestClass()]
    public class FuncParamsProcessorErrors
    {
        [TestMethod()]
        public void DuplicateParameter()
        {
            new Parser("Code/FuncParamsProcessor/Errors/DuplicateParameter.qk", "DuplicateParameter", out var module);
            try {
                new Visitors.FuncParamsProcessor().Visit(module);
            } catch (CompilationError exc) {
                Assert.AreEqual(ErrorType.DuplicateParameter, exc.Type);
                return;
            }
            throw new Exception("Failed");
        }
    }
}