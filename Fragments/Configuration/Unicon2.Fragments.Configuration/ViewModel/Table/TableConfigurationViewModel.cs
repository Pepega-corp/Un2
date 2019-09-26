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
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.SharedResources.Behaviors;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Configuration.ViewModel.Table
{
    public class TableConfigurationViewModel : RuntimeConfigurationItemViewModelBase
    {
        private readonly ObservableCollection<IRuntimeConfigurationItemViewModel> _itemGroupsToTransform;
        private ConfigTableValueViewModel _deviceValue;
        private ConfigTableValueViewModel _localValue;

        public TableConfigurationViewModel(ObservableCollection<IRuntimeConfigurationItemViewModel> itemGroupsToTransform)
        {
            _itemGroupsToTransform = itemGroupsToTransform;
            Initialize();
        }
        private void Initialize()
        {
            DeviceValue=new ConfigTableValueViewModel(new DynamicDataTable(GetColumnNames(_itemGroupsToTransform).ToList(), _itemGroupsToTransform.Select((model => model.Header)).ToList(),false));new DynamicDataTable(GetColumnNames(_itemGroupsToTransform).ToList(), _itemGroupsToTransform.Select((model => model.Header)).ToList(),false);
            _itemGroupsToTransform.ForEach((group => DeviceValue.ConfigTable.AddFormattedValueViewModel(GetRowFromItemGroup(group,true))));
            LocalValue = new ConfigTableValueViewModel(new DynamicDataTable(GetColumnNames(_itemGroupsToTransform).ToList(), _itemGroupsToTransform.Select((model => model.Header)).ToList(), false)); new DynamicDataTable(GetColumnNames(_itemGroupsToTransform).ToList(), _itemGroupsToTransform.Select((model => model.Header)).ToList(), false);
            _itemGroupsToTransform.ForEach((group => LocalValue.ConfigTable.AddFormattedValueViewModel(GetRowFromItemGroup(group, false))));

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

        private List<IFormattedValueViewModel> GetRowFromItemGroup(IRuntimeConfigurationItemViewModel group, bool isDeviceValue)
        {
            var result=new List<IFormattedValueViewModel>();

            group.ChildStructItemViewModels.ForEach((item =>
            {
                if (item.ChildStructItemViewModels.Any())
                {
                    result.AddRange(GetRowFromItemGroup(item, isDeviceValue));
                }
                else
                {
                    result.Add(GetCellViewModel(item, isDeviceValue));
                }
            }));
            return result;
        }

        private IFormattedValueViewModel GetCellViewModel(IRuntimeConfigurationItemViewModel runtimeConfigurationItemViewModel,bool isDeviceValue)
        {
            if (runtimeConfigurationItemViewModel is ILocalAndDeviceValueContainingViewModel localAndDeviceValueContainingViewModel)
            {
                return isDeviceValue ? localAndDeviceValueContainingViewModel.DeviceValue : localAndDeviceValueContainingViewModel.LocalValue;
            }
            return null;
        }
        


    
        public override string TypeName => ConfigurationKeys.CONFIG_TABLE_VIEW;

        public override string StrongName => ConfigurationKeys.RUNTIME + ConfigurationKeys.CONFIG_TABLE_VIEW+ ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

        #region Implementation of ILocalAndDeviceValueContainingViewModel

        public ConfigTableValueViewModel DeviceValue
        {
            get => _deviceValue;
            set => SetProperty(ref _deviceValue,value);
        }

        public ConfigTableValueViewModel LocalValue
        {
            get => _localValue;
            set => SetProperty(ref _localValue, value);
        }

        #region Overrides of DisposableBindableBase

        protected override void OnDisposing()
        {
            base.OnDisposing();
        }

        #endregion

        #endregion
    }


    public class ConfigTableValueViewModel :ViewModelBase, IStronglyNamed
    {
        private DynamicDataTable _configTable;

        public ConfigTableValueViewModel(DynamicDataTable dynamicDataTable )
        {
            ConfigTable = dynamicDataTable;
        }
        public DynamicDataTable ConfigTable
        {
            get => _configTable;
            set => SetProperty(ref _configTable, value);
        }
        #region Implementation of IStronglyNamed

        public string StrongName => ConfigurationKeys.RUNTIME + ConfigurationKeys.CONFIG_TABLE_VALUE + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

        #endregion
    }

}
