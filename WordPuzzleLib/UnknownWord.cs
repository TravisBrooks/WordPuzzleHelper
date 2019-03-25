using System;
using System.Collections.Generic;
using System.Linq;

namespace WordPuzzleLib
{
    public class UnknownWord
    {
        public const string UnknownToken = "?";
        private string[] _newWordTemplate;

        /// <summary>
        /// Give a pattern for the unknown word with known letters filled and and UnknownToken used for missing letters
        /// </summary>
        /// <param name="wordPattern"></param>
        public UnknownWord(string wordPattern)
        {
            WordPattern = wordPattern.Trim().ToLower();
            _newWordTemplate = new string[wordPattern.Length];
            UnknownCharCount = wordPattern.Length - (wordPattern.Replace(UnknownToken, string.Empty)).Length;
            for (var i = 0; i < wordPattern.Length; i++)
            {
                var letter = wordPattern[i].ToString();
                _newWordTemplate[i] = letter;
            }
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
        /// <param name="letters">Must have as many letters as there were unknown characters</param>
        /// <returns></returns>
        public string[] FillInUnknown(IList<string> letters)
        {
            var letterCnt = letters.Count;
            if (letterCnt != UnknownCharCount)
            {
                throw new ArgumentException("You provided " + letterCnt + " letters but the unknown characters are " + UnknownCharCount + ".  These 2 counts must be the same.");
            }

            var letterIndex = 0;

            var wordLen = _newWordTemplate.Length;
            var newWordArr = new string[wordLen];
            Array.Copy(_newWordTemplate, newWordArr, wordLen);
            for (var i = 0; i < wordLen; i++)
            {
                var letter = newWordArr[i];
                if (letter == UnknownToken)
                {
                    newWordArr[i] = letters[letterIndex++].ToLower();
                }
            }

            var newWord = string.Join(String.Empty, newWordArr);
            if (SubWordCounts == null || Enumerable.Any<int>(SubWordCounts) == false)
            {
                return new[] { newWord };
            }

            var newSubWords = new string[SubWordCounts.Length];
            var currIdx = 0;
            for (var i=0; i<newSubWords.Length; i++)
            {
                var subWord = newWord.Substring(currIdx, SubWordCounts[i]);
                newSubWords[i] = subWord;
                currIdx = SubWordCounts[i];
            }

            return newSubWords;
        }
    }
}
