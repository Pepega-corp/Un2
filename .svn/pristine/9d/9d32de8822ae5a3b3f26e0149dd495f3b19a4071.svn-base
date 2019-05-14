using System;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Unicon2.Fragments.Programming
{
    [TemplatePart(Name = "PART_ItemsHolder", Type = typeof(Panel))]
    public class TabControlEx : TabControl
    {
        private Panel _itemsHolderPanel;

        public TabControlEx()
        {
            // This is necessary so that we get the initial databound selected item
            ItemContainerGenerator.StatusChanged += this.ItemContainerGenerator_StatusChanged;
        }

        /// <summary>
        /// If containers are done, generate the selected item
        /// </summary>
        private void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
        {
            if (ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
            {
                ItemContainerGenerator.StatusChanged -= this.ItemContainerGenerator_StatusChanged;
                this.UpdateSelectedItem();
            }
        }

        /// <summary>
        /// Get the ItemsHolder and generate any children
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this._itemsHolderPanel = GetTemplateChild("PART_ItemsHolder") as Panel;
            this.UpdateSelectedItem();
        }

        /// <summary>
        /// When the items change we remove any generated panel children and add any new ones as necessary
        /// </summary>
        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            if (this._itemsHolderPanel == null)
                return;

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Reset:
                    this._itemsHolderPanel.Children.Clear();
                    break;

                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems != null)
                    {
                        SelectedIndex = Items.Count - 1;
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems != null)
                    {
                        foreach (var item in e.OldItems)
                        {
                            ContentPresenter cp = this.FindChildContentPresenter(item);
                            if (cp != null)
                                this._itemsHolderPanel.Children.Remove(cp);
                        }
                    }

                    // Don't do anything with new items because we don't want to
                    // create visuals that aren't being shown

                    this.UpdateSelectedItem();
                    break;

                case NotifyCollectionChangedAction.Replace:
                    throw new NotImplementedException("Replace not implemented yet");
            }
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            this.UpdateSelectedItem();
        }

        private void UpdateSelectedItem()
        {
            if (this._itemsHolderPanel == null)
                return;

            // Generate a ContentPresenter if necessary
            TabItem item = this.GetSelectedTabItem();
            if (item != null && this.FindChildContentPresenter(item) == null)
                this.CreateChildContentPresenter(item);

            // show the right child
            foreach (ContentPresenter child in this._itemsHolderPanel.Children)
            {
                TabItem tabItem = child.Tag as TabItem;
                child.Visibility = tabItem != null && tabItem.IsSelected ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void CreateChildContentPresenter(object item)
        {
            if (item == null)
                return;
            ContentPresenter cp = new ContentPresenter();
            cp.Content = item is TabItem ? ((TabItem) item).Content : item;
            cp.ContentTemplate = SelectedContentTemplate;
            cp.ContentTemplateSelector = SelectedContentTemplateSelector;
            cp.ContentStringFormat = SelectedContentStringFormat;
            cp.Visibility = Visibility.Collapsed;
            cp.Tag = item is TabItem ? item : ItemContainerGenerator.ContainerFromItem(item);
            this._itemsHolderPanel.Children.Add(cp);
        }

        private ContentPresenter FindChildContentPresenter(object data)
        {
            if (data is TabItem)
                data = (data as TabItem).Content;

            if (data == null)
                return null;

            return this._itemsHolderPanel?.Children
                .Cast<ContentPresenter>()
                .FirstOrDefault(cp => cp.Content == data);
        }

        protected TabItem GetSelectedTabItem()
        {
            object selectedItem = SelectedItem;
            if (selectedItem == null)
                return null;

            return selectedItem as TabItem ?? ItemContainerGenerator.ContainerFromIndex(SelectedIndex) as TabItem;
        }
    }
}
