using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Core.Tests
{
    [TestFixture]
    public class EncryptTest
    {
        [TestCase("A", "0")]
        [TestCase("B", "1")]
        public void ReturnsLetterGivenLetter(string letterFrom, string letterTo)
        {
            Assert.AreEqual(Crypto.CustomEncrypt(letterFrom),letterTo);
        }

        [TestCase("HelloWorld", "7VZR`OVU\\[")]
        [TestCase("BlahTest", "1]ONE]ZW")]
        public void ReturnStringGivenString(string stringFrom, string stringTo)
        {
            Assert.AreEqual(Crypto.CustomEncrypt(stringFrom), stringTo);
        }

        [Test]
        public void ReturnMyPassword()
        {
            Assert.AreEqual(Crypto.Encrypt("HelloWorld"), "3BoYGw/5pWf/EBKaKoBI9w==");
        }

        [Test]
        public void StandardNotEqualCustom()
        {
            Assert.AreNotEqual(Crypto.Encrypt("HelloWorld"),Crypto.StandardEncrypt("HelloWorld"));
        }
    }
}
