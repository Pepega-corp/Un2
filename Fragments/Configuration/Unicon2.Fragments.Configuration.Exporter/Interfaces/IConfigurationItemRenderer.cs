using System.Collections.Generic;
using System.Web.Mvc;
using Unicon2.Fragments.Configuration.Exporter.Utils;
using Unicon2.Fragments.Configuration.Infrastructure.Export;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Functional;
using Unicon2.Presentation.Infrastructure.TreeGrid;

namespace Unicon2.Fragments.Configuration.Exporter.Interfaces
{
    public interface IConfigurationItemRenderer
    {
        Maybe<List<TagBuilder>> RenderHtmlFromItem(IConfigurationItemViewModel configurationItem,
            SelectorForItemsGroup selectorForItemsGroup, int depthLevel = 0);
    }
 
    public class RenderData
    {
        public RenderData(string stringToRender, string cssClassName = null)
        {
            StringToRender = stringToRender;
            CssClassName = cssClassName;
        }

        public string StringToRender { get; }
        public string CssClassName { get; }
    }

    public class ConfigTableRowRenderer
    {
        public static ConfigTableRowRenderer Create()
        {
            return new ConfigTableRowRenderer();
        }

        private RenderData _nameRenderData;
        private int _depthLevel;
        private string _deviceData;
        private string _localData;
        private string _measureUnit;
        private string _range;
        private bool _shouldRenderEmptyItems = true;
        private bool _isDeviceDataPrinting;
        private bool _isLocalDataPrinting;

        public ConfigTableRowRenderer SetName(RenderData nameRenderData)
        {
            _nameRenderData = nameRenderData;
            return this;
        }

        public ConfigTableRowRenderer SetDepth(int depthLevel)
        {
            _depthLevel = depthLevel;
            return this;
        }

        public ConfigTableRowRenderer SetDeviceData(Result<string> deviceData)
        {
            deviceData.OnSuccess((s => _deviceData = s));
            return this;
        }
        public ConfigTableRowRenderer SetLocalData(Result<string> localData)
        {
            localData.OnSuccess((s => _localData = s));
            return this;
        }

        public ConfigTableRowRenderer SetMeasureUnit(Result<string> measureUnit)
        {
            measureUnit.OnSuccess((s => _measureUnit = s));
            return this;
        }

        public ConfigTableRowRenderer SetShouldRenderEmptyItems(bool shouldRenderEmptyItems)
        {
            _shouldRenderEmptyItems = shouldRenderEmptyItems;
            return this;
        }

        public ConfigTableRowRenderer SetRange(Result<string> range)
        {
            range.OnSuccess((s => _range = s));
            return this;
        }


        private void RenderDataToTag(TagBuilder tagBuilder, string dataToRender, bool shouldRenderEmptyItems,
            bool isRenderingAllowed = true)
        {
            Result<string>.Create(dataToRender,
                    isRenderingAllowed && CheckIfDataNeedsToRender(dataToRender, shouldRenderEmptyItems))
                .OnSuccess(s => AddDataTag(tagBuilder, s));
        }

        private void AddDataTag(TagBuilder tagBuilder, string data)
        {
            TagBuilder tag = new TagBuilder("td");
            tag.AddToInnerHtml(data);
            tagBuilder.AddTagToInnerHtml(tag);
        }

        private bool CheckIfDataNeedsToRender(string data, bool shouldRenderEmptyItems)
        {
            return !string.IsNullOrEmpty(data) || (string.IsNullOrEmpty(data) && shouldRenderEmptyItems);
        }

        public ConfigTableRowRenderer SetSelectors(bool isDeviceDataPrinting, bool isLocalDataPrinting)
        {
            _isDeviceDataPrinting = isDeviceDataPrinting;
            _isLocalDataPrinting = isLocalDataPrinting;
            return this;
        }

        public TagBuilder Render()
        {
            TagBuilder tableRowForItems = new TagBuilder("tr");

            TagBuilder nameTableItem = new TagBuilder("td");
            string offsetString = "";
            for (int i = 0; i < _depthLevel; i++)
            {
                offsetString += "&nbsp;&nbsp;&nbsp;&nbsp;";
            }

            if (_nameRenderData.CssClassName != null)
            {
                nameTableItem.AddCssClass(_nameRenderData.CssClassName);
            }

            nameTableItem.AddToInnerHtml(offsetString + _nameRenderData.StringToRender);

            tableRowForItems.AddTagToInnerHtml(nameTableItem);
            RenderDataToTag(tableRowForItems, _deviceData,_shouldRenderEmptyItems, _isDeviceDataPrinting);
            RenderDataToTag(tableRowForItems, _localData,_shouldRenderEmptyItems, _isLocalDataPrinting);
            RenderDataToTag(tableRowForItems, _measureUnit, _shouldRenderEmptyItems);
            RenderDataToTag(tableRowForItems, _range, _shouldRenderEmptyItems);
            return tableRowForItems;
        }
    }
}