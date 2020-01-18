using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;
using Unicon2.Presentation.Infrastructure.TreeGrid;

namespace Unicon2.Fragments.Configuration.Editor.Behaviors
{
    /// <summary>
    /// поведение для ToggleButton, добавляет и удаляет строки в гриде
    /// </summary>
    public class TreeViewToggleButtonBehavior : Behavior<ToggleButton>
    {
        private ToggleButton _assToggleButton;

        protected override void OnAttached()
        {
            this._assToggleButton = this.AssociatedObject;
            if (!(this._assToggleButton.DataContext is IConfigurationItemViewModel)) return;
            (this._assToggleButton.DataContext as IConfigurationItemViewModel).Checked += this.TreeGridItemCheched;

            this._assToggleButton.Checked += this.TreeViewToggleButtonBehavior_Checked;
            this._assToggleButton.Unchecked += this.TreeViewToggleButtonBehavior_Unchecked;
            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            this._assToggleButton = this.AssociatedObject;
            if (!(this._assToggleButton.DataContext is IConfigurationItemViewModel)) return;
            (this._assToggleButton.DataContext as IConfigurationItemViewModel).Checked -= this.TreeGridItemCheched;
            this._assToggleButton.Checked -= this.TreeViewToggleButtonBehavior_Checked;
            this._assToggleButton.Unchecked -= this.TreeViewToggleButtonBehavior_Unchecked;
            base.OnDetaching();
        }


        private void TreeGridItemCheched(bool? isToExpand)
        {
            if (!this._assToggleButton.IsEnabled) return;
            if (!this._assToggleButton.IsChecked.HasValue) return;
            if (isToExpand.HasValue)
            {
                if (isToExpand.Value)
                {
                    if (this._assToggleButton.IsChecked.Value)
                    {
                        this.RefreshToggleButtonRows();
                    }

                }
                this._assToggleButton.IsChecked = isToExpand;
            }
            else
            {
                this._assToggleButton.IsChecked = !this._assToggleButton.IsChecked.Value;
            }
        }

        private void RefreshToggleButtonRows()
        {
            ObservableCollection<IConfigurationItemViewModel> allItems =
                this._assToggleButton.Tag as ObservableCollection<IConfigurationItemViewModel>;
            IConfigurationItemViewModel currentItem = this._assToggleButton.DataContext as IConfigurationItemViewModel;
            if (allItems != null)
            {
                int startIndex =
                    allItems.IndexOf(currentItem);
                if (startIndex < 0) return;
                if (currentItem.ChildStructItemViewModels is IEnumerable<IConfigurationItemViewModel>)
                {
                    IEnumerable<IConfigurationItemViewModel> childItems =
                        currentItem.ChildStructItemViewModels;
                    foreach (IConfigurationItemViewModel child in childItems)
                    {
                        child.Checked?.Invoke(false);
                        startIndex++;
                        if (allItems.Count > startIndex)
                        {
                            if (allItems[startIndex].Level == child.Level)
                            {
                                if (allItems[startIndex] == child) continue;
                                if (allItems.Count > startIndex)
                                {
                                    if (allItems[startIndex].Level == allItems[startIndex + 1].Level)
                                        allItems.Move(startIndex, startIndex + 1);
                                }
                            }
                            else
                            {
                                allItems.Insert(startIndex, child);
                            }

                        }
                        else
                        {
                            //allItems.Add(child);
                            this.AddTreeGridItem(allItems, child, currentItem, startIndex);
                        }
                    }
                }
            }
        }

        private void TreeViewToggleButtonBehavior_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            ObservableCollection<IConfigurationItemViewModel> allItems =
                (sender as ToggleButton).Tag as ObservableCollection<IConfigurationItemViewModel>;
            if (allItems == null) return;

            IConfigurationItemViewModel oldItem = ((sender as ToggleButton).DataContext as IConfigurationItemViewModel);
            if (oldItem == null) return;
            oldItem.IsChecked = false;
            this.DeleteTreeGridItem(allItems, oldItem, false);
        }

        private void TreeViewToggleButtonBehavior_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            IConfigurationItemViewModel oldItem = ((sender as ToggleButton).DataContext as IConfigurationItemViewModel);
            if (oldItem == null) return;
            oldItem.IsChecked = true;
            ObservableCollection<IConfigurationItemViewModel> treeGridItems = ((sender as ToggleButton)?.Tag as ObservableCollection<IConfigurationItemViewModel>);
            int index = treeGridItems.IndexOf(oldItem);
            if (oldItem?.ChildStructItemViewModels is IEnumerable<IConfigurationItemViewModel>)
            {
                IEnumerable<IConfigurationItemViewModel> newItems = oldItem.ChildStructItemViewModels;
                foreach (IConfigurationItemViewModel newItem in newItems)
                {
                    index++;
                    this.AddTreeGridItem(treeGridItems, newItem, oldItem, index);
                }

                return;
            }
        }



        private void AddTreeGridItem(ObservableCollection<IConfigurationItemViewModel> treeGridItems,
            IConfigurationItemViewModel childConfigurationItemViewModel,
            IConfigurationItemViewModel parentConfigurationItemViewModel, int index)
        {
            childConfigurationItemViewModel.Level = parentConfigurationItemViewModel.Level + 1;
            treeGridItems.Insert(index, childConfigurationItemViewModel);
        }

        /// <summary>
        /// рекурсивное удаление всей ветки
        /// </summary>
        /// <param name="treeGridItems">коллекция всех объектов в строках</param>
        /// <param name="removingItem">текущие элемент</param>
        /// <param name="removeFromCollection">удалять ли из коллекции текущий элемент</param>
        private void DeleteTreeGridItem(ObservableCollection<IConfigurationItemViewModel> treeGridItems,
            IConfigurationItemViewModel removingItem, bool removeFromCollection)
        {
            if (removingItem.ChildStructItemViewModels is IEnumerable<IConfigurationItemViewModel>)
            {
                IEnumerable<IConfigurationItemViewModel> newItems =
                    removingItem.ChildStructItemViewModels;
                foreach (IConfigurationItemViewModel newItem in newItems)
                {
                    this.DeleteTreeGridItem(treeGridItems, newItem, true);
                }
            }
            else if (removingItem.ChildStructItemViewModels is IConfigurationItemViewModel)
            {
                this.DeleteTreeGridItem(treeGridItems, removingItem.ChildStructItemViewModels as IConfigurationItemViewModel, true);

            }
            if (removeFromCollection) this.RemoveItemFromList(treeGridItems, removingItem);


        }

        private void RemoveItemFromList(ObservableCollection<IConfigurationItemViewModel> treeGridItems,
            IConfigurationItemViewModel removingItem)
        {
            treeGridItems.Remove(removingItem);
            removingItem.Level = 0;
        }
    }
}