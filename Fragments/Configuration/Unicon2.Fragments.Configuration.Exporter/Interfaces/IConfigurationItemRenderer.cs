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
            SelectorForItemsGroup selectorForItemsGroup = null, int depthLevel = 0);
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
        public static Result<T> Create(T item,bool isSuccess)
        {
            return new Result<T>(item,isSuccess);
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
      
    }

    

    public class ConfigTableRowRenderer
    {
        public static ConfigTableRowRenderer Create()
        {
            return new ConfigTableRowRenderer();
        }

        private string _name;
        private int _depthLevel;
        private string _deviceData;
        private string _localData;
        private string _measureUnit;
        private string _range;

        public ConfigTableRowRenderer SetName(string name)
        {
            _name = name;
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

        public ConfigTableRowRenderer SetRange(string range)
        {

            _range = range;

            return this;
        }

        public TagBuilder Render()
        {
            TagBuilder tableRowForItems = new TagBuilder("tr");
            
            TagBuilder nameTableItem = new TagBuilder("td");
            string offsetString = "";
            for (int i = 0; i < _depthLevel; i++)
            {
                offsetString += "  -  ";
            }
            nameTableItem.AddToInnerHtml(offsetString+_name);

            TagBuilder deviceDataTableItem = new TagBuilder("td");
            deviceDataTableItem.AddToInnerHtml(_deviceData);

            TagBuilder localDataTableItem = new TagBuilder("td");
            localDataTableItem.AddToInnerHtml(_localData);

            TagBuilder measureUnitTableItem = new TagBuilder("td");
            measureUnitTableItem.AddToInnerHtml(_measureUnit);

            TagBuilder rangeTableItem = new TagBuilder("td");
            rangeTableItem.AddToInnerHtml(_range);

            tableRowForItems.AddTagToInnerHtml(nameTableItem);
            tableRowForItems.AddTagToInnerHtml(deviceDataTableItem);
            tableRowForItems.AddTagToInnerHtml(localDataTableItem);
            tableRowForItems.AddTagToInnerHtml(measureUnitTableItem);
            tableRowForItems.AddTagToInnerHtml(rangeTableItem);
            return tableRowForItems;
        }
    }
}