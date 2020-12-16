using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NUnit.Framework;
using WordPuzzleHelper;
using WordPuzzleHelper.Puzzle;

namespace WordPuzzleHelperTest.Puzzle
{
    [TestFixture]
    public class WordSlidePuzzleTest
    {
        [Test]
        public void UnknownTokenKnownForTesting()
        {
            // I want to be able to use the token directly in strings here because it makes whats being tested less verbose, meaning easier to
            // to follow read tests. 
            Assert.That('?', Is.EqualTo(UnknownWord.UnknownToken), "The UnknownToken has changed, be sure to change all the tests in this file that assumed what the token was");
        }

        [Test]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public void NullUnknownCtor()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new WordSlidePuzzle(knownWords: null));
            Assert.That(ex.ParamName, Is.EqualTo("knownWords"), "had ArgumentNullException but not for expected parameter");
        }

        [Test]
        public void SolveNullUnknown()
        {
            var puzzle = new WordSlidePuzzle(new KnownWords(Enumerable.Empty<string>()));
            var ex = Assert.Throws<ArgumentNullException>(() => puzzle.Solve(pattern: null, Array.Empty<char[]>()));
            Assert.That(ex.ParamName, Is.EqualTo("pattern"), "had ArgumentNullException but not for expected parameter");
        }

        [Test]
        public void SolveNullLetters()
        {
            var puzzle = new WordSlidePuzzle(new KnownWords(Enumerable.Empty<string>()));
            var ex = Assert.Throws<ArgumentNullException>(() => puzzle.Solve("test", availableLetters: null));
            Assert.That(ex.ParamName, Is.EqualTo("availableLetters"), "had ArgumentNullException but not for expected parameter");
        }

        [Test]
        public void SolveLetterSizeMismatch()
        {
            var puzzle = new WordSlidePuzzle(new KnownWords(Enumerable.Empty<string>()));
            char[][] letters = {
                new[] {'a'},
                new[] {'b'},
                new[] {'c'}
            };
            var ex = Assert.Throws<ArgumentException>(() => puzzle.Solve("????", letters));
            Assert.That(ex.Message, Is.EqualTo("The partial word had a length of 4 but the available letters only had 3 slots."));
        }

        [Test]
        public void SolveNullSubWordAvailableLetterMismatch()
        {
            var knownWord = "four";
            var knownWords = new KnownWords(new [] {knownWord});
            //var unknownWord = new UnknownWord(new string(UnknownWord.UnknownToken, knownWord.Length + 1));
            var wordPattern = new string(UnknownWord.UnknownToken, knownWord.Length + 1);
            Assert.That(knownWords.AllWordsOfLength(wordPattern.Length), Is.Empty, "precondition: there is no known word the length of the unknown word");
            //Assert.That(unknownWord.SubWordCounts, Is.Null, "precondition: unknownWord.SubWordCounts must be null");
            var puzzle = new WordSlidePuzzle(knownWords);
            var availableLetters = new char[wordPattern.Length][];
            for (var i = 0; i < availableLetters.Length; i++)
            {
                availableLetters[i] = new[] {'X'};
            }

            var solutions = puzzle.Solve(wordPattern, availableLetters);
            Assert.That(solutions, Is.Empty, "If there are no known words of the required length the solution should be empty");
        }

        // TODO: WordSlidePuzzle doesn't currently know how to solve multiwords
        //[Test]
        //public void SolveSetSubWordAvailableLetterMismatch()
        //{
        //    var w1 = "un";
        //    var w2 = "two";
        //    var knownWords = new KnownWords(new[] { "three", "four" });
        //    var pattern = new string(UnknownWord.UnknownToken, (w1 + w2).Length);
        //    var unknownWord = new UnknownWord(pattern);
        //    unknownWord.SetSubWordCounts(new[] { w1.Length, w2.Length });
        //    Assert.That(knownWords.AllWordsOfLength(w1.Length), Is.Empty, "precondition: no known word of length of a sub word");
        //    Assert.That(unknownWord.SubWordCounts, Is.EquivalentTo(new[] { w1.Length, w2.Length }));
        //    var puzzle = new WordSlidePuzzle(knownWords);
        //    var availableLetters = new char[unknownWord.WordPattern.Length][];
        //    for (var i = 0; i < availableLetters.Length; i++)
        //    {
        //        availableLetters[i] = new[] { 'X' };
        //    }

        //    var solutions = puzzle.Solve(unknownWord, availableLetters);
        //    Assert.That(solutions, Is.Empty, "If there are no known words of the required length the solution should be empty");
        //}

        [Test]
        [TestCaseSource(nameof(_SolveSingleWordTestData))]
        public void SolveSingleWord(KnownWords knownWords, string wordPattern, char[][] letters, string expectedWord)
        {
            var puzzle = new WordSlidePuzzle(knownWords);
            var solutions = puzzle.Solve(wordPattern, letters);
            Assert.That(solutions.Contains(expectedWord));
        }

        //[Test]
        ////[TestCaseSource(nameof(_SolveMultiWordTestData))]
        //public void SolveMultiWord()
        //{
        //    var knownWords = new KnownWords(new[] { "if", "in", "an", "then", "than", "else" });
        //    var wordPattern = new UnknownWord("??????????");
        //    wordPattern.SetSubWordCounts(new[] { 2, 4, 4 });
        //    char[][] letters = {
        //        new[] {'A', 'Y', 'I'},
        //        new[] {'F', 'N', 'O'},
        //        new[] {'t', 'H', 'O'},
        //        new[] {'t', 'H', 'O'},
        //        new[] {'W', 'A', 'E'},
        //        new[] {'F', 'N', 'O'},
        //        new[] {'W', 'A', 'E'},
        //        new[] {'y', 'L', 'b'},
        //        new[] {'s', 's'},
        //        new[] {'k', 'E', 'y'},
        //    };

        //    var puzzle = new WordSlidePuzzle(knownWords);
        //    var ex = Assert.Throws<ArgumentException>(() => puzzle.Solve(wordPattern, letters));
        //    Assert.That(ex.Message,Is.EqualTo("A word slide puzzle cannot solve for multiple words"));
        //}

        // TODO: WordSlidePuzzle doesn't currently know how to solve multiwords
        //public static IEnumerable<TestCaseData> _SolveMultiWordTestData()
        //{
        //    {
        //        var knownWords = new KnownWords(new[] { "if", "in", "an", "then", "than", "else" });
        //        var unknownWord = new UnknownWord("??????????");
        //        unknownWord.SetSubWordCounts(new[] { 2, 4, 4 });
        //        char[][] letters = {
        //            new[] {'A', 'Y', 'I'},
        //            new[] {'F', 'N', 'O'},
        //            new[] {'t', 'H', 'O'},
        //            new[] {'t', 'H', 'O'},
        //            new[] {'W', 'A', 'E'},
        //            new[] {'F', 'N', 'O'},
        //            new[] {'W', 'A', 'E'},
        //            new[] {'y', 'L', 'b'},
        //            new[] {'s', 's'},
        //            new[] {'k', 'E', 'y'},
        //        };
        //        var expectedWord = "if then else";
        //        yield return new TestCaseData(knownWords, unknownWord, letters, expectedWord)
        //            .SetName("expectedWord: if then else");
        //    }
        //}

        public static IEnumerable<TestCaseData> _SolveSingleWordTestData()
        {
            {
                var knownWords = new KnownWords(new[]{"if", "in", "an"});
                var wordPattern = "??";
                char[][] letters = {
                    new[] {'A', 'Y', 'I'},
                    new[] {'F', 'N', 'O'}
                };
                var expectedWord = "if";
                yield return new TestCaseData(knownWords, wordPattern, letters, expectedWord)
                    .SetName("expectedWord: if");
            }

            {
                var knownWords = new KnownWords(new[] { "walking", "talking", "walk", "talk" });
                var wordPattern = "w??????";
                char[][] letters = {
                    new[] {'T', 'W'},
                    new[] {'A', 'o'},
                    new[] {'p', 'l', 'N'},
                    new[] {'e', 'a', 'k'},
                    new[] {'i', 'v'},
                    new[] {'i', 'n'},
                    new[] {'G', 'n'},
                };
                var expectedWord = "walking";
                yield return new TestCaseData(knownWords, wordPattern, letters, expectedWord)
                    .SetName("expectedWord: walking");
            }
        }

    }
}
