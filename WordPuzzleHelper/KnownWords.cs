using System;
using System.Collections.Generic;
using System.IO;

namespace WordPuzzleHelper
{
    public class KnownWords
    {
        private string _wordFileName;
        private bool _loadedWords;
        private Dictionary<int, HashSet<string>> _lenToWordsIndex;

        public KnownWords(string wordFileName)
        {
            if (File.Exists(wordFileName) == false)
            {
                wordFileName = Path.Combine(System.IO.Directory.GetCurrentDirectory(), wordFileName);
                if (File.Exists(wordFileName) == false)
                {
                    throw new ArgumentException("could not find file: " + wordFileName);
                }
            }

            _wordFileName = wordFileName;
            _loadedWords = false;
        }

        public HashSet<string> AllWordsOfLength(int wordLen)
        {
            _LoadWords();
            if (_lenToWordsIndex.ContainsKey(wordLen) == false)
            {
                return new HashSet<string>();
            }

            return _lenToWordsIndex[wordLen];
        }

        private void _LoadWords()
        {
            if (_loadedWords == false)
            {
                _lenToWordsIndex = new Dictionary<int, HashSet<string>>();
                var file = new StreamReader(_wordFileName);
                string line = null;
                while ((line = file.ReadLine()) != null)
                {
                    line = line.Trim().ToLower();
                    var len = line.Length;
                    if (_lenToWordsIndex.ContainsKey(len) == false)
                    {
                        _lenToWordsIndex[len] = new HashSet<string>();
                    }
                    _lenToWordsIndex[len].Add(line);
                }
                _loadedWords = true;
            }
        }
    }
}
