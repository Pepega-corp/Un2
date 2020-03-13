using System.Collections.Generic;
using System.Linq;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Configuration.ViewModel
{
    public class MainConfigItemViewModel : ViewModelBase
    {
        private bool _isSelected;
        private List<IConfigurationItemViewModel> _relatedRows;
        private bool _isTableSelected;

        public MainConfigItemViewModel(IEnumerable<MainConfigItemViewModel> childConfigItemViewModels,
            IConfigurationItemViewModel relatedConfigurationItemViewModel)
        {
            ChildConfigItemViewModels = childConfigItemViewModels;
            RelatedConfigurationItemViewModel = relatedConfigurationItemViewModel;


            IsTableEnabled =
                (relatedConfigurationItemViewModel is IItemGroupViewModel groupViewModel) &&
                groupViewModel.IsTableViewAllowed;

           // IGroupWithReiterationInfo groupWithReiterationInfo = null;

            //if (RelatedConfigurationItemViewModel is IItemGroupViewModel groupViewModelWithreit &&
            //    groupViewModelWithreit.Model is IItemsGroup itemsGroup &&
            //    itemsGroup.GroupInfo is IGroupWithReiterationInfo groupWithReiteration)
            //{
            //    IsGroupWithReiteration = groupWithReiteration.IsReiterationEnabled;
            //    groupWithReiterationInfo = groupWithReiteration;
            //}

            if (!childConfigItemViewModels.Any() || IsGroupWithReiteration)
            {
                var relatedRows = new List<IConfigurationItemViewModel>();
                if (IsGroupWithReiteration)
                {
                    //var factory = StaticContainer.Container.Resolve<IRuntimeConfigurationItemViewModelFactory>();
                    //var children = groupWithReiterationInfo.SubGroups.Select(
                    //    (info => factory.CreateGroupWithReiterationViewModel(info))).ToList();

                    //FillRelatedRows(relatedRows, children, 0);

                }
                else
                {
                    FillRelatedRows(relatedRows, relatedConfigurationItemViewModel.ChildStructItemViewModels, 0);
                }

                RelatedRows = relatedRows;
            }

        }

        public MainConfigItemViewModel(IEnumerable<MainConfigItemViewModel> childConfigItemViewModels)
        {

        }

        public bool IsGroupWithReiteration { get; set; }


        private void FillRelatedRows(List<IConfigurationItemViewModel> currentRows,
            IEnumerable<IConfigurationItemViewModel> children, int level)
        {
            foreach (var child in children)
            {
                child.Level = level;
                currentRows.Add(child);
                FillRelatedRows(currentRows, child.ChildStructItemViewModels, level + 1);
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