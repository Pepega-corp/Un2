using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Unicon2.Fragments.Configuration.Exporter.Interfaces;
using Unicon2.Fragments.Configuration.Infrastructure.Export;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Model.Properties;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.Configuration.Exporter.ItemRenderers
{
    public class DefaultPropertyRenderer : IConfigurationItemRenderer
    {
        public DefaultPropertyRenderer()
        {
            
        }
        #region Overrides of ConfigurationItemRendererBase

        public Maybe<List<TagBuilder>> RenderHtmlFromItem(IConfigurationItem configurationItem,
            SelectorForItemsGroup selectorForItemsGroup = null, int depthLevel = 0)
        {
            DefaultProperty defaultProperty = configurationItem as DefaultProperty;
            return Maybe<List<TagBuilder>>.FromValue(new List<TagBuilder>()
            {
                ConfigTableRowRenderer
                    .Create()
                    .SetDepth(depthLevel)
                    .SetName(defaultProperty.Name)
                    .SetIf(() => defaultProperty.DeviceUshortsValue?.Length > 0)
                    .OnSuccess(item => item.SetDeviceData(defaultProperty.UshortsFormatter.Format(defaultProperty.DeviceUshortsValue).AsString()))
                    .SetIf(() => defaultProperty.LocalUshortsValue?.Length > 0)
                    .OnSuccess(item1 => item1.SetLocalData(defaultProperty.UshortsFormatter.Format(defaultProperty.LocalUshortsValue).AsString()))
                    .SetIf(() => defaultProperty.Range != null)
                    .OnSuccess((item2 =>item2.SetRange($"[{defaultProperty.Range.RangeFrom} : {defaultProperty.Range.RangeTo}]")))
                    .SetIf(() => defaultProperty.IsMeasureUnitEnabled).OnSuccess((renderer =>renderer.SetMeasureUnit(defaultProperty.MeasureUnit)))
                    .Render()
            });
        }

        #endregion
    }
}