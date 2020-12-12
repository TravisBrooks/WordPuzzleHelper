using System;
using System.Collections.Generic;
using System.Linq;
using WordPuzzleHelper;
using WordPuzzleHelper.Puzzle;
using WordPuzzleHelper.Util;

namespace WordPuzzleHelperConsole
{

    /// <summary>
    /// This class is a work in progress, I know i want some sort of abstraction about how the menus and stuff are supposed to work I'm just not sure
    /// really what I want to do yet. This first attempt is to make something that functions so there is something to compare alt designs to
    /// </summary>
    public static class ConsoleController
    {
        public static void MainMenu(KnownWords knownWords)
        {
            Console.WriteLine("Enter the number for what type of puzzle you are trying to solve:");
            Console.WriteLine("1. Anagram");
            Console.WriteLine("2. Crossword");
            Console.WriteLine("3. Word Slide");
            //Console.WriteLine("4. Scrabble");
            var choiceStr = _ReadLn();
            if (int.TryParse(choiceStr, out var choice))
            {
                try
                {
                    switch (choice)
                    {
                        case 1:
                            AnagramMenu(knownWords);
                            break;
                        case 2:
                            CrosswordMenu(knownWords);
                            break;
                        case 3:
                            WordSlideMenu(knownWords);
                            break;
                        //case 4:
                        //    ScrabbleMenu(knownWords);
                        //    break;
                        default:
                            Console.WriteLine("The menu option " + choice + " is not available.");
                            SolveMoreOrQuit(knownWords);
                            break;
                    }
                }
                catch (PrintToConsoleException pce)
                {
                    Console.WriteLine(pce.Message);
                    SolveMoreOrQuit(knownWords);
                }
            }
            else
            {
                Console.WriteLine("The menu option " + choiceStr + " is not available.");
                SolveMoreOrQuit(knownWords);
            }
        }

        public static void SolveMoreOrQuit(KnownWords knownWords)
        {
            Console.WriteLine();
            Console.WriteLine("Press ENTER to solve another puzzle, or type any key + ENTER to quit.");
            var ans = Console.ReadLine()?.Trim() ?? "ctrlZ";
            if (string.Empty.Equals(ans))
            {
                Console.WriteLine();
                MainMenu(knownWords);
            }
        }

        public static void AnagramMenu(KnownWords knownWords)
        {
            Console.WriteLine("Type in the word you want to find anagrams for:");
            var word = _ReadLn();
            var anagramPuzzle = new AnagramPuzzle(knownWords);
            var answers = anagramPuzzle.Solve(word).ToList();
            Console.WriteLine($"There are {answers.Count} possible anagrams:");
            foreach (var answer in answers)
            {
                Console.WriteLine(answer);
            }
            SolveMoreOrQuit(knownWords);
        }

        public static void CrosswordMenu(KnownWords knownWords)
        {
            Console.WriteLine($"Enter what you know about the word with {UnknownWord.UnknownToken} for missing characters:");
            var wordPattern = _ReadUnknownWord();

            Console.WriteLine("If there is more than 1 word enter the letter count of each word followed by space, if 1 word just ENTER.");
            Console.WriteLine("Example: if the solved clue is THATSALLFOLKS then the letter counts to type are '5 3 5':");
            var subWordCount = _ReadSubWordCount();

            var crosswordPuzzle = new CrosswordPuzzle(knownWords);
            var answers = crosswordPuzzle.Solve(wordPattern, subWordCount).ToList();
            Console.WriteLine($"There are {answers.Count} possible solutions:");
            foreach (var answer in answers)
            {
                Console.WriteLine(answer);
            }

            SolveMoreOrQuit(knownWords);
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
                if (int.TryParse(str, out var n))
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

        //public static void ScrabbleMenu(KnownWords knownWords)
        //{
        //    Console.WriteLine($"Enter what you know about the word with {UnknownWord.UnknownToken} for missing characters:");
        //    var wordPattern = _ReadUnknownWord();

        //    Console.WriteLine("Type available letters as a single string with no spaces:");
        //    var availableLetters = _ReadAvailableLetters();

        //    var scrabblePuzzle = new ScrabblePuzzle(knownWords);
        //    var answers = scrabblePuzzle.Solve(wordPattern, availableLetters).ToList();

        //    Console.WriteLine($"There are {answers.Count} possible solutions:");
        //    foreach (var answer in answers)
        //    {
        //        Console.WriteLine(answer);
        //    }

        //    SolveMoreOrQuit(knownWords);
        //}

        private static string _ReadLn()
        {
            var ln = (Console.ReadLine() ?? string.Empty).Trim();
            return ln;
        }

        private static char[] _ReadAvailableLetters()
        {
            var letterStr = _ReadLn();
            if (string.IsNullOrWhiteSpace(letterStr))
            {
                throw new PrintToConsoleException("That was a pretty bad selection of letters, don't think you're solving this puzzle...");
            }

            var letters = letterStr.ToAlphabetArray();
            return letters;
        }

        private static UnknownWord _ReadUnknownWord()
        {
            var wordPattern = _ReadLn();
            if (string.IsNullOrWhiteSpace(wordPattern))
            {
                throw new PrintToConsoleException("That word pattern looks a bit brief, don't think you're solving this puzzle...");
            }
            var unknownPattern = new UnknownWord(wordPattern);
            return unknownPattern;
        }

        public static void WordSlideMenu(KnownWords knownWords)
        {
            Console.WriteLine($"Enter what you know about the word with {UnknownWord.UnknownToken} for missing characters:");
            var wordPattern = _ReadUnknownWord();

            char[][] availableLetters = _ReadWordSlideLetters(wordPattern);
            var wordSlide = new WordSlidePuzzle(knownWords);
            var answers = wordSlide.Solve(wordPattern, availableLetters).ToList();

            Console.WriteLine($"There are {answers.Count} possible solutions:");
            foreach (var answer in answers)
            {
                Console.WriteLine(answer);
            }

            SolveMoreOrQuit(knownWords);
        }

        private static char[][] _ReadWordSlideLetters(UnknownWord wordPattern)
        {
            var wordLen = wordPattern.WordPattern.Length;
            Console.WriteLine("Enter the available letters for each position in the word, starting at position 1:");
            var availableLetters = new char[wordLen][];
            for (var i = 0; i < wordLen; i++)
            {
                var c = wordPattern.WordPattern[i];
                if (c == UnknownWord.UnknownToken)
                {
                    Console.WriteLine("position " + (i+1) + " enter possible letters without spaces then press enter:");
                    var possibleLetters = _ReadLn();
                    availableLetters[i] = possibleLetters.ToAlphabetArray();
                }
                else
                {
                    Console.WriteLine("position " + (i+1) + ", its known the character is " + c + ", moving to next position...");
                    availableLetters[i] = new char[0];
                }
            }

            return availableLetters;
        }
    }
}
