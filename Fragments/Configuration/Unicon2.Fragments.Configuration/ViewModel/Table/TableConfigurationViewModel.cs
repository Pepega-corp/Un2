using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Behaviors;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.Events;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.SharedResources.Behaviors;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Configuration.ViewModel.Table
{
    public class TableConfigurationViewModel :ViewModelBase
    {
        private readonly ObservableCollection<IRuntimeConfigurationItemViewModel> _itemGroupsToTransform;
        private DynamicPropertiesTable _dynamicPropertiesTable;

        public TableConfigurationViewModel(ObservableCollection<IRuntimeConfigurationItemViewModel> itemGroupsToTransform)
        {
            _itemGroupsToTransform = itemGroupsToTransform;
            Initialize();

        }

        public DynamicPropertiesTable DynamicPropertiesTable
        {
            get => _dynamicPropertiesTable;
            set => SetProperty(ref _dynamicPropertiesTable , value);
        }

        private void Initialize()
        {
            DynamicPropertiesTable=new DynamicPropertiesTable(GetColumnNames(_itemGroupsToTransform).ToList(), _itemGroupsToTransform.Select((model => model.Header)).ToList(), false);
            _itemGroupsToTransform.ForEach((group => DynamicPropertiesTable.AddPropertyViewModel(GetRowFromItemGroup(group,true))));
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

        private List<ILocalAndDeviceValueContainingViewModel> GetRowFromItemGroup(IRuntimeConfigurationItemViewModel group, bool isDeviceValue)
        {
            var result=new List<ILocalAndDeviceValueContainingViewModel>();

            group.ChildStructItemViewModels.ForEach((item =>
            {
                if (item.ChildStructItemViewModels.Any())
                {
                    result.AddRange(GetRowFromItemGroup(item, isDeviceValue));
                }
                else
                {
                    result.Add(item as ILocalAndDeviceValueContainingViewModel);
                }
            }));
            return result;
        }
        
    }
}
