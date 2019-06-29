namespace Unicon2.Fragments.Configuration.Matrix.Interfaces.Model
{
    public interface IMatrixMemoryVariable
    {
        string Name { get; set; }
        ushort StartAddressWord { get; set; }
        ushort StartAddressBit { get; set; }
    }
}