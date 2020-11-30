using System.Collections.Generic;

namespace WordPuzzleHelper.Util
{
    public static class StringUtil
    {
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
