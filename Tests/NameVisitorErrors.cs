using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Quirk.Tests
{
    [TestClass()]
    public class NameVisitorErrors
    {
        [TestMethod()]
        public void DuplicateParameter()
        {
            new Parser("Code/NameVisitor/Errors/DuplicateParameter.qk", "DuplicateParameter", out var module);
            try {
                new Visitors.NameVisitor(module);
            } catch (CompilationError exc) {
                Assert.AreEqual(ErrorType.DuplicateParameter, exc.Type);
                return;
            }
            throw new Exception("Failed");
        }

        [TestMethod()]
        public void ObjectIsNotDefined()
        {
            new Parser("Code/NameVisitor/Errors/ObjectIsNotDefined.qk", "ObjectIsNotDefined", out var module);
            try {
                new Visitors.NameVisitor(module);
            } catch (CompilationError exc) {
                Assert.AreEqual(ErrorType.ObjectIsNotDefined, exc.Type);
                return;
            }
            throw new Exception("Failed");
        }
    }
}