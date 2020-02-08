using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Behaviors;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Fragments.Configuration.ViewModel.Properties;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.Events;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.SharedResources.Behaviors;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Configuration.ViewModel.Table
{
    public class TableConfigurationViewModel : ViewModelBase
    {
        private readonly ObservableCollection<IConfigurationItemViewModel> _itemGroupsToTransform;
        private DynamicPropertiesTable _dynamicPropertiesTable;

        public TableConfigurationViewModel(
            ObservableCollection<IConfigurationItemViewModel> itemGroupsToTransform)
        {
            _itemGroupsToTransform = itemGroupsToTransform;
            Initialize();
        }

        public DynamicPropertiesTable DynamicPropertiesTable
        {
            get => _dynamicPropertiesTable;
            set => SetProperty(ref _dynamicPropertiesTable, value);
        }

        private void Initialize()
        {
            var columnNamesWithProperties = new List<Tuple<string, IConfigurationItemViewModel>>();
            FillColumnNames(_itemGroupsToTransform, columnNamesWithProperties);
            var lookup = columnNamesWithProperties.ToLookup(tuple => tuple.Item1, tuple => tuple.Item2);
            var columnNames = lookup.Select(models => models.Key).ToList();
            DynamicPropertiesTable = new DynamicPropertiesTable(columnNames,
                _itemGroupsToTransform.Select((model => model.Header)).ToList(), false);
            _itemGroupsToTransform.ForEach((group =>
                DynamicPropertiesTable.AddPropertyViewModel(GetRowFromItemGroup(group, lookup, columnNames)
                    .Select((tuple => tuple.Value)).ToList())));
        }







        private void FillColumnNames(IEnumerable<IConfigurationItemViewModel> items,
            List<Tuple<string, IConfigurationItemViewModel>> columnNamesWithProperties)
        {
            foreach (var item in items)
            {
                if (item.ChildStructItemViewModels.Any())
                {
                    FillColumnNames(item.ChildStructItemViewModels, columnNamesWithProperties);
                }
                else
                {
                    columnNamesWithProperties.Add(
                        new Tuple<string, IConfigurationItemViewModel>(item.Header, item));
                }
            }
        }

        private Dictionary<string, ILocalAndDeviceValueContainingViewModel> GetRowFromItemGroup(
            IConfigurationItemViewModel group, ILookup<string, IConfigurationItemViewModel> lookup,
            List<string> columnNames, Dictionary<string, ILocalAndDeviceValueContainingViewModel> initialList = null)
        {
            if (initialList == null)
            {
                initialList = new Dictionary<string, ILocalAndDeviceValueContainingViewModel>();
                foreach (var columnName in columnNames)
                {
                    initialList.Add(columnName, null);
                }
            }

            group.ChildStructItemViewModels.ForEach((item =>
            {
                if (item.ChildStructItemViewModels.Any())
                {
                    GetRowFromItemGroup(item, lookup, columnNames, initialList);
                }
                else
                {
                    InsertProperty(initialList, lookup, item);
                }
            }));
            return initialList;
        }


        private void InsertProperty(Dictionary<string, ILocalAndDeviceValueContainingViewModel> resultList,
            ILookup<string, IConfigurationItemViewModel> lookup,
            IConfigurationItemViewModel itemToAdd)
        {

            var columnName = lookup.FirstOrDefault((models => models.Contains(itemToAdd)))?.Key;
            if (columnName != null)
                resultList[columnName] = itemToAdd as ILocalAndDeviceValueContainingViewModel;
        }

    }
}