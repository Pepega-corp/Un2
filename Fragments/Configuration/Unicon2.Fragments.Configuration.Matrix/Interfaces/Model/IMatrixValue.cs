using Unicon2.Infrastructure.Values;

namespace Unicon2.Fragments.Configuration.Matrix.Interfaces.Model
{
    public interface IMatrixValue: IFormattedValue
    {
        IMatrixTemplate MatrixTemplate { get; set; }
    }
}