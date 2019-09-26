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
            this._assToggleButton = this.AssociatedObject;
            if (!(this._assToggleButton.DataContext is IRuntimeConfigurationItemViewModel)) return;
            (this._assToggleButton.DataContext as IRuntimeConfigurationItemViewModel).Checked += this.TreeGridItemCheched;
            if (this._assToggleButton.IsLoaded)
            {
                AssToggleButton_Loaded(null, null);
            }

            this._assToggleButton.Unloaded += this._assToggleButton_Unloaded;
            this._assToggleButton.Loaded += this.AssToggleButton_Loaded;
            base.OnAttached();
        }

        private void AssToggleButton_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            this._assToggleButton.Checked += this.TreeViewToggleButtonBehavior_Checked;
            this._assToggleButton.Unchecked += this.TreeViewToggleButtonBehavior_Unchecked;
        }

        //OnDetaching не срабатывает, поэтому подпись на событие кнопки
        private void _assToggleButton_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            this._assToggleButton.Checked -= this.TreeViewToggleButtonBehavior_Checked;
            this._assToggleButton.Unchecked -= this.TreeViewToggleButtonBehavior_Unchecked;
        }

        private void TreeGridItemCheched(bool? isToExpand)
        {
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
            ObservableCollection<IRuntimeConfigurationItemViewModel> allItems =
                this._assToggleButton.Tag as ObservableCollection<IRuntimeConfigurationItemViewModel>;
            IRuntimeConfigurationItemViewModel currentItem = this._assToggleButton.DataContext as IRuntimeConfigurationItemViewModel;
            if (allItems != null)
            {
                int startIndex =
                    allItems.IndexOf(currentItem);
                if (startIndex < 0) return;
                if (currentItem.ChildStructItemViewModels is IEnumerable<IRuntimeConfigurationItemViewModel>)
                {
                    IEnumerable<IRuntimeConfigurationItemViewModel> childItems =
                        currentItem.ChildStructItemViewModels;
                    foreach (IRuntimeConfigurationItemViewModel child in childItems)
                    {
                        child.Checked?.Invoke(false);
                        startIndex++;
                        if (allItems.Count > startIndex)
                        {
                            if (allItems[startIndex].GetType() == child.GetType())
                            {
                                if (allItems[startIndex] == child) continue;
                                else
                                {

                                    if (allItems.Count > startIndex)
                                    {
                                        if (allItems[startIndex].GetType() == allItems[startIndex + 1].GetType())
                                            allItems.Move(startIndex, startIndex + 1);
                                    }

                                }
                            }
                            else
                            {
                                allItems.Insert(startIndex, child);
                            }

                        }
                        else
                        {
                            allItems.Add(child);
                        }
                    }
                }
            }
        }

        private void TreeViewToggleButtonBehavior_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            ObservableCollection<IRuntimeConfigurationItemViewModel> allItems =
                (sender as ToggleButton).Tag as ObservableCollection<IRuntimeConfigurationItemViewModel>;
            if (allItems == null) return;

            IRuntimeConfigurationItemViewModel oldItem = ((sender as ToggleButton).DataContext as IRuntimeConfigurationItemViewModel);
            if (oldItem == null) return;
            oldItem.IsChecked = false;
            this.DeleteTreeGridItem(allItems, oldItem, false);
        }

        private void TreeViewToggleButtonBehavior_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            IRuntimeConfigurationItemViewModel oldItem = ((sender as ToggleButton).DataContext as IRuntimeConfigurationItemViewModel);
            if (oldItem == null) return;
            oldItem.IsChecked = true;
            ObservableCollection<IRuntimeConfigurationItemViewModel> treeGridItems = ((sender as ToggleButton)?.Tag as ObservableCollection<IRuntimeConfigurationItemViewModel>);
            int index = treeGridItems.IndexOf(oldItem);
            if (oldItem?.ChildStructItemViewModels is IEnumerable<IRuntimeConfigurationItemViewModel>)
            {
                IEnumerable<IRuntimeConfigurationItemViewModel> newItems = oldItem.ChildStructItemViewModels;
                foreach (IRuntimeConfigurationItemViewModel newItem in newItems)
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
                        this.AddTreeGridItem(treeGridItems, newItem, oldItem, index);
                    }
                }
            }
        }

        private void AddTreeGridItem(ObservableCollection<IRuntimeConfigurationItemViewModel> treeGridItems,
            IRuntimeConfigurationItemViewModel newConfigurationItem,
            IRuntimeConfigurationItemViewModel oldConfigurationItem, int index)
        {
            newConfigurationItem.Level = oldConfigurationItem.Level + 1;
            treeGridItems.Insert(index, newConfigurationItem);
        }

        /// <summary>
        /// рекурсивное удаление всей ветки
        /// </summary>
        /// <param name="treeGridItems">коллекция всех объектов в строках</param>
        /// <param name="removingItem">текущие элемент</param>
        /// <param name="removeFromCollection">удалять ли из коллекции текущий элемент</param>
        private void DeleteTreeGridItem(ObservableCollection<IRuntimeConfigurationItemViewModel> treeGridItems,
            IRuntimeConfigurationItemViewModel removingItem, bool removeFromCollection)
        {
            if (removingItem.ChildStructItemViewModels is IEnumerable<IRuntimeConfigurationItemViewModel>)
            {
                IEnumerable<IRuntimeConfigurationItemViewModel> newItems =
                    removingItem.ChildStructItemViewModels;
                foreach (IRuntimeConfigurationItemViewModel newItem in newItems)
                {
                    this.DeleteTreeGridItem(treeGridItems, newItem, true);
                }
            }
            else if (removingItem.ChildStructItemViewModels is IRuntimeConfigurationItemViewModel)
            {
                this.DeleteTreeGridItem(treeGridItems, removingItem.ChildStructItemViewModels as IRuntimeConfigurationItemViewModel, true);

            }
            if (removeFromCollection) this.RemoveItemFromList(treeGridItems, removingItem);
        }

        private void RemoveItemFromList(ObservableCollection<IRuntimeConfigurationItemViewModel> treeGridItems,
            IRuntimeConfigurationItemViewModel removingItem)
        {
            treeGridItems.Remove(removingItem);
            removingItem.Level = 0;
        }
    }
}