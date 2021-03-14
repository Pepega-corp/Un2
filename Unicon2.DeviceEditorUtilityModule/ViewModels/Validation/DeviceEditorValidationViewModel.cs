using System;
using System.Collections.Generic;
using System.Windows.Input;
using Unicon2.Presentation.Infrastructure.ViewModels.Validation;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.DeviceEditorUtilityModule.ViewModels.Validation
{
    public class DeviceEditorValidationViewModel:ViewModelBase, IDeviceEditorValidationViewModel
    {
        private readonly Func<List<EditorValidationErrorViewModel>> _getValidationErrors;

        public DeviceEditorValidationViewModel(Func<List<EditorValidationErrorViewModel>> getValidationErrors)
        {
            _getValidationErrors = getValidationErrors;
            ErrorViewModels = new List<EditorValidationErrorViewModel>();
            OpenPopup = new RelayCommand(OnOpenPopupExecute);
            RefreshErrors = new RelayCommand(() =>
            {
                ErrorViewModels = getValidationErrors();
            });
        }

        private void OnOpenPopupExecute()
        {
            IsValidationPopupOpen = true;
        }

        private List<EditorValidationErrorViewModel> _errorViewModels;
        private bool _isSuccess;
        private bool _isValidationPopupOpen;

        public List<EditorValidationErrorViewModel> ErrorViewModels
        {
            get => _errorViewModels;
            set
            {
                _errorViewModels = value;
                RaisePropertyChanged();
                IsSuccess = value.Count == 0;
            }
        }

        
        public bool IsSuccess
        {
            get => _isSuccess;
            set
            {
                _isSuccess = value;
                RaisePropertyChanged();
            }
        }

        public ICommand RefreshErrors { get; }

        public bool IsValidationPopupOpen
        {
            get => _isValidationPopupOpen;
            set
            {
                _isValidationPopupOpen = value;
                RaisePropertyChanged();
            }
        }
        
        public ICommand OpenPopup { get; }
    }
}