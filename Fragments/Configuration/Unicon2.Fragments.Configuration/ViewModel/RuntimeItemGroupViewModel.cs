using System.Collections.Generic;
using System.Linq;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;
using Unicon2.Fragments.Configuration.ViewModel.Table;
using Unicon2.Infrastructure;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;

namespace Unicon2.Fragments.Configuration.ViewModel
{
    public class RuntimeItemGroupViewModel : RuntimeConfigurationItemViewModelBase, IRuntimeItemGroupViewModel, IAsTableViewModel
    {

        private bool _isTableView;
        private TableConfigurationViewModel _tableConfigurationViewModel;
        private bool _isTableViewAllowed;
        private List<RuntimeFilterViewModel> _filterViewModels;

        public RuntimeItemGroupViewModel()
        {
            IsCheckable = true;
        }

        private void OnTryTransformToTable()
        {
            if (!IsTableView) return;
            if (TableConfigurationViewModel == null) 
            {
                TryTransformToTable();
            }
        }
        public void TryTransformToTable()
        {
            if (!IsTableView) return;
            if (ChildStructItemViewModels.All(model => model is RuntimeItemGroupViewModel))
            {
                TableConfigurationViewModel = new TableConfigurationViewModel(ChildStructItemViewModels.ToList(), FilterViewModels);
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

        public override string StrongName => ConfigurationKeys.RUNTIME_DEFAULT_ITEM_GROUP +
                                             ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;


        public List<RuntimeFilterViewModel> FilterViewModels
        {
            get => _filterViewModels;
            set
            {
                _filterViewModels = value;
                RaisePropertyChanged();
            }
        }


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

        public bool IsMain { get; set; }
        public int Offset { get; set; }
    }
}