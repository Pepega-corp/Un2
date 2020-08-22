using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using Unicon2.Fragments.Programming.Adorners;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;
using Unicon2.Fragments.Programming.Other;
using Unicon2.Fragments.Programming.ViewModels;

namespace Unicon2.Fragments.Programming.Behaviors
{
    // хоть поведение используется для элемента Thumb, 
    // но из него можно получить информацию ContentControl,
    // в котором содержится вся нужная информация о VM
    public class DesignerItemBehavior : Behavior<Thumb>
    {
        private ILogicElementViewModel _currentItemContent;
        private SchemeTabViewModel _tabViewModel;
        private Canvas _designerCanvas;
        private Point? _dragPoint;
        private List<ContentPresenter> _contentPresenters;
        private List<Rect> _thumbRects;

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseMove += this.OnMouseMove;
            AssociatedObject.PreviewMouseDown += this.OnPreviewMouseDown;
            AssociatedObject.MouseDoubleClick += this.OnMouseDoubleClick;
            this._currentItemContent = AssociatedObject.DataContext as ILogicElementViewModel; //VM текущего элемента
            if (this._currentItemContent == null) return;
            this._designerCanvas = CommonHelper.GetDesignerCanvas(AssociatedObject); //Canvas, на котором отображаются элементы
            if (this._designerCanvas != null)
            {
                this._tabViewModel = this._designerCanvas.DataContext as SchemeTabViewModel;// VM текущей вкладки схемы
            }
            this._contentPresenters = new List<ContentPresenter>();
            this._thumbRects = new List<Rect>();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.MouseMove -= this.OnMouseMove;
            AssociatedObject.PreviewMouseLeftButtonDown -= this.OnPreviewMouseDown;
            AssociatedObject.MouseDoubleClick -= this.OnMouseDoubleClick;
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!this._dragPoint.HasValue || e.LeftButton != MouseButtonState.Pressed || this._contentPresenters.Count == 0) return;
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this._designerCanvas);
            if (adornerLayer != null )
            {
                DragItemsAdorner connectorAdorner = new DragItemsAdorner(this._designerCanvas, this._contentPresenters, this._thumbRects, this._dragPoint.Value);
                adornerLayer.Add(connectorAdorner);
                e.Handled = true;
            }
        }

        private void OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right || e.ClickCount != 1)
            {
                this._dragPoint = null;
                return;
            }
            if ((Keyboard.Modifiers & (ModifierKeys.Shift | ModifierKeys.Control)) != ModifierKeys.None)
            {
                this._currentItemContent.IsSelected = !this._currentItemContent.IsSelected;
            }
            else if (!this._currentItemContent.IsSelected)
            {
                foreach (ISelectable item in this._tabViewModel.ElementCollection)
                    item.IsSelected = false;
                this._currentItemContent.IsSelected = true;
            }

            _tabViewModel.OnSelectChanged();
            
            this._dragPoint = e.GetPosition(this._designerCanvas);
            // перетаскивать будем все выделенные контролы относительно того, который тащим мышкой
            this._contentPresenters.Clear();
            this._contentPresenters.AddRange(this._designerCanvas.Children.OfType<ContentPresenter>()
                .Where(cc => cc.Content is ILogicElementViewModel && ((ILogicElementViewModel)cc.Content).IsSelected));
            // проверка для границ проводится будет по Thumb, а не по ContentPresenter,
            // чтобы сам блок и его выводы располагались в точках, кратных 5
            this._thumbRects.Clear();
            foreach (ContentPresenter presenter in this._contentPresenters)
            {
                Thumb thumb = CommonHelper.GetThumbOfPresenter(presenter);
                Rect boundsRect = VisualTreeHelper.GetDescendantBounds(thumb);
                Rect thumbRect = thumb.TransformToAncestor(this._designerCanvas).TransformBounds(boundsRect);
                this._thumbRects.Add(thumbRect);
            }

            e.Handled = true;
        }

        private void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left) return;
            this._currentItemContent.OpenPropertyWindow();
            e.Handled = true;
        }
    }
}
