using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Unicon2.Fragments.Configuration.Behaviors;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Dependencies.Conditions;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;
using Unicon2.Fragments.Configuration.ViewModel.Helpers;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Configuration.ViewModel.Table
{
    public class TableConfigurationViewModel : ViewModelBase
    {
        private readonly List<IConfigurationItemViewModel> _itemGroupsToTransform;
        private List<IConfigurationItemViewModel> _filteredGroupsToTransform;

        private readonly List<RuntimeFilterViewModel> _runtimeFilterViewModels;
        private DynamicPropertiesTable _dynamicPropertiesTable;
        private DynamicPropertiesTable _filteredPropertiesTable;
        private bool _isFilterApplied;


        public TableConfigurationViewModel(List<IConfigurationItemViewModel> itemGroupsToTransform,
            List<RuntimeFilterViewModel> runtimeFilterViewModels)
        {
            _itemGroupsToTransform = itemGroupsToTransform;
            _runtimeFilterViewModels = runtimeFilterViewModels;
            Initialize();
        }

        public DynamicPropertiesTable DynamicPropertiesTable
        {
            get => _dynamicPropertiesTable;
            set => SetProperty(ref _dynamicPropertiesTable, value);
        }

        public DynamicPropertiesTable FilteredPropertiesTable
        {
            get => _filteredPropertiesTable;
            set => SetProperty(ref _filteredPropertiesTable, value);
        }

        public bool IsFilterApplied
        {
            get => _isFilterApplied;
            set
            {
                _isFilterApplied = value;
                RaisePropertyChanged();
            }
        }

        private bool IsGroupCorrespondToFilters(IEnumerable<IConfigurationItemViewModel> viewModels)
        {
            foreach (var viewModel in viewModels)
            {
                if (viewModel is RuntimeItemGroupViewModel runtimeItemGroupViewModel)
                {
                    if (!IsGroupCorrespondToFilters(runtimeItemGroupViewModel.ChildStructItemViewModels))
                    {
                        return true;
                    }
                }

                if (viewModel is IRuntimePropertyViewModel propertyViewModel)
                {
                    if (CheckConditions(GetValueToCompare(propertyViewModel)))
                    {
                        return true;
                    }
                }
            }

            return false;
        }


        private ushort GetValueToCompare(IRuntimePropertyViewModel runtimePropertyViewModel)
        {
            var propertyUshort =
                runtimePropertyViewModel.DeviceContext.DeviceMemory.LocalMemoryValues[runtimePropertyViewModel.Address];
            if (runtimePropertyViewModel is IRuntimeSubPropertyViewModel subPropertyViewModel)
            {
                var resultBitArray = new bool[16];
                var boolArray = propertyUshort.GetBoolArrayFromUshort();
                int counter = 0;
                for (int i = 0; i < 16; i++)
                {
                    if (subPropertyViewModel.BitNumbersInWord.Contains(i))
                    {
                        resultBitArray[counter] = boolArray[i];
                        counter++;
                    }

                }

                propertyUshort = resultBitArray.BoolArrayToUshort();
            }

            return propertyUshort;
        }

        private bool CheckConditions(ushort valueToCompare)
        {
            foreach (var filterViewModel in _runtimeFilterViewModels)
            {
                if (!filterViewModel.IsActivated) continue;

                foreach (var condition in filterViewModel.Conditions)
                {
                    var res = ConditionHelper.CheckCondition(condition as ICompareCondition,valueToCompare);
                    if (res.IsSuccess && !res.Item)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void Initialize()
        {
            IsFilterApplied = _runtimeFilterViewModels.Any(model => model.IsActivated);
            if (IsFilterApplied)
            {
                _filteredGroupsToTransform =
                    _itemGroupsToTransform.Where(model => IsGroupCorrespondToFilters(model.ChildStructItemViewModels))
                        .ToList();
                var columnNamesWithPropertiesFiltered = new List<Tuple<string, IConfigurationItemViewModel>>();
                FillColumnNames(_filteredGroupsToTransform, columnNamesWithPropertiesFiltered);
                var lookupFiltered = columnNamesWithPropertiesFiltered.ToLookup(tuple => tuple.Item1, tuple => tuple.Item2);
                var columnNamesFiltered = lookupFiltered.Select(models => models.Key).ToList();
                FilteredPropertiesTable = new DynamicPropertiesTable(columnNamesFiltered,
                    _filteredGroupsToTransform.Select((model => model.Header)).ToList(), false);
                _filteredGroupsToTransform.ForEach((group =>
                    FilteredPropertiesTable.AddPropertyViewModel(GetRowFromItemGroup(group, lookupFiltered, columnNamesFiltered)
                        .Select((tuple => tuple.Value)).ToList())));
            }


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