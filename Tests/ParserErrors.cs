﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Quirk.Tests
{
    [TestClass()]
    public class ParserErrors
    {
        void Test(string name)
        {
            try {
                new Parser($"Code/Parser/Errors/{name}.qk", name, out var module);
            } catch (CompilationError exc) {
                Assert.AreEqual(ErrorType.InvalidSyntax, exc.Type);
                return;
            }
            throw new Exception("Failed");
        }

        [TestMethod()] public void Module() { Test("Module"); }
        [TestMethod()] public void SimpleStmnt() { Test("SimpleStmnt"); }
        [TestMethod()] public void ExprStmnt() { Test("ExprStmnt"); }
        [TestMethod()] public void OrTest() { Test("OrTest"); }
        [TestMethod()] public void AndTest() { Test("AndTest"); }
        [TestMethod()] public void NotTest() { Test("NotTest"); }
    }
}