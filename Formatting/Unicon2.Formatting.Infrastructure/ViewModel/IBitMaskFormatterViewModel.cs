using System.Collections.ObjectModel;
using System.Windows.Input;
using Unicon2.Infrastructure.Extensions;

namespace Unicon2.Formatting.Infrastructure.ViewModel
{
    public interface IBitMaskFormatterViewModel
    {
        ObservableCollection<StringWrapper> BitSignatures { get; set; }
        StringWrapper SelectedBitSignature { get; set; }
        ICommand AddSignatureCommand { get; }
        ICommand DeleteSignatureCommand { get; }

    }
}