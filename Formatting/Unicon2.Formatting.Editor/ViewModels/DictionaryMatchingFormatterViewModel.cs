using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Unicon2.Formatting.Editor.ViewModels.Validators;
using Unicon2.Formatting.Infrastructure.Keys;
using Unicon2.Formatting.Infrastructure.Model;
using Unicon2.Formatting.Infrastructure.ViewModel;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Services;
using Unicon2.Unity.Commands;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Formatting.Editor.ViewModels
{
    public class DictionaryMatchingFormatterViewModel : UshortsFormatterViewModelBase,
        IDictionaryMatchingFormatterViewModel
    {
        private readonly ITypesContainer _container;
        private IDictionaryMatchingFormatter _dictionaryMatchingFormatter;
        private ObservableCollection<BindableKeyValuePair<ushort, string>> _keyValuesDictionary;
        private BindableKeyValuePair<ushort, string> _selectedKeyValuePair;
        private DictionaryMatchingFormatterValidator _validator;
        private bool _isKeysIsNumbersOfBits;

        public DictionaryMatchingFormatterViewModel(ITypesContainer container)
        {
            this._container = container;
            this._dictionaryMatchingFormatter = this._container.Resolve<IUshortsFormatter>(StringKeys.DICTIONARY_MATCHING_FORMATTER) as IDictionaryMatchingFormatter;
            this.KeyValuesDictionary = new ObservableCollection<BindableKeyValuePair<ushort, string>>();
            this.AddKeyValuePairCommand = new RelayCommand(this.OnAddKeyValuePairExecute);
            this.DeleteKeyValuePairCommand = new RelayCommand(this.OnDeleteKeyValuePairExecute, this.CanExecuteDeleteKeyValuePair);
            this.ImportFromSharedTablesCommand = new RelayCommand(this.OnExecuteImportFromSharedTables);
            this._validator = new DictionaryMatchingFormatterValidator(this._container.Resolve<ILocalizerService>());
        }

        private void OnExecuteImportFromSharedTables()
        {

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

        public override string StrongName => this._dictionaryMatchingFormatter?.StrongName;

        public override IUshortsFormatter GetFormatter()
        {
            this.SaveChanges();
            return this._dictionaryMatchingFormatter;
        }

        public override void InitFromFormatter(IUshortsFormatter ushortsFormatter)
        {
            this.KeyValuesDictionary.Clear();
            if (ushortsFormatter == null)
            {
                this._dictionaryMatchingFormatter = this._container.Resolve<IUshortsFormatter>(StringKeys.DICTIONARY_MATCHING_FORMATTER) as IDictionaryMatchingFormatter;
                this.IsKeysAreNumbersOfBits = false;
            }
            if (!(ushortsFormatter is IDictionaryMatchingFormatter)) return;
            IDictionaryMatchingFormatter dictionaryMatchingFormatter = ushortsFormatter as IDictionaryMatchingFormatter;
            this._dictionaryMatchingFormatter = dictionaryMatchingFormatter;
            foreach (KeyValuePair<ushort, string> kvp in this._dictionaryMatchingFormatter.StringDictionary)
            {
                this.KeyValuesDictionary.Add(new BindableKeyValuePair<ushort, string>(kvp.Key, kvp.Value));
            }
            this.IsKeysAreNumbersOfBits = this._dictionaryMatchingFormatter.IsKeysAreNumbersOfBits;
        }

        private void SaveChanges()
        {
            this._dictionaryMatchingFormatter.StringDictionary = new Dictionary<ushort, string>();
            foreach (BindableKeyValuePair<ushort, string> bkvp in this.KeyValuesDictionary)
            {
                this._dictionaryMatchingFormatter.StringDictionary.Add(bkvp.Key, bkvp.Value);
            }
            this._dictionaryMatchingFormatter.IsKeysAreNumbersOfBits = this.IsKeysAreNumbersOfBits;
        }

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

        public override object Model
        {
            get => this._dictionaryMatchingFormatter;
            set
            {
                if (value is IDictionaryMatchingFormatter)
                    this._dictionaryMatchingFormatter = (IDictionaryMatchingFormatter)value;
                this.InitFromFormatter(this._dictionaryMatchingFormatter);
            }
        }

        public override object Clone()
        {
            DictionaryMatchingFormatterViewModel cloneDictionaryMatchingFormatterViewModel = new DictionaryMatchingFormatterViewModel(this._container);
            this.SaveChanges();
            cloneDictionaryMatchingFormatterViewModel.InitFromFormatter(this._dictionaryMatchingFormatter.Clone() as IUshortsFormatter);
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
            FluentValidation.Results.ValidationResult result = this._validator.Validate(this);
            this.SetValidationErrors(result);
        }

        public bool IsInEditMode { get; set; }

        public void StartEditElement()
        {
            this.IsInEditMode = true;
        }

        public void StopEditElement()
        {
            this.SaveChanges();
            this.IsInEditMode = false;
        }
    }
}