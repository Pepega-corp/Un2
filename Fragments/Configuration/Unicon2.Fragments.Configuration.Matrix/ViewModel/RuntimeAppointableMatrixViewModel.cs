using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Properties;
using Unicon2.Fragments.Configuration.Matrix.Keys;
using Unicon2.Fragments.Configuration.ViewModel;
using Unicon2.Fragments.Configuration.ViewModel.Properties;
using Unicon2.Fragments.Configuration.ViewModel.Table;
using Unicon2.Infrastructure;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Unity.Commands;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Configuration.Matrix.ViewModel
{

    public class RuntimeAppointableMatrixViewModel : RuntimePropertyViewModel,IAsTableViewModel
    {
        private TableConfigurationViewModel _tableConfigurationViewModel;

        public RuntimeAppointableMatrixViewModel(ITypesContainer container, IValueViewModelFactory valueViewModelFactory) : base(container, valueViewModelFactory)
        {
            ShowDetails = new RelayCommand(OnShowDetails);
            TableConfigurationViewModel = new TableConfigurationViewModel(ChildStructItemViewModels);

        }

        private void OnShowDetails()
        {

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

        public override string TypeName => ConfigurationKeys.APPOINTABLE_MATRIX;
        public override string StrongName => ConfigurationKeys.RUNTIME + ConfigurationKeys.APPOINTABLE_MATRIX + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;


        public RelayCommand ShowDetails { get; }

        #endregion


        #region Implementation of IConfigurationAsTableViewModel

        public bool IsTableView
        {
            get => true;
            set { }
        }

        public string AsossiatedDetailsViewName => "MatrixTableValueView";

        #endregion
    }
}

