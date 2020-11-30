using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WordPuzzleHelper;

namespace WordPuzzleHelperConsole
{
    static class Program
    {

        static void Main(string[] args)
        {
            var knownWords = new KnownWords(ConfigValues.WordFileName);
            ConsoleController.MainMenu(knownWords);
        }

    }
}
