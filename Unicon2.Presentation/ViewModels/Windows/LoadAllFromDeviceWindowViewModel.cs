using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Unicon2.Infrastructure.Common;
using Unicon2.Presentation.ViewModels.Validators;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Presentation.ViewModels.Windows
{
    public class LoadAllFromDeviceWindowViewModel : ValidatableBindableBase
    {
        private readonly Func<string, Task> _loader;
        private string _pathToFolderToSave;
        private bool _isSavedSuccessfully;
        private bool _isLoadingInProgress;

        public LoadAllFromDeviceWindowViewModel(List<ILoadFragmentViewModel> loadFragmentViewModels,
            Func<string, Task> loader)
        {
            _loader = loader;
            LoadFragmentViewModels = loadFragmentViewModels;
            OpenSavedFolderCommand = new RelayCommand(OnOpenSavedFolder, CanOpenSavedFolder);
            SaveToFolderCommand = new RelayCommand(OnSaveToFolder, CanExecuteSaveToFolder);
            SetFolderCommand = new RelayCommand(OnSetFolder);
        }

        private void OnSetFolder()
        {
            FolderBrowserDialog folderBrowserDialog=new FolderBrowserDialog();
            var result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                PathToFolderToSave = folderBrowserDialog.SelectedPath;
            }
        }

        private bool CanOpenSavedFolder()
        {
            Validate();
            return !IsLoadingInProgress && !HasErrors && IsSavedSuccessfully;
        }

        protected override void OnValidate()
        {
            SetValidationErrors(new LoadAllFromDeviceWindowViewModelValidator().Validate(this));
            base.OnValidate();
        }

        private bool CanExecuteSaveToFolder()
        {
            Validate();
            return !IsLoadingInProgress && !HasErrors;
        }

        private async void OnSaveToFolder()
        {
            try
            {
                IsLoadingInProgress = true;
                await _loader(PathToFolderToSave);
            }
            finally
            {
                IsLoadingInProgress = false;
            }
        }
        

        private void OnOpenSavedFolder()
        {
            Process.Start(PathToFolderToSave);
        }

        public List<ILoadFragmentViewModel> LoadFragmentViewModels { get; }

        public string PathToFolderToSave
        {
            get => _pathToFolderToSave;
            set
            {
                _pathToFolderToSave = value;
                RaisePropertyChanged();
                FireErrorsChanged();
            }
        }

        public bool IsSavedSuccessfully
        {
            get => _isSavedSuccessfully;
            set
            {
                _isSavedSuccessfully = value;
                RaisePropertyChanged();
                OpenSavedFolderCommand.RaiseCanExecuteChanged();
                SaveToFolderCommand.RaiseCanExecuteChanged();
            }
        }

        public bool IsLoadingInProgress
        {
            get => _isLoadingInProgress;
            set
            {
                _isLoadingInProgress = value;
                RaisePropertyChanged();
                OpenSavedFolderCommand.RaiseCanExecuteChanged();
                SaveToFolderCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand OpenSavedFolderCommand { get; }

        public RelayCommand SaveToFolderCommand { get; }
        public RelayCommand SetFolderCommand { get; }

    }

    public interface ILoadFragmentViewModel
    {
        bool IsFragmentLoadingInProgress { get; set; }
        bool IsSelectedForLoading { get; set; }

    }

    public class DefaultLoadFragmentViewModel : ViewModelBase, ILoadFragmentViewModel
    {
        private bool _isFragmentLoadingInProgress;
        private bool _isSelectedForLoading;

        public bool IsFragmentLoadingInProgress
        {
            get => _isFragmentLoadingInProgress;
            set
            {
                _isFragmentLoadingInProgress = value;
                RaisePropertyChanged();
            }
        }

        public bool IsSelectedForLoading
        {
            get => _isSelectedForLoading;
            set
            {
                _isSelectedForLoading = value;
                RaisePropertyChanged();
            }
        }
    }
}