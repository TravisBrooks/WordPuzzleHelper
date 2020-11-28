using System.Collections.Generic;

namespace WordPuzzleHelper.Puzzle
{
    public class CrosswordPuzzle : BasePuzzle
    {
        public CrosswordPuzzle(KnownWords knownWords) : base(knownWords)
        {
        }

        public IEnumerable<string> Solve(string wordPattern, int[] subWordCount)
        {
            var letters = ConfigValues.DefaultAlphabet;
            var unknownWord = new UnknownWord(wordPattern);
            unknownWord.SetSubWordCounts(subWordCount);
            var matches = WordSearcher.Search(_knownWords, unknownWord, letters);

            return matches;
        }

    }
}
