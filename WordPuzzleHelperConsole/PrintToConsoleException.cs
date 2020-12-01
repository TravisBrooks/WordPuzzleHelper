using System;

namespace WordPuzzleHelperConsole
{
    public class PrintToConsoleException : Exception
    {
        public PrintToConsoleException(string message): base(($"ERROR: {message}"))
        {
        }
    }
}
