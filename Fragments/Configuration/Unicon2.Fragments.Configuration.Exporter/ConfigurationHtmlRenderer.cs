using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Unicon2.Fragments.Configuration.Exporter.Interfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Infrastructure.Interfaces.DataOperations;



namespace Unicon2.Fragments.Configuration.Exporter
{
    public class ConfigurationHtmlRenderer : IHtmlRenderer<IDeviceConfiguration>
    {
        private readonly IItemRendererFactory _itemRendererFactory;

        public ConfigurationHtmlRenderer(IItemRendererFactory itemRendererFactory)
        {
            _itemRendererFactory = itemRendererFactory;
        }


        #region Implementation of IHtmlRenderer<IDeviceConfiguration>

        public string RenderHtmlString(IDeviceConfiguration deviceConfiguration)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("<!DOCTYPE html>\n<html>\n<head>\n<meta charset=\"utf-8\" />\n<title> HTML Document </title>\n</head>\n<body>");
            foreach (var rootConfigurationItem in deviceConfiguration.RootConfigurationItemList)
            {
                stringBuilder.Append(_itemRendererFactory.GetConfigurationItemRenderer(rootConfigurationItem).RenderHtmlFromItem(rootConfigurationItem));
            }
            stringBuilder.Append("</body>\n</html>");

            return stringBuilder.ToString();
        }

        #endregion
    }
}