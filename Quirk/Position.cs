namespace Quirk
{
    public struct Position
    {
        private readonly string text;
        private readonly int lineStart;

        public readonly string Filename;
        public readonly int Line;
        public readonly int Column;


        public Position(string filename, int line, int column, string text, int lineStart)
        {
            this.text = text;
            this.lineStart = lineStart;

            Filename = filename;
            Line = line;
            Column = column;
        }

        public string LineValue {
            get {
                var end = lineStart;
                while (end < text.Length && text[end] != '\r' && text[end] != '\n') {
                    end += 1;
                }
                return text.Substring(lineStart, end - lineStart);
            }
        }
    }
}