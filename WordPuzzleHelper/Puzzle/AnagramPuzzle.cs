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
            var cleanedWord = new string(letters);
            if (letters.Length < 2)
            {
                throw new ArgumentException("You cannot create an anagram of a word unless there are at least 2 letters");
            }
            var wordPattern = new string(UnknownWord.UnknownToken, cleanedWord.Length);
            var unknownWord = new UnknownWord(wordPattern);
            var matches = WordSearcher.Search(KnownWords, unknownWord, letters)
                                      .Where(ana => string.Equals(cleanedWord, ana) == false)
                                      .ToList();
            return matches;
        }

    }
}
