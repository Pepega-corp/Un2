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
            this._fragmentPaneViewModelgettingFunc = fragmentPaneViewModelgettingFunc;
            this._localizerService = localizerService;
        }


        public IFragmentPaneViewModel GetFragmentPaneViewModel(IFragmentViewModel fragmentViewModel,
            IEnumerable<IDeviceViewModel> deviceViewModels)
        {
            IFragmentPaneViewModel fragmentPaneViewModel = this._fragmentPaneViewModelgettingFunc();
            fragmentPaneViewModel.FragmentViewModel = fragmentViewModel;


            IDeviceViewModel deviceViewModel = this.GetParentDevice(deviceViewModels, fragmentViewModel);

            //событие изменения подписи устройства
            void OnDeviceViewModelOnPropertyChanged(object s, PropertyChangedEventArgs e)
            {
                if (s is IDeviceViewModel)
                {
                    IDeviceViewModel dvm = s as IDeviceViewModel;
                    if (e.PropertyName == nameof(dvm.DeviceSignature))
                    {
                        this.SetPaneTitle(fragmentPaneViewModel, fragmentViewModel, dvm);
                    }
                }
            }

            deviceViewModel.PropertyChanged += OnDeviceViewModelOnPropertyChanged;


            LocalizeDictionary.Instance.PropertyChanged += (o, e) =>
            {
                this.SetPaneTitle(fragmentPaneViewModel, fragmentViewModel, deviceViewModel);
            };

            this.SetPaneTitle(fragmentPaneViewModel, fragmentViewModel, deviceViewModel);

            return fragmentPaneViewModel;
        }


        private void SetPaneTitle(IFragmentPaneViewModel fragmentPaneViewModel, IFragmentViewModel fragmentViewModel, IDeviceViewModel deviceViewModel)
        {
            if (!(this._localizerService.TryGetLocalizedString(fragmentViewModel.NameForUiKey, out string title)))
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
