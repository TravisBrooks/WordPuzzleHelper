using System;

namespace WordPuzzleHelper
{
    public class PrintToConsoleException : Exception
    {
        public PrintToConsoleException(string message): base(($"ERROR: {message}"))
        {
        }
    }
}
