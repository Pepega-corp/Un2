using System.Windows;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;
using System.Windows.Shapes;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;
using Unicon2.Fragments.Programming.Other;

namespace Unicon2.Fragments.Programming.Behaviors
{
    public class ElementsLibraryBehavior : Behavior<Rectangle>
    {
        // вью модель перетаскиваемого объекта
        private ILogicElementViewModel _selectedItem;
        private Rectangle _selectRect;
        private bool _dragStart;

        protected override void OnAttached()
        {
            base.OnAttached();
            this._selectRect = AssociatedObject;
            this._selectRect.MouseMove += this.OnMouseMove;
            this._selectRect.MouseLeftButtonDown += this.OnRectClick;
            this._selectRect.MouseLeftButtonUp += this.OnMouseUp;
            this._selectedItem = (ILogicElementViewModel)this._selectRect.DataContext;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this._selectRect.MouseMove -= this.OnMouseMove;
            this._selectRect.MouseLeftButtonDown -= this.OnRectClick;
            this._selectRect.MouseLeftButtonUp -= this.OnMouseUp;
        }

        private void OnRectClick(object sender, MouseButtonEventArgs e)
        {
            this._dragStart = e.ButtonState == MouseButtonState.Pressed;
            e.Handled = true;
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            this._dragStart = false;
            e.Handled = true;
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (this._dragStart && e.LeftButton == MouseButtonState.Pressed)
            {
                var item = this._selectedItem.Clone();
                var dragObject = new DragElement(item);
                DragDrop.DoDragDrop(this._selectRect, dragObject, DragDropEffects.Copy);
                e.Handled = true;
            }
            else
            {
                this._dragStart = false;
            }
        }
    }
}
