using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Quirk.Lexeme;

namespace Quirk.Tests
{
    [TestClass()] public class ScannerErrors
    {
        [TestMethod()] public void Indents()
        {
            var scan = new Scanner("Code/Scanner/Errors/Indents.qk");
            Assert.AreEqual(Indent, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(Id, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(NewLine, scan.Lexeme);
            try {
                scan.Next();
            } catch (CompilationError exc) {
                Assert.AreEqual(ErrorType.ExpectedAnIndentedBlock, exc.Type);
                return;
            }
            throw new Exception("Failed");
        }
    }
}