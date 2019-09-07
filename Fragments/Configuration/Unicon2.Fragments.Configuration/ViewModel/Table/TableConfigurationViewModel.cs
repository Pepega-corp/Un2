using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.SharedResources.Behaviors;

namespace Unicon2.Fragments.Configuration.ViewModel.Table
{
    public class TableConfigurationViewModel : RuntimeConfigurationItemViewModelBase
    {
        private readonly ObservableCollection<IRuntimeConfigurationItemViewModel> _itemGroupsToTransform;
        private DynamicDataTable _configTable;

        public TableConfigurationViewModel(ObservableCollection<IRuntimeConfigurationItemViewModel> itemGroupsToTransform)
        {
            _itemGroupsToTransform = itemGroupsToTransform;
            Initialize();
        }
        private void Initialize()
        {
            ConfigTable=new DynamicDataTable(GetColumnNames(_itemGroupsToTransform).ToList(), _itemGroupsToTransform.Select((model => model.Header)).ToList(),false);
            _itemGroupsToTransform.ForEach((group => ConfigTable.AddFormattedValueViewModel(GetRowFromItemGroup(group))));
        }

        private List<string> GetColumnNames(IEnumerable<IRuntimeConfigurationItemViewModel> items)
        {
            var columnNames = new List<string>();
            var columnNameSource = items.First().ChildStructItemViewModels;
            foreach (var innerChilditem in columnNameSource)
            {
                if (innerChilditem is IGroupedConfigurationItemViewModel)
                {
                    columnNames.AddRange(innerChilditem.ChildStructItemViewModels.Select((model =>model.Header )));
                }
                else
                {
                    columnNames.Add(innerChilditem.Header);
                }
            }

            return columnNames;
        }

        private List<IFormattedValueViewModel> GetRowFromItemGroup(IRuntimeConfigurationItemViewModel group)
        {
            var result=new List<IFormattedValueViewModel>();

            group.ChildStructItemViewModels.ForEach((item =>
            {
                if (item.ChildStructItemViewModels.Any())
                {
                    result.AddRange(GetRowFromItemGroup(item));
                }
                else
                {
                    result.Add(GetCellViewModel(item));
                }
            }));
            return result;
        }

        private IFormattedValueViewModel GetCellViewModel(IRuntimeConfigurationItemViewModel runtimeConfigurationItemViewModel)
        {
            if (runtimeConfigurationItemViewModel is ILocalAndDeviceValueContainingViewModel localAndDeviceValueContainingViewModel)
            {
                return localAndDeviceValueContainingViewModel.DeviceValue;
            }
            return null;
        }

        public DynamicDataTable ConfigTable
        { 
            get => _configTable;
            set => SetProperty(ref _configTable, value);
        }

        public override string TypeName => ConfigurationKeys.CONFIG_TABLE_VIEW;

        public override string StrongName => ConfigurationKeys.RUNTIME + ConfigurationKeys.CONFIG_TABLE_VIEW+ ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;
    }
}
