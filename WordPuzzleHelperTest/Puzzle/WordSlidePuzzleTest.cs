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
            var ex = Assert.Throws<ArgumentNullException>(() => puzzle.Solve(unknownWord: null, Array.Empty<char[]>()));
            Assert.That(ex.ParamName, Is.EqualTo("unknownWord"), "had ArgumentNullException but not for expected parameter");
        }

        [Test]
        public void SolveNullLetters()
        {
            var puzzle = new WordSlidePuzzle(new KnownWords(Enumerable.Empty<string>()));
            var ex = Assert.Throws<ArgumentNullException>(() => puzzle.Solve(new UnknownWord("test"), availableLetters: null));
            Assert.That(ex.ParamName, Is.EqualTo("availableLetters"), "had ArgumentNullException but not for expected parameter");
        }

        [Test]
        public void SolveLetterSizeMismatch()
        {
            var puzzle = new WordSlidePuzzle(new KnownWords(Enumerable.Empty<string>()));
            var unknownWord = new UnknownWord("????");
            char[][] letters = {
                new[] {'a'},
                new[] {'b'},
                new[] {'c'}
            };
            var ex = Assert.Throws<ArgumentException>(() => puzzle.Solve(unknownWord, letters));
            Assert.That(ex.Message, Is.EqualTo("The partial word had a length of 4 but the available letters only had 3 slots."));
        }

        [Test]
        [TestCaseSource(nameof(_SolveTestData))]
        public void SolveTest(KnownWords knownWords, UnknownWord unknownWord, char[][] letters, string expectedWord)
        {
            var puzzle = new WordSlidePuzzle(knownWords);
            var solutions = puzzle.Solve(unknownWord, letters);
            Assert.That(solutions.Contains(expectedWord));
        }

        public static IEnumerable<TestCaseData> _SolveTestData()
        {
            {
                var knownWords = new KnownWords(new[]{"if", "in", "an"});
                var unknownWord = new UnknownWord("??");
                char[][] letters = {
                    new[] {'A', 'Y', 'I'},
                    new[] {'F', 'N', 'O'}
                };
                var expectedWord = "if";
                yield return new TestCaseData(knownWords, unknownWord, letters, expectedWord)
                    .SetName("expectedWord: if");
            }

            {
                var knownWords = new KnownWords(new[] { "walking", "talking", "walk", "talk" });
                var unknownWord = new UnknownWord("???????");
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
                yield return new TestCaseData(knownWords, unknownWord, letters, expectedWord)
                    .SetName("expectedWord: walking");
            }
        }

    }
}
