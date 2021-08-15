using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Presentation.ViewModels
{
    public class ExcelListSelectWindowViewModel : ViewModelBase
    {
        private string _selectedExcelList;

        public ExcelListSelectWindowViewModel(List<string> excelListsAvailable)
        {
            ExcelListsAvailable = excelListsAvailable;
            SelectedExcelList = ExcelListsAvailable.First();
            OnOk = new RelayCommand<object>(OnOkExecute);
            OnCancel=new RelayCommand<object>(OnCancelExecute);
        }

        private void OnOkExecute(object o)
        {
            if (o is Window window)
            {
                IsCancelled = false;
                window.Close();
            }
        }
        private void OnCancelExecute(object o)
        {
            if (o is Window window)
            {
                IsCancelled = true;
                window.Close();
            }
        }

        public bool IsCancelled { get; set; }
        public string SelectedExcelList
        {
            get => _selectedExcelList;
            set
            {
                _selectedExcelList = value;
                RaisePropertyChanged();
            }
        }

        public List<string> ExcelListsAvailable { get; }
        
        public ICommand OnOk { get; }
        public ICommand OnCancel { get; }

    }
}