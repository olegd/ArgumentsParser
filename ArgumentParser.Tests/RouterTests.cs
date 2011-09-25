using System;
using System.Reflection;
using NUnit.Framework;

namespace ArgumentParser.Tests
{
    [TestFixture]
    public class RouterTests
    {
        [Test]
        public void RouteCommand_StaticMethodWithoutAttributes_IsRouted()
        {
            bool mergeCommandInvoked = false;
            CommandContainerForTests.MergeCommandCallback = () => mergeCommandInvoked = true;
            
            var router = new Router();
            router.Route(new [] {"MergeCommand"});

            Assert.IsTrue(mergeCommandInvoked);
        }
    }

    public class CommandContainerForTests
    {
        public static Action MergeCommandCallback { get; set; }

        [Command]
        public static void MergeCommand()
        {
            InvokeCallback(MergeCommandCallback);
        }

        private static void InvokeCallback(Action commandCallback)
        {
            if (commandCallback != null)
            {
                commandCallback.Invoke();
            }
        }
    }
}