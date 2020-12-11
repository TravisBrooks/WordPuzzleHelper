using System.Collections.Generic;
using System.Linq;
using WordPuzzleHelper.Util;

namespace WordPuzzleHelper
{
    public static class WordSearcher
    {
        public static IEnumerable<string> Search(KnownWords knownWords, UnknownWord unknownWord, char[] availableLetters)
        {
            if (_AllUnknownWordLengthsReasonable(knownWords, unknownWord) == false)
            {
                return new HashSet<string>();
            }

            var potentialMatches = new HashSet<string>();
            // This word search algorithm is more difficult than necessary because it takes the availableLetters param. Most word puzzles
            // don't have a funny alphabet or a fixed pool of known letters (like scrabble) so this extra work is not necessary.
            // Contemplating a fast/simple version for the typical crossword style puzzle
            // TODO: the permutation thing seems non-optimal. Instead should try to filter the known words.
            foreach (var replacementLetters in Permutations.OfSampleSize(availableLetters, unknownWord.UnknownCharCount))
            {
                var subWords = unknownWord.FillInUnknown(replacementLetters);
                if (subWords.All(knownWords.IsKnownWord))
                {
                    var newWord = string.Join(" ", subWords);
                    potentialMatches.Add(newWord);
                }
            }

            return potentialMatches.OrderBy(w => w).ToList();
        }

        public static IEnumerable<string> Search(KnownWords knownWords, UnknownWord unknownWord, char[][] availableLetters)
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
            if (unknownWord.SubWordCounts == null || unknownWord.SubWordCounts.Any() == false)
            {
                if (knownWords.AllWordsOfLength(unknownWord.WordPattern.Length).Count == 0)
                {
                    return false;
                }
            }
            else
            {
                if (unknownWord.SubWordCounts.Any(cnt => knownWords.AllWordsOfLength(cnt).Count == 0))
                {
                    return false;
                }
            }

            return true;
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
