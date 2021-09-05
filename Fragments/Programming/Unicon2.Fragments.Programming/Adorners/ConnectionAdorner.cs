using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Unicon2.Fragments.Programming.Behaviors;
using Unicon2.Fragments.Programming.Infrastructure;
using Unicon2.Fragments.Programming.Other;
using Unicon2.Fragments.Programming.ViewModels;

namespace Unicon2.Fragments.Programming.Adorners
{
    public class ConnectionAdorner : Adorner
    {
        private Canvas _designerCanvas;
        private ConnectionBehavior _behavior;
        private ConnectionViewModel _connection;
        private ConnectorViewModel _connector;
        private Pen _drawingPen; // перо мнимой линии
        private ConnectorViewModel _hitConnector;
        private PathGeometry _currentPath;

        public ConnectionAdorner(Canvas designerCanvas, ConnectionBehavior connectionBehavior) : base(designerCanvas)
        {
            _designerCanvas = designerCanvas;

            _behavior = connectionBehavior;

            _connector = _behavior.Connector;
            _connection = _connector.Connection;

            _drawingPen = new Pen(Brushes.LightSlateGray, 1);
            _drawingPen.LineJoin = PenLineJoin.Round;
            _drawingPen.DashStyle = DashStyles.Solid;

            Cursor = Cursors.Cross;
        }

        private ConnectorViewModel HitConnector
        {
            set
            {
                if (_hitConnector != null)
                {
                    _hitConnector.IsDragConnection = false;
                }
                _hitConnector = value;
                if (_hitConnector != null)
                {
                    _hitConnector.IsDragConnection = true;
                }
            }
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            dc.DrawGeometry(null, _drawingPen, _currentPath);
            dc.DrawRectangle(Brushes.Transparent, null, new Rect(RenderSize));
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (_hitConnector != null)
            {
                // _connection.SinkConnectors.Remove(_connector);
                // _connection.SinkConnectors.Add(_hitConnector);

                _behavior.UpdateConnector(_hitConnector);

                _hitConnector.IsDragConnection = false;
            }

            if (IsMouseCaptured) 
                ReleaseMouseCapture();

            var adornerLayer = AdornerLayer.GetAdornerLayer(_designerCanvas);
            if (adornerLayer != null)
            {
                adornerLayer.Remove(this);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (!IsMouseCaptured) 
                    CaptureMouse();

                HitTesting(e.GetPosition(this));
                GetPathGeometry(e.GetPosition(this));
                InvalidateVisual();
            }
            else
            {
                if (IsMouseCaptured) ReleaseMouseCapture();
            }
        }

        private void GetPathGeometry(Point position)
        {
            _currentPath = _currentPath ?? new PathGeometry();

            List<Point> pathPoints;
            var sourceConnector = (ConnectorViewModel)_connection.Segments.First(c => c.IsSource).Point1;
            var sourceInfo = new PathFinder.ConnectorInfo
            {
                ConnectorPoint = sourceConnector.ConnectorPosition,
                Orientation = sourceConnector.Orientation,
                ConnectorParentX = sourceConnector.ParentViewModel.X,
                ConnectorParentY = sourceConnector.ParentViewModel.Y
            };

            if (_hitConnector == null)
            {
                pathPoints = PathFinder.GetConnectionLine(sourceInfo, position, ConnectorOrientation.NONE);
            }
            else
            {
                var hitInfo = new PathFinder.ConnectorInfo
                {
                    ConnectorPoint = _hitConnector.ConnectorPosition,
                    Orientation = _hitConnector.Orientation,
                    ConnectorParentX = _hitConnector.ParentViewModel.X,
                    ConnectorParentY = _hitConnector.ParentViewModel.Y
                };

                pathPoints = PathFinder.GetConnectionLine(sourceInfo, hitInfo);
            }

            if (pathPoints.Count > 0)
            {
                _currentPath.Figures.Clear();
                var figure = new PathFigure { StartPoint = pathPoints[0] };
                pathPoints.Remove(pathPoints[0]);
                figure.Segments.Add(new PolyLineSegment(pathPoints, true));
                _currentPath.Figures.Add(figure);
            }
        }

        private void HitTesting(Point hitPoint)
        {
            var hitObject = _designerCanvas.InputHitTest(hitPoint) as DependencyObject;
            if (hitObject == null || !(hitObject is Border || hitObject is Ellipse))
            {
                HitConnector = null;
                return;
            }
            var fe = (FrameworkElement)hitObject;
            var cvm = fe.DataContext as ConnectorViewModel;
            var connectionSource = _connection.SourceSegment.Point1 as ConnectorViewModel;
            if (cvm == null || connectionSource == null || cvm.Orientation == connectionSource.Orientation)
            {
                HitConnector = null;
                return;
            }
            
            HitConnector = cvm;
        }
    }
}
