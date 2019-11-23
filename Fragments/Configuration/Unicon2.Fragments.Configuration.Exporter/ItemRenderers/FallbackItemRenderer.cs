using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Exporter.Interfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;

namespace Unicon2.Fragments.Configuration.Exporter.ItemRenderers
{
    public class FallbackItemRenderer : IConfigurationItemRenderer
    {
        #region Implementation of IConfigurationItemRenderer

        public string RenderHtmlFromItem(IConfigurationItem configurationItem)
        {
            return $"<h1> {configurationItem.Name} </h1>";
        }

        #endregion
    }
}