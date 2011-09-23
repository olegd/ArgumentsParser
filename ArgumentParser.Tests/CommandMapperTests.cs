using System.Collections.Generic;
using Moq;
using NUnit.Framework;

namespace ArgumentParser.Tests
{
    [TestFixture]
    public class CommandMapperTests
    {
        private Mock<IHandlersDiscoverer> _handlerDiscovererMock;
        private CommandMapper _commandMapper;

        [SetUp]
        public void BeforeEachTest()
        {
            _handlerDiscovererMock = new Mock<IHandlersDiscoverer>();
            _commandMapper = new CommandMapper(_handlerDiscovererMock.Object);
        }
        
        [Test]
        public void MapCommand_CommandWithoutArguments_ReturnsMappedhandler()
        {
            AddReturnedHandler(new HandlerDescriptor {Name = "merge"});
            
            var result = _commandMapper.MapCommand(new[] {"merge"});

            Assert.IsNotNull(result);
            Assert.That(result.HandlerName, Is.EqualTo("merge"));
        }

        [Test]
        public void MapCommand_CommandWitOneFlag_ReturnsCorrectHandler_MappsOneFlag()
        {
            var handler = new HandlerDescriptor {Name = "merge"};
            handler.Flags.Add("someFlag");
            AddReturnedHandler(handler);

            var result = _commandMapper.MapCommand(new[] { "merge","someFlag" });

            Assert.IsNotNull(result);
            Assert.That(result.HandlerName, Is.EqualTo("merge"));
            Assert.That(result.ArgumentValues.ContainsKey("someFlag"));
        }
        
        [Test]
        public void MapCommand_DefinedCommandHasTwoFlagsButArgumentContainsOnlyOneFlag_MappsOneFlagAndDoesNotMapTheOther()
        {
            var handler = new HandlerDescriptor { Name = "merge" };
            handler.Flags.Add("someFlag");
            handler.Flags.Add("someOtherFlag");
            AddReturnedHandler(handler);

            var result = _commandMapper.MapCommand(new[] { "merge", "someFlag" });

            Assert.IsNotNull(result);
            Assert.That(result.HandlerName, Is.EqualTo("merge"));
            Assert.IsTrue(result.ArgumentValues.ContainsKey("someFlag"));
            Assert.IsFalse(result.ArgumentValues.ContainsKey("someOtherFlag"));
        }

        [Test]
        public void MapCommand_DefinedCommandHasTwoFlagsButArgumentContainsOnlyOneFlag_MappsBothFlags()
        {
            var handler = new HandlerDescriptor { Name = "merge" };
            handler.Flags.Add("someFlag");
            handler.Flags.Add("someOtherFlag");
            AddReturnedHandler(handler);

            var result = _commandMapper.MapCommand(new[] { "merge", "someFlag", "someOtherFlag" });

            Assert.IsNotNull(result);
            Assert.That(result.HandlerName, Is.EqualTo("merge"));
            Assert.IsTrue(result.ArgumentValues.ContainsKey("someFlag"));
            Assert.IsTrue(result.ArgumentValues.ContainsKey("someOtherFlag"));
        }

        [Test]
        public void MapCommand_DefinedCommandHasAnArgument_MappsArguemtAndValue()
        {
            var handler = new HandlerDescriptor { Name = "merge" };
            handler.Arguments.Add("branchName");
            AddReturnedHandler(handler);

            var result = _commandMapper.MapCommand(new[] { "merge", "superBranch" });

            Assert.IsNotNull(result);
            Assert.That(result.HandlerName, Is.EqualTo("merge"));
            CollectionAssert.Contains(result.ArgumentValues.Keys, "branchName");
            Assert.That(result.ArgumentValues["branchName"], Is.EqualTo("superBranch"));
        }

        private void AddReturnedHandler(IHandlerDescriptor handlerToAdd)
        {
            List<IHandlerDescriptor> handlers = _handlerDiscovererMock.Object.GetHandlers() 
                                    ?? new List<IHandlerDescriptor>();
            handlers.Add(handlerToAdd);

            _handlerDiscovererMock.Setup(x => x.GetHandlers())
                .Returns(handlers);
        }
    }
}