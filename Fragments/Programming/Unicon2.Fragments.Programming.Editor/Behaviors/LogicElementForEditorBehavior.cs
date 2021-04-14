using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;
using Unicon2.Fragments.Programming.Editor.ViewModel;
using Unicon2.Fragments.Programming.Infrastructure.HelperClasses;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementEditorViewModels;

namespace Unicon2.Fragments.Programming.Editor.Behaviors
{
    public class LogicElementForEditorBehavior : Behavior<ListView>
    {
        private ProgrammingEditorViewModel _programmingEditorViewModel;
        private ListView _listView;
        private bool _dragStart;

        protected override void OnAttached()
        {
            base.OnAttached();
            this._listView = AssociatedObject;
            this._listView.MouseMove += this.OnMouseMove;
            this._listView.MouseLeftButtonDown += this.OnRectClick;
            this._listView.MouseLeftButtonUp += this.OnMouseUp;
            this._programmingEditorViewModel = (ProgrammingEditorViewModel)this._listView.DataContext;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this._listView.MouseMove -= this.OnMouseMove;
            this._listView.MouseLeftButtonDown -= this.OnRectClick;
            this._listView.MouseLeftButtonUp -= this.OnMouseUp;
        }

        private void OnRectClick(object sender, MouseButtonEventArgs e)
        {
            this._dragStart = e.ButtonState == MouseButtonState.Pressed;
            e.Handled = true;
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            this._dragStart = false;
            //TODO do check for cena drop
            e.Handled = true;
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (this._dragStart && e.LeftButton == MouseButtonState.Pressed)
            {
                //ILogicElementEditorViewModel item = (ILogicElementEditorViewModel)this._selectedItem.Clone();
                ILogicElementEditorViewModel item = this._programmingEditorViewModel.SelectedNewLogicElemItem;
                DragEditorElement dragObject = new DragEditorElement(item);
                DragDrop.DoDragDrop(this._listView, dragObject, DragDropEffects.Copy);
                e.Handled = true;
            }
            else
            {
                this._dragStart = false;
            }
        }
    }
}
