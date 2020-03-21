using System.Linq;
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

            if (AssociatedObject.Visibility == Visibility.Collapsed)
                return;

            this._isDrag = false;
            AssociatedObject.MouseLeftButtonDown += this.OnPreviewMouseLeftButtonDown;
            AssociatedObject.MouseMove += this.OnMouseMove;
            AssociatedObject.MouseLeftButtonUp += this.OnMouseLeftButtonUp;

            this._designerCanvas = CommonHelper.GetDesignerCanvas(AssociatedObject);
            this._tabViewModel = this._designerCanvas.DataContext as SchemeTabViewModel;

            this._connectorViewModel = (ConnectorViewModel)AssociatedObject.DataContext;
            this._connectorViewModel.SetBehaviour(this);
            this.UpdateConnectorPosition();
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
            var adornerLayer = AdornerLayer.GetAdornerLayer(this._designerCanvas);
            if (adornerLayer != null)
            {
                var connectorAdorner = new ConnectorAdorner(this._designerCanvas, this._connectorViewModel);
                adornerLayer.Add(connectorAdorner);
                e.Handled = true;
            }
        }

        private void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this._tabViewModel != null)
            {
                foreach (var schemeElementViewModel in this._tabViewModel.ElementCollection.Where(ec=>ec is ILogicElementViewModel))
                {
                    var viewModel = (ILogicElementViewModel) schemeElementViewModel;
                    viewModel.IsSelected = false;
                }
            }
            this._isDrag = true;
            e.Handled = true;
        }

        public void UpdateConnectorPosition()
        {
            this._connectorViewModel.ConnectorPosition = this.GetConnectorPoint();
        }
        // Получение позиции точки начала линии относительно Canvas, в котором расположены все элементы
        public Point GetConnectorPoint()
        {
            var pointInGlobalCanvas = AssociatedObject.TransformToAncestor(this._designerCanvas)
                .Transform(new Point(AssociatedObject.Width / 2, AssociatedObject.Height / 2));

            return pointInGlobalCanvas;
        }
    }
}
