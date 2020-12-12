using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NUnit.Framework;
using WordPuzzleHelper;

namespace WordPuzzleHelperTest
{
    [TestFixture]
    public class UnknownWordTest
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
        public void CtorThrowsForNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new UnknownWord(pattern: null));
            Assert.That(ex.ParamName, Is.EqualTo("pattern"), "had ArgumentNullException but not for expected parameter");
        }

        [Test]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public void CtorThrowsForWhitespace()
        {
            var pattern = "   ";
            var ex = Assert.Throws<ArgumentException>(() => new UnknownWord(pattern));
            Assert.That(ex.Message, Is.EqualTo("pattern cannot be only whitespace"));
        }

        [Test]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public void CtorThrowsForMoreThanOneWord()
        {
            var pattern = "one two";
            var ex = Assert.Throws<ArgumentException>(() => new UnknownWord(pattern));
            Assert.That(ex.Message, Is.EqualTo($"The word pattern had some unexpected white space in it (ie it is several words): {pattern}"));
        }

        [Test]
        [TestCaseSource(nameof(_CtorData))]
        public void CtorHappyPathTest(string pattern, string expectedWordPattern, int expectedUnknownCharCnt)
        {
            var subject = new UnknownWord(pattern);
            Assert.That(subject.WordPattern, Is.EqualTo(expectedWordPattern), "unexpected value for WordPattern property");
            Assert.That(subject.UnknownCharCount, Is.EqualTo(expectedUnknownCharCnt), "unexpected value for UnknownCharCount property");
        }

        [Test]
        public void SubWordCountTooShort()
        {
            var pattern = "oneTWOthree";
            var subject = new UnknownWord(pattern);
            var subWordCounts = new[] { 2, 2, 4 };
            var wordPatternLength = 11;
            Assert.That(subject.WordPattern.Length, Is.EqualTo(wordPatternLength), "precondition: the length of the WordPattern");
            Assert.That(subWordCounts.Sum(), Is.LessThan(wordPatternLength), "precondition: the subWordCounts about to be applied add up to less than the length of word pattern");
            var ex = Assert.Throws<ArgumentException>(() => subject.SetSubWordCounts(subWordCounts));
            Assert.That(ex.Message, Is.EqualTo("Summing all the word counts is " + subWordCounts.Sum() + " but unknown word has " + wordPatternLength + " letters."));
        }

        [Test]
        public void SubWordCountTooLong()
        {
            var pattern = "oneTWOthree";
            var subject = new UnknownWord(pattern);
            var subWordCounts = new[] { 12 };
            var wordPatternLength = 11;
            Assert.That(subject.WordPattern.Length, Is.EqualTo(wordPatternLength), "precondition: the length of the WordPattern");
            Assert.That(subWordCounts.Sum(), Is.GreaterThan(wordPatternLength), "precondition: the subWordCounts about to be applied add up to more than the length of word pattern");
            var ex = Assert.Throws<ArgumentException>(() => subject.SetSubWordCounts(subWordCounts));
            Assert.That(ex.Message, Is.EqualTo("Summing all the word counts is " + subWordCounts.Sum() + " but unknown word has " + wordPatternLength + " letters."));
        }

        [Test]
        public void SubWordCountHappyPath()
        {
            var pattern = "oneTWOthree";
            var subject = new UnknownWord(pattern);
            var subWordCounts = new[] { 3, 3, 5 };
            subject.SetSubWordCounts(subWordCounts);
            Assert.That(subject.SubWordCounts, Is.EquivalentTo(subWordCounts), "sub word array not expected");
        }

        [Test]
        public void FillInUnknownTooFewLetters()
        {
            var pattern = "??";
            var subject = new UnknownWord(pattern);
            var letters = new[] {'x'};
            var ex = Assert.Throws<ArgumentException>(() => subject.FillInUnknown(letters));
            Assert.That(ex.Message, Is.EqualTo($"You provided {letters.Length} letters but the unknown characters are {pattern.Length}.  These 2 counts must be the same."));
        }

        [Test]
        public void FillInUnknownTooManyLetters()
        {
            var pattern = "??";
            var subject = new UnknownWord(pattern);
            var letters = new[] { 'x', 'y', 'z' };
            var ex = Assert.Throws<ArgumentException>(() => subject.FillInUnknown(letters));
            Assert.That(ex.Message, Is.EqualTo($"You provided {letters.Length} letters but the unknown characters are {pattern.Length}.  These 2 counts must be the same."));
        }

        [Test]
        [TestCaseSource(nameof(_FillInUnknownData))]
        public void FillInUnknownHappyPath(string pattern, int[] subWords, char[] letters, string[] expectedWords)
        {
            var subject = new UnknownWord(pattern);
            if (subWords != null)
            {
                subject.SetSubWordCounts(subWords);
            }

            var words = subject.FillInUnknown(letters);
            Assert.That(words, Is.EquivalentTo(expectedWords), "FillInUnknown did not produce the expected words");
        }

        private static IEnumerable<TestCaseData> _FillInUnknownData()
        {
            {
                var pattern = "  ?ord  ";
                int[] subWords = null;
                var letters = new[] { 'w' };
                var expectedWords = new[] {"word"};
                yield return new TestCaseData(pattern, subWords, letters, expectedWords)
                    .SetName("Fill in 1 letter, 1 word");
            }

            {
                var pattern = "????";
                int[] subWords = null;
                var letters = new[] { 'w', 'o', 'r', 'd' };
                var expectedWords = new[] { "word" };
                yield return new TestCaseData(pattern, subWords, letters, expectedWords)
                    .SetName("Fill in all letters, 1 word");
            }

            {
                var pattern = "O?eTwoThree";
                int[] subWords = {3, 3, 5};
                var letters = new[] { 'n' };
                var expectedWords = new[] { "one", "two", "three" };
                yield return new TestCaseData(pattern, subWords, letters, expectedWords)
                    .SetName("Fill in 1 letter, 3 words");
            }

            {
                var pattern = "O?eTw?Thr??";
                int[] subWords = { 3, 3, 5 };
                var letters = new[] { 'n', 'o', 'e', 'e' };
                var expectedWords = new[] { "one", "two", "three" };
                yield return new TestCaseData(pattern, subWords, letters, expectedWords)
                    .SetName("Fill in many letters, 3 words");
            }
        }

        private static IEnumerable<TestCaseData> _CtorData()
        {
            {
                var pattern = "\t  word  \t  ";
                var expectedWordPattern = "word";
                var expectedUnknownCharCnt = 0;
                yield return new TestCaseData(pattern, expectedWordPattern, expectedUnknownCharCnt)
                    .SetName("whitespace, no unknowns");
            }

            {
                var pattern = "\t  ?o?d  \t  ";
                var expectedWordPattern = "?o?d";
                var expectedUnknownCharCnt = 2;
                yield return new TestCaseData(pattern, expectedWordPattern, expectedUnknownCharCnt)
                    .SetName("whitespace, 2 unknowns");
            }

            {
                var pattern = "WORD";
                var expectedWordPattern = "word";
                var expectedUnknownCharCnt = 0;
                yield return new TestCaseData(pattern, expectedWordPattern, expectedUnknownCharCnt)
                    .SetName("all uppercase, no unknowns");
            }

            {
                var pattern = "?WO?RD?";
                var expectedWordPattern = $"?wo?rd?";
                var expectedUnknownCharCnt = 3;
                yield return new TestCaseData(pattern, expectedWordPattern, expectedUnknownCharCnt)
                    .SetName("all uppercase, 3 unknowns");
            }
        }

    }
}
