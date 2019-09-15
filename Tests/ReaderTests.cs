using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Quirk.Tests
{
    [TestClass()]
    public class ReaderTests
    {
        void Check(Reader reader, string lineValue, char value, int line, int column)
        {
            var pos = reader.Position;

            Assert.AreEqual(value, reader.Value);
            Assert.AreEqual(line, pos.Line);
            Assert.AreEqual(column, pos.Column);
            Assert.AreEqual(lineValue, pos.LineValue);

            reader.Next();
        }

        [TestMethod()]
        public void EmptyFile()
        {
            var reader = new Reader("Code/Reader/Empty.qk");
            Check(reader, "", char.MaxValue, 1, 1);
            Check(reader, "", char.MaxValue, 1, 1);
        }

        [TestMethod()]
        public void Letters()
        {
            var reader = new Reader("Code/Reader/Letters.qk");
            Check(reader, "abc", 'a', 1, 1);
            Check(reader, "abc", 'b', 1, 2);
            Check(reader, "abc", 'c', 1, 3);
            Check(reader, "abc", char.MaxValue, 1, 4);
        }

        [TestMethod()]
        public void Newline()
        {
            var reader = new Reader("Code/Reader/Newline/Win.qk");
            Check(reader, "", '\r', 1, 1);
            Check(reader, "", char.MaxValue, 2, 1);

            reader = new Reader("Code/Reader/Newline/Unix.qk");
            Check(reader, "", '\n', 1, 1);
            Check(reader, "", char.MaxValue, 2, 1);

            reader = new Reader("Code/Reader/Newline/Mac.qk");
            Check(reader, "", '\r', 1, 1);
            Check(reader, "", char.MaxValue, 2, 1);
        }

        [TestMethod()]
        public void Tab()
        {
            var reader = new Reader("Code/Reader/Tab.qk");
            Check(reader, "\t1", '\t', 1, 1);
            Check(reader, "\t1", '1', 1, 5);
            Check(reader, "\t1", '\r', 1, 6);

            Check(reader, "2\t2", '2', 2, 1);
            Check(reader, "2\t2", '\t', 2, 2);
            Check(reader, "2\t2", '2', 2, 5);
            Check(reader, "2\t2", '\r', 2, 6);

            Check(reader, "33\t3", '3', 3, 1);
            Check(reader, "33\t3", '3', 3, 2);
            Check(reader, "33\t3", '\t', 3, 3);
            Check(reader, "33\t3", '3', 3, 5);
            Check(reader, "33\t3", '\r', 3, 6);

            Check(reader, "444\t4", '4', 4, 1);
            Check(reader, "444\t4", '4', 4, 2);
            Check(reader, "444\t4", '4', 4, 3);
            Check(reader, "444\t4", '\t', 4, 4);
            Check(reader, "444\t4", '4', 4, 5);
            Check(reader, "444\t4", '\r', 4, 6);

            Check(reader, "\t55\t5", '\t', 5, 1);
            Check(reader, "\t55\t5", '5', 5, 5);
            Check(reader, "\t55\t5", '5', 5, 6);
            Check(reader, "\t55\t5", '\t', 5, 7);
            Check(reader, "\t55\t5", '5', 5, 9);
            Check(reader, "\t55\t5", char.MaxValue, 5, 10);
        }

        [TestMethod()]
        public void Text()
        {
            var reader = new Reader("Code/Reader/Text.qk");
            Check(reader, "7@", '7', 1, 1);
            Check(reader, "7@", '@', 1, 2);
            Check(reader, "7@", '\r', 1, 3);
            Check(reader, "\txZ", '\t', 2, 1);
            Check(reader, "\txZ", 'x', 2, 5);
            Check(reader, "\txZ", 'Z', 2, 6);
            Check(reader, "\txZ", '\r', 2, 7);
            Check(reader, "йЯ", 'й', 3, 1);
            Check(reader, "йЯ", 'Я', 3, 2);
            Check(reader, "йЯ", char.MaxValue, 3, 3);
            Check(reader, "йЯ", char.MaxValue, 3, 3);

        }
    }
}