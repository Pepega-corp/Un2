namespace Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings.QuickMemoryAccess
{
    public interface IAddressableItem
    {
        ushort Address { get; }
        ushort NumberOfPoints { get; }
    }
}