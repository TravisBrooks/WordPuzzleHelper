using System;
using System.Collections.Generic;
using System.IO;

namespace WordPuzzleHelper
{
    public class KnownWords
    {
        private readonly string _wordFileName;
        private readonly Lazy<Dictionary<int, HashSet<string>>> _lenToWordsIndex;

        public KnownWords(string wordFileName)
        {
            if (File.Exists(wordFileName) == false)
            {
                wordFileName = Path.Combine(Directory.GetCurrentDirectory(), wordFileName);
                if (File.Exists(wordFileName) == false)
                {
                    throw new ArgumentException("could not find file: " + wordFileName);
                }
            }

            _wordFileName = wordFileName;
            _lenToWordsIndex = new Lazy<Dictionary<int, HashSet<string>>>(_LazyLoadWords);
        }

        public HashSet<string> AllWordsOfLength(int wordLen)
        {
            if (_lenToWordsIndex.Value.ContainsKey(wordLen) == false)
            {
                return new HashSet<string>();
            }

            return _lenToWordsIndex.Value[wordLen];
        }

        public bool IsKnownWord(string word)
        {
            var wordSet = AllWordsOfLength(word.Length);
            var isKnown = wordSet.Contains(word);
            return isKnown;
        }

        private Dictionary<int, HashSet<string>> _LazyLoadWords()
        {
            var lenToWords = new Dictionary<int, HashSet<string>>();
            using (var file = new StreamReader(_wordFileName))
            {
                string line = null;
                while ((line = file.ReadLine()) != null)
                {
                    line = line.Trim().ToLower();
                    var len = line.Length;
                    if (lenToWords.ContainsKey(len) == false)
                    {
                        lenToWords[len] = new HashSet<string>();
                    }

                    lenToWords[len].Add(line);
                }
            }

            return lenToWords;
        }
    }
}
