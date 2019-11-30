using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.WebPages.Html;
using Unicon2.Fragments.Configuration.Exporter.Interfaces;
using Unicon2.Fragments.Configuration.Exporter.Utils;
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
            string main =
                "<!DOCTYPE html>\n <html>\n <head>\n <meta charset =\"utf-8\"/>\n<title> HTML Document </title>\n</head>";
            TagBuilder body = new TagBuilder("body");
            foreach (var rootConfigurationItem in deviceConfiguration.RootConfigurationItemList)
            {
                body.AddTagToInnerHtml(_itemRendererFactory.GetConfigurationItemRenderer(rootConfigurationItem).RenderHtmlFromItem(rootConfigurationItem));
            }
            main += body;

            main += ("\n</html>");

            return main;
        }
       
        #endregion
    }
}