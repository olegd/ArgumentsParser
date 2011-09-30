Arguments Parser
===========

Arguments parser helps wire command line arguments to the methods that should handle them.

Example:
--

You are designing a program that should support **merge** command, should take an argument named **branchName** and an optional **force** flag 


> This is how you are picturing it beeing called: 

    coolutil.exe merge myNewBranch force


    public class CommandHandler
    {
        [Command]
        public void Merge(string branchName, bool force)
        {         
        }
    }

ArgumentsParser assumes a convention that Handler Methods are names after commands: if console application takes merge as a first argument, by convention, argument parser will try to find handler method named Merge (case insensitive)

You can override this convention by providing a Name argument to the CommandAttribute:

    public class CommandHandler
    {
        [Command(Name="merge")]
        public void AnyMethodName(string branchName, bool force)
        {         
        }
    }


Dependency Injection support
--
Arguments parser supports Common Service Locator:

>Just set ServiceProvider with your favourite dependency injection repository and ArgumentParser will inject the dependencies from the ServiceProvider, when constructing handler objects: 

    ServiceLocator.SetServiceProvider(()=>StructureMapDependencyResolver());


=======