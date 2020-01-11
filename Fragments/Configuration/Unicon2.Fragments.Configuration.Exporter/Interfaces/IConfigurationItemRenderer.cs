using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using Unicon2.Fragments.Configuration.Exporter.Utils;
using Unicon2.Fragments.Configuration.Infrastructure.Export;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Infrastructure.Common;

namespace Unicon2.Fragments.Configuration.Exporter.Interfaces
{
    public interface IConfigurationItemRenderer
    {
        Maybe<List<TagBuilder>> RenderHtmlFromItem(IConfigurationItem configurationItem,
            SelectorForItemsGroup selectorForItemsGroup, int depthLevel = 0);
    }

    public class Result<T>
    {
        private readonly bool _isSuccess;
        private readonly T _item;

        public Result(T item, bool isSuccess)
        {
            _item = item;
            _isSuccess = isSuccess;
        }

        public static Result<T> Create(T item, bool isSuccess)
        {
            return new Result<T>(item, isSuccess);
        }

        public T OnSuccess(Action<T> onSuccessFunc)
        {
            if (_isSuccess)
            {
                onSuccessFunc(_item);
            }

            return _item;
        }

        public T Item => _item;

        public bool IsSuccess
        {
            get { return _isSuccess; }
        }
    }

    public static class ResultExtensions
    {
        public static Result<T> SetIf<T>(this T item, Func<bool> ifSet)
        {
            return Result<T>.Create(item, ifSet());
        }

        public static Result<T> SetIf<T>(this T item, Func<T, bool> ifSet)
        {
            return Result<T>.Create(item, ifSet(item));
        }
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


        public ConfigTableRowRenderer SetDeviceData(string deviceData)
        {
            _deviceData = deviceData;
            return this;
        }

        public ConfigTableRowRenderer SetMeasureUnit(string measureUnit)
        {
            _measureUnit = measureUnit;

            return this;
        }

        public ConfigTableRowRenderer SetLocalData(string localData)
        {
            _localData = localData;

            return this;
        }

        public ConfigTableRowRenderer SetShouldRenderEmptyItems(bool shouldRenderEmptyItems)
        {
            _shouldRenderEmptyItems = shouldRenderEmptyItems;

            return this;
        }

        public ConfigTableRowRenderer SetRange(string range)
        {
            _range = range;

            return this;
        }


        private void RenderDataToTag(TagBuilder tagBuilder, string dataToRender, bool shouldRenderEmptyItems,
            bool isRenderingAllowed = true)
        {
            dataToRender
                .SetIf((data) =>isRenderingAllowed && CheckIfDataNeedsToRender(data, shouldRenderEmptyItems))
                .OnSuccess((s => { AddDataTag(tagBuilder, s); }));
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