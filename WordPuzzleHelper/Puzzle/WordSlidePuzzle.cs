using System;
using System.Collections.Generic;

namespace WordPuzzleHelper.Puzzle
{
    public class WordSlidePuzzle : BasePuzzle
    {
        public WordSlidePuzzle(KnownWords knownWords) : base(knownWords)
        {
        }

        /// <summary>
        /// Solves a "word slide" type puzzle, some apps call this "quote slide" or something similar. This type of puzzle is where you
        /// fill in the blank words in a phrase by sliding individual characters down to make up the words in the phrase, with words
        /// stacked on top of each other so there are multiple characters possible for each slot in a word.
        /// </summary>
        /// <param name="unknownWord"></param>
        /// <param name="availableLetters"></param>
        /// <returns></returns>
        public IEnumerable<string> Solve(UnknownWord unknownWord, char[][] availableLetters)
        {
            var wordLen = unknownWord.WordPattern.Length;
            if (wordLen != availableLetters.Length)
            {
                throw new ArgumentException($"The partial word had a length of {wordLen} but the available letters only had {availableLetters.Length} slots.");
            }

            var matches = WordSearcher.Search(KnownWords, unknownWord, availableLetters);
            return matches;
        }

    }
}
