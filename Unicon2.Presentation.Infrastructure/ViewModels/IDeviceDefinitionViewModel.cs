using Unicon2.Infrastructure.ViewModel;

namespace Unicon2.Presentation.Infrastructure.ViewModels
{
    public interface IDeviceDefinitionViewModel : IViewModel
    {
        string Name { get; set; }
        string ConnectionDescription { get; set; }

    }
}