namespace Unicon2.Infrastructure.Values.Matrix
{
    public interface IMatrixMemoryVariable
    {
        string Name { get; set; }
        ushort StartAddressWord { get; set; }
        ushort StartAddressBit { get; set; }
    }
}