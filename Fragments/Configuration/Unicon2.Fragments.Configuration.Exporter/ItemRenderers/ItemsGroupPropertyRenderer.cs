using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Exporter.Interfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Configuration.Exporter.ItemRenderers
{
    public class ItemsGroupPropertyRenderer: IConfigurationItemRenderer
    {
        private readonly IItemRendererFactory _itemRendererFactory;

        public ItemsGroupPropertyRenderer(IItemRendererFactory itemRendererFactory)
        {
            _itemRendererFactory = itemRendererFactory;
        }
        #region Overrides of ConfigurationItemRendererBase

        public  string RenderHtmlFromItem(IConfigurationItem configurationItem)
        {
            StringBuilder stringBuilder=new StringBuilder();
            var group = configurationItem as IItemsGroup;
            stringBuilder.Append($"<h1> {group.Name} </h1>");
            group.ConfigurationItemList.ForEach((item =>
                {
                    stringBuilder.Append(_itemRendererFactory.GetConfigurationItemRenderer(item)
                        .RenderHtmlFromItem(item));
                }));
            return stringBuilder.ToString();
        }

        #endregion
    }
}
