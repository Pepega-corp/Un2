using System;
using System.Collections.Generic;
using System.ComponentModel;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.ViewModels.Device;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using WPFLocalizeExtension.Engine;

namespace Unicon2.Presentation.Factories
{
    public class FragmentPaneViewModelFactory : IFragmentPaneViewModelFactory
    {
        private readonly Func<IFragmentPaneViewModel> _fragmentPaneViewModelgettingFunc;
        private readonly ILocalizerService _localizerService;

        public FragmentPaneViewModelFactory(Func<IFragmentPaneViewModel> fragmentPaneViewModelgettingFunc, ILocalizerService localizerService)
        {
            _fragmentPaneViewModelgettingFunc = fragmentPaneViewModelgettingFunc;
            _localizerService = localizerService;
        }


        public IFragmentPaneViewModel GetFragmentPaneViewModel(IFragmentViewModel fragmentViewModel,
            IEnumerable<IDeviceViewModel> deviceViewModels)
        {
            IFragmentPaneViewModel fragmentPaneViewModel = _fragmentPaneViewModelgettingFunc();
            fragmentPaneViewModel.FragmentViewModel = fragmentViewModel;


            IDeviceViewModel deviceViewModel = GetParentDevice(deviceViewModels, fragmentViewModel);

            //событие изменения подписи устройства
            void OnDeviceViewModelOnPropertyChanged(object s, PropertyChangedEventArgs e)
            {
                if (s is IDeviceViewModel)
                {
                    IDeviceViewModel dvm = s as IDeviceViewModel;
                    if (e.PropertyName == nameof(dvm.DeviceSignature))
                    {
                        SetPaneTitle(fragmentPaneViewModel, fragmentViewModel, dvm);
                    }
                }
            }

            deviceViewModel.PropertyChanged += OnDeviceViewModelOnPropertyChanged;


            LocalizeDictionary.Instance.PropertyChanged += (o, e) =>
            {
                SetPaneTitle(fragmentPaneViewModel, fragmentViewModel, deviceViewModel);
            };

            SetPaneTitle(fragmentPaneViewModel, fragmentViewModel, deviceViewModel);

            return fragmentPaneViewModel;
        }


        private void SetPaneTitle(IFragmentPaneViewModel fragmentPaneViewModel, IFragmentViewModel fragmentViewModel, IDeviceViewModel deviceViewModel)
        {
            if (!(_localizerService.TryGetLocalizedString(fragmentViewModel.NameForUiKey, out string title)))
            {
                title = fragmentViewModel.NameForUiKey;
            }
            
            fragmentPaneViewModel.FragmentTitle =
                title +
                " (" + deviceViewModel.DeviceSignature + ")";
        }

        private IDeviceViewModel GetParentDevice(IEnumerable<IDeviceViewModel> deviceViewModels,
            IFragmentViewModel fragmentViewModel)
        {
            foreach (IDeviceViewModel deviceViewModel in deviceViewModels)
            {
                if (deviceViewModel.FragmentViewModels.Contains(fragmentViewModel)) return deviceViewModel;
            }
            return null;
        }

    }
}
