using System;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Unicon2.Fragments.Programming.Other
{
    [TemplatePart(Name = "PART_ItemsContainer", Type = typeof(Canvas))]
    public class DesignerCanvasItemsControl : ItemsControl
    {
        private Canvas _itemsContainer;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this._itemsContainer = GetTemplateChild("PART_ItemsContainer") as Canvas;
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            if (this._itemsContainer == null)
                return;

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Reset:
                    this._itemsContainer.Children.Clear();
                    break;

                case NotifyCollectionChangedAction.Add:
                    foreach (object item in e.NewItems)
                    {
                        this.CreateChildContentControl(item);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems != null)
                    {
                        foreach (var item in e.OldItems)
                        {
                            ContentControl cp = this.FindChildContentPresenter(item);
                            if (cp != null)
                                this._itemsContainer.Children.Remove(cp);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    throw new NotImplementedException("Replace not implemented yet");
            }
        }

        private void CreateChildContentControl(object item)
        {
            if (item == null)
                return;
            ContentControl cp = new ContentControl();
            cp.Content = item;
            //if (item is BaseElementViewModel)
            //{
            //    Canvas.SetLeft(cp, ((BaseElementViewModel)item).X);
            //    Canvas.SetTop(cp, ((BaseElementViewModel)item).Y);
            //}
            this._itemsContainer.Children.Add(cp);
        }

        private ContentControl FindChildContentPresenter(object data)
        {
            if (data is ContentControl)
                data = ((ContentControl) data).Content;
            if (data == null)
                return null;

            return this._itemsContainer?.Children
                .Cast<ContentControl>()
                .FirstOrDefault(cp => cp.Content == data);
        }
    }
}
