using System;
using System.Collections.Generic;
using System.Linq;

namespace WordPuzzleHelper.Puzzle
{
    public class AnagramPuzzle : BasePuzzle
    {
        public AnagramPuzzle(KnownWords knownWords) : base(knownWords)
        {
        }

        public IEnumerable<string> Solve(string originalWord)
        {
            if (originalWord == null || originalWord.Length < 2)
            {
                throw new ArgumentException("You cannot create an anagram of a word unless there is at least 2 letters");
            }
            var wordPattern = new string('?', originalWord.Length);
            var unknownWord = new UnknownWord(wordPattern);
            var letters = originalWord.Select(c => c.ToString()).ToList();
            var matches = WordSearcher.Search(_knownWords, unknownWord, letters).Where(ana => string.Equals(originalWord, ana) == false).ToList();

            return matches;
        }

    }
}
