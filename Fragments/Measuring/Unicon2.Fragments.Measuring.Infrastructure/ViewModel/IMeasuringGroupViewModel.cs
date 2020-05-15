using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Unicon2.Fragments.Measuring.Infrastructure.ViewModel.Elements;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.ViewModel;

namespace Unicon2.Fragments.Measuring.Infrastructure.ViewModel
{
    public interface IMeasuringGroupViewModel : IStronglyNamed
    {
        ObservableCollection<IMeasuringElementViewModel> MeasuringElementViewModels { get; set; }
        string Header { get; set; }
        Task LoadGroup();

    }
}