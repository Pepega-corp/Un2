﻿using System.Collections.ObjectModel;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;

namespace Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime
{
    public interface IRuntimeConfigurationViewModel : IFragmentViewModel, IDeviceContextConsumer, IStronglyNamed
    {
        object SelectedConfigDetails { get; }

        ObservableCollection<IRuntimeConfigurationItemViewModel> RootConfigurationItemViewModels { get; set; }
        ObservableCollection<IRuntimeConfigurationItemViewModel> AllRows { get; set; }
        IRuntimeBaseValuesViewModel BaseValuesViewModel { get; set; }
    }
}