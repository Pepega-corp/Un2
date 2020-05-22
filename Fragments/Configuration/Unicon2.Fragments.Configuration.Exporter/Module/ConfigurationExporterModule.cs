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
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Interfaces.DataOperations;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Configuration.Exporter.Module
{
   public class ConfigurationExporterModule: IUnityModule
    {
        public void Initialize(ITypesContainer container)
        {
            container.Register<IHtmlRenderer<IRuntimeConfigurationViewModel, ConfigurationExportSelector>,ConfigurationHtmlRenderer>();
            container.Register<IConfigurationItemRenderer,DefaultPropertyRenderer>(ConfigurationKeys.RUNTIME_DEFAULT_PROPERTY + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
            container.Register<IConfigurationItemRenderer, ComplexPropertyRenderer>(ConfigurationKeys.RUNTIME + ConfigurationKeys.COMPLEX_PROPERTY + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
            container.Register<IConfigurationItemRenderer, MatrixRenderer>(ConfigurationKeys.RUNTIME + ConfigurationKeys.APPOINTABLE_MATRIX + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);

            container.Register<IConfigurationItemRenderer, ItemsGroupPropertyRenderer>(ConfigurationKeys.RUNTIME_DEFAULT_ITEM_GROUP +
                                                                                       ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
            container.Register<IItemRendererFactory, ItemRendererFactory>();

        }
    }
}
