using Unicon2.Fragments.Configuration.Exporter.Interfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Model.Properties;

namespace Unicon2.Fragments.Configuration.Exporter.ItemRenderers
{
    public class DefaultPropertyRenderer : IConfigurationItemRenderer
    {
        #region Overrides of ConfigurationItemRendererBase

        public string RenderHtmlFromItem(IConfigurationItem configurationItem)
        {
            DefaultProperty defaultProperty = configurationItem as DefaultProperty;
            return $"{defaultProperty.LocalUshortsValue}";
        }

        #endregion
    }
}