using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Presentation.Infrastructure.Services.CommandStack;

namespace Unicon2.Presentation.Services
{
    public class CommandStackService: ICommandStackService
    {

        private Stack<IStackingCommand> _undoStack = new Stack<IStackingCommand>();
        private Stack<IStackingCommand> _redoStack = new Stack<IStackingCommand>();

        public CommandStackService()
        {
        }

        public void AddStackingCommand(IStackingCommand newStackingCommand)
        {
            _undoStack.Push(newStackingCommand);
            _redoStack.Clear();
            CommandsChanged?.Invoke();
        }

        public void DisposeCommandFromStackByDependency(ICommandStackDependencySource dep)
        {
            var relatedItems = _undoStack.Where(command => command.Dependencies.Any(o => o == dep));
            CommandsChanged?.Invoke();

        }

        public List<string> GetNextAvailableCommands()
        {
            return _redoStack.Select(command => command.ViewName).ToList();
        }

        public List<string> GetPrevAvailableCommands()
        {
            return _undoStack.Select(command => command.ViewName).ToList();
        }

        public async Task UndoCommands(int commandsCount)
        {
            for (var i = 0; i < commandsCount; i++)
            {
                var command = _undoStack.Pop();
                await command.Undo();
                _redoStack.Push(command);
            }
            CommandsChanged?.Invoke();
        }

        public async Task RedoCommands(int commandsCount)
        {
            for (var i = 0; i < commandsCount; i++)
            {
                var command = _redoStack.Pop();
                await command.Do();
                _undoStack.Push(command);
            }
            CommandsChanged?.Invoke();
        }

        public Action CommandsChanged { get; set; }
    }
}
