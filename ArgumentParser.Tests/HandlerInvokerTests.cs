using System.Collections.Generic;
using ArgumentParser.Handling;
using NUnit.Framework;

namespace ArgumentParser.Tests
{
    [TestFixture]
    public class HandlerInvokerTests
    {
        private HandlerInvoker _invoker;

        [SetUp]
        public void BeforeEachTest()
        {
            _invoker = new HandlerInvoker();
        }

        [Test]
        public void MapArguments_HandlerHasNoFlagsOrArgumentsANDCommandHasNoFlagsOrArguments_NothingIsMapped()
        {
            var handler = HandlerObjectMother.CreateHandler("merge");

            var actualMappedArguemtns = _invoker.MapArguments(handler, new[] {"merge"});

            Assert.That(actualMappedArguemtns.Count, Is.EqualTo(0));
        }

        [Test]
        public void MapArguments_HandlerHasNoFlagsOrArgumentsANDCommandEmptyArguments_NothingIsMapped()
        {
            var handler = HandlerObjectMother.CreateHandler("merge");

            var actualMappedArguemtns = _invoker.MapArguments(handler, new[] {"merge", "", " "});

            Assert.That(actualMappedArguemtns.Count, Is.EqualTo(0));
        }
        
        [Test]
        public void MapArguments_HandlerHasOneFlagANDCommandHasOneFlag_FlagIsMappedToTrue()
        {
            var handler = HandlerObjectMother.CreateHandler("merge", flags: new[] {"someFlag"});

            var actualMappedArguemtns = _invoker.MapArguments(handler, new[] {"merge", "someFlag"});

            AssertIsMapped(actualMappedArguemtns, "someFlag", true);
        }

        [Test]
        public void MapArguments_HandlerHasTwoFlagsBUTCommandHasOnlyOneFlag_OneFlagIsMappedToTrueAnotherOneIsNotMapped()
        {
            var handler = HandlerObjectMother.CreateHandler("merge", flags: new[] {"someFlag", "someOtherFlag"});

            var actualMappedArguemtns = _invoker.MapArguments(handler, new[] {"merge", "someFlag"});

            AssertIsMapped(actualMappedArguemtns, "someFlag", true);
            AssertIsNotMapped(actualMappedArguemtns, "someOtherFlag");
        }

        [Test]
        public void MapArguments_HandlerHasTwoFlagsANDCommandHasTwoFlags_BothFlagsAreMappedToTrue()
        {
            var handler = HandlerObjectMother.CreateHandler("merge", flags: new[] { "someFlag", "someOtherFlag" });

            var actualMappedArguemtns = _invoker.MapArguments(handler, new[] { "merge", "someFlag", "someOtherFlag" });

            AssertIsMapped(actualMappedArguemtns, "someFlag", true);
            AssertIsMapped(actualMappedArguemtns, "someOtherFlag", true);
        }

        [Test]
        public void MapArguments_HandlerHasArgumentANDCommandArgument_MappsArgumentAndValue()
        {
            var handler = HandlerObjectMother.CreateHandler("merge", arguments: new[] { "branchName" });

            var actualMappedArguemtns = _invoker.MapArguments(handler, new[] { "merge", "branchA" });

            AssertIsMapped(actualMappedArguemtns, "branchName", "branchA");
        }

        [Test]
        public void MapArguments_HandlerHasTwoArgumentsANDCommandHasTwoArguments_MappsBothArgumentsAndValues()
        {
            var handler = HandlerObjectMother.CreateHandler("merge", arguments: new[] { "branchName", "anotherBranchName" });

            var actualMappedArguemtns = _invoker.MapArguments(handler, new[] { "merge", "branchA", "branchB" });

            AssertIsMapped(actualMappedArguemtns, "branchName", "branchA");
            AssertIsMapped(actualMappedArguemtns, "anotherBranchName", "branchB");
        }
        
        private void AssertIsMapped(Dictionary<string, object> mappedArgs, string parameterName,
                                        object expectedValue)
        {
            Assert.That(mappedArgs.ContainsKey(parameterName),
                         "Expected that {0} would be mapped, but it was not".With(parameterName));

            Assert.That(mappedArgs[parameterName], Is.EqualTo(expectedValue),
                        "Expected that the value of: {0} would be {1}, but was {2}"
                            .With(parameterName, expectedValue.ToString(), mappedArgs[parameterName].SafeToString()));
        }

        private void AssertIsNotMapped(Dictionary<string, object> mappedArgs, string flagName)
        {
            Assert.IsFalse(mappedArgs.ContainsKey(flagName),
                           "Expected that flag {0} would NOT be mapped, but it was mapped".With(flagName));
        }

    }
}