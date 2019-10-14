using System.Windows.Input;

namespace Unicon2.Fragments.Configuration.Editor.Interfaces.Tree
{
    public interface IAddressIncreaseableDecreaseable
    {
        ushort AddressIteratorValue { get; set; }
        ICommand IncreaseAddressCommand { get; }
        ICommand DecreaseAddressCommand { get; }

    }
}