using Unicon2.Fragments.Configuration.Exporter.Interfaces;
using Unicon2.Fragments.Configuration.Exporter.ItemRenderers;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Configuration.Exporter.Factories
{
   public class ItemRendererFactory: IItemRendererFactory
    {
        private readonly ITypesContainer _typesContainer;

        public ItemRendererFactory(ITypesContainer typesContainer)
        {
            _typesContainer = typesContainer;
        }

        public IConfigurationItemRenderer GetConfigurationItemRenderer(IConfigurationItemViewModel configurationItem)
        {
            try
            {
              return _typesContainer.Resolve<IConfigurationItemRenderer>(configurationItem.StrongName);
            }
            catch
            {
                return new FallbackItemRenderer();
            }
        }
    }
}
