using System;
using System.Collections.Generic;
using System.Linq;

namespace WordPuzzleHelper
{
    public class UnknownWord
    {
        public const char UnknownToken = '?';
        private readonly char[] _newWordTemplate;

        /// <summary>
        /// Give a pattern for the unknown word with known letters filled in and the UnknownToken used for missing letters
        /// </summary>
        /// <param name="pattern"></param>
        public UnknownWord(string pattern)
        {
            if (pattern == null)
            {
                throw new ArgumentNullException(nameof(pattern));
            }

            if (string.IsNullOrWhiteSpace(pattern))
            {
                throw new ArgumentException($"{nameof(pattern)} cannot be only whitespace");
            }

            pattern = pattern.Trim();
            if (pattern.Any(char.IsWhiteSpace))
            {
                throw new ArgumentException($"The word pattern had some unexpected white space in it (ie it is several words): {pattern}");
            }

            WordPattern = pattern.ToLower();
            _newWordTemplate = WordPattern.ToCharArray();
            UnknownCharCount = WordPattern.Count(c => c == UnknownToken);
        }

        public readonly string WordPattern;
        public readonly int UnknownCharCount;

        /// <summary>
        /// Some crosswords have phrases made up of sub-words, the clue usually indicates the char length for each sub-word
        /// </summary>
        public int[] SubWordCounts { get; private set; }

        /// <summary>
        /// Set the sub-word counts, for example if the solved clue is THATSALLFOLKS then subWordCounts would be new[]{5, 3, 5}
        /// </summary>
        /// <param name="subWordCounts"></param>
        public void SetSubWordCounts(int[] subWordCounts)
        {
            if (subWordCounts != null)
            {
                var subWordLength = subWordCounts.Sum();
                if (subWordLength != WordPattern.Length)
                {
                    throw new ArgumentException("Summing all the word counts is " + subWordLength + " but unknown word has " + WordPattern.Length + " letters.");
                }
            }

            SubWordCounts = subWordCounts;
        }

        /// <summary>
        /// Returns a new string array with the unknown characters filled in with the provided letters.
        /// </summary>
        /// <param name="letters">Must have as many letters as there are unknown characters</param>
        /// <returns></returns>
        public string[] FillInUnknown(IList<char> letters)
        {
            var letterCnt = letters.Count;
            if (letterCnt != UnknownCharCount)
            {
                throw new ArgumentException($"You provided {letterCnt} letters but the unknown characters are {UnknownCharCount}.  These 2 counts must be the same.");
            }

            var letterIndex = 0;

            var wordLen = _newWordTemplate.Length;
            var newWordArr = new char[wordLen];
            Array.Copy(_newWordTemplate, newWordArr, wordLen);
            for (var i = 0; i < wordLen; i++)
            {
                var letter = newWordArr[i];
                if (letter == UnknownToken)
                {
                    newWordArr[i] = letters[letterIndex++];
                }
            }

            var newWord = new string(newWordArr);
            if (SubWordCounts == null || SubWordCounts.Any() == false)
            {
                return new[] { newWord };
            }

            var newSubWords = new string[SubWordCounts.Length];
            var currentIdx = 0;
            for (var i=0; i<newSubWords.Length; i++)
            {
                var subWord = newWord.Substring(currentIdx, SubWordCounts[i]);
                newSubWords[i] = subWord;
                currentIdx += SubWordCounts[i];
            }

            return newSubWords;
        }
    }
}