using System.Threading.Tasks;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Values;

namespace Unicon2.Presentation.Infrastructure.Services
{
    public interface IPropertyValueService
    {
        Task<Result<IFormattedValue>> GetValueOfProperty(object property, DeviceContext.DeviceContext deviceContext);
    }
}
