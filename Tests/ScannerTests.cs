using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Quirk.Lexeme;

namespace Quirk.Tests
{
    [TestClass()]
    public class ScannerTests
    {
        [TestMethod()]
        public void Operators()
        {
            var scan = new Scanner("Code/Scanner/Operators.qk");
            Assert.AreEqual(LeftParenthesis, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(RightParenthesis, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(FullStop, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(Comma, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(Colon, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(Semicolon, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(Plus, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(Minus, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(Star, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(Power, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(Slash, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(DoubleSlash, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(Percent, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(Less, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(LeftShift, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(LessOrEqual, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(Assignment, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(Equal, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(NotEqual, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(Greater, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(GreaterOrEqual, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(RightShift, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(BitAnd, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(BitXor, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(BitOr, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(BitNot, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(EndMarker, scan.Lexeme);
        }

        [TestMethod()]
        public void Keywords()
        {
            var scan = new Scanner("Code/Scanner/Keywords.qk");
            Assert.AreEqual(KwAnd, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(KwDef, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(KwFalse, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(KwNot, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(KwOr, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(KwPass, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(KwTrue, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(EndMarker, scan.Lexeme);
        }

        [TestMethod()]
        public void Names()
        {
            var scan = new Scanner("Code/Scanner/Names.qk");
            Assert.AreEqual(Id, scan.Lexeme);
            Assert.AreEqual("Id", scan.TextValue());
            scan.Next();
            Assert.AreEqual(Id, scan.Lexeme);
            Assert.AreEqual("_x", scan.TextValue());
            scan.Next();
            Assert.AreEqual(Id, scan.Lexeme);
            Assert.AreEqual("проверка", scan.TextValue());
            scan.Next();
            Assert.AreEqual(EndMarker, scan.Lexeme);
        }

        [TestMethod()]
        public void Indents()
        {
            var scan = new Scanner("Code/Scanner/Indents.qk");
            // ·# comment¶
            // x¶
            Assert.AreEqual(Id, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(NewLine, scan.Lexeme);
            scan.Next();
            // ··x¶
            Assert.AreEqual(Indent, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(Id, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(NewLine, scan.Lexeme);
            scan.Next();
            // ··x¶
            Assert.AreEqual(Id, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(NewLine, scan.Lexeme);
            scan.Next();
            // → x¶
            Assert.AreEqual(Indent, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(Id, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(NewLine, scan.Lexeme);
            scan.Next();
            // ··# comment¶
            // ····x¶
            Assert.AreEqual(Id, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(NewLine, scan.Lexeme);
            scan.Next();
            // ··x¶
            Assert.AreEqual(Dedent, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(Id, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(NewLine, scan.Lexeme);
            scan.Next();
            // ¶
            // ··x¶
            Assert.AreEqual(Id, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(NewLine, scan.Lexeme);
            scan.Next();
            // x¶
            Assert.AreEqual(Dedent, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(Id, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(NewLine, scan.Lexeme);
            scan.Next();
            // → x¶
            Assert.AreEqual(Indent, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(Id, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(NewLine, scan.Lexeme);
            scan.Next();
            // → → x¶
            Assert.AreEqual(Indent, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(Id, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(NewLine, scan.Lexeme);
            scan.Next();
            // x
            Assert.AreEqual(Dedent, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(Dedent, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(Id, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(EndMarker, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(EndMarker, scan.Lexeme);
        }

        [TestMethod()]
        public void Integers()
        {
            var scan = new Scanner("Code/Scanner/Integers.qk");
            Assert.AreEqual(Int, scan.Lexeme);
            Assert.AreEqual(12300, scan.ToInt());
            scan.Next();
            Assert.AreEqual(EndMarker, scan.Lexeme);
        }

        [TestMethod()]
        public void Floats()
        {
            var scan = new Scanner("Code/Scanner/Floats.qk");
            Assert.AreEqual(Float, scan.Lexeme);
            Assert.AreEqual(3.14f, scan.ToSingle());
            scan.Next();
            Assert.AreEqual(Float, scan.Lexeme);
            Assert.AreEqual(10f, scan.ToSingle());
            scan.Next();
            Assert.AreEqual(Float, scan.Lexeme);
            Assert.AreEqual(.001f, scan.ToSingle());
            scan.Next();
            Assert.AreEqual(EndMarker, scan.Lexeme);
        }

        [TestMethod()]
        public void NewLines()
        {
            var scan = new Scanner("Code/Scanner/NewLines.qk");
            Assert.AreEqual(RightParenthesis, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(NewLine, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(LeftParenthesis, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(RightParenthesis, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(NewLine, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(LeftParenthesis, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(RightParenthesis, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(NewLine, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(LeftParenthesis, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(LeftParenthesis, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(LeftParenthesis, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(RightParenthesis, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(RightParenthesis, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(RightParenthesis, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(NewLine, scan.Lexeme);
            scan.Next();
            Assert.AreEqual(EndMarker, scan.Lexeme);
        }
    }
}