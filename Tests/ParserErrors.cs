using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Quirk.Tests
{
    [TestClass()]
    public class ParserErrors
    {
        void Test(string name, ErrorType type = ErrorType.InvalidSyntax)
        {
            try {
                new Parser($"Code/Parser/Errors/{name}.qk", name, out var module);
            } catch (CompilationError exc) {
                Assert.AreEqual(type, exc.Type);
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
        [TestMethod()] public void Comparison() { Test("Comparison"); }
        [TestMethod()] public void Expr() { Test("Expr"); }
        [TestMethod()] public void XorExpr() { Test("XorExpr"); }
        [TestMethod()] public void AndExpr() { Test("AndExpr"); }
        [TestMethod()] public void ShiftExpr() { Test("ShiftExpr"); }
        [TestMethod()] public void ArithExpr() { Test("ArithExpr"); }
        [TestMethod()] public void Term() { Test("Term"); }
        [TestMethod()] public void Factor() { Test("Factor"); }
        [TestMethod()] public void Power() { Test("Power"); }
        [TestMethod()] public void AtomParenthesis() { Test("AtomParenthesis"); }
        [TestMethod()] public void Trailer() { Test("Trailer"); }
        [TestMethod()] public void FuncDef1() { Test("FuncDef1"); }
        [TestMethod()] public void FuncDef2() { Test("FuncDef2"); }
        [TestMethod()] public void FuncDef3() { Test("FuncDef3"); }
        [TestMethod()] public void FuncDef4() { Test("FuncDef4"); }
        [TestMethod()] public void FuncDef5() { Test("FuncDef5"); }
        [TestMethod()] public void Parameters() { Test("Parameters"); }
        [TestMethod()] public void TypedFormalParamDef() { Test("TypedFormalParamDef"); }
        [TestMethod()] public void Suite() { Test("Suite", ErrorType.ExpectedAnIndentedBlock); }
    }
}