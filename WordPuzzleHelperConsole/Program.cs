using System.IO;
using WordPuzzleHelper;

namespace WordPuzzleHelperConsole
{
    static class Program
    {

        static void Main(string[] args)
        {
            var knownWords = new KnownWords(File.ReadAllLines(ConfigValues.WordFileName));
            ConsoleController.MainMenu(knownWords);
        }

    }
}
