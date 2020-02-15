using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;

namespace Unicon2.Fragments.Configuration.Matrix.Interfaces.Model
{
    public interface IAppointableMatrix:IConfigurationItem
    {
        IMatrixTemplate MatrixTemplate { get; set; }
    }
}