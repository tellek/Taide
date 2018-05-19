using System;

namespace taide.Helpers {
    public static class Cli {
        public static void PrintLine(string text){
            Console.WriteLine(text);
        }

        public static void PrintLine(string text, ConsoleColor color){
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        public static void Print(string text){
            Console.Write(text);
        }

        public static void Print(string text, ConsoleColor color){
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }
    }
    
}

