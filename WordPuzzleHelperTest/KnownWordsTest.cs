using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NUnit.Framework;
using WordPuzzleHelper;

namespace WordPuzzleHelperTest
{
    [TestFixture]
    public class KnownWordsTest
    {
        [Test]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        [SuppressMessage("ReSharper", "ExpressionIsAlwaysNull")]
        public void NullKnownWords()
        {
            string[] allWords = null;
            var ex = Assert.Throws<ArgumentNullException>(() => new KnownWords(allKnownWords: allWords));
            Assert.That(ex.ParamName, Is.EqualTo("allKnownWords"), "had ArgumentNullException but not for expected parameter");
        }

        [Test]
        [TestCaseSource(nameof(_WordsWithWhiteSpaceData))]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public void WordsCannotHaveWhiteSpaceInsideThem(string word)
        {
            var ex = Assert.Throws<ArgumentException>(() => new KnownWords(new []{word}));
            Assert.That(ex.Message, Is.EqualTo($"A word in the dictionary had some unexpected white space in it (ie it is several words): {word}"));
        }

        private static IEnumerable<TestCaseData> _WordsWithWhiteSpaceData()
        {
            {
                var word = "white space";
                yield return new TestCaseData(word).SetName("space char");
            }

            {
                var word = @"white
space";
                yield return new TestCaseData(word).SetName("carriage return");
            }

            {
                var word = "white\tspace";
                yield return new TestCaseData(word).SetName("tab char");
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
            var ex = Assert.Throws<ArgumentException>(() => knownWords.AllWordsOfLength(0));
            Assert.That(ex.Message, Is.EqualTo("A known word must have at least 1 character"));
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
