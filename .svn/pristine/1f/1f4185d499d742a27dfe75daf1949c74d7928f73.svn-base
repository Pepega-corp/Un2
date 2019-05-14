using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Unicon2.Fragments.Journals.Editor.Interfaces;
using Unicon2.Fragments.Journals.Editor.Interfaces.JournalParameters;
using Unicon2.Fragments.Journals.Editor.Views;
using Unicon2.Fragments.Journals.Infrastructure.Model;
using Unicon2.Fragments.Journals.Infrastructure.Model.JournalParameters;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Interfaces.Factories;
using Unicon2.Unity.Commands;

namespace Unicon2.Fragments.Journals.Editor.ViewModel.JournalParameters
{
    public class DependentJournalParameterEditorViewModel : JournalParameterEditorViewModel,
        IDependentJournalParameterEditorViewModel
    {
        private readonly IJournalConditionEditorViewModelFactory _journalConditionEditorViewModelFactory;
        private readonly IApplicationGlobalCommands _applicationGlobalCommands;
        private readonly IFormatterEditorFactory _formatterEditorFactory;
        private List<IJournalParameter> _availableJournalParameters;

        public DependentJournalParameterEditorViewModel(IDependentJournalParameter journalParameter,
            IJournalConditionEditorViewModelFactory journalConditionEditorViewModelFactory,
            IApplicationGlobalCommands applicationGlobalCommands, IFormatterEditorFactory formatterEditorFactory) : base(journalParameter)
        {
            this._journalConditionEditorViewModelFactory = journalConditionEditorViewModelFactory;
            this._applicationGlobalCommands = applicationGlobalCommands;
            this._formatterEditorFactory = formatterEditorFactory;
            this.CancelCommand = new RelayCommand<object>(this.OnCancel);
            this.SubmitCommand = new RelayCommand<object>(this.OnSubmit);
            this.AddConditionCommand = new RelayCommand<object>(this.OnAddConditionExecute);
            this.JournalConditionEditorViewModels = new ObservableCollection<IJournalConditionEditorViewModel>();
            this.DeleteConditionCommand = new RelayCommand<object>(this.OnDeleteConditionExecute);
            this.ShowFormatterParameters = new RelayCommand<object>(this.OnShowFormatterParameters);

        }

        private void OnShowFormatterParameters(object obj)
        {
            this._formatterEditorFactory.EditFormatterByUser(this._journalParameter);
            this.RaisePropertyChanged(nameof(this.FormatterString));
        }

        private void OnDeleteConditionExecute(object obj)
        {
            this.JournalConditionEditorViewModels.Remove(obj as IJournalConditionEditorViewModel);
        }

        private void OnAddConditionExecute(object obj)
        {
            this.JournalConditionEditorViewModels.Add(this._journalConditionEditorViewModelFactory
                .CreateJournalConditionEditorViewModel(this._availableJournalParameters));
        }

        #region Implementation of IDependentJournalParameterEditorViewModel

        public ICommand AddConditionCommand { get; }
        public ICommand DeleteConditionCommand { get; }
        public ICommand ShowFormatterParameters { get; }
        public ICommand SubmitCommand { get; }
        public ICommand CancelCommand { get; }
        public ObservableCollection<IJournalConditionEditorViewModel> JournalConditionEditorViewModels { get; }

        public void SetAvaliableJournalParameters(List<IJournalParameter> availableJournalParameters)
        {
            this._availableJournalParameters = availableJournalParameters;
        }

        private void OnSubmit(object obj)
        {
            foreach (IJournalConditionEditorViewModel journalConditionEditorViewModel in this.JournalConditionEditorViewModels)
            {
                journalConditionEditorViewModel.StopEditElement();
            }
            base.StopEditElement();
            if (obj is Window)
            {
                (obj as Window).Close();
            }
        }

        private void OnCancel(object obj)
        {
            this.SetModel(this._journalParameter);
            this.StopEditElement();
            if (obj is Window)
            {
                (obj as Window).Close();
            }
        }

        #endregion


        #region Overrides of JournalParameterEditorViewModel

        public override void StartEditElement()
        {
            this._applicationGlobalCommands.ShowWindowModal(() => new DependentParameterEditorWindow(), this);

            base.StartEditElement();
        }


        protected override void SaveModel()
        {
            (this._journalParameter as IDependentJournalParameter).JournalParameterDependancyConditions.Clear();
            foreach (IJournalConditionEditorViewModel journalConditionEditorViewModel in this.JournalConditionEditorViewModels)
            {
                (this._journalParameter as IDependentJournalParameter).JournalParameterDependancyConditions.Add(
                    journalConditionEditorViewModel.Model as IJournalCondition);
            }
            base.SaveModel();
        }

        protected override void SetModel(object value)
        {
            base.SetModel(value);
            this.JournalConditionEditorViewModels.Clear();
            foreach (IJournalCondition journalParameterDependancyCondition in (this._journalParameter as IDependentJournalParameter).JournalParameterDependancyConditions)
            {
                this.JournalConditionEditorViewModels.Add(
                    this._journalConditionEditorViewModelFactory.CreateJournalConditionEditorViewModel(
                        journalParameterDependancyCondition, this._availableJournalParameters));
            }

        }


        #endregion






    }
}