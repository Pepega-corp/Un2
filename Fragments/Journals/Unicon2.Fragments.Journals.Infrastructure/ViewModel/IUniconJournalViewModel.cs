using System.Collections.Generic;
using System.Windows.Input;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.SharedResources.Behaviors;

namespace Unicon2.Fragments.Journals.Infrastructure.ViewModel
{
    public interface IUniconJournalViewModel : IStronglyNamed, IDeviceContextConsumer, IFragmentInitializable
    {
        List<string> JournalParametersNameList { get; set; }

        //   ObservableCollection<IJournalRecordViewModel> JournalRecordViewModelCollection { get; set; }
        DynamicDataTable Table { get; set; }
        ICommand LoadCommand { get; set; }
        bool CanExecuteJournalLoading { get; set; }
    }
}