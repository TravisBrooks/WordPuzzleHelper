using System;
using System.Collections.Generic;
using System.Linq;
using WordPuzzleHelper.Util;

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
        /// <param name="pattern"></param>
        /// <param name="availableLetters"></param>
        /// <returns></returns>
        public IEnumerable<string> Solve(string pattern, char[][] availableLetters)
        {
            if (pattern == null)
            {
                throw new ArgumentNullException(nameof(pattern));
            }
            if (availableLetters == null)
            {
                throw new ArgumentNullException(nameof(availableLetters));
            }
            var unknownWord = new UnknownWord(pattern);
            var wordLen = unknownWord.WordPattern.Length;
            if (wordLen != availableLetters.Length)
            {
                throw new ArgumentException($"The partial word had a length of {wordLen} but the available letters only had {availableLetters.Length} slots.");
            }

            var matches = _WordSlideSearch(KnownWords, unknownWord, availableLetters);
            return matches;
        }

        private static IEnumerable<string> _WordSlideSearch(KnownWords knownWords, UnknownWord unknownWord, char[][] availableLetters)
        {
            if (_AllUnknownWordLengthsReasonable(knownWords, unknownWord) == false)
            {
                return new HashSet<string>();
            }

            availableLetters = _EnsureAllLower(availableLetters);
            var searchLetters = _FillInSearchLettersFromPartialWord(unknownWord, availableLetters);

            var wordLen = unknownWord.WordPattern.Length;
            var possibleWords = knownWords.AllWordsOfLength(wordLen);
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

        private static char[][] _EnsureAllLower(char[][] availableLetters)
        {
            var lower = new List<char[]>();
            foreach (var letters in availableLetters)
            {
                lower.Add(new string(letters).ToAlphabetArray());
            }
            return lower.ToArray();
        }

        private static bool _AllUnknownWordLengthsReasonable(KnownWords knownWords, UnknownWord unknownWord)
        {
            // TODO: if i ever get around to implementing subwords this fancier check will be needed.
            //if (unknownWord.SubWordCounts == null || unknownWord.SubWordCounts.Any() == false)
            //{
            //    if (knownWords.AllWordsOfLength(unknownWord.WordPattern.Length).Count == 0)
            //    {
            //        return false;
            //    }
            //}
            //else
            //{
            //    if (unknownWord.SubWordCounts.Any(cnt => knownWords.AllWordsOfLength(cnt).Count == 0))
            //    {
            //        return false;
            //    }
            //}

            return knownWords.AllWordsOfLength(unknownWord.WordPattern.Length).Any();
        }

        /// <summary>
        /// Builds an index based on char position and set of possible chars for that position
        /// </summary>
        /// <param name="partialWordOrLength"></param>
        /// <param name="availableLetters"></param>
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
                    searchLetters[i] = new HashSet<char>(availableLetters[i]);
                }
                else
                {
                    searchLetters[i] = new HashSet<char>(new[] { c });
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
