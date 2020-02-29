using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Unicon2.Formatting.Editor.Visitors;
using Unicon2.Formatting.Infrastructure.Keys;
using Unicon2.Formatting.Infrastructure.ViewModel;
using Unicon2.Infrastructure.Common;
using Unicon2.Unity.Commands;

namespace Unicon2.Formatting.Editor.ViewModels
{
    public class DictionaryMatchingFormatterViewModel : UshortsFormatterViewModelBase,
        IDictionaryMatchingFormatterViewModel
    {
        private ObservableCollection<BindableKeyValuePair<ushort, string>> _keyValuesDictionary;
        private BindableKeyValuePair<ushort, string> _selectedKeyValuePair;
        //private DictionaryMatchingFormatterValidator _validator;
        private bool _isKeysIsNumbersOfBits;

        public DictionaryMatchingFormatterViewModel()
        {
            this.KeyValuesDictionary = new ObservableCollection<BindableKeyValuePair<ushort, string>>();
            this.AddKeyValuePairCommand = new RelayCommand(this.OnAddKeyValuePairExecute);
            this.DeleteKeyValuePairCommand = new RelayCommand(this.OnDeleteKeyValuePairExecute, this.CanExecuteDeleteKeyValuePair);
            this.ImportFromSharedTablesCommand = new RelayCommand(this.OnExecuteImportFromSharedTables);
           // this._validator = new DictionaryMatchingFormatterValidator(this._container.Resolve<ILocalizerService>());
        }

        private void OnExecuteImportFromSharedTables()
        {

        }
        public override T Accept<T>(IFormatterViewModelVisitor<T> visitor)
        {
            return visitor.VisitDictionaryMatchFormatter(this);
        }
        private bool CanExecuteDeleteKeyValuePair()
        {
            return this.SelectedKeyValuePair != null;
        }

        private void OnDeleteKeyValuePairExecute()
        {
            if (!this.KeyValuesDictionary.Contains(this.SelectedKeyValuePair)) return;
            this.KeyValuesDictionary.Remove(this.SelectedKeyValuePair);
            this.SelectedKeyValuePair = null;
            this.FireErrorsChanged(nameof(this.KeyValuesDictionary));
        }

        private void OnAddKeyValuePairExecute()
        {
            this.AddNewKeyPair();
            this.FireErrorsChanged(nameof(this.KeyValuesDictionary));
        }

        private void AddNewKeyPair()
        {
            if (this.KeyValuesDictionary.Count == 0)
            {
                this.KeyValuesDictionary.Add(new BindableKeyValuePair<ushort, string>());
                return;
            }
            ushort maxKey = this.KeyValuesDictionary.Select(pair => pair.Key).Max();
            BindableKeyValuePair<ushort, string> keyPair = new BindableKeyValuePair<ushort, string> { Key = (ushort)(maxKey + 1), Value = string.Empty };
            this.KeyValuesDictionary.Add(keyPair);
        }

        public override string StrongName => StringKeys.DICTIONARY_MATCHING_FORMATTER;
        
        public bool IsValid
        {
            get
            {
                this.FireErrorsChanged(nameof(this.KeyValuesDictionary));
                return !this.HasErrors;
            }
        }

        public ICommand AddKeyValuePairCommand { get; }

        public ICommand ImportFromSharedTablesCommand { get; }

        public ICommand DeleteKeyValuePairCommand { get; }

        public override object Clone()
        {
            DictionaryMatchingFormatterViewModel cloneDictionaryMatchingFormatterViewModel = new DictionaryMatchingFormatterViewModel();
      //      cloneDictionaryMatchingFormatterViewModel.InitFromFormatter(this._dictionaryMatchingFormatter.Clone() as IUshortsFormatter);
            return cloneDictionaryMatchingFormatterViewModel;
        }

        public ObservableCollection<BindableKeyValuePair<ushort, string>> KeyValuesDictionary
        {
            get => this._keyValuesDictionary;
            set
            {
                this._keyValuesDictionary = value;
                this.RaisePropertyChanged();
            }
        }

        public bool IsKeysAreNumbersOfBits
        {
            get { return this._isKeysIsNumbersOfBits; }
            set
            {
                this._isKeysIsNumbersOfBits = value;
                this.RaisePropertyChanged();
            }
        }

        public BindableKeyValuePair<ushort, string> SelectedKeyValuePair
        {
            get { return this._selectedKeyValuePair; }
            set
            {
                if (this._selectedKeyValuePair != null) this._selectedKeyValuePair.IsInEditMode = false;
                this._selectedKeyValuePair = value;
                if (this._selectedKeyValuePair != null) this._selectedKeyValuePair.IsInEditMode = true;
                this.RaisePropertyChanged();
                (this.DeleteKeyValuePairCommand as RelayCommand)?.RaiseCanExecuteChanged();
                this.FireErrorsChanged(nameof(this.KeyValuesDictionary));
            }
        }

        protected override void OnValidate()
        {
          //  FluentValidation.Results.ValidationResult result = this._validator.Validate(this);
         //   this.SetValidationErrors(result);
        }

        public bool IsInEditMode { get; set; }

        public void StartEditElement()
        {
            this.IsInEditMode = true;
        }

        public void StopEditElement()
        {
           // this.SaveChanges();
            this.IsInEditMode = false;
        }
    }
}