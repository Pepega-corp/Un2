using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unicon2.Infrastructure.Common;

namespace Unicon2.Presentation.Infrastructure.Services.CommandStack
{
    public interface ICommandStackService
    {
        void AddStackingCommand(IStackingCommand newStackingCommand);
        void DisposeCommandFromStackByDependency(ICommandStackDependencySource dep);
        List<string> GetNextAvailableCommands(); 
        List<string> GetPrevAvailableCommands();
        Task UndoCommands(int commandsCount);
        Task RedoCommands(int commandsCount);
        Action CommandsChanged { get; set; } 
    }

    public static class CommandStackServiceExtensions
    {
        public static void TryDisposeCommandFromStack(this ICommandStackDependencySource obj)
        {
            StaticContainer.Container.Resolve<ICommandStackService>().DisposeCommandFromStackByDependency(obj);
        }

        public static async Task ExecuteCommandAndAddToStack(this IStackingCommand command)
        {
            await command.Do();
            StaticContainer.Container.Resolve<ICommandStackService>().AddStackingCommand(command);
        }
    }
}