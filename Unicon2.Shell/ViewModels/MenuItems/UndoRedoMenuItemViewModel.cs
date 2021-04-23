using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.Services.CommandStack;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Shell.ViewModels.MenuItems
{
    public class UndoRedoMenuItemViewModel : ViewModelBase,IStronglyNamed
    {
        private readonly ICommandStackService _commandStackService;

        public UndoRedoMenuItemViewModel(ICommandStackService commandStackService)
        {
            _commandStackService = commandStackService;
            UndoOneAction = new RelayCommand(async () =>
            {
               await commandStackService.UndoCommands(1);
            },() => _commandStackService.GetPrevAvailableCommands().Any());

            RedoOneAction = new RelayCommand(async () =>
            {
                await commandStackService.RedoCommands(1);
            }, () => _commandStackService.GetNextAvailableCommands().Any());

            _commandStackService.CommandsChanged += () =>
            {
                RaisePropertyChanged(nameof(UndoActionsList));
                RaisePropertyChanged(nameof(RedoActionsList));

                (UndoOneAction as RelayCommand)?.RaiseCanExecuteChanged();
                (RedoOneAction as RelayCommand)?.RaiseCanExecuteChanged();
                (UndoActions as RelayCommand)?.RaiseCanExecuteChanged();
                (RedoActions as RelayCommand)?.RaiseCanExecuteChanged();

            };
        }

        public List<string> UndoActionsList { get; }
        public List<string> RedoActionsList { get; }
        public ICommand UndoOneAction { get; }
        public ICommand RedoOneAction { get; }
        public int NumOfUndoActions { get; set; }
        public int NumOfRedoActions { get; set; }
        public ICommand UndoActions { get; }
        public ICommand RedoActions { get; }
        public string StrongName => nameof(UndoRedoMenuItemViewModel);
    }
}