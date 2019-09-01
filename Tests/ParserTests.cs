using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Quirk.Lexeme;

namespace Quirk.Tests
{
    [TestClass()]
    public class ParserTests
    {
        [TestMethod()]
        public void Test()
        {
            var parser = new Parser("Code/Parser/Test.qk", "Test", out var module);
            module.Accept(new Visitors.NameFinder());
            module.Accept(new Visitors.CodeGenerator());

            var process = new Process();
            process.StartInfo = new ProcessStartInfo() {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "clang",
                Arguments = "Test.bc -o test.exe",
            };
            process.Start();
        }

        [TestMethod()]
        public void Functions()
        {
            new Parser("Code/Parser/Functions.qk", "Functions", out var module);
        }
    }
}