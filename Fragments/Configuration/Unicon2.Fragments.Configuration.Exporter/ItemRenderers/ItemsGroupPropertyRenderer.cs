using System.Text;
using System.Web.Mvc;
using Unicon2.Fragments.Configuration.Exporter.Interfaces;
using Unicon2.Fragments.Configuration.Exporter.Utils;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;

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

        public TagBuilder RenderHtmlFromItem(IConfigurationItem configurationItem)
        {
            var group = configurationItem as IItemsGroup;
            TagBuilder groupName = new TagBuilder("h1") {InnerHtml = group.Name};
            group.ConfigurationItemList.ForEach((item =>
            {
                groupName.AddTagToInnerHtml(_itemRendererFactory.GetConfigurationItemRenderer(item)
                    .RenderHtmlFromItem(item));
            }));
            return groupName;
        }

        #endregion
    }
}