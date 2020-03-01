using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.ViewModel;

namespace Unicon2.Infrastructure.Interfaces.Factories
{
  public interface IFormatterEditorFactory
    {
        void EditFormatterByUser(IUshortFormattableEditorViewModel ushortFormattableViewModel);
    }
}
