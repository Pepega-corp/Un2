using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Unicon2.Fragments.Configuration.Editor.Interfaces;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Services;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Configuration.Editor.ViewModels
{
    public class BaseValuesWindowViewModel : ViewModelBase
    {
        private readonly IBaseValuesViewModel _baseValuesViewModel;
        private string _message;

        public BaseValuesWindowViewModel(IBaseValuesViewModel baseValuesViewModel)
        {
            _baseValuesViewModel = baseValuesViewModel;
            BaseValuesViewModels = new ObservableCollection<IBaseValueViewModel>();
            AddBaseValueCommand = new RelayCommand(OnAddBaseValueExecute);
            OnOk = new RelayCommand<object>(OnOkExecute);
            OnCancel = new RelayCommand<object>(OnCancelExecute);
            DeleteBaseValueCommand = new RelayCommand<object>(OnDeleteBaseValueExecute);
            UploadBaseValueMemoryFromFileCommand =
                new RelayCommand<object>(OnUploadBaseValueMemoryFromFileCommandExecute);
            BaseValuesViewModels.AddCollection(baseValuesViewModel.BaseValuesViewModels.Select(model =>
                new BaseValueViewModel()
                    {Name = model.Name, BaseValuesMemory = model.BaseValuesMemory, IsBaseValuesMemoryFilled = true}));
        }

        private void OnUploadBaseValueMemoryFromFileCommandExecute(object obj)
        {
            if (!(obj is IBaseValueViewModel baseValueViewModel)) return;
            var filePath = StaticContainer.Container.Resolve<IApplicationGlobalCommands>()
                .SelectFileToOpen("", " CNF файл (*.cnf)|*.cnf");
            filePath.OnSuccess(info =>
            {
                var memoryValues = StaticContainer.Container.Resolve<ISerializerService>()
                    .DeserializeFromFile<Dictionary<ushort, ushort>>(info.FullName);
                baseValueViewModel.BaseValuesMemory = memoryValues;
                baseValueViewModel.IsBaseValuesMemoryFilled = true;
                CheckValuesFilled();
                _message = info.Name + " " + StaticContainer.Container.Resolve<ILocalizerService>()
                    .GetLocalizedString("Loaded");
                RaisePropertyChanged(nameof(Message));
            });
        }

        private void OnDeleteBaseValueExecute(object obj)
        {
            if (obj is IBaseValueViewModel baseValueViewModel)
            {
                BaseValuesViewModels.Remove(baseValueViewModel);
            }
        }

        private void OnCancelExecute(object obj)
        {
            if (obj is Window window)
            {
                window.Close();
            }
        }

        private bool CheckValuesFilled()
        {
            if (!BaseValuesViewModels.All(model => model.IsBaseValuesMemoryFilled))
            {
                _message = StaticContainer.Container.Resolve<ILocalizerService>().GetLocalizedString(ApplicationGlobalNames.StatusMessages.VALUES_MUST_BE_FILLED);
                RaisePropertyChanged(nameof(Message));
                return false;
            }

            return true;
        }

        private void OnOkExecute(object obj)
        {
            if (!CheckValuesFilled())
            {
                return;
            }
            _baseValuesViewModel.BaseValuesViewModels.Clear();
            _baseValuesViewModel.BaseValuesViewModels.AddCollection(BaseValuesViewModels);
            if (obj is Window window)
            {
                window.Close();
            }
        }

        private void OnAddBaseValueExecute()
        {
            BaseValuesViewModels.Add(new BaseValueViewModel(){Name = StaticContainer.Container.Resolve<ILocalizerService>().GetLocalizedString("New")});
        }

        public ObservableCollection<IBaseValueViewModel> BaseValuesViewModels { get; }
        public ICommand UploadBaseValueMemoryFromFileCommand { get; }
        public ICommand AddBaseValueCommand { get; }
        public ICommand DeleteBaseValueCommand { get; }
        public ICommand OnOk { get; }
        public ICommand OnCancel { get; }

        public string Message => _message;
    }
}