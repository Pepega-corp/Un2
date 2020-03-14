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
    public class ConnectionAdorner : Adorner
    {
        private Canvas _designerCanvas;
        private ConnectionViewModel _connection;
        private Pen _drawingPen; // перо мнимой линии
        private ConnectorViewModel _hitConnector;
        private PathGeometry _currentPath;

        public ConnectionAdorner(Canvas designerCanvas, ConnectionViewModel connection) : base(designerCanvas)
        {
            this._designerCanvas = designerCanvas;
            this._connection = connection;
            this._connection.StrokeDashArray = new DoubleCollection(new double[] { 4, 2 });
            this._drawingPen = new Pen(Brushes.LightSlateGray, 1);
            this._drawingPen.LineJoin = PenLineJoin.Round;
            this._drawingPen.DashStyle = DashStyles.Solid;

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

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            dc.DrawGeometry(null, this._drawingPen, this._currentPath);
            dc.DrawRectangle(Brushes.Transparent, null, new Rect(RenderSize));
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (this._hitConnector != null)
            {
                //this._connection.Sink = this._hitConnector;
                this._hitConnector.IsDragConnection = false;
            }

            if (IsMouseCaptured) ReleaseMouseCapture();
            var adornerLayer = AdornerLayer.GetAdornerLayer(this._designerCanvas);
            if (adornerLayer != null)
            {
                adornerLayer.Remove(this);
            }
            this._connection.StrokeDashArray = DashStyles.Solid.Dashes;
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

        private void GetPathGeometry(Point position)
        {
            this._currentPath = this._currentPath ?? new PathGeometry();

            List<Point> pathPoints;

            var sourceInfo = new PathFinder.ConnectorInfo
            {
                //ConnectorPoint = this._connection.Source.ConnectorPoint,
                //Orientation = this._connection.Source.Orientation,
                //ConnectorParentX = this._connection.Source.ParentViewModel.X,
                //ConnectorParentY = this._connection.Source.ParentViewModel.Y
            };

            if (this._hitConnector == null)
            {
                pathPoints = PathFinder.GetConnectionLine(sourceInfo, position, ConnectorOrientation.NONE);
            }
            else
            {
                var hitInfo = new PathFinder.ConnectorInfo
                {
                    ConnectorPoint = this._hitConnector.ConnectorPoint,
                    Orientation = this._hitConnector.Orientation,
                    ConnectorParentX = this._hitConnector.ParentViewModel.X,
                    ConnectorParentY = this._hitConnector.ParentViewModel.Y
                };

                pathPoints = PathFinder.GetConnectionLine(sourceInfo, hitInfo);
            }

            if (pathPoints.Count > 0)
            {
                this._currentPath.Figures.Clear();
                var figure = new PathFigure();
                figure.StartPoint = pathPoints[0];
                pathPoints.Remove(pathPoints[0]);
                figure.Segments.Add(new PolyLineSegment(pathPoints, true));
                this._currentPath.Figures.Add(figure);
            }
        }

        private void HitTesting(Point hitPoint)
        {
            var hitObject = this._designerCanvas.InputHitTest(hitPoint) as DependencyObject;
            if (hitObject == null || !(hitObject is Border || hitObject is Ellipse))
            {
                this.HitConnector = null;
                return;
            }
            var fe = (FrameworkElement)hitObject;
            var cvm = fe.DataContext as ConnectorViewModel;
            if (cvm == null /*|| cvm.Model.Orientation == this._connection.Source.Orientation*/)
            {
                this.HitConnector = null;
                return;
            }
            //нахождение точки присоединения элементу
            var itemRect = VisualTreeHelper.GetDescendantBounds(fe);
            var itemBounds =
                fe.TransformToAncestor(this._designerCanvas).TransformBounds(itemRect);
            cvm.ConnectorPoint = new Point(itemBounds.X + (itemBounds.Right - itemBounds.X) / 2, itemBounds.Y + (itemBounds.Bottom - itemBounds.Y) / 2);
            this.HitConnector = cvm;
        }
    }
}
