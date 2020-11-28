using System.Collections.Generic;

namespace WordPuzzleHelper.Puzzle
{
    public class ScrabblePuzzle : BasePuzzle
    {
        public ScrabblePuzzle(KnownWords knownWords) : base(knownWords)
        {
        }

        // TODO: I'm not taking the blank tile into consideration. Its a wild card initially but after its value has been assigned once then all blank tiles have that same value.
        public IEnumerable<string> Solve(List<string> letters, string wordPattern)
        {
            var unknownWord = new UnknownWord(wordPattern);
            var matches = WordSearcher.Search(KnownWords, unknownWord, letters);
            // TODO: sort the words with the highest scrabble point value first
            return matches;
        }
    }
}
