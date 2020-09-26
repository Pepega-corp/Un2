using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using GongSolutions.Wpf.DragDrop;
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
        private bool _useDefaultMessage;
        private string _defaultMessage;

        public DictionaryMatchingFormatterViewModel()
        {
            KeyValuesDictionary = new ObservableCollection<BindableKeyValuePair<ushort, string>>();
            AddKeyValuePairCommand = new RelayCommand(OnAddKeyValuePairExecute);
            DeleteKeyValuePairCommand = new RelayCommand(OnDeleteKeyValuePairExecute, CanExecuteDeleteKeyValuePair);
            ImportFromSharedTablesCommand = new RelayCommand(OnExecuteImportFromSharedTables);
           // this._validator = new DictionaryMatchingFormatterValidator(this._container.Resolve<ILocalizerService>());
        }

        private void OnExecuteImportFromSharedTables()
        {
            var t=DragDrop.DataFormat;
        }
        public override T Accept<T>(IFormatterViewModelVisitor<T> visitor)
        {
            return visitor.VisitDictionaryMatchFormatter(this);
        }
        private bool CanExecuteDeleteKeyValuePair()
        {
            return SelectedKeyValuePair != null;
        }

        private void OnDeleteKeyValuePairExecute()
        {
            if (!KeyValuesDictionary.Contains(SelectedKeyValuePair)) return;
            KeyValuesDictionary.Remove(SelectedKeyValuePair);
            SelectedKeyValuePair = null;
            FireErrorsChanged(nameof(KeyValuesDictionary));
        }

        private void OnAddKeyValuePairExecute()
        {
            AddNewKeyPair();
            FireErrorsChanged(nameof(KeyValuesDictionary));
        }

        private void AddNewKeyPair()
        {
            if (KeyValuesDictionary.Count == 0)
            {
                KeyValuesDictionary.Add(new BindableKeyValuePair<ushort, string>());
                return;
            }
            ushort maxKey = KeyValuesDictionary.Select(pair => pair.Key).Max();
            BindableKeyValuePair<ushort, string> keyPair = new BindableKeyValuePair<ushort, string> { Key = (ushort)(maxKey + 1), Value = string.Empty };
            KeyValuesDictionary.Add(keyPair);
        }

        public override string StrongName => StringKeys.DICTIONARY_MATCHING_FORMATTER;
        
        public bool IsValid
        {
            get
            {
                FireErrorsChanged(nameof(KeyValuesDictionary));
                return !HasErrors;
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
            get => _keyValuesDictionary;
            set
            {
                _keyValuesDictionary = value;
                RaisePropertyChanged();
            }
        }

        public bool IsKeysAreNumbersOfBits
        {
            get { return _isKeysIsNumbersOfBits; }
            set
            {
                _isKeysIsNumbersOfBits = value;
                RaisePropertyChanged();
            }
        }

        public bool UseDefaultMessage
        {
            get => _useDefaultMessage;
            set
            {
                _useDefaultMessage = value;
                RaisePropertyChanged();
            }
        }

        public string DefaultMessage
        {
            get => _defaultMessage;
            set
            {
                _defaultMessage = value;
                RaisePropertyChanged();
            }
        }

        public BindableKeyValuePair<ushort, string> SelectedKeyValuePair
        {
            get { return _selectedKeyValuePair; }
            set
            {
                if (_selectedKeyValuePair != null) _selectedKeyValuePair.IsInEditMode = false;
                _selectedKeyValuePair = value;
                if (_selectedKeyValuePair != null) _selectedKeyValuePair.IsInEditMode = true;
                RaisePropertyChanged();
                (DeleteKeyValuePairCommand as RelayCommand)?.RaiseCanExecuteChanged();
                FireErrorsChanged(nameof(KeyValuesDictionary));
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
            IsInEditMode = true;
        }

        public void StopEditElement()
        {
           // this.SaveChanges();
            IsInEditMode = false;
        }
    }
}