using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interactivity;
using Unicon2.Fragments.Programming.Adorners;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;
using Unicon2.Fragments.Programming.ViewModels;

namespace Unicon2.Fragments.Programming.Behaviors
{
    public class ConnectorBehavior : Behavior<Border>
    {
        private bool _isDrag;
        private Canvas _designerCanvas;
        private SchemeTabViewModel _tabViewModel;
        private ConnectorViewModel _connectorViewModel;

        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject.Visibility == Visibility.Collapsed) return;

            this._isDrag = false;
            AssociatedObject.MouseLeftButtonDown += this.OnPreviewMouseLeftButtonDown;
            AssociatedObject.MouseMove += this.OnMouseMove;
            AssociatedObject.MouseLeftButtonUp += this.OnMouseLeftButtonUp;
            this._connectorViewModel = AssociatedObject.DataContext as ConnectorViewModel;
            //if (this._connectorViewModel != null && this._connectorViewModel.SelfBehavior == null)
            //    this._connectorViewModel.SetBehavior(this);

            this._designerCanvas = CommonHelper.GetDesignerCanvas(AssociatedObject);
            if (this._designerCanvas != null)
            {
                this._tabViewModel = this._designerCanvas.DataContext as SchemeTabViewModel;
                this.GetConnectorPoint();
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PreviewMouseLeftButtonDown -= this.OnPreviewMouseLeftButtonDown;
            AssociatedObject.MouseMove -= this.OnMouseMove;
            AssociatedObject.MouseLeftButtonUp -= this.OnMouseLeftButtonUp;
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            this._isDrag = false;
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!this._isDrag || e.LeftButton != MouseButtonState.Pressed) return;
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this._designerCanvas);
            if (adornerLayer != null)
            {
                ConnectorAdorner connectorAdorner = new ConnectorAdorner(this._designerCanvas, this._connectorViewModel);
                adornerLayer.Add(connectorAdorner);
                e.Handled = true;
            }
        }

        private void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this._tabViewModel != null)
            {
                foreach (ILogicElementViewModel model in this._tabViewModel.ElementCollection)
                {
                    model.IsSelected = false;
                }
            }
            this._isDrag = true;
            this.GetConnectorPoint();
            e.Handled = true;
        }
        
        // Получение позиции точки начала линии относительно Canvas, в котором расположены все элементы
        public void GetConnectorPoint()
        {
            if(this._designerCanvas == null)return;
            this._connectorViewModel.ConnectorPoint =
                AssociatedObject.TransformToAncestor(this._designerCanvas)
                    .Transform(new Point(AssociatedObject.Width / 2, AssociatedObject.Height / 2));
        }
        
    }
}
