using System;
using NUnit.Framework;
using WordPuzzleHelper.Util;

namespace WordPuzzleHelperTest.Util
{
    [TestFixture]
    public class StringUtilTest
    {
        [Test]
        public void ToAlphabetArrayHappyPath()
        {
            var testString = "abcd1234";
            var subject = testString.ToAlphabetArray();
            var expected = new[] { 'a', 'b', 'c', 'd', '1', '2', '3', '4' };
            Assert.That(subject, Is.EqualTo(expected));
        }

        [Test]
        public void ToAlphabetArrayNoWhiteSpace()
        {
            var testString = @" 

hello        there  

";
            var subject = testString.ToAlphabetArray();
            var expected = new[] { 'h', 'e', 'l', 'l', 'o', 't', 'h', 'e', 'r', 'e' };
            Assert.That(subject, Is.EqualTo(expected));
        }

        [Test]
        public void ToAlphabetArrayToLowerCase()
        {
            var testString = "aBcDeF";
            var subject = testString.ToAlphabetArray();
            var expected = new[] { 'a', 'b', 'c', 'd', 'e', 'f' };
            Assert.That(subject, Is.EqualTo(expected));
        }

        [Test]
        public void ToAlphabetArrayEmptyString()
        {
            var testString = string.Empty;
            var subject = testString.ToAlphabetArray();
            var expected = Array.Empty<char>();
            Assert.That(subject, Is.EqualTo(expected));
        }

        [Test]
        public void ToAlphabetArrayNullString()
        {
            string testString = null;
            var subject = testString.ToAlphabetArray();
            var expected = Array.Empty<char>();
            Assert.That(subject, Is.EqualTo(expected));
        }
    }
}
