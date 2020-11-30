using System;
using System.Collections.Generic;
using System.Linq;

namespace WordPuzzleHelper.Puzzle
{
    public class WordSlidePuzzle : BasePuzzle
    {
        public WordSlidePuzzle(KnownWords knownWords) : base(knownWords)
        {
        }

        // TODO: move all of this logic into WordSearcher

        /// <summary>
        /// Solves a "word slide" type puzzle, some apps call this "quote slide" or something similar. This type of puzzle is where you
        /// fill in the blank words in a phrase by sliding individual characters down to make up the words in the phrase, with words
        /// stacked on top of each other so there are multiple characters possible for each slot in a word.
        /// </summary>
        /// <param name="partialWordOrLength"></param>
        /// <param name="availableLetters"></param>
        /// <returns></returns>
        public IEnumerable<string> Solve(UnknownWord partialWordOrLength, char[][] availableLetters)
        {
            var wordLen = partialWordOrLength.WordPattern.Length;
            if (wordLen != availableLetters.Length)
            {
                throw new ArgumentException($"The partial word had a length of {wordLen} but the available letters only had {availableLetters.Length} slots.");
            }

            var searchLetters = _FillInSearchLettersFromPartialWord(partialWordOrLength, availableLetters);

            var possibleWords = KnownWords.AllWordsOfLength(wordLen);
            var matches = new List<string>();
            foreach (var word in possibleWords)
            {
                if (_IsPossibleMatch(searchLetters, word))
                {
                    matches.Add(word);
                }
            }

            matches = matches.OrderBy(w => w).ToList();
            return matches;
        }

        /// <summary>
        /// Builds an index based on char position and set of possible chars for that position
        /// </summary>
        /// <param name="availableLetters"></param>
        /// <param name="partialWordOrLength"></param>
        /// <param name="wordLen"></param>
        /// <returns></returns>
        private static HashSet<char>[] _FillInSearchLettersFromPartialWord(
            UnknownWord partialWordOrLength,
            char[][] availableLetters
            )
        {
            var searchLetters = new HashSet<char>[availableLetters.Length];
            for (var i = 0; i < availableLetters.Length; i++)
            {
                var c = partialWordOrLength.WordPattern[i];
                if (c == UnknownWord.UnknownToken)
                {
                    searchLetters[i] =  new HashSet<char>(availableLetters[i]);
                }
                else
                {
                    searchLetters[i] = new HashSet<char>(new []{c});
                }
            }

            return searchLetters;
        }

        private static bool _IsPossibleMatch(HashSet<char>[] searchLetters, string word)
        {
            for (var i = 0; i < searchLetters.Length; i++) 
            {
                if (searchLetters[i].Contains(word[i]) == false)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
