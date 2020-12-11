using System.Collections.Generic;

namespace WordPuzzleHelper.Util
{
    public static class StringUtil
    {
        /// <summary>
        /// This is similar to the string.ToCharArray() method, but removes all whitespace and lower cases all letters. Duplicate letters are allowed.
        /// </summary>
        /// <param name="allLetters"></param>
        /// <returns></returns>
        public static char[] ToAlphabetArray(this string allLetters)
        {
            if (allLetters == null)
            {
                return new char[0];
            }

            allLetters = allLetters.ToLower();
            var ls = new List<char>();
            foreach (var letter in allLetters)
            {
                if (char.IsWhiteSpace(letter))
                {
                    continue;
                }
                ls.Add(letter);
            }

            return ls.ToArray();
        }
    }
}
