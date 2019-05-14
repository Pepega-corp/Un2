using System.Windows.Input;

namespace Unicon2.Infrastructure.Interfaces
{
    public interface ISharedBitViewModel
    {
        int NumberOfBit { get; set; }
        bool Value { get;  }
        object Owner { get;  }
        ICommand ChangeValueByOwnerCommand { get; set; }
        void Refresh();
    }
}