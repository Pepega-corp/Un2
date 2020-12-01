using Unicon2.Presentation.Infrastructure.TreeGrid;

namespace Unicon2.Fragments.Configuration.Exporter.Interfaces
{
    public interface IItemRendererFactory
    {
        IConfigurationItemRenderer GetConfigurationItemRenderer(IConfigurationItemViewModel configurationItem);
    }
}