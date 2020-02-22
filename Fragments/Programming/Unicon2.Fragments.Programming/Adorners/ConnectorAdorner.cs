using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Unicon2.Fragments.Programming.Infrastructure;
using Unicon2.Fragments.Programming.ViewModels;

namespace Unicon2.Fragments.Programming.Adorners
{
    public class ConnectorAdorner : Adorner
    {
        private SchemeTabViewModel _schemeTabViewModel;
        private PathGeometry _pathGeometry;
        private readonly Canvas _designerCanvas;
        private readonly ConnectorViewModel _sourceConnector;
        private ConnectorViewModel _hitConnector;
        private readonly Pen _drawingPen;

        public ConnectorAdorner(Canvas designer, ConnectorViewModel sourceConnectorViewModel) : base(designer)
        {
            this._designerCanvas = designer;
            this._schemeTabViewModel = this._designerCanvas.DataContext as SchemeTabViewModel;
            this._sourceConnector = sourceConnectorViewModel;
            this._drawingPen = new Pen(Brushes.LightSlateGray, 1)
            {
                LineJoin = PenLineJoin.Round,
                DashStyle = new DashStyle(new List<double> {8, 3}, 0)
            };

            Cursor = Cursors.Cross;
        }

        private ConnectorViewModel HitConnector
        {
            set
            {
                if (this._hitConnector != null)
                {
                    this._hitConnector.IsDragConnection = false;
                }
                this._hitConnector = value;
                if (this._hitConnector != null)
                {
                    this._hitConnector.IsDragConnection = true;
                }
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (this._hitConnector != null)
            {
                ConnectionViewModel connectionViewModel =
                    this._sourceConnector.Model.Orientation == ConnectorOrientation.RIGHT
                    ? new ConnectionViewModel(this._sourceConnector, this._hitConnector, this._pathGeometry)
                    : new ConnectionViewModel(this._hitConnector, this._sourceConnector, this._pathGeometry);
                ConnectionViewModel.AddNewConnectionNumber(connectionViewModel);

                this._schemeTabViewModel.ElementCollection.Add(connectionViewModel);

                this._hitConnector.IsDragConnection = false;
            }

            if (IsMouseCaptured)
                ReleaseMouseCapture();

            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this._designerCanvas);
            adornerLayer?.Remove(this);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (!IsMouseCaptured) CaptureMouse();
                this.HitTesting(e.GetPosition(this));
                this.GetPathGeometry(e.GetPosition(this));
                InvalidateVisual();
            }
            else
            {
                if (IsMouseCaptured) ReleaseMouseCapture();
            }
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            dc.DrawGeometry(null, this._drawingPen, this._pathGeometry);
            dc.DrawRectangle(Brushes.Transparent, null, new Rect(RenderSize));
        }

        private void GetPathGeometry(Point position)
        {
            this._pathGeometry = this._pathGeometry ?? new PathGeometry();

            List<Point> pathPoints = this._hitConnector == null
                ? PathFinder.GetConnectionLine(this._sourceConnector, position, ConnectorOrientation.NONE)
                : PathFinder.GetConnectionLine(this._sourceConnector, this._hitConnector);

            if (pathPoints.Count > 0)
            {
                this._pathGeometry.Figures.Clear();
                PathFigure figure = new PathFigure();
                figure.StartPoint = pathPoints[0];
                pathPoints.Remove(pathPoints[0]);
                figure.Segments.Add(new PolyLineSegment(pathPoints, true));
                this._pathGeometry.Figures.Add(figure);
            }
        }

        private void HitTesting(Point hitPoint)
        {
            DependencyObject hitObject = this._designerCanvas.InputHitTest(hitPoint) as DependencyObject;
            if (hitObject == null || !(hitObject is Border || hitObject is Ellipse))
            {
                this.HitConnector = null;
                return;
            }
            FrameworkElement fe = (FrameworkElement)hitObject;
            if (fe.Visibility != Visibility.Visible)
            {
                this.HitConnector = null;
                return;
            }
            ConnectorViewModel cvm = fe.DataContext as ConnectorViewModel;
            if (cvm == null || cvm.Model.Orientation == this._sourceConnector.Model.Orientation)
            {
                this.HitConnector = null;
                return;
            }
            //нахождение точки присоединения элементу
            Rect itemRect = VisualTreeHelper.GetDescendantBounds(fe);
            Rect itemBounds =
                fe.TransformToAncestor(this._designerCanvas).TransformBounds(itemRect);
            cvm.ConnectorPoint = new Point(itemBounds.X + (itemBounds.Right - itemBounds.X) / 2, itemBounds.Y + (itemBounds.Bottom - itemBounds.Y) / 2);
            this.HitConnector = cvm;
        }
    }
}
