using System;

namespace ASP.Blog
{
    public class Logger : ILogger
    {
        public void WriteError(string errorMessage)
        {
            Console.BackgroundColor = ConsoleColor.DarkGreen; 
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(errorMessage);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void WriteEvent(string eventMessage)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(eventMessage);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
