using System.Collections.Generic;
using System.Web.Mvc;
using Unicon2.Fragments.Configuration.Exporter.Interfaces;
using Unicon2.Fragments.Configuration.Infrastructure.Export;
using Unicon2.Infrastructure.Common;
using Unicon2.Presentation.Infrastructure.TreeGrid;

namespace Unicon2.Fragments.Configuration.Exporter.ItemRenderers
{
    public class FallbackItemRenderer : IConfigurationItemRenderer
    {
        public Maybe<List<TagBuilder>> RenderHtmlFromItem(IConfigurationItemViewModel configurationItem,
            SelectorForItemsGroup selectorForItemsGroup = null, int depthLevel = 0)
        {
            return Maybe<List<TagBuilder>>.FromValue(new List<TagBuilder>()
            {
                ConfigTableRowRenderer
                    .Create()
                    .SetDepth(depthLevel)
                    .SetName(new RenderData(configurationItem.Header))
                    .SetSelectors(selectorForItemsGroup.IsPrintDeviceValuesAllowed,selectorForItemsGroup.IsPrintLocalValuesAllowed)
                    .Render()
            });
        }
    }
}