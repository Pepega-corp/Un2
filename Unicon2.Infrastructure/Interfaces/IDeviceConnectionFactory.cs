using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.ViewModel;

namespace Unicon2.Infrastructure.Interfaces
{
    /// <summary>
    /// Описывает логику фабрики подключений
    /// </summary>
    public interface IDeviceConnectionFactory
    {

        /// <summary>
        /// создание объекта подключения
        /// </summary>
        /// <returns></returns>
        IDeviceConnection CreateDeviceConnection();
    

        IViewModel CreateDeviceConnectionViewModel();

    }
}