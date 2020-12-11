using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using WordPuzzleHelper;

namespace WordPuzzleHelperTest
{
    [TestFixture]
    public class KnownWordsTest
    {
        [Test]
        public void NullKnownWords()
        {
            string[] allWords = null;
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.Throws<ArgumentNullException>(() => new KnownWords(allWords));
        }

        [Test]
        [TestCaseSource(nameof(_WordsWithWhiteSpaceData))]
        public void WordsCannotHaveWhiteSpaceInsideThem(IEnumerable<string> allWords)
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentException>(() => new KnownWords(allWords));
        }

        private static IEnumerable<TestCaseData> _WordsWithWhiteSpaceData()
        {
            {
                IEnumerable<string> allWords = new[] { "white space" };
                yield return new TestCaseData(allWords).SetName("space char");
            }

            {
                IEnumerable<string> allWords = new [] { @"white
space" };
                yield return new TestCaseData(allWords).SetName("carriage return");
            }

            {
                IEnumerable<string> allWords = new [] { "white\tspace" };
                yield return new TestCaseData(allWords).SetName("tab char");
            }
        }

        [Test]
        public void KnownWordsIgnoresWhitespaceWords()
        {
            var oneSpace = " ";
            var threeSpace = "   ";
            var allWords = new[] { oneSpace, threeSpace };
            var knownWords = new KnownWords(allWords);

            Assert.That(knownWords.IsKnownWord(oneSpace), Is.False, nameof(oneSpace));
            Assert.That(knownWords.IsKnownWord(threeSpace), Is.False, nameof(threeSpace));
        }

        [Test]
        public void KnownWordsIgnoresWhitespace()
        {
            var wordWithPadding = "    padding   ";
            var allWords = new[] { wordWithPadding, "another", "word" };
            var knownWords = new KnownWords(allWords);

            Assert.That(knownWords.IsKnownWord(wordWithPadding), Is.False, "a word with white space padding should not have been known");
            Assert.That(knownWords.IsKnownWord(wordWithPadding.Trim()), Is.True, "a word with white space padding Trimmed should have been known");
        }

        [Test]
        public void AllWordsOfLengthThrowsForZeroLength()
        {
            var allWords = new[] { string.Empty, "aa", "bb", "aaa", "bbb", "1234" };
            var knownWords = new KnownWords(allWords);
            Assert.Throws<ArgumentException>(() => knownWords.AllWordsOfLength(0));
        }

        [Test]
        [TestCaseSource(nameof(_AllWordsOfLengthTestData))]
        public void AllWordsOfLengthTest(string[] allWords, int wordLen, string[] expected)
        {
            var knownWords = new KnownWords(allWords);
            var unsortedActual = knownWords.AllWordsOfLength(wordLen);
            var actual = unsortedActual.OrderBy(s => s);
            Assert.That(actual, Is.EquivalentTo(expected));
        }

        private static IEnumerable<TestCaseData> _AllWordsOfLengthTestData()
        {
            var allWords = new[] { "aa", "bb", "aaa", "bbb", "1234" };

            {
                var wordLen = 1;
                var expected = new string[0];
                yield return new TestCaseData(allWords, wordLen, expected)
                    .SetName("wordLen 1");
            }

            {
                var wordLen = 2;
                var expected = new[] { "aa", "bb" };
                yield return new TestCaseData(allWords, wordLen, expected)
                    .SetName("wordLen 2");
            }

            {
                var wordLen = 3;
                var expected = new[] { "aaa", "bbb" };
                yield return new TestCaseData(allWords, wordLen, expected)
                    .SetName("wordLen 3");
            }

            {
                var wordLen = 4;
                var expected = new[] { "1234" };
                yield return new TestCaseData(allWords, wordLen, expected)
                    .SetName("wordLen 4");
            }

            {
                var wordLen = 100;
                var expected = new string[0];
                yield return new TestCaseData(allWords, wordLen, expected)
                    .SetName("wordLen 100");
            }
        }

    }
}
