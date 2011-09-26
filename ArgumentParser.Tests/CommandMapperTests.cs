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
            AddReturnedHandler("merge");
            
            var result = _commandMapper.MapCommand(new[] {"merge"});

            Assert.IsNotNull(result);
            Assert.That(result.HandlerName, Is.EqualTo("merge"));
        }

        [Test]
        public void MapCommand_CommandCanNotBeFound_ThrowsException()
        {
            AddReturnedHandler("merge");

            Assert.Throws<CommandMappingException>(()=>
                _commandMapper.MapCommand(new[] { "anotherCommand" }));
        }

        [Test]
        public void MapCommand_CommandWitOneFlag_ReturnsCorrectHandler_MappsOneFlag()
        {
            AddReturnedHandler("merge", flags: new[] { "someFlag" });

            var result = _commandMapper.MapCommand(new[] { "merge","someFlag" });

            Assert.IsNotNull(result);
            Assert.That(result.HandlerName, Is.EqualTo("merge"));
            Assert.That(result.ArgumentValues.ContainsKey("someFlag"));
        }
        
        [Test]
        public void MapCommand_DefinedCommandHasTwoFlagsButArgumentContainsOnlyOneFlag_MappsOneFlagAndDoesNotMapTheOther()
        {
            AddReturnedHandler("merge", flags: new[] { "someFlag", "someOtherFlag" });

            var result = _commandMapper.MapCommand(new[] { "merge", "someFlag" });

            Assert.IsTrue(result.ArgumentValues.ContainsKey("someFlag"));
            Assert.IsFalse(result.ArgumentValues.ContainsKey("someOtherFlag"));
        }

        [Test]
        public void MapCommand_DefinedCommandHasTwoFlagsButArgumentContainsOnlyOneFlag_MappsBothFlags()
        {
            AddReturnedHandler("merge", flags: new[] { "someFlag", "someOtherFlag" });
            
            var result = _commandMapper.MapCommand(new[] { "merge", "someFlag", "someOtherFlag" });

            Assert.IsTrue(result.ArgumentValues.ContainsKey("someFlag"));
            Assert.IsTrue(result.ArgumentValues.ContainsKey("someOtherFlag"));
        }

        [Test]
        public void MapCommand_DefinedCommandHasAnArgument_MappsArguemtAndValue()
        {
            AddReturnedHandler("merge", arguments: new[] {"branchName"});

            var result = _commandMapper.MapCommand(new[] { "merge", "superBranch" });

            CollectionAssert.Contains(result.ArgumentValues.Keys, "branchName");
            Assert.That(result.ArgumentValues["branchName"], Is.EqualTo("superBranch"));
        }

        [Test]
        public void MapCommand_DefinedCommandHasTwoArguments_MappsBothArgumentsAndValues()
        {
            AddReturnedHandler("merge", arguments: new[] { "branchName", "anotherBranchName" });

            var result = _commandMapper.MapCommand(new[] { "merge", "superBranch", "testingBranch" });

            CollectionAssert.Contains(result.ArgumentValues.Keys, "branchName");
            Assert.That(result.ArgumentValues["branchName"], Is.EqualTo("superBranch"));

            CollectionAssert.Contains(result.ArgumentValues.Keys, "anotherBranchName");
            Assert.That(result.ArgumentValues["anotherBranchName"], Is.EqualTo("testingBranch"));
        }

        [Test]
        public void MapCommand_DefinedCommandHasLessArgumentsThanPassed_ThrowsExceptions()
        {
            AddReturnedHandler("merge", arguments: new[] { "branchName" });

            Assert.Throws<CommandMappingException>(()=>
                _commandMapper.MapCommand(new[] { "merge", "superBranch", "testingBranch" })
            );
        }

        [Test]
        public void MapCommand_DefinedCommandHasMoreArgumentsThanPassed_ThrowsExceptions()
        {
            AddReturnedHandler("merge", arguments: new[] { "branchName", "anotherBranchName" });

            Assert.Throws<CommandMappingException>(() =>
                _commandMapper.MapCommand(new[] { "merge", "superBranch" })
            );
        }

        private void AddReturnedHandler(string name, string[] arguments = null, string[] flags = null)
        {
            arguments = arguments ?? new string[0];
            flags = flags ?? new string[0];

            var handler = new HandlerDescriptor {CommandName = name};
            handler.Arguments.AddRange(arguments);
            handler.Flags.AddRange(flags);

            AddReturnedHandler(handler);
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