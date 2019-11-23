using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;

namespace Unicon2.Fragments.Configuration.Exporter.Interfaces
{
    public interface IConfigurationItemRenderer
    {
        string RenderHtmlFromItem(IConfigurationItem configurationItem);
    }
}