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
            originalWord = (originalWord ?? string.Empty).Trim();
            if (originalWord.Length < 2)
            {
                throw new ArgumentException("You cannot create an anagram of a word unless there are at least 2 letters");
            }
            var wordPattern = new string(UnknownWord.UnknownToken, originalWord.Length);
            var unknownWord = new UnknownWord(wordPattern);
            var letters = originalWord.ToAlphabetArray();
            var matches = WordSearcher.Search(KnownWords, unknownWord, letters)
                                      .Where(ana => string.Equals(originalWord, ana) == false)
                                      .ToList();
            return matches;
        }

    }
}
