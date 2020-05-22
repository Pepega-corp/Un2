using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Unicon2.Fragments.Configuration.Exporter.Interfaces;
using Unicon2.Fragments.Configuration.Infrastructure.Export;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;
using Unicon2.Fragments.Configuration.Model.Properties;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.TreeGrid;

namespace Unicon2.Fragments.Configuration.Exporter.ItemRenderers
{
	public class DefaultPropertyRenderer : IConfigurationItemRenderer
	{
		
		public Maybe<List<TagBuilder>> RenderHtmlFromItem(IConfigurationItemViewModel configurationItem,
			SelectorForItemsGroup selectorForItemsGroup, int depthLevel = 0)
		{
			IRuntimePropertyViewModel defaultProperty = configurationItem as IRuntimePropertyViewModel;
			return Maybe<List<TagBuilder>>.FromValue(new List<TagBuilder>()
			{
				ConfigTableRowRenderer
					.Create()
					.SetDepth(depthLevel)
					.SetName(new RenderData(defaultProperty.Header))
					.SetDeviceData(Result<string>.Create(() => defaultProperty.DeviceValue.AsString(),
						defaultProperty.DeviceValue != null))
					.SetLocalData(Result<string>.Create(() => defaultProperty.LocalValue
							.AsString(),
						defaultProperty.LocalValue != null))
					.SetRange(Result<string>.Create(
						$"[{defaultProperty.RangeViewModel.RangeFrom} : {defaultProperty.RangeViewModel.RangeTo}]",
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