using System.Collections.Generic;

namespace WordPuzzleHelper.Puzzle
{
    public class CrosswordPuzzle : BasePuzzle
    {
        private readonly string[] _defaultAlphabet;

        public CrosswordPuzzle(KnownWords knownWords, string[] defaultAlphabet) : base(knownWords)
        {
            _defaultAlphabet = defaultAlphabet;
        }

        public IEnumerable<string> Solve(string wordPattern, int[] subWordCount)
        {
            var unknownWord = new UnknownWord(wordPattern);
            unknownWord.SetSubWordCounts(subWordCount);
            var matches = WordSearcher.Search(KnownWords, unknownWord, _defaultAlphabet);

            return matches;
        }

    }
}
