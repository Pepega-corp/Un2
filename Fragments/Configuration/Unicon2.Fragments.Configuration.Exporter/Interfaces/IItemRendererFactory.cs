using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;

namespace Unicon2.Fragments.Configuration.Exporter.Interfaces
{
    public interface IItemRendererFactory
    {
        IConfigurationItemRenderer GetConfigurationItemRenderer(IConfigurationItem configurationItem);
    }
}