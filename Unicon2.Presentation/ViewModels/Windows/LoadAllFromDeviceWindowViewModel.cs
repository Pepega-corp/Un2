using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.ViewModels.Validators;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Presentation.ViewModels.Windows
{
    public class LoadAllFromDeviceWindowViewModel : ValidatableBindableBase
    {
        private readonly Func<string, Task<LoadingResult>> _loader;
        private readonly string _deviceSignature;
        private string _pathToFolderToSave;
        private bool _isSavedSuccessfully;
        private bool _isLoadingInProgress;
        private string _saveIndicatorMessage;

        public LoadAllFromDeviceWindowViewModel(List<ILoadFragmentViewModel> loadFragmentViewModels,
            Func<string, Task<LoadingResult>> loader, string deviceSignature)
        {
            _loader = loader;
            _deviceSignature = deviceSignature;
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
               var res= await _loader(PathToFolderToSave);
               SetLoadingResult(res);
            }
            catch
            {
                SetLoadingResult(LoadingResult.Fail);
            }
            finally
            {
                IsLoadingInProgress = false;
            }
        }
        

        private void OnOpenSavedFolder()
        {
            Process.Start(PathToFolderToSave+ "\\" + _deviceSignature);
        }

        private void SetLoadingResult(LoadingResult loadingResult)
        {
            var localizer = StaticContainer.Container.Resolve<ILocalizerService>();
            switch (loadingResult)
            {
                case LoadingResult.Success:
                    SaveIndicatorMessage =
                        localizer.GetLocalizedString(ApplicationGlobalNames.StatusMessages.SAVED_SUCCESSFULLY) + ": " +
                        PathToFolderToSave;
                    break;
                case LoadingResult.SuccessWithIssues:
                    SaveIndicatorMessage =
                       $"{localizer.GetLocalizedString(ApplicationGlobalNames.StatusMessages.SAVED_WITH_ISSUES)}:{PathToFolderToSave}" ;                   
                    break;
                case LoadingResult.Fail:
                    SaveIndicatorMessage =
                        $"{localizer.GetLocalizedString(ApplicationGlobalNames.StatusMessages.FAIL)}" ;       
                    break;
                case LoadingResult.InProgress:
                    SaveIndicatorMessage =
                        localizer.GetLocalizedString(ApplicationGlobalNames.StatusMessages.IN_PROGRESS) + ": " +
                        PathToFolderToSave;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(loadingResult), loadingResult, null);
            }

            IsSavedSuccessfully = loadingResult == LoadingResult.Success ||
                                  loadingResult == LoadingResult.SuccessWithIssues;
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
                OpenSavedFolderCommand.RaiseCanExecuteChanged();
                SaveToFolderCommand.RaiseCanExecuteChanged();
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

        public string SaveIndicatorMessage
        {
            get => _saveIndicatorMessage;
            set
            {
                _saveIndicatorMessage = value; 
                RaisePropertyChanged();
            }
        }

        public RelayCommand SaveToFolderCommand { get; }
        public RelayCommand SetFolderCommand { get; }

    }

    public interface ILoadFragmentViewModel
    {
        bool IsFragmentLoadingInProgress { get; set; }
        bool IsSelectedForLoading { get; set; }
        string FragmentName { get; set; }
        string UiName { get; set; }
    }

    public class DefaultLoadFragmentViewModel : ViewModelBase, ILoadFragmentViewModel
    {
        private bool _isFragmentLoadingInProgress;
        private bool _isSelectedForLoading;
        private string _fragmentName;
        private string _uiName;

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

        public string FragmentName
        {
            get => _fragmentName;
            set
            {
                _fragmentName = value;
                RaisePropertyChanged();
            }
        }

        public string UiName
        {
            get => _uiName;
            set
            {
                _uiName = value;
                RaisePropertyChanged();
            }
        }
    }

    public enum LoadingResult
    {
        Success,
        SuccessWithIssues,
        InProgress,
        Fail
    }
}