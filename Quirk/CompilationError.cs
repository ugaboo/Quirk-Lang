using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Quirk
{
    public enum ErrorType
    {
        UnexpectedToken,
        ExpectedAnIndentedBlock,
        InvalidSyntax,
        DuplicateParameter,
    }

    public class CompilationError : Exception
    {
        public string RootFolder = "";       // placeholder

        private static string[] messages;

        public readonly ErrorType Type;
        public readonly Position Position;


        public CompilationError(ErrorType type)
        {
            this.Type = type;
        }

        public void Print()
        {
            if (messages == null) {
                messages = File.ReadAllLines("");
            }

            var file = Path.GetFullPath(Position.Filename);
            if (file.IndexOf(RootFolder) == 0) {
                file = file.Remove(0, RootFolder.Length);
            }
            if (file.Length > 0 && file[0] == Path.DirectorySeparatorChar) {
                file = file.Remove(0, 1);
            }

            var oldColor = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{file}:{Position.Line}:{Position.Column}:");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(" Error: ");

            Console.ForegroundColor = ConsoleColor.White;
            var num = (int)Type;
            if (num < messages.Length) {
                Console.WriteLine(messages[num]);
            } else {
                Console.WriteLine($"Error #{num}");
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(Position.LineValue);

            Console.ForegroundColor = ConsoleColor.Green;
            for (var i = 1; i < Position.Column; ++i) {
                Console.Write(" ");
            }
            Console.WriteLine("^");

            Console.ForegroundColor = oldColor;
        }
    }
}
