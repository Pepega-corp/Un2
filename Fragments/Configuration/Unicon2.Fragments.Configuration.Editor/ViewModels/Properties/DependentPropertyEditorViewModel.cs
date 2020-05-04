using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Unicon2.Fragments.Configuration.Editor.Interfaces.DependentProperty;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Tree;
using Unicon2.Fragments.Configuration.Editor.View;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.DependentProperty;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Unity.Commands;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Configuration.Editor.ViewModels.Properties
{
    public class DependentPropertyEditorViewModel : PropertyEditorViewModel, IDependentPropertyEditorViewModel
    {
        private readonly IApplicationGlobalCommands _applicationGlobalCommands;
        private readonly Func<IConditionViewModel> _conditionViewModelGettingFunc;
        private readonly IFormatterEditorFactory _formatterEditorFactory;
        private IConditionViewModel _selectedConditionViewModel;

        public DependentPropertyEditorViewModel(ITypesContainer container, IRangeViewModel rangeViewModel, ILocalizerService localizerService,
	        IApplicationGlobalCommands applicationGlobalCommands, Func<IConditionViewModel> conditionViewModelGettingFunc,IFormatterEditorFactory formatterEditorFactory) : base(container, rangeViewModel, localizerService)
        {
            _applicationGlobalCommands = applicationGlobalCommands;
            _conditionViewModelGettingFunc = conditionViewModelGettingFunc;
            _formatterEditorFactory = formatterEditorFactory;
            SubmitCommand = new RelayCommand<object>(OnSubmitExecute);
            CancelCommand = new RelayCommand<object>(OnCancelExecute);
            ConditionViewModels = new ObservableCollection<IConditionViewModel>();
            AddConditionCommand = new RelayCommand(OnAddConditionExecute);
            DeleteConditionCommand = new RelayCommand(OnDeleteConditionExecute, CanExecuteDeleteCondition);
            ShowFormatterParameters = new RelayCommand(() =>
            {
	            _formatterEditorFactory.EditFormatterByUser(this);
            });
		}

        public RelayCommand ShowFormatterParameters { get; }

        private bool CanExecuteDeleteCondition()
        {
            return SelectedConditionViewModel != null;
        }

        private void OnDeleteConditionExecute()
        {
            SelectedConditionViewModel.Dispose();
            ConditionViewModels.Remove(SelectedConditionViewModel);
        }

        private void OnAddConditionExecute()
        {
            IConditionViewModel newConditionViewModel = _conditionViewModelGettingFunc();
            ConditionViewModels.Add(newConditionViewModel);
        }

        private void OnSubmitExecute(object obj)
        {
            StopEditElement();
            (obj as Window)?.Close();
        }

        public override T Accept<T>(IConfigurationItemViewModelVisitor<T> visitor)
        {
            return visitor.VisitDependentProperty(this);
        }

        private void OnCancelExecute(object obj)
        {
	        (obj as Window)?.Close();
        }


        public override string StrongName => ConfigurationKeys.DEPENDENT_PROPERTY +
                                             ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL;




        protected override string GetTypeName()
        {
            return ConfigurationKeys.DEPENDENT_PROPERTY;
        }


        public override void StartEditElement()
        {
            _isInEditMode = true;
            _applicationGlobalCommands.ShowWindowModal((() => new DependentPropertyEditorWindow()), this);
        }


        public ICommand SubmitCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand AddConditionCommand { get; }
        public ICommand DeleteConditionCommand { get; }

        public IConditionViewModel SelectedConditionViewModel
        {
            get { return _selectedConditionViewModel; }
            set
            {
                _selectedConditionViewModel = value;
                ((RelayCommand)DeleteConditionCommand).RaiseCanExecuteChanged();
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<IConditionViewModel> ConditionViewModels { get; set; }
    }
}
