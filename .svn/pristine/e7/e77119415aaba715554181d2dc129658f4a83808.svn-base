using System.Windows.Input;
using Unicon2.Infrastructure.ViewModel;
using Unicon2.Presentation.Infrastructure.ViewModels;

namespace Unicon2.ModuleDeviceEditing.Interfaces
{
   public interface IDeviceEditingViewModel
    {
        /// <summary>
        /// Выбранное подключение
        /// </summary>
        IViewModel SelectedDeviceConnection{ get; set; }
        /// <summary>
        /// Выбранное устройство
        /// </summary>
        IDeviceDefinitionViewModel SelectedDevice { get; set; }
        /// <summary>
        /// Текущий режим (редактирование, добавление)
        /// </summary>
        ModesEnum CurrentMode { get; set; }
        /// <summary>
        /// Подпись устройства
        /// </summary>
        string DeviceSignature { get; set; }
        /// <summary>
        /// Комманда открыть устройство из файла
        /// </summary>
        ICommand OpenDeviceFromFileCommand { get; set; }

        ICommand ApplyDefaultSettingsCommand { get; }

    }
}