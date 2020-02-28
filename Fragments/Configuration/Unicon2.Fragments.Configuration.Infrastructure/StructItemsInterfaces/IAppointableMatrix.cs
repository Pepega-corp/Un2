using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Infrastructure.Values.Matrix;

namespace Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces
{
    public interface IAppointableMatrix : IProperty
    {
        IMatrixTemplate MatrixTemplate { get; set; }
    }
}