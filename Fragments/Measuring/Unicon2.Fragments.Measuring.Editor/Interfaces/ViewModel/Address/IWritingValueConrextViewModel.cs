using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel.Address
{
    public interface IWritingValueContextViewModel : IStronglyNamed
    {
        ushort Address { get; set; }
        ushort NumberOfFunction { get; set; }
        ushort ValueToWrite { get; set; }
    }
}