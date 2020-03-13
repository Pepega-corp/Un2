using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Presentation.Infrastructure.TreeGrid;

namespace Unicon2.SharedResources.Behaviors
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
            (_assToggleButton.DataContext as IConfigurationItemViewModel).Checked += TreeGridItemCheched;
            if (_assToggleButton.IsLoaded)
            {
                _assToggleButton.Checked += TreeViewToggleButtonBehavior_Checked;
                _assToggleButton.Unchecked += TreeViewToggleButtonBehavior_Unchecked;
            }

            _assToggleButton.Unloaded += _assToggleButton_Unloaded;
            _assToggleButton.Loaded += _assToggleButton_Loaded;
            if (_assToggleButton.DataContext is IConfigurationItemViewModel)
            {
                if ((_assToggleButton.DataContext as IConfigurationItemViewModel).IsChecked)
                {
                    TreeViewToggleButtonBehavior_Checked(_assToggleButton, null);
                    _assToggleButton.IsChecked = true;
                }
            }
            base.OnAttached();
        }

        private void _assToggleButton_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _assToggleButton.Checked += TreeViewToggleButtonBehavior_Checked;
            _assToggleButton.Unchecked += TreeViewToggleButtonBehavior_Unchecked;
        }

        //OnDetaching не срабатывает, поэтому подпись на событие кнопки
        private void _assToggleButton_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _assToggleButton.Checked -= TreeViewToggleButtonBehavior_Checked;
            _assToggleButton.Unchecked -= TreeViewToggleButtonBehavior_Unchecked;
        }

        private void TreeGridItemCheched(bool? isToExpand)
        {
            if (!_assToggleButton.IsChecked.HasValue) return;
            if (isToExpand.HasValue)
            {
                if (isToExpand.Value)
                {
                    if (_assToggleButton.IsChecked.Value)
                    {
                        RefreshToggleButtonRows();
                    }

                }
                _assToggleButton.IsChecked = isToExpand;
            }
            else
            {
                _assToggleButton.IsChecked = !_assToggleButton.IsChecked.Value;
            }
        }

        private void RefreshToggleButtonRows()
        {
            ObservableCollection<IConfigurationItemViewModel> allItems =
                _assToggleButton.Tag as ObservableCollection<IConfigurationItemViewModel>;
            IConfigurationItemViewModel currentItem = _assToggleButton.DataContext as IConfigurationItemViewModel;
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
                            if (allItems[startIndex].GetType() == child.GetType())
                            {
                                if (allItems[startIndex] == child) continue;

                                if (allItems.Count > startIndex)
                                {
                                    if (allItems[startIndex].GetType() == allItems[startIndex + 1].GetType())
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
                            allItems.Add(child);
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
            DeleteTreeGridItem(allItems, oldItem, false);
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
                    bool isAdded = false;
                    if (newItem is IGroupedConfigurationItemViewModel)
                    {
                        if (!(newItem as IGroupedConfigurationItemViewModel).IsGroupedProperty)
                        {
                            newItem.ChildStructItemViewModels.ForEach(model =>
                            {
                                index++;
                                AddTreeGridItem(treeGridItems, model, oldItem, index);
                            });
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
                    DeleteTreeGridItem(treeGridItems, newItem, true);
                }
            }
            else if (removingItem.ChildStructItemViewModels is IConfigurationItemViewModel)
            {
                DeleteTreeGridItem(treeGridItems, removingItem.ChildStructItemViewModels as IConfigurationItemViewModel, true);

            }
            if (removeFromCollection) RemoveItemFromList(treeGridItems, removingItem);


        }

        private void RemoveItemFromList(ObservableCollection<IConfigurationItemViewModel> treeGridItems,
            IConfigurationItemViewModel removingItem)
        {
            treeGridItems.Remove(removingItem);
            removingItem.Level = 0;
        }
    }
}