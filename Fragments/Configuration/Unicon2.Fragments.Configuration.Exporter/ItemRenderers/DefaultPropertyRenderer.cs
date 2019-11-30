using System;
using System.Web.Mvc;
using Unicon2.Fragments.Configuration.Exporter.Interfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Model.Properties;

namespace Unicon2.Fragments.Configuration.Exporter.ItemRenderers
{
    public class DefaultPropertyRenderer : IConfigurationItemRenderer
    {
        #region Overrides of ConfigurationItemRendererBase

        public TagBuilder RenderHtmlFromItem(IConfigurationItem configurationItem)
        {
            DefaultProperty defaultProperty = configurationItem as DefaultProperty;
            TagBuilder item = new TagBuilder("span") {InnerHtml = defaultProperty.LocalUshortsValue?.ToString()};
            return item;
        }

        #endregion
    }
}