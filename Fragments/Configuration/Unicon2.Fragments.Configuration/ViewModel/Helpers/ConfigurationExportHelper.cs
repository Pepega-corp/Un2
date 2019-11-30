using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Infrastructure.Interfaces.DataOperations;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Configuration.ViewModel.Helpers
{
    public class ConfigurationExportHelper
    {
        public static string ExportConfiguration(IDeviceConfiguration deviceConfiguration,ITypesContainer typesContainer)
        {
           return typesContainer.Resolve<IHtmlRenderer<IDeviceConfiguration>>().RenderHtmlString(deviceConfiguration);
        }
    }
}
