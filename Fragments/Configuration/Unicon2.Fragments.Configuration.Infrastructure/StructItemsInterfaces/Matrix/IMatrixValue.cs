using Unicon2.Infrastructure.Values;

namespace Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Matrix
{
    public interface IMatrixValue: IFormattedValue
    {
        IMatrixTemplate MatrixTemplate { get; set; }
    }
}