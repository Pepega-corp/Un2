using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Unicon2.Fragments.Journals.Editor.Interfaces;
using Unicon2.Fragments.Journals.Editor.Interfaces.JournalParameters;
using Unicon2.Fragments.Journals.Editor.Views;
using Unicon2.Fragments.Journals.Infrastructure.Model.JournalParameters;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Unity.Commands;

namespace Unicon2.Fragments.Journals.Editor.ViewModel.JournalParameters
{
    public class ComplexJournalParameterEditorViewModel : JournalParameterEditorViewModel, IComplexJournalParameterEditorViewModel
    {
        private readonly IJournalParametersEditorViewModelFactory _journalParametersEditorViewModelFactory;
        private readonly IApplicationGlobalCommands _applicationGlobalCommands;
        private ISubJournalParameterEditorViewModel _selectedSubJournalParameterEditorViewModel;

        public ComplexJournalParameterEditorViewModel(IComplexJournalParameter journalParameter,
            IJournalParametersEditorViewModelFactory journalParametersEditorViewModelFactory,
            IApplicationGlobalCommands applicationGlobalCommands, Func<ISharedBitViewModel> sharedBitViewModelGettingFunc) : base(journalParameter)
        {
            this._journalParametersEditorViewModelFactory = journalParametersEditorViewModelFactory;
            this._applicationGlobalCommands = applicationGlobalCommands;
            this.SubJournalParameterEditorViewModels = new ObservableCollection<ISubJournalParameterEditorViewModel>();
            this.AddSubParameterCommand = new RelayCommand(this.OnAddSubParameterExecute);
            this.SubmitCommand = new RelayCommand<object>(this.OnSubmit);
            this.CancelCommand = new RelayCommand<object>(this.OnCancel);
            this.DeleteParameterCommand = new RelayCommand(this.OnDeleteParameterExecute, this.CanExecuteDeleteParameter);
            this.MainBitNumbersInWordCollection = new ObservableCollection<ISharedBitViewModel>();
            for (int i = 0; i < 16; i++)
            {
                ISharedBitViewModel sharedBitViewModel = sharedBitViewModelGettingFunc();
                sharedBitViewModel.NumberOfBit = i;
                this.MainBitNumbersInWordCollection.Add(sharedBitViewModel);
            }
        }

        private bool CanExecuteDeleteParameter()
        {
            return this._selectedSubJournalParameterEditorViewModel != null;
        }

        private void OnDeleteParameterExecute()
        {
            this.SubJournalParameterEditorViewModels.Remove(this._selectedSubJournalParameterEditorViewModel);
        }


        private void OnSubmit(object obj)
        {
            foreach (ISubJournalParameterEditorViewModel subJournalParameterEditorViewModel in this.SubJournalParameterEditorViewModels)
            {
                subJournalParameterEditorViewModel.StopEditElement();
            }
            this.StopEditElement();
            (obj as Window)?.Close();
        }

        private void OnCancel(object obj)
        {
            this.SetModel(this._journalParameter);
            this.StopEditElement();
            (obj as Window)?.Close();
        }

        private void OnAddSubParameterExecute()
        {
            this.SubJournalParameterEditorViewModels.Add(this._journalParametersEditorViewModelFactory.CreateJournalSubParameterEditorViewModel(this.MainBitNumbersInWordCollection));
        }

        public ISubJournalParameterEditorViewModel AddSubJournalParameterEditorViewModel()
        {
            ISubJournalParameterEditorViewModel subParameterEditorViewModel = this._journalParametersEditorViewModelFactory.CreateJournalSubParameterEditorViewModel(this.MainBitNumbersInWordCollection);
            this.SubJournalParameterEditorViewModels.Add(subParameterEditorViewModel);
            return subParameterEditorViewModel;
        }

        public ICommand AddSubParameterCommand { get; }
        public ICommand DeleteParameterCommand { get; }

        public ISubJournalParameterEditorViewModel SelectedSubJournalParameterEditorViewModel
        {
            get { return this._selectedSubJournalParameterEditorViewModel; }
            set
            {
                this._selectedSubJournalParameterEditorViewModel = value;
                (this.DeleteParameterCommand as RelayCommand)?.RaiseCanExecuteChanged();
                this.RaisePropertyChanged();
            }
        }

        public ObservableCollection<ISubJournalParameterEditorViewModel> SubJournalParameterEditorViewModels { get; set; }
        public ObservableCollection<ISharedBitViewModel> MainBitNumbersInWordCollection { get; set; }
        public ICommand SubmitCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        protected override void SetModel(object value)
        {
            if (value is IComplexJournalParameter)
            {
                IComplexJournalParameter complexJournalParameter = value as IComplexJournalParameter;
                this.SubJournalParameterEditorViewModels.Clear();
                foreach (ISubJournalParameter subJournalParameter in complexJournalParameter.ChildJournalParameters)
                {
                    this.SubJournalParameterEditorViewModels.Add(this._journalParametersEditorViewModelFactory.CreateJournalSubParameterEditorViewModel(subJournalParameter, this.MainBitNumbersInWordCollection));

                }
            }
            base.SetModel(value);
        }

        protected override void SaveModel()
        {
            (this._journalParameter as IComplexJournalParameter).ChildJournalParameters.Clear();
            foreach (ISubJournalParameterEditorViewModel subJournalParameterEditorViewModel in this.SubJournalParameterEditorViewModels)
            {
                (this._journalParameter as IComplexJournalParameter).ChildJournalParameters.Add(subJournalParameterEditorViewModel.Model as ISubJournalParameter);
            }
            base.SaveModel();
        }

        public override void StartEditElement()
        {
            //base.StartEditElement();
            this._applicationGlobalCommands.ShowWindowModal(() => new ComplexParameterEditorWindow(), this);
        }
    }
}
