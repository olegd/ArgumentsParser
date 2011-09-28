using System.Collections.Generic;
using ArgumentParser.Configuration;
using ArgumentParser.Core;
using ArgumentParser.Routing;
using Moq;
using NUnit.Framework;

namespace ArgumentParser.Tests
{
    [TestFixture]
    public class CommandToHandlerMapperTests
    {
        private Mock<IHandlerProvider> _handlerDiscovererMock;
        private CommandToHandlerMapper _commandToHandlerMapper;

        [SetUp]
        public void BeforeEachTest()
        {
            _handlerDiscovererMock = new Mock<IHandlerProvider>();
            _commandToHandlerMapper = new CommandToHandlerMapper(_handlerDiscovererMock.Object);
        }
        
        [Test]
        public void MapCommand_CommandWithoutArguments_ReturnsMappedhandler()
        {
            AddReturnedHandler("merge");
            
            var result = _commandToHandlerMapper.Map(new[] {"merge"});

            Assert.IsNotNull(result);
            Assert.That(result.CommandName, Is.EqualTo("merge"));
        }

        [Test]
        public void MapCommand_CommandCanNotBeFound_ThrowsException()
        {
            AddReturnedHandler("merge");

            Assert.Throws<CommandMappingException>(()=>
                _commandToHandlerMapper.Map(new[] { "anotherCommand" }));
        }



        [Test]
        public void MapCommand_DefinedCommandHasLessArgumentsThanPassed_ThrowsExceptions()
        {
            AddReturnedHandler("merge", arguments: new[] { "branchName" });

            var handler = _commandToHandlerMapper.Map(new[] {"merge", "superBranch", "testingBranch"});

            Assert.IsNull(handler);
        }

        [Test]
        public void MapCommand_DefinedCommandHasMoreArgumentsThanPassed_ThrowsExceptions()
        {
            AddReturnedHandler("merge", arguments: new[] { "branchName", "anotherBranchName" });

            var handler = _commandToHandlerMapper.Map(new[] {"merge", "superBranch"});

            Assert.IsNull(handler);
        }

        private void AddReturnedHandler(string name, string[] arguments = null, string[] flags = null)
        {
            var handler = HandlerObjectMother.CreateHandler(name, arguments, flags);
            AddReturnedHandler(handler);
        }

        private void AddReturnedHandler(IHandler handlerToAdd)
        {
            List<IHandler> handlers = _handlerDiscovererMock.Object.GetHandlers() 
                                    ?? new List<IHandler>();
            handlers.Add(handlerToAdd);

            _handlerDiscovererMock.Setup(x => x.GetHandlers())
                .Returns(handlers);
        }
    }
}