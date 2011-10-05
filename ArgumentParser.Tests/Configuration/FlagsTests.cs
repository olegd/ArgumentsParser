using ArgumentParser.Configuration;
using NUnit.Framework;

namespace ArgumentParser.Tests.Configuration
{
    [TestFixture]
    public class FlagsTests
    {
        [Test]
        public void Resolve_Nosynonyms_ArgumentFound_ReturnsArgument()
        {
            var flags = new Flags();
            flags.Add("argument");

            var result = flags.Resolve("argument");

            Assert.That(result, Is.EqualTo("argument"));
        }

        [Test]
        public void Resolve_Nosynonyms_ArgumentNotFound_ReturnsNull()
        {
            var flags = new Flags();

            var result = flags.Resolve("argument");

            Assert.IsNull(result);
        }

        [Test]
        public void Resolve_Onesynonym_Resolvesynonym_ReturnsArgument()
        {
            var flags = new Flags();
            flags.Add("argument", "-a");

            var result = flags.Resolve("-a");

            Assert.That(result, Is.EqualTo("argument"));
        }

        [Test]
        public void Resolve_Twosynonyms_ResolveSecondsynonym_ReturnsArgument()
        {
            var flags = new Flags();
            flags.Add("argument", "-a", "--a");

            var result = flags.Resolve("--a");

            Assert.That(result, Is.EqualTo("argument"));
        }

        [Test]
        public void Resolve_Twosynonyms_ResolveArgument_ReturnsArgument()
        {
            var flags = new Flags();
            flags.Add("argument", "-a", "--a");

            var result = flags.Resolve("argument");

            Assert.That(result, Is.EqualTo("argument"));
        }
    }
}