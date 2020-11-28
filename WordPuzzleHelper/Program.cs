using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace WordPuzzleHelper
{
    static class Program
    {
        private static string _ReadLn()
        {
            var ln = (Console.ReadLine() ?? string.Empty).Trim();
            return ln;
        }

        private static List<string> _ReadAvailableLetters()
        {
            var letterStr = _ReadLn();
            if (string.IsNullOrWhiteSpace(letterStr))
            {
                //throw new PrintToConsoleException("That was a pretty bad selection of letters, don't think you're solving this puzzle bud.");
                return ConfigValues.DefaultAlphabet;
            }

            var letters = letterStr.Select(c => c.ToString()).ToList();
            return letters;
        }

        private static UnknownWord _ReadUnknownWord()
        {
            var wordPattern = _ReadLn();
            if (string.IsNullOrWhiteSpace(wordPattern))
            {
                throw new PrintToConsoleException("That word pattern looks a bit brief, don't think you're solving this puzzle bud.");
            }
            var unknownPattern = new UnknownWord(wordPattern);
            return unknownPattern;
        }

        private static int[] _ReadSubWordCount()
        {
            var letterCntPattern = _ReadLn();
            if (letterCntPattern == string.Empty)
            {
                return null;
            }

            var cntStrs = letterCntPattern.Split(null);
            var intList = new List<int>();
            foreach (var str in cntStrs)
            {
                if (int.TryParse(str, out int n))
                {
                    intList.Add(n);
                }
                else
                {
                    throw new PrintToConsoleException($"When entering the word counts you typed something that was not an integer: {str}");
                }
            }

            return intList.ToArray();
        }


        // Nov 21 2020
        // I think what i really want is instead of a do it all single thing is to have different options to do specific types of puzzles:
        // Crossword
        // Anagram
        // Scrabble
        // Raw access (current behavior)


        // this is a temporary experiment, planning on redoing the console menu
        //static void Main_Faster(string[] args)
        //{
        //    var knownWords = new KnownWords(ConfigValues.WordFileName);
        //    Console.WriteLine("Enter word pattern, use ? for unknown letters");
        //    var wordPattern = _ReadLn();
        //    var wordsOfCorrectLen = knownWords.AllWordsOfLength(wordPattern.Length);
        //    var matches = wordsOfCorrectLen.Where(word => _WordMatchesPattern(word, wordPattern)).OrderBy(w => w);
        //    Console.WriteLine("Matches:\r\n" + string.Join("\r\n", matches));
        //}

        //private static bool _WordMatchesPattern(string word, string wordPattern)
        //{
        //    for (var i=0; i<word.Length; i++)
        //    {
        //        var patternChar = wordPattern[i];
        //        if (patternChar == '?')
        //        {
        //            continue;
        //        }

        //        var wordChar = word[i];
        //        if (wordChar != patternChar)
        //        {
        //            return false;
        //        }
        //    }

        //    return true;
        //}

        static void Main(string[] args)
        {
            var knownWords = new KnownWords(ConfigValues.WordFileName);
            var runLoop = true;
            var stopwatch = new Stopwatch();
            while (runLoop)
            {
                try
                {
                    Console.WriteLine("Type available letters as a single string, or just ENTER to use default alphabet:");
                    var letters = _ReadAvailableLetters();

                    Console.WriteLine($"Enter what you know about the word with {UnknownWord.UnknownToken} for missing characters:");
                    var unknownWord = _ReadUnknownWord();

                    Console.WriteLine("If there is more than 1 word enter letter count of each word followed by space, if 1 word just ENTER.");
                    Console.WriteLine("Example: if the solved clue is THATSALLFOLKS then the letter counts to type are '5 3 5':");
                    var subwordCount = _ReadSubWordCount();
                    unknownWord.SetSubWordCounts(subwordCount);

                    stopwatch.Start();
                    var matches = WordSearcher.Search(knownWords, unknownWord, letters);
                    stopwatch.Stop();
                    var totalTime = stopwatch.Elapsed;
                    stopwatch.Reset();
                    if (matches.Any())
                    {
                        Console.WriteLine($"Found {matches.Count()} potential matches:");
                        foreach (var match in matches)
                        {
                            Console.WriteLine(match);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No Matches found...");
                    }

                    Console.WriteLine(totalTime.TotalMilliseconds + " total milliseconds to search");
                    Console.WriteLine();
                    //runLoop = false;
                }
                catch (PrintToConsoleException pce)
                {
                    Console.WriteLine(pce.Message);
                    Console.WriteLine();
                }
            }
        }
    }
}
