using System;
using System.Linq;
using System.Windows.Input;
using Unicon2.Fragments.Configuration.Infrastructure.Factories;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;
using Unicon2.Fragments.Configuration.ViewModel.Table;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Unity.Commands;

namespace Unicon2.Fragments.Configuration.ViewModel
{
    public class RuntimeItemGroupViewModel : RuntimeConfigurationItemViewModelBase, IRuntimeItemGroupViewModel, IAsTableViewModel
    {

        private bool _isTableView;
        private TableConfigurationViewModel _tableConfigurationViewModel;
        private bool _isTableViewAllowed;

        public RuntimeItemGroupViewModel()
        {
            this.IsCheckable = true;
        }

        private void OnTryTransformToTable()
        {
            if (!IsTableView) return;
            if (ChildStructItemViewModels.All((model => model is RuntimeItemGroupViewModel)) &&
                TableConfigurationViewModel == null) 
            {
                TableConfigurationViewModel = new TableConfigurationViewModel(ChildStructItemViewModels);
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

        public override string TypeName => ConfigurationKeys.DEFAULT_ITEM_GROUP;

        public virtual string StrongName => ConfigurationKeys.RUNTIME_DEFAULT_ITEM_GROUP +
                                             ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;


       

        public bool IsTableView
        {
            get => _isTableView;
            set
            {
                SetProperty(ref _isTableView, value);
                OnTryTransformToTable();
            }
        }

        public string AsossiatedDetailsViewName => "ConfigAsTableView";

        public bool IsTableViewAllowed
        {
            get => _isTableViewAllowed;
            set => SetProperty(ref _isTableViewAllowed, value);
        }
    }
}