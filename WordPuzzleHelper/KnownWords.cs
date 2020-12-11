using System;
using System.Collections.Generic;
using System.Linq;

namespace WordPuzzleHelper
{
    public class KnownWords
    {
        // Currently this data structure is basically an index based on word length. HashSet works good for exact matching but is meh for fuzzy searching
        private readonly Dictionary<int, HashSet<string>> _lenToWordsIndex;

        public KnownWords(IEnumerable<string> allKnownWords)
        {
            if (allKnownWords == null)
            {
                throw new ArgumentNullException(nameof(allKnownWords));
            }
            var lenToWords = new Dictionary<int, HashSet<string>>();
            foreach (var word in allKnownWords)
            {
                _AddKnownWord(lenToWords, word);
            }
            _lenToWordsIndex = lenToWords;
        }

        public HashSet<string> AllWordsOfLength(int wordLen)
        {
            if (wordLen < 1)
            {
                throw new ArgumentException("A known word must have at least 1 character");
            }

            if (_lenToWordsIndex.ContainsKey(wordLen) == false)
            {
                return new HashSet<string>();
            }

            return _lenToWordsIndex[wordLen];
        }

        public bool IsKnownWord(string word)
        {
            var wordSet = AllWordsOfLength(word.Length);
            var isKnown = wordSet.Contains(word);
            return isKnown;
        }

        private static void _AddKnownWord(Dictionary<int, HashSet<string>> lenToWords, string word)
        {
            if (string.IsNullOrWhiteSpace(word))
            {
                return;
            }

            word = word.Trim().ToLower();
            if (word.Any(char.IsWhiteSpace))
            {
                throw new ArgumentException($"A word in the dictionary had some unexpected white space in it (ie it is several words): {word}");
            }

            var len = word.Length;
            if (lenToWords.ContainsKey(len) == false)
            {
                lenToWords[len] = new HashSet<string>();
            }

            lenToWords[len].Add(word);
        }
    }
}
