using System;
using System.Collections.Generic;
using System.Linq;
using WordPuzzleHelper.Util;

namespace WordPuzzleHelper.Puzzle
{
    public class AnagramPuzzle : BasePuzzle
    {
        public AnagramPuzzle(KnownWords knownWords) : base(knownWords)
        {
        }

        public IEnumerable<string> Solve(string originalWord)
        {
            var letters = originalWord.ToAlphabetArray();
            if (letters.Any(c => c == UnknownWord.UnknownToken))
            {
                throw new ArgumentException("There cannot be any unknown characters in the word to find its anagrams.");
            }

            var cleanedWord = new string(letters);
            if (letters.Length < 2)
            {
                throw new ArgumentException("You cannot create an anagram of a word unless there are at least 2 letters");
            }

            var matches = new List<string>();
            var sortedArr = _AllLettersSorted(cleanedWord);
            foreach (var potential in KnownWords.AllWordsOfLength(cleanedWord.Length))
            {
                if (string.Equals(cleanedWord, potential))
                {
                    continue;
                }

                var potentialArr = _AllLettersSorted(potential);
                if (sortedArr.SequenceEqual(potentialArr))
                {
                    matches.Add(potential);
                }
            }

            matches = matches.OrderBy(w => w).ToList();
            return matches;
        }

        private static char[] _AllLettersSorted(string word)
        {
            var letters = word.ToCharArray();
            Array.Sort(letters);
            return letters;
        }

    }
}
