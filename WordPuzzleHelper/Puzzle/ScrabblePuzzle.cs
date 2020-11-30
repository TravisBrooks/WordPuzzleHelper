using System.Collections.Generic;

namespace WordPuzzleHelper.Puzzle
{
    public class ScrabblePuzzle : BasePuzzle
    {
        public ScrabblePuzzle(KnownWords knownWords) : base(knownWords)
        {
        }

        public IEnumerable<string> Solve(UnknownWord partialWordOrLength, char[] availableLetters)
        {
            var matches = WordSearcher.Search(KnownWords, partialWordOrLength, availableLetters);
            // TODO: sort the words with the highest scrabble point value first
            // TODO: I'm not taking the blank tile into consideration. Its a wild card initially but after its value has been assigned once then all blank tiles have that same value.
            // TODO: The scrabble puzzle is pretty poorly served by whats going on here. Main issue being I don't really play scrabble that much. Maybe the word `scrabble` is wrong here???
            return matches;
        }
    }
}
