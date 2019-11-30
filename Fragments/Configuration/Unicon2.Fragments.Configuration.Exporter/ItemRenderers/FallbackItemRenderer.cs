using System;
using System.Web.Mvc;
using Unicon2.Fragments.Configuration.Exporter.Interfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;

namespace Unicon2.Fragments.Configuration.Exporter.ItemRenderers
{
    public class FallbackItemRenderer : IConfigurationItemRenderer
    {
        #region Implementation of IConfigurationItemRenderer

        public TagBuilder RenderHtmlFromItem(IConfigurationItem configurationItem)
        {
            return new TagBuilder("h1") {InnerHtml = configurationItem.Name};
        }

        #endregion
    }
}