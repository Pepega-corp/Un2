﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;
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

    public class RuntimeRuntimeAppointableMatrixViewModel : RuntimePropertyViewModel, IRuntimeAppointableMatrixViewModel
    {
        private TableConfigurationViewModel _tableConfigurationViewModel;
        private bool _isTableView;

        public RuntimeRuntimeAppointableMatrixViewModel()
        {
            TableConfigurationViewModel = new TableConfigurationViewModel(ChildStructItemViewModels);
        }
        public TableConfigurationViewModel TableConfigurationViewModel
        {
            get => _tableConfigurationViewModel;
            set { SetProperty(ref _tableConfigurationViewModel, value); }
        }

        public override string TypeName => ConfigurationKeys.APPOINTABLE_MATRIX;
        
        public bool IsTableView
        {
            get => _isTableView;
            set => SetProperty(ref _isTableView, value);
        }

        public bool IsTableViewAllowed => true;
        public string AsossiatedDetailsViewName => "MatrixTableValueView";
    }
}