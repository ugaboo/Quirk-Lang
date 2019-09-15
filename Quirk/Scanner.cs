using System.Globalization;
using System.Text;
using System.Collections.Generic;

namespace Quirk
{
    public class Scanner
    {
        static Dictionary<string, Lexeme> keywords = new Dictionary<string, Lexeme>() {
            { "and", Lexeme.KwAnd },
            { "def", Lexeme.KwDef },
            { "or", Lexeme.KwOr},
            { "not", Lexeme.KwNot },
            { "pass", Lexeme.KwPass },
            { "True", Lexeme.KwTrue },
            { "False", Lexeme.KwFalse },
        };

        Reader reader;

        Stack<int> indentation = new Stack<int>();
        int ignoreNewLine;

        StringBuilder textValue, intPart, floatPart, expPart;

        public Lexeme Lexeme { get; private set; }
        public Position LexemePosition { get; private set; }


        public Scanner(string filename)
        {
            indentation.Push(0);

            textValue = new StringBuilder();
            intPart = new StringBuilder();
            floatPart = new StringBuilder();
            expPart = new StringBuilder();

            reader = new Reader(filename);
            Next();
        }

        void SetLexeme(Lexeme value)
        {
            Lexeme = value;
            reader.Next();
        }

        public void Next()
        {
            do {
                Lexeme = Lexeme.Error;
                LexemePosition = reader.Position;

                textValue.Clear();
                intPart.Clear();
                floatPart.Clear();
                expPart.Clear();

                if (reader.Position.Column == 1) {
                    Indentation();
                    if (Lexeme == Lexeme.Indent || Lexeme == Lexeme.Dedent) {
                        break;
                    }
                }

                if (Whitespace()) {
                    SetLexeme(Lexeme.Ignore);
                } else if (IdStart()) {
                    Id();
                    ConvertToKeyword();
                } else if (Digit()) {
                    Int();
                } else {
                    switch (reader.Value) {
                        case char.MaxValue: SetLexeme(Lexeme.EndMarker); break;
                        case '\r':
                        case '\n':
                            if (ignoreNewLine > 0) {
                                SetLexeme(Lexeme.Ignore);
                            } else {
                                SetLexeme(Lexeme.NewLine);
                            }
                            break;
                        case '#':
                            SkipComment();
                            Lexeme = Lexeme.Ignore;
                            break;
                        case ',': SetLexeme(Lexeme.Comma); break;
                        case ':': SetLexeme(Lexeme.Colon); break;
                        case ';': SetLexeme(Lexeme.Semicolon); break;
                        case '~': SetLexeme(Lexeme.BitNot); break;
                        case '\\':
                            reader.Next();
                            switch (reader.Value) {
                                case '\r':
                                case '\n': SetLexeme(Lexeme.Ignore); break;
                                default: throw new CompilationError(ErrorType.UnexpectedToken);
                            }
                            break;
                        case '!':
                            reader.Next();
                            switch (reader.Value) {
                                case '=': SetLexeme(Lexeme.NotEqual); break;
                                default: throw new CompilationError(ErrorType.UnexpectedToken);
                            }
                            break;
                        case '<':
                            reader.Next();
                            switch (reader.Value) {
                                case '<': SetLexeme(Lexeme.LeftShift); break;
                                case '=': SetLexeme(Lexeme.LessOrEqual); break;
                                default: Lexeme = Lexeme.Less; break;
                            }
                            break;
                        case '>':
                            reader.Next();
                            switch (reader.Value) {
                                case '>': SetLexeme(Lexeme.RightShift); break;
                                case '=': SetLexeme(Lexeme.GreaterOrEqual); break;
                                default: Lexeme = Lexeme.Greater; break;
                            }
                            break;
                        case '=':
                            reader.Next();
                            switch (reader.Value) {
                                case '=': SetLexeme(Lexeme.Equal); break;
                                default: Lexeme = Lexeme.Assignment; break;
                            }
                            break;
                        case '(':
                            SetLexeme(Lexeme.LeftParenthesis);
                            ignoreNewLine += 1;
                            break;
                        case ')':
                            SetLexeme(Lexeme.RightParenthesis);
                            if (ignoreNewLine > 0) {
                                ignoreNewLine -= 1;
                            }
                            break;
                        case '+':
                            reader.Next();
                            switch (reader.Value) {
                                default: Lexeme = Lexeme.Plus; break;
                            }
                            break;
                        case '-':
                            reader.Next();
                            switch (reader.Value) {
                                default: Lexeme = Lexeme.Minus; break;
                            }
                            break;
                        case '*':
                            reader.Next();
                            switch (reader.Value) {
                                case '*': SetLexeme(Lexeme.Power); break;
                                default: Lexeme = Lexeme.Star; break;
                            }
                            break;
                        case '/':
                            reader.Next();
                            switch (reader.Value) {
                                case '/': SetLexeme(Lexeme.DoubleSlash); break;
                                default: Lexeme = Lexeme.Slash; break;
                            }
                            break;
                        case '%':
                            reader.Next();
                            switch (reader.Value) {
                                default: Lexeme = Lexeme.Percent; break;
                            }
                            break;
                        case '^':
                            reader.Next();
                            switch (reader.Value) {
                                default: Lexeme = Lexeme.BitXor; break;
                            }
                            break;
                        case '|':
                            reader.Next();
                            switch (reader.Value) {
                                default: Lexeme = Lexeme.BitOr; break;
                            }
                            break;
                        case '&':
                            reader.Next();
                            switch (reader.Value) {
                                default: Lexeme = Lexeme.BitAnd; break;
                            }
                            break;
                        case '.':
                            reader.Next();
                            if (Digit()) {
                                Float();
                            } else {
                                Lexeme = Lexeme.FullStop;
                            }
                            break;
                    }
                }
            } while (Lexeme == Lexeme.Ignore);
        }

        void Indentation()
        {
            var indent = 0;

            while (true) {
                if (reader.Value == ' ') {
                    indent += 1;
                } else if (reader.Value == '\t') {
                    indent = (indent / Settings.TabSize + 1) * Settings.TabSize;
                } else if (reader.Value == '\r' || reader.Value == '\n') {
                    // empty string, restarting
                    indent = 0;
                } else if (reader.Value == '#') {
                    SkipComment();
                    indent = 0;
                } else if (reader.Value == '\f') {
                    // ignore
                } else {
                    break;
                }
                reader.Next();
            }

            if (indent > indentation.Peek()) {
                indentation.Push(indent);
                Lexeme = Lexeme.Indent;
            } else if (indent < indentation.Peek()) {
                do {
                    indentation.Pop();
                } while (indent < indentation.Peek());

                if (indent == indentation.Peek()) {
                    Lexeme = Lexeme.Dedent;
                } else {
                    throw new CompilationError(ErrorType.ExpectedAnIndentedBlock);
                }
            } else {
                Lexeme = Lexeme.Ignore;
            }
        }

        bool IdStart()
        {
            switch (char.GetUnicodeCategory(reader.Value)) {
                case UnicodeCategory.UppercaseLetter:
                case UnicodeCategory.LowercaseLetter:
                case UnicodeCategory.TitlecaseLetter:
                case UnicodeCategory.ModifierLetter:
                case UnicodeCategory.OtherLetter:
                case UnicodeCategory.LetterNumber:
                    return true;
            }
            switch (reader.Value) {
                case '_':
                case '\u1885':
                case '\u1886':
                case '\u2118':
                case '\u212E':
                case '\u309B':
                case '\u309C':
                    return true;
            }
            return false;
        }

        bool IdContinue()
        {
            switch (char.GetUnicodeCategory(reader.Value)) {
                case UnicodeCategory.NonSpacingMark:
                case UnicodeCategory.SpacingCombiningMark:
                case UnicodeCategory.DecimalDigitNumber:
                case UnicodeCategory.ConnectorPunctuation:
                    return true;
            }
            switch (reader.Value) {
                case '\u00B7':
                case '\u0387':
                case '\u1369':
                case '\u1370':
                case '\u1371':
                case '\u19DA':
                    return true;
            }
            return IdStart();
        }

        void Id()
        {
            Lexeme = Lexeme.Id;
            do {
                textValue.Append(reader.Value);
                reader.Next();
            } while (IdContinue());
        }

        void ConvertToKeyword()
        {
            if (keywords.TryGetValue(textValue.ToString(), out var keyword)) {
                Lexeme = keyword;
            }
        }

        bool Digit()
        {
            return (reader.Value >= '0' && reader.Value <= '9');
        }

        void Int()
        {
            Lexeme = Lexeme.Int;
            while (Digit()) {
                intPart.Append(reader.Value);
                reader.Next();
            }
            if (reader.Value == '.') {
                reader.Next();
                Float();
            }
        }

        void Float()
        {
            Lexeme = Lexeme.Float;
            while (Digit()) {
                floatPart.Append(reader.Value);
                reader.Next();
            }
        }

        bool Whitespace()
        {
            return (reader.Value == ' ' || reader.Value == '\t' || reader.Value == '\f');
        }

        void SkipComment()
        {
            do {
                reader.Next();
            } while (reader.Value != '\n' && reader.Value != '\r' && reader.Value != char.MaxValue);
        }

        public int ToInt()
        {
            return int.Parse(intPart.ToString(), CultureInfo.InvariantCulture);
        }

        public float ToSingle()
        {
            return float.Parse($"{intPart}.{floatPart}", CultureInfo.InvariantCulture);
        }

        public string TextValue()
        {
            return textValue.ToString();
        }
    }
}