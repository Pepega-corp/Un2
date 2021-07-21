using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Unicon2.Formatting.Editor.ViewModels.Validators;
using Unicon2.Formatting.Editor.Visitors;
using Unicon2.Formatting.Infrastructure.Keys;
using Unicon2.Formatting.Infrastructure.ViewModel;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Interfaces.Excel;
using Unicon2.Infrastructure.Services;
using Unicon2.Unity.Commands;
using DragDrop = GongSolutions.Wpf.DragDrop.DragDrop;


namespace Unicon2.Formatting.Editor.ViewModels
{
    public class DictionaryMatchingFormatterViewModel : UshortsFormatterViewModelBase,
        IDictionaryMatchingFormatterViewModel
    {
        private ObservableCollection<BindableKeyValuePair<ushort, string>> _keyValuesDictionary;
        private BindableKeyValuePair<ushort, string> _selectedKeyValuePair;
        private DictionaryMatchingFormatterValidator _validator;
        private bool _isKeysIsNumbersOfBits;
        private bool _useDefaultMessage;
        private string _defaultMessage;

        public DictionaryMatchingFormatterViewModel()
        {
            KeyValuesDictionary = new ObservableCollection<BindableKeyValuePair<ushort, string>>();
            AddKeyValuePairCommand = new RelayCommand(OnAddKeyValuePairExecute);
            DeleteKeyValuePairCommand = new RelayCommand(OnDeleteKeyValuePairExecute, CanExecuteDeleteKeyValuePair);
            ImportFromSharedTablesCommand = new RelayCommand(OnExecuteImportFromSharedTables);
            this._validator = new DictionaryMatchingFormatterValidator(StaticContainer.Container.Resolve<ILocalizerService>());
           ImportFromExcelCommand = new RelayCommand(OnImportFromExcel);
           ExportToExcelCommand = new RelayCommand(OnExportToExcel);
        }

        private void OnImportFromExcel()
        {
            var dictionaryRes = StaticContainer.Container.Resolve<IExcelImporter>().ImportFromExcel(worksheet =>
            {
                if (string.IsNullOrWhiteSpace(worksheet.GetCellValue(1, 3).Item))
                {
                    return;
                }

                try
                {
                    var dictionary = new Dictionary<ushort, string>();
                    int counter = 2;
                    while (worksheet.GetCellValue(counter, 1).Item != null &&
                           !string.IsNullOrWhiteSpace(worksheet.GetCellValue(counter, 1).Item.ToString()))
                    {
                        dictionary[ushort.Parse(worksheet.GetCellValue(counter, 2).Item.ToString())] =
                            worksheet.GetCellValue(counter, 3).Item.ToString();
                        counter++;
                    }

                    KeyValuesDictionary.Clear();
                    foreach (KeyValuePair<ushort, string> kvp in dictionary)
                    {
                        KeyValuesDictionary.Add(new BindableKeyValuePair<ushort, string>(kvp.Key, kvp.Value));
                    }

                }
                catch
                {
                    MessageBox.Show(StaticContainer.Container.Resolve<ILocalizerService>().GetLocalizedString(
                        ApplicationGlobalNames.StatusMessages
                            .FORMAT_ERROR), "Excel");
                }
            });
        }

        private async void OnExportToExcel()
        {
            
            var dictionary = new Dictionary<ushort, string>();
            foreach (BindableKeyValuePair<ushort, string> bkvp in KeyValuesDictionary)
            {
                dictionary.Add(bkvp.Key, bkvp.Value);
            }
            await StaticContainer.Container.Resolve<IExcelExporter>().ExportToExcel(
                (worksheet)=>
            {
                //Add the headers
                worksheet.SetCellValue(1,1, "№");
                worksheet.SetCellValue(1,2, "Ключ");
                worksheet.SetCellValue(1,3, "Значение");
                int counter = 0;
                foreach (var keyValue in dictionary)
                {
                    counter++;
                    worksheet.SetCellValue(counter + 1,1, counter.ToString());
                    worksheet.SetCellValue(counter + 1,2, keyValue.Key.ToString());
                    worksheet.SetCellValue(counter + 1,3, keyValue.Value.ToString());
                }
            }, "dict");
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
            DictionaryMatchingFormatterViewModel cloneDictionaryMatchingFormatterViewModel =
                new DictionaryMatchingFormatterViewModel();
            cloneDictionaryMatchingFormatterViewModel.KeyValuesDictionary =
                new ObservableCollection<BindableKeyValuePair<ushort, string>>(this.KeyValuesDictionary.ToList());
            //      cloneDictionaryMatchingFormatterViewModel.InitFromFormatter(this._dictionaryMatchingFormatter.Clone() as IUshortsFormatter);
            cloneDictionaryMatchingFormatterViewModel.DefaultMessage = DefaultMessage;
            cloneDictionaryMatchingFormatterViewModel.UseDefaultMessage = UseDefaultMessage;
            cloneDictionaryMatchingFormatterViewModel.IsKeysAreNumbersOfBits = IsKeysAreNumbersOfBits;

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

        public ICommand ImportFromExcelCommand { get; }
        public ICommand ExportToExcelCommand { get; }

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
            FluentValidation.Results.ValidationResult result = this._validator.Validate(this);
            this.SetValidationErrors(result);
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