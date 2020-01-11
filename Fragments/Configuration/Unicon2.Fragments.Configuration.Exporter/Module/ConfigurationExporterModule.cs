using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Exporter.Factories;
using Unicon2.Fragments.Configuration.Exporter.Interfaces;
using Unicon2.Fragments.Configuration.Exporter.ItemRenderers;
using Unicon2.Fragments.Configuration.Infrastructure.Export;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Infrastructure.Interfaces.DataOperations;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Configuration.Exporter.Module
{
   public class ConfigurationExporterModule: IUnityModule
    {
        public void Initialize(ITypesContainer container)
        {
            container.Register<IHtmlRenderer<IDeviceConfiguration, ConfigurationExportSelector>,ConfigurationHtmlRenderer>();
            container.Register<IConfigurationItemRenderer,DefaultPropertyRenderer>(ConfigurationKeys.DEFAULT_PROPERTY);
            container.Register<IConfigurationItemRenderer, ComplexPropertyRenderer>(ConfigurationKeys.COMPLEX_PROPERTY);
            container.Register<IConfigurationItemRenderer, DefaultPropertyRenderer>(ConfigurationKeys.SUB_PROPERTY);
            container.Register<IConfigurationItemRenderer, DefaultPropertyRenderer>(ConfigurationKeys.DEPENDENT_PROPERTY);
            container.Register<IConfigurationItemRenderer, MatrixRenderer>(ConfigurationKeys.APPOINTABLE_MATRIX);

            container.Register<IConfigurationItemRenderer, ItemsGroupPropertyRenderer>(ConfigurationKeys.DEFAULT_ITEM_GROUP);
            container.Register<IItemRendererFactory, ItemRendererFactory>();

        }
    }
}
