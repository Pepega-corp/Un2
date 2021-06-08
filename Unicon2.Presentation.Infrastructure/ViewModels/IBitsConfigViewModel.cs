using System.Collections.ObjectModel;

namespace Unicon2.Presentation.Infrastructure.ViewModels
{
    public interface IBitsConfigViewModel
    {
        bool IsFromBits { get; set; }
        ObservableCollection<IBitViewModel> BitNumbersInWord { get; set; }
        (ushort address, ushort numberOfPoints) GetAddressInfo();
    }

    public interface IBitViewModel
    {
        bool IsChecked { get; set; }
        int BitNumber { get; }
        bool IsBitEditEnabled { get; }
        string OwnerTooltip { get; }
    }
}