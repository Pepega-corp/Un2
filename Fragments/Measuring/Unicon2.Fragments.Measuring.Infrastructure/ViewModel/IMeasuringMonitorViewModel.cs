using System;
using System.Collections.ObjectModel;
using System.Windows.Data;
using Unicon2.Fragments.Measuring.Infrastructure.ViewModel.Elements;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;

namespace Unicon2.Fragments.Measuring.Infrastructure.ViewModel
{
    public interface IMeasuringMonitorViewModel:IFragmentViewModel,IDisposable, IDeviceDataProvider, IStronglyNamed
	{
        ObservableCollection<IMeasuringGroupViewModel> MeasuringGroupViewModels { get; set; }
        IMeasuringGroupViewModel SelectedMeasuringGroupViewModel { get; set; }
        ObservableCollection<IMeasuringElementViewModel> MeasuringElementViewModels { get; set; }
        CollectionView MeasuringElementListCollectionView { get; set; }

       bool IsListViewSelected { get; set; }
    }
}