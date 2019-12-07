using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Unicon2.Fragments.Configuration.Exporter.Interfaces;
using Unicon2.Fragments.Configuration.Infrastructure.Export;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Infrastructure.Common;

namespace Unicon2.Fragments.Configuration.Exporter.ItemRenderers
{
    public class FallbackItemRenderer : IConfigurationItemRenderer
    {
        #region Implementation of IConfigurationItemRenderer

        public Maybe<List<TagBuilder>> RenderHtmlFromItem(IConfigurationItem configurationItem,
            SelectorForItemsGroup selectorForItemsGroup = null, int depthLevel = 0)
        {
            return Maybe<List<TagBuilder>>.FromValue(new List<TagBuilder>()
            {
                ConfigTableRowRenderer
                    .Create()
                    .SetDepth(depthLevel)
                    .SetName(configurationItem.Name)
                    .Render()
            });
        }

        #endregion
    }
}