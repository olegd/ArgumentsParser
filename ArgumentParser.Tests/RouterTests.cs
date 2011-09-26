using System;
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

        [Test]
        public void RouteCommand_StaticMethodWithoutOneFlagFlagIsNotPassed_FlagValueDefaultToFalse()
        {
            //Arrange
            bool commandInvoked = false;
            bool passedForceMergeFlagValue = false;
            CommandContainerForTests.MergeCommand2Callback
                = forceMerge =>
                      {
                          commandInvoked = true;
                          passedForceMergeFlagValue = forceMerge;
                      };
            
            //Act
            var router = new Router();
            router.Route(new[] { "MergeCommand2" });

            //Assert
            Assert.IsTrue(commandInvoked);
            Assert.IsFalse(passedForceMergeFlagValue);
        }

        [Test]
        public void RouteCommand_StaticMethodWithoutOneFlagFlagIsPassed_FlagValueIsPassedToTheHandler()
        {
            //Arrange
            bool commandInvoked = false;
            bool passedForceMergeFlagValue = false;
            CommandContainerForTests.MergeCommand2Callback
                = forceMerge =>
                {
                    commandInvoked = true;
                    passedForceMergeFlagValue = forceMerge;
                };

            //Act
            var router = new Router();
            router.Route(new[] { "MergeCommand2", "forceMerge" });

            //Assert
            Assert.IsTrue(commandInvoked);
            Assert.IsTrue(passedForceMergeFlagValue);
        }

        [Test]
        public void RouteCommand_StaticMethodWithoutOneFlagAndOneArgument_FlagIsNotPassedButArgumentIsPassed_ArgumentIsPassedToHandlerFlagIsDefault()
        {
            //Arrange
            bool commandInvoked = false;
            bool passedForceMergeFlagValue = false;
            string passedBranchNameValue = "";
            CommandContainerForTests.MergeCommand3Callback
                = (branchName, forceMerge) =>
                {
                    commandInvoked = true;
                    passedForceMergeFlagValue = forceMerge;
                    passedBranchNameValue = branchName;
                };

            //Act
            var router = new Router();
            router.Route(new[] { "MergeCommand3", "SuperBranch" });

            //Assert
            Assert.IsTrue(commandInvoked);
            Assert.That(passedBranchNameValue, Is.EqualTo("SuperBranch"));
            Assert.IsFalse(passedForceMergeFlagValue);
        }
    }

    public class CommandContainerForTests
    {
        public static Action MergeCommandCallback { get; set; }
        public static Action<bool> MergeCommand2Callback { get; set; }
        public static Action<string, bool> MergeCommand3Callback { get; set; }

        [Command]
        public static void MergeCommand()
        {
            MergeCommandCallback.Invoke();
        }

        [Command]
        public static void MergeCommand2(bool forceMerge)
        {
            MergeCommand2Callback.Invoke(forceMerge);
        }

        [Command]
        public static void MergeCommand3(string branchToMerge, bool forceMerge)
        {
            MergeCommand3Callback.Invoke(branchToMerge, forceMerge);
        }
    }
}