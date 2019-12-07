using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Unicon2.Fragments.Configuration.Exporter.Interfaces;
using Unicon2.Fragments.Configuration.Exporter.Utils;
using Unicon2.Fragments.Configuration.Infrastructure.Export;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Infrastructure.Common;

namespace Unicon2.Fragments.Configuration.Exporter.ItemRenderers
{
    public class ItemsGroupPropertyRenderer : IConfigurationItemRenderer
    {
        private readonly IItemRendererFactory _itemRendererFactory;

        public ItemsGroupPropertyRenderer(IItemRendererFactory itemRendererFactory)
        {
            _itemRendererFactory = itemRendererFactory;
        }

        #region Overrides of ConfigurationItemRendererBase

        public Maybe<List<TagBuilder>> RenderHtmlFromItem(IConfigurationItem configurationItem,
            SelectorForItemsGroup selectorForItemsGroup = null, int depthLevel = 0)
        {
            if (selectorForItemsGroup == null || !selectorForItemsGroup.IsSelected)
            {
                return Maybe<List<TagBuilder>>.Empty();
            }

            var group = configurationItem as IItemsGroup;
            List<TagBuilder> tagBuilders = new List<TagBuilder>();
            tagBuilders.Add(ConfigTableRowRenderer
                .Create()
                .SetName(group.Name)
                .SetDepth(depthLevel)
                .Render());

            group.ConfigurationItemList.ForEach((item =>
            {
                _itemRendererFactory
                    .GetConfigurationItemRenderer(item)
                    .RenderHtmlFromItem(item,
                        selectorForItemsGroup.Selectors.FirstOrDefault((itemsGroup =>
                            itemsGroup.RelatedItemsGroup == item)), depthLevel + 1)
                    .OnNotEmpty(result => tagBuilders.AddRange(result));
            }));
            return Maybe<List<TagBuilder>>.FromValue(tagBuilders);
        }

        #endregion
    }
}