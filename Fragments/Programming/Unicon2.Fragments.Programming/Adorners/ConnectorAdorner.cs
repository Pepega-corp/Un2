using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Unicon2.Fragments.Programming.Infrastructure;
using Unicon2.Fragments.Programming.Model;
using Unicon2.Fragments.Programming.ViewModels;

namespace Unicon2.Fragments.Programming.Adorners
{
    public class ConnectorAdorner : Adorner
    {
        private SchemeTabViewModel _schemeTabViewModel;
        private PathGeometry _pathGeometry;
        private readonly Canvas _designerCanvas;
        private readonly Pen _drawingPen;
        private readonly ConnectorViewModel _sourceConnector;
        private ConnectorViewModel _hitConnector;

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
            this._pathGeometry = new PathGeometry();
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
                //TODO сделать проверку на наличие уже существующей связи и добавлять выводы к ней
                if (this._sourceConnector.Orientation == ConnectorOrientation.RIGHT && this._sourceConnector.Model.ConnectionNumber != -1
                    || this._hitConnector.Orientation == ConnectorOrientation.RIGHT && this._hitConnector.Model.ConnectionNumber != -1)
                {
                    var newConnection = new Connection();
                    newConnection.AddConnector(this._sourceConnector.Model);
                    newConnection.AddConnector(this._hitConnector.Model);
                    newConnection.Path = this._pathGeometry;
                    var connectionViewModel = new ConnectionViewModel(newConnection);

                    this._schemeTabViewModel.AddConnectionToProgramm(connectionViewModel);
                    this._schemeTabViewModel.ElementCollection.Add(connectionViewModel);
                }
                else
                {
                    
                }

                this._hitConnector.IsDragConnection = false;
            }

            if (IsMouseCaptured)
                ReleaseMouseCapture();

            var adornerLayer = AdornerLayer.GetAdornerLayer(this._designerCanvas);
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
            List<Point> pathPoints;

            var sourceInfo = new PathFinder.ConnectorInfo
            {
                ConnectorPoint = this._sourceConnector.ConnectorPoint,
                Orientation = this._sourceConnector.Orientation,
                ConnectorParentX = this._sourceConnector.ParentViewModel.X,
                ConnectorParentY = this._sourceConnector.ParentViewModel.Y
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
                this._pathGeometry.Figures.Clear();
                var figure = new PathFigure();
                figure.StartPoint = pathPoints[0];
                pathPoints.Remove(pathPoints[0]);
                figure.Segments.Add(new PolyLineSegment(pathPoints, true));
                this._pathGeometry.Figures.Add(figure);
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
            if (fe.Visibility != Visibility.Visible)
            {
                this.HitConnector = null;
                return;
            }
            var cvm = fe.DataContext as ConnectorViewModel;
            if (cvm == null || cvm.Model.Orientation == this._sourceConnector.Model.Orientation)
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
