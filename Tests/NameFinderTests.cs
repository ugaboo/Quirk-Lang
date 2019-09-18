using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Quirk.Tests
{
    [TestClass()]
    public class NameFinderTests
    {
        [TestMethod()]
        public void Test()
        {
            new Parser("Code/NameFinder/Test.qk", "Test", out var module);
            new Visitors.FuncParamsProcessor().Visit(module);
            new Visitors.NameFinder().Visit(module);


        }
    }
}