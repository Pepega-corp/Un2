using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Infrastructure.Common;
using Unicon2.Presentation.Infrastructure.Events;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Configuration.ViewModel
{
   public class MainConfigItemViewModel:ViewModelBase
    {
        private bool _isSelected;
        private List<IConfigurationItemViewModel> _relatedRows;
        private bool _isTableSelected;

        public MainConfigItemViewModel(IEnumerable<MainConfigItemViewModel> childConfigItemViewModels, IConfigurationItemViewModel relatedConfigurationItemViewModel)
        {
            ChildConfigItemViewModels = childConfigItemViewModels;
            RelatedConfigurationItemViewModel = relatedConfigurationItemViewModel;
            IsTableEnabled = (relatedConfigurationItemViewModel is IItemGroupViewModel groupViewModel) && groupViewModel.IsTableViewAllowed;
            if (!childConfigItemViewModels.Any())
            {
                var relatedRows = new List<IConfigurationItemViewModel>();
                FillRelatedRows(relatedRows, relatedConfigurationItemViewModel, 0);
                RelatedRows = relatedRows;
            }

        }

 
        private void FillRelatedRows(List<IConfigurationItemViewModel> currentRows, IConfigurationItemViewModel row, int level)
        {
            foreach (var child in row.ChildStructItemViewModels)
            {
                child.Level = level;
                currentRows.Add(child);
                FillRelatedRows(currentRows, child, level + 1);
            }
        }

        public IEnumerable<MainConfigItemViewModel> ChildConfigItemViewModels { get; }

        public IConfigurationItemViewModel RelatedConfigurationItemViewModel { get; }


        public List<IConfigurationItemViewModel> RelatedRows
        {
            get => _relatedRows;
            set
            {
                _relatedRows = value;
                RaisePropertyChanged();
            }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value; 
                RaisePropertyChanged();
            }
        }
        public bool IsTableEnabled { get; }
        public bool IsTableSelected
        {
            get => _isTableSelected;
            set
            {
                _isTableSelected = value;
                RaisePropertyChanged();
            }
        }
    }
}
