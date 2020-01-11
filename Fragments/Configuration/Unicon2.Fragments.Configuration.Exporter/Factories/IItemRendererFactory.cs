using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Exporter.Interfaces;
using Unicon2.Fragments.Configuration.Exporter.ItemRenderers;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
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

        public IConfigurationItemRenderer GetConfigurationItemRenderer(IConfigurationItem configurationItem)
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
