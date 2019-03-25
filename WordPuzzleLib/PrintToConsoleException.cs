using System;

namespace WordPuzzleLib
{
    public class PrintToConsoleException : Exception
    {
        public PrintToConsoleException(string message): base((string) ($"ERROR: {message}"))
        {
        }
    }
}
