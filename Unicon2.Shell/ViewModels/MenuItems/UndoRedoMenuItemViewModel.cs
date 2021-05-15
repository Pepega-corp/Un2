using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.Services.CommandStack;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Shell.ViewModels.MenuItems
{
    public class UndoRedoMenuItemViewModel : ViewModelBase,IStronglyNamed
    {
        private readonly ICommandStackService _commandStackService;
        private readonly ILocalizerService _localizerService;
        private bool _isCommandsPopupOpen;
        private List<CommandViewModel> _actionsList;

        public UndoRedoMenuItemViewModel(ICommandStackService commandStackService, ILocalizerService localizerService)
        {
            _commandStackService = commandStackService;
            _localizerService = localizerService;
            UndoOneAction = new RelayCommand(async () =>
            {
               await commandStackService.UndoCommands(1);
            },() => _commandStackService.GetPrevAvailableCommands().Any());

            RedoOneAction = new RelayCommand(async () =>
            {
                await commandStackService.RedoCommands(1);
            }, () => _commandStackService.GetNextAvailableCommands().Any());

            UndoActions=new RelayCommand(() =>
            {
                ActionsList = _commandStackService.GetPrevAvailableCommands()
                    .Select((s, num) => new CommandViewModel(num, s)).ToList();
                IsCommandsPopupOpen = true;
            },()=>_commandStackService.GetPrevAvailableCommands().Any());

            SelectCommand = new RelayCommand<object>(((args) =>
            {
                if (!(args is MouseEventArgs mouseEventArgs)) return;
                if (!(mouseEventArgs.Source is FrameworkElement frameworkElement)) return;
                if (frameworkElement.DataContext is CommandViewModel commandViewModel)
                {
                    ActionsList.Where(model => model.NumberOfCommand <= commandViewModel.NumberOfCommand)
                        .ForEach(model => model.IsSelected = true);
                }
            }));

            _commandStackService.CommandsChanged += () =>
            {
                RaisePropertyChanged(nameof(ActionsList));
                RaisePropertyChanged(nameof(UndoButtonTooltip));
                RaisePropertyChanged(nameof(RedoButtonTooltip));

                (UndoOneAction as RelayCommand)?.RaiseCanExecuteChanged();
                (RedoOneAction as RelayCommand)?.RaiseCanExecuteChanged();
                (UndoActions as RelayCommand)?.RaiseCanExecuteChanged();
                (RedoActions as RelayCommand)?.RaiseCanExecuteChanged();
            };
        }

        public List<CommandViewModel> ActionsList
        {
            get => _actionsList;
            set
            {
                _actionsList = value; 
                RaisePropertyChanged();
            }
        }

        public ICommand UndoOneAction { get; }
        public ICommand RedoOneAction { get; }
        public int NumOfUndoActions { get; set; }
        public int NumOfRedoActions { get; set; }
        public ICommand UndoActions { get; }
        public ICommand RedoActions { get; }
        public string StrongName => nameof(UndoRedoMenuItemViewModel);

        public string UndoButtonTooltip => _localizerService.GetLocalizedString("Cancel") + " " +
                                           _commandStackService.GetPrevAvailableCommands().FirstOrDefault();
        public string RedoButtonTooltip => _localizerService.GetLocalizedString("Redo") + " " +
                                           _commandStackService.GetNextAvailableCommands().FirstOrDefault();

        public bool IsCommandsPopupOpen
        {
            get => _isCommandsPopupOpen;
            set
            {
                _isCommandsPopupOpen = value; 
                RaisePropertyChanged();
            }
        }
        public ICommand SelectCommand { get; }

    }

    public class CommandViewModel:ViewModelBase
    {
        private bool _isSelected;

        public CommandViewModel(int numberOfCommand, string name)
        {
            NumberOfCommand = numberOfCommand;
            Name = name;
        }

        public string Name { get; }
        public int NumberOfCommand { get; }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                RaisePropertyChanged();
            }
        }
    }
}