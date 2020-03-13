using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Presentation.Infrastructure.TreeGrid;

namespace Unicon2.Fragments.Configuration.Behaviors
{
    /// <summary>
    /// поведение для ToggleButton, добавляет и удаляет строки в гриде
    /// </summary>
    public class TreeViewToggleButtonBehavior : Behavior<ToggleButton>
    {
        private ToggleButton _assToggleButton;

        protected override void OnAttached()
        {
            _assToggleButton = AssociatedObject;
            if (!(_assToggleButton.DataContext is IConfigurationItemViewModel)) return;
            InitTree(AssociatedObject);
            base.OnAttached();
        }

        
        private void InitTree(ToggleButton toggleButton)
        {
            IConfigurationItemViewModel oldItem = (toggleButton.DataContext as IConfigurationItemViewModel);
            if (oldItem == null) return;
            if (oldItem is IAsTableViewModel configurationAsTableViewModel&&configurationAsTableViewModel.IsTableView)
            {
                return;
            }
            oldItem.IsChecked = true;
            ObservableCollection<IConfigurationItemViewModel> treeGridItems = (toggleButton?.Tag as ObservableCollection<IConfigurationItemViewModel>);
            int index = treeGridItems.IndexOf(oldItem);
            if (oldItem?.ChildStructItemViewModels is IEnumerable<IConfigurationItemViewModel>)
            {
                IEnumerable<IConfigurationItemViewModel> newItems = oldItem.ChildStructItemViewModels;
                foreach (IConfigurationItemViewModel newItem in newItems)
                {
                    bool isAdded = false;
                    if (newItem is IGroupedConfigurationItemViewModel)
                    {
                        if (!(newItem as IGroupedConfigurationItemViewModel).IsGroupedProperty)
                        {
                            newItem.ChildStructItemViewModels.ForEach((model =>
                            {
                                index++;
                                AddTreeGridItem(treeGridItems, model, oldItem, index);
                            }));
                            isAdded = true;
                        }
                    }
                    if (!isAdded)
                    {
                        index++;
                        AddTreeGridItem(treeGridItems, newItem, oldItem, index);
                    }
                }
            }
        }

        private void AddTreeGridItem(ObservableCollection<IConfigurationItemViewModel> treeGridItems,
            IConfigurationItemViewModel newConfigurationItem,
            IConfigurationItemViewModel oldConfigurationItem, int index)
        {
            newConfigurationItem.Level = oldConfigurationItem.Level + 1;
            treeGridItems.Insert(index, newConfigurationItem);
        }
        
    }
}