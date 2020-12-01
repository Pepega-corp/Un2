using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Unicon2.Fragments.Measuring.Infrastructure.ViewModel.Elements;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.Measuring.Infrastructure.ViewModel
{
    public interface IMeasuringGroupViewModel : IStronglyNamed
    {
        ObservableCollection<IMeasuringElementViewModel> MeasuringElementViewModels { get; set; }
        string Header { get; set; }
        Task LoadGroup();

    }
}