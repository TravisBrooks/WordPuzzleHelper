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
        /// <param name="availableLetters"></param>
        /// <param name="partialWordOrLength"></param>
        /// <returns></returns>
        public IEnumerable<string> Solve(char[][] availableLetters, UnknownWord partialWordOrLength)
        {
            var wordLen = partialWordOrLength.WordPattern.Length;
            var searchLetters = _FillInSearchLettersFromPartialWord(availableLetters, partialWordOrLength, wordLen);

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
            char[][] availableLetters,
            UnknownWord partialWordOrLength,
            int wordLen
            )
        {
            var searchLetters = new HashSet<char>[wordLen];
            for (var i = 0; i < wordLen; i++)
            {
                var c = partialWordOrLength.WordPattern[i];
                if (c == UnknownWord.UnknownToken[0]) // TODO: de-string things throughout app when char will work just fine
                {
                    searchLetters[i] =  new HashSet<char>(availableLetters[i]);;
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
