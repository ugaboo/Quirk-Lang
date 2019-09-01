using System.IO;

namespace Quirk
{
    public class Reader
    {
        readonly string filename;
        readonly string text;
        int index;
        int line = 1;
        int column = 1;
        int lineStart;

        public char Value { get; private set; }
        public Position Position => new Position(filename, line, column, text, lineStart);


        public Reader(string filename)
        {
            this.filename = filename;
            text = File.ReadAllText(filename);
            Value = index < text.Length ? text[index] : char.MaxValue;
        }

        public void Next()
        {
            if (Value == '\r') {
                if (index + 1 < text.Length && text[index + 1] == '\n') {
                    // Windows
                    index += 2;
                } else {
                    // Mac
                    index += 1;
                }
                lineStart = index;
                line += 1;
                column = 1;
            } else if (Value == '\n') {
                // Unix
                index += 1;
                lineStart = index;
                line += 1;
                column = 1;
            } else if (Value == '\t') {
                index += 1;
                column = ((column - 1) / Settings.TabSize + 1) * Settings.TabSize + 1;
            } else if (Value != char.MaxValue) {
                index += 1;
                column += 1;
            }
            Value = index < text.Length ? text[index] : char.MaxValue;
        }
    }
}