using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Unicon2.Fragments.Journals.Editor.Interfaces;
using Unicon2.Fragments.Journals.Editor.Interfaces.JournalParameters;
using Unicon2.Fragments.Journals.Infrastructure.Keys;
using Unicon2.Fragments.Journals.Infrastructure.Model;
using Unicon2.Fragments.Journals.Infrastructure.Model.JournalParameters;
using Unicon2.Infrastructure.Common;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Journals.Editor.ViewModel
{

    public class RecordTemplateEditorViewModel : ViewModelBase, IRecordTemplateEditorViewModel
    {
        private readonly IJournalParametersEditorViewModelFactory _journalParametersEditorViewModelFactory;
        private readonly IFormatterEditorFactory _formatterEditorFactory;
        private IRecordTemplate _recordTemplate;
        private IJournalParameterEditorViewModel _selectedJournalParameterEditorViewModel;
        private object _model;

        public RecordTemplateEditorViewModel(IJournalParametersEditorViewModelFactory journalParametersEditorViewModelFactory,
            IFormatterEditorFactory formatterEditorFactory, IRecordTemplate recordTemplate
            )
        {
            this._recordTemplate = recordTemplate;
            this.AllJournalParameterEditorViewModels = new ObservableCollection<IJournalParameterEditorViewModel>();
            this._journalParametersEditorViewModelFactory = journalParametersEditorViewModelFactory;
            this._formatterEditorFactory = formatterEditorFactory;
            this.AddRecordParameterCommand = new RelayCommand(this.OnAddRecordParameterExecute);
            this.AddComplexRecordParameterCommand = new RelayCommand(this.OnAddComplexRecordParameterExecute);
            this.EditElementCommand = new RelayCommand(this.OnEditElementExecute, this.CanExecuteEditElement);
            this.SetElementUpCommand = new RelayCommand(this.OnSetElementUpExecute, this.CanExecuteSetElementUp);
            this.SetElementDownCommand = new RelayCommand(this.OnSetElementDownExecute, this.CanExecuteSetElementDown);
            this.DeleteElementCommand = new RelayCommand(this.OnDeleteElementExecute, this.CanDeleteElementDown);
            this.AddDependentParameterCommand = new RelayCommand(this.OnAddDependentParameterExecute);
            this.ShowFormatterParametersCommand =
                new RelayCommand(this.OnShowFormatterParametersExecute, this.CanShowFormatterParametersExecute);

        }

        public ObservableCollection<IJournalParameterEditorViewModel> AllJournalParameterEditorViewModels { get; set; }


        private void OnAddDependentParameterExecute()
        {
            IDependentJournalParameterEditorViewModel newJournalParameterEditorViewModel = this._journalParametersEditorViewModelFactory.CreateDependentJournalParameterEditorViewModel(this.GetAllJournalParameters());
            this.AllJournalParameterEditorViewModels.Add(newJournalParameterEditorViewModel);
            this.SelectedJournalParameterEditorViewModel = newJournalParameterEditorViewModel;
            this.CompleteAdding();
        }


        private List<IJournalParameter> GetAllJournalParameters()
        {
            List<IJournalParameter> allJournalParameters = new List<IJournalParameter>();

            foreach (IJournalParameterEditorViewModel journalParameterEditorViewModel in this.AllJournalParameterEditorViewModels)
            {
                IJournalParameter journalParameter = journalParameterEditorViewModel.Model as IJournalParameter;
                allJournalParameters.Add(journalParameter);
                if (journalParameter is IComplexJournalParameter)
                {
                    allJournalParameters.AddRange((journalParameter as IComplexJournalParameter).ChildJournalParameters);
                }
            }
            return allJournalParameters;
        }

        private bool CanShowFormatterParametersExecute()
        {
            return ((this.SelectedJournalParameterEditorViewModel != null) && (!(this.SelectedJournalParameterEditorViewModel is IComplexJournalParameterEditorViewModel)));
        }

        private void OnShowFormatterParametersExecute()
        {
            this._formatterEditorFactory.EditFormatterByUser(SelectedJournalParameterEditorViewModel);
            this.SelectedJournalParameterEditorViewModel.StopEditElement();
        }

        private bool CanDeleteElementDown()
        {
            return this.SelectedJournalParameterEditorViewModel != null;
        }

        private void OnDeleteElementExecute()
        {
            this._recordTemplate.JournalParameters.Remove(this.SelectedJournalParameterEditorViewModel.Model as IJournalParameter);
            this.AllJournalParameterEditorViewModels.Remove(this.SelectedJournalParameterEditorViewModel);
        }

        private bool CanExecuteSetElementDown()
        {
            if (this.AllJournalParameterEditorViewModels.Count < 2) return false;
            int startIndex = this.AllJournalParameterEditorViewModels.IndexOf(this.SelectedJournalParameterEditorViewModel);
            int destIndex = startIndex + 1;
            if ((destIndex < 0) || (this.AllJournalParameterEditorViewModels.Count <= destIndex)) return false;
            return true;
        }

        private void OnSetElementDownExecute()
        {
            int startIndex = this.AllJournalParameterEditorViewModels.IndexOf(this.SelectedJournalParameterEditorViewModel);
            int destIndex = startIndex + 1;
            if ((destIndex < 0) || (this.AllJournalParameterEditorViewModels.Count <= destIndex)) return;
            this.AllJournalParameterEditorViewModels.Move(startIndex, destIndex);
            this._recordTemplate.JournalParameters.Clear();
            foreach (IJournalParameterEditorViewModel parameterEditorViewModel in this.AllJournalParameterEditorViewModels)
            {
                this._recordTemplate.JournalParameters.Add(parameterEditorViewModel.Model as IJournalParameter);
            }
            this.SelectedJournalParameterEditorViewModel = this.AllJournalParameterEditorViewModels[destIndex];

        }

        private bool CanExecuteSetElementUp()
        {
            if (this.AllJournalParameterEditorViewModels.Count < 2) return false;
            int startIndex = this.AllJournalParameterEditorViewModels.IndexOf(this.SelectedJournalParameterEditorViewModel);
            int destIndex = startIndex - 1;
            if (destIndex < 0) return false;
            return true;
        }

        private void OnSetElementUpExecute()
        {
            int startIndex = this.AllJournalParameterEditorViewModels.IndexOf(this.SelectedJournalParameterEditorViewModel);
            int destIndex = startIndex - 1;
            if (destIndex < 0) return;
            this.AllJournalParameterEditorViewModels.Move(startIndex, destIndex);
            this._recordTemplate.JournalParameters.Clear();
            foreach (IJournalParameterEditorViewModel parameterEditorViewModel in this.AllJournalParameterEditorViewModels)
            {
                this._recordTemplate.JournalParameters.Add(parameterEditorViewModel.Model as IJournalParameter);
            }
            this.SelectedJournalParameterEditorViewModel = this.AllJournalParameterEditorViewModels[destIndex];
        }

        private bool CanExecuteEditElement()
        {
            return this.SelectedJournalParameterEditorViewModel != null;
        }

        private void OnEditElementExecute()
        {
            this.SelectedJournalParameterEditorViewModel.StartEditElement();
        }

        private void OnAddComplexRecordParameterExecute()
        {
            IComplexJournalParameterEditorViewModel newJournalParameterEditorViewModel = this._journalParametersEditorViewModelFactory
                .CreateComplexJournalParameterEditorViewModel();
            this.AllJournalParameterEditorViewModels.Add(newJournalParameterEditorViewModel);
            this.SelectedJournalParameterEditorViewModel = newJournalParameterEditorViewModel;
            this.CompleteAdding();
        }

        private void OnAddRecordParameterExecute()
        {
            IJournalParameterEditorViewModel newJournalParameterEditorViewModel = this._journalParametersEditorViewModelFactory
                .CreateJournalParameterEditorViewModel();
            this.AllJournalParameterEditorViewModels.Add(newJournalParameterEditorViewModel);
            this.SelectedJournalParameterEditorViewModel = newJournalParameterEditorViewModel;
            this.CompleteAdding();
        }

        private void CompleteAdding()
        {
            this.SelectedJournalParameterEditorViewModel?.StartEditElement();
        }


        public IJournalParameterEditorViewModel SelectedJournalParameterEditorViewModel
        {
            get { return this._selectedJournalParameterEditorViewModel; }
            set
            {
                this._selectedJournalParameterEditorViewModel?.StopEditElement();
                this._selectedJournalParameterEditorViewModel = value;
                (this.EditElementCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (this.SetElementDownCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (this.SetElementUpCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (this.ShowFormatterParametersCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (this.DeleteElementCommand as RelayCommand)?.RaiseCanExecuteChanged();

                this.RaisePropertyChanged();
            }
        }

        protected void SetModel(object value)
        {
            if (value == null) return;
            this._recordTemplate = value as IRecordTemplate;
            this.AllJournalParameterEditorViewModels.Clear();
            foreach (IJournalParameter journalParameter in this._recordTemplate.JournalParameters)
            {
                if (journalParameter is IComplexJournalParameter)
                {
                    this.AllJournalParameterEditorViewModels.Add(this._journalParametersEditorViewModelFactory
                        .CreateComplexJournalParameterEditorViewModel(journalParameter as IComplexJournalParameter));
                }
                else if (journalParameter is IDependentJournalParameter)
                {
                    this.AllJournalParameterEditorViewModels.Add(this._journalParametersEditorViewModelFactory
                        .CreateDependentJournalParameterEditorViewModel(journalParameter as IDependentJournalParameter, this.GetAllJournalParameters()));
                }
                else
                {
                    this.AllJournalParameterEditorViewModels.Add(this._journalParametersEditorViewModelFactory
                        .CreateJournalParameterEditorViewModel(journalParameter));
                }
            }
        }


        private IRecordTemplate GetModel()
        {
            this._recordTemplate.JournalParameters.Clear();
            foreach (IJournalParameterEditorViewModel parameterEditorViewModel in this.AllJournalParameterEditorViewModels)
            {
                parameterEditorViewModel.StopEditElement();
                this._recordTemplate.JournalParameters.Add(parameterEditorViewModel.Model as IJournalParameter);
            }
            return this._recordTemplate;
        }

        public ICommand AddRecordParameterCommand { get; }
        public ICommand AddComplexRecordParameterCommand { get; }

        public ICommand AddDependentParameterCommand { get; }


        public ICommand SetElementUpCommand { get; }
        public ICommand SetElementDownCommand { get; }
        public ICommand EditElementCommand { get; }
        public ICommand DeleteElementCommand { get; }
        public ICommand ShowFormatterParametersCommand { get; }


        public string StrongName => JournalKeys.RECORD_TEMPLATE;

        public object Model
        {
            get { return this.GetModel(); }
            set { this.SetModel(value); }
        }
        
    }
}