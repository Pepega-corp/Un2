using System;
using System.Linq;
using System.Windows.Input;
using Unicon2.Fragments.Configuration.Infrastructure.Factories;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Fragments.Configuration.ViewModel.Table;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Unity.Commands;

namespace Unicon2.Fragments.Configuration.ViewModel
{
    public class RuntimeItemGroupViewModel : RuntimeConfigurationItemViewModelBase, IItemGroupViewModel, IConfigurationAsTableViewModel
    {
        private readonly IRuntimeConfigurationItemViewModelFactory _runtimeConfigurationItemViewModelFactory;
        private bool _isTableView;
        private TableConfigurationViewModel _tableConfigurationViewModel;

        public RuntimeItemGroupViewModel(IRuntimeConfigurationItemViewModelFactory runtimeConfigurationItemViewModelFactory)
        {
            this._runtimeConfigurationItemViewModelFactory = runtimeConfigurationItemViewModelFactory;
            this.IsCheckable = true;
            TryTransformToTableCommand=new RelayCommand(OnTryTransformToTable);

        }

        private void OnTryTransformToTable()
        {
            var numberOfChildItemList = new int[ChildStructItemViewModels.Count];
            for (int i = 0; i < ChildStructItemViewModels.Count; i++)
            {
                numberOfChildItemList[i] = ChildStructItemViewModels[i].ChildStructItemViewModels.Count;
            }

            var isAllNumbersEqual=numberOfChildItemList.Distinct().Count()==1;
            if (ChildStructItemViewModels.All((model => model is RuntimeItemGroupViewModel))&&isAllNumbersEqual)
            {
                TableConfigurationViewModel = new TableConfigurationViewModel(ChildStructItemViewModels);
                IsTableView = !IsTableView;
            }
        }

        public TableConfigurationViewModel TableConfigurationViewModel
        {
            get => _tableConfigurationViewModel;
            set
            {
                SetProperty(ref _tableConfigurationViewModel, value);
            }
        }

        #region Overrides of ConfigurationItemViewModelBase

        public override string TypeName => ConfigurationKeys.DEFAULT_ITEM_GROUP;

        public override string StrongName => ConfigurationKeys.RUNTIME_DEFAULT_ITEM_GROUP +
                                             ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;


        #region Overrides of ConfigurationItemViewModelBase

        protected override void SetModel(object model)
        {
            this.ChildStructItemViewModels.Clear();
            foreach (IConfigurationItem configurationItem in ((IItemsGroup) model).ConfigurationItemList)
            {
                this.ChildStructItemViewModels.Add(this._runtimeConfigurationItemViewModelFactory
                    .CreateRuntimeConfigurationItemViewModel(configurationItem));
            }
            base.SetModel(model);
        }

        #endregion

        #endregion

        #region Implementation of IItemGroupViewModel

        public bool IsTableView
        {
            get => _isTableView;
            set => SetProperty(ref _isTableView, value);
        }

        public ICommand TryTransformToTableCommand { get; }
        #endregion
    }
}