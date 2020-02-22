using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;

namespace Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Matrix
{
    public interface IAppointableMatrix: IProperty
    {
        IMatrixTemplate MatrixTemplate { get; set; }
    }
}