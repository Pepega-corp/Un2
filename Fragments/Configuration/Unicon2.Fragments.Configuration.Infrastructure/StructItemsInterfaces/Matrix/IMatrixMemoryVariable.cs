namespace Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Matrix
{
    public interface IMatrixMemoryVariable
    {
        string Name { get; set; }
        ushort StartAddressWord { get; set; }
        ushort StartAddressBit { get; set; }
    }
}