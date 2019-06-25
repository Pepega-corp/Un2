using Unicon2.Infrastructure.Values;

namespace Unicon2.Fragments.Configuration.Matrix.Interfaces.Model
{
    public interface IMatrixValue: IFormattedValue
    {
        ushort[] ValueUshorts { get; set; }
        IMatrixTemplate MatrixTemplate { get; set; }
    }
}