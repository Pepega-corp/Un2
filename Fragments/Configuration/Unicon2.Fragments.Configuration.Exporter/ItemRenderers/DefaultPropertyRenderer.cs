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

        public Maybe<List<TagBuilder>> RenderHtmlFromItem(IConfigurationItem configurationItem,
            SelectorForItemsGroup selectorForItemsGroup, int depthLevel = 0)
        {
            DefaultProperty defaultProperty = configurationItem as DefaultProperty;
            return Maybe<List<TagBuilder>>.FromValue(new List<TagBuilder>()
            {
                ConfigTableRowRenderer
                    .Create()
                    .SetDepth(depthLevel)
                    .SetName(new RenderData(defaultProperty.Name))
                    .SetDeviceData(Result<string>.Create(() => defaultProperty.UshortsFormatter
                            .Format(defaultProperty.DeviceUshortsValue).AsString(),
                        defaultProperty.DeviceUshortsValue?.Length > 0 && defaultProperty.UshortsFormatter != null))
                    .SetLocalData(Result<string>.Create(() => defaultProperty.UshortsFormatter
                            .Format(defaultProperty.LocalUshortsValue)
                            .AsString(),
                        defaultProperty.LocalUshortsValue?.Length > 0 && defaultProperty.UshortsFormatter != null))
                    .SetRange(Result<string>.Create(
                        $"[{defaultProperty.Range.RangeFrom} : {defaultProperty.Range.RangeTo}]",
                        defaultProperty.IsRangeEnabled))
                    .SetMeasureUnit(Result<string>.Create(defaultProperty.MeasureUnit,
                        defaultProperty.IsMeasureUnitEnabled))
                    .SetSelectors(selectorForItemsGroup.IsPrintDeviceValuesAllowed,
                        selectorForItemsGroup.IsPrintLocalValuesAllowed)
                    .Render()
            });
        }
    }
}