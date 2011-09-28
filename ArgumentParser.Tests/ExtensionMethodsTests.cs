using NUnit.Framework;

namespace ArgumentParser.Tests
{
    [TestFixture]
    public class ExtensionMethodsTests
    {
        [Test]
        public void RemoveEmptyElements_ArrayHasOneEmptyElement_ItIsRemoved()
        {
            var result = new[] {"first", " "}.RemoveEmptyElements();

            Assert.That(result.Length, Is.EqualTo(1));
        }

        [Test]
        public void RemoveEmptyElements_ArrayHasTwoEmptyElements_TwoElementsAreRemoved()
        {
            var result = new[] { "first", " ", ""}.RemoveEmptyElements();

            Assert.That(result.Length, Is.EqualTo(1));
        }

        [Test]
        public void RemoveEmptyElements_ArrayHasNoEmptyElements_NothingIsRemoved()
        {
            var result = new[] { "first", "second ", "third" }.RemoveEmptyElements();

            Assert.That(result.Length, Is.EqualTo(3));
        }
    }
}