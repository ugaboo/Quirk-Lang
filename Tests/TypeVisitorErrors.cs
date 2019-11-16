using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Quirk.Tests
{
    [TestClass()]
    public class TypeVisitorErrors
    {
        [TestMethod()]
        public void CantDetermineType()
        {
            new Parser("Code/TypeVisitor/Errors/CantDetermineType.qk", "CantDetermineType", out var module);
            try {
                new Visitors.TypeVisitor(module);
            } catch (CompilationError exc) {
                Assert.AreEqual(ErrorType.CantDetermineType, exc.Type);
                return;
            }
            throw new Exception("Failed");
        }

        [TestMethod()]
        public void AssignmentIsNotPossible()
        {
            new Parser("Code/TypeVisitor/Errors/AssignmentIsNotPossible.qk", "AssignmentIsNotPossible", out var module);
            try {
                new Visitors.TypeVisitor(module);
            } catch (CompilationError exc) {
                Assert.AreEqual(ErrorType.AssignmentIsNotPossible, exc.Type);
                return;
            }
            throw new Exception("Failed");
        }
    }
}