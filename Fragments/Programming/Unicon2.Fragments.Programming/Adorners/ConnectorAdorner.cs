using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Unicon2.Fragments.Programming.Infrastructure;
using Unicon2.Fragments.Programming.Model;
using Unicon2.Fragments.Programming.Other;
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
        private List<Point> _pathPoints;

        public ConnectorAdorner(Canvas designer, ConnectorViewModel sourceConnectorViewModel) : base(designer)
        {
            this._designerCanvas = designer;
            this._schemeTabViewModel = this._designerCanvas.DataContext as SchemeTabViewModel;
            this._sourceConnector = sourceConnectorViewModel;
            this._drawingPen = new Pen(Brushes.LightSlateGray, 1)
            {
                LineJoin = PenLineJoin.Round,
                DashStyle = new DashStyle(new List<double> { 8, 3 }, 0)
            };

            Cursor = Cursors.Cross;
            this._pathGeometry = new PathGeometry();
            _pathPoints = new List<Point>();
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
                ConnectorViewModel rightConnector, leftConnector;
                if (this._sourceConnector.Orientation == ConnectorOrientation.RIGHT)
                {
                    rightConnector = this._sourceConnector;
                    leftConnector = this._hitConnector;
                }
                else
                {
                    rightConnector = this._hitConnector;
                    leftConnector = this._sourceConnector;
                }

                if (rightConnector.Connected)
                {
                    var connectionViewModel =
                        this._schemeTabViewModel.GetConnectionViewModel(rightConnector.ConnectionNumber);
                    // connectionViewModel.SinkConnectors.Add(leftConnector);
                }
                else
                {
                    var pathSegmentPoints = GetSegmentPointsList(rightConnector.Model, leftConnector.Model);
                    var newConnection = new Connection(pathSegmentPoints, this._schemeTabViewModel.GetNextConnectionNumber());
                    var segmentPointsViewModels = CreateSegmentPointsViewModels(pathSegmentPoints, rightConnector, leftConnector);
                    var connectionViewModel = new ConnectionViewModel(newConnection, segmentPointsViewModels);

                    this._schemeTabViewModel.AddConnectionToProgramm(connectionViewModel);
                    this._schemeTabViewModel.ElementCollection.Add(connectionViewModel);
                }

                this._hitConnector.IsDragConnection = false;
            }

            if (IsMouseCaptured)
                ReleaseMouseCapture();

            var adornerLayer = AdornerLayer.GetAdornerLayer(this._designerCanvas);
            adornerLayer?.Remove(this);
        }

        private List<SegmentPoint> GetSegmentPointsList(Connector rightConnector, Connector leftConnector)
        {
            var pathSegmentPoints = new List<SegmentPoint>(_pathPoints.Count);
            pathSegmentPoints.Add(rightConnector);
            for (var i = 1; i < _pathPoints.Count - 1; i++)
            {
                pathSegmentPoints.Add(new SegmentPoint(_pathPoints[i]));
            }

            pathSegmentPoints.Add(leftConnector);
            return pathSegmentPoints;
        }
        
        private List<ConnectionSegmentViewModel> CreateSegmentPointsViewModels(List<SegmentPoint> pathSegmentPoints, 
            ConnectorViewModel rightConnector, ConnectorViewModel leftConnector)
        {
            var segmentPointsViewModels = new List<ConnectionSegmentViewModel>();
            var startSegment = new ConnectionSegmentViewModel(rightConnector, new SegmentPointViewModel(pathSegmentPoints[1]));
            rightConnector.UpdateSegment(startSegment);
            segmentPointsViewModels.Add(startSegment);
            for (var i = 1; i < pathSegmentPoints.Count - 2; i += 2)
            {
                segmentPointsViewModels.Add(new ConnectionSegmentViewModel(new SegmentPointViewModel(pathSegmentPoints[i]), 
                    new SegmentPointViewModel(pathSegmentPoints[i+1])));
            }

            var lastSegment = new ConnectionSegmentViewModel(new SegmentPointViewModel(pathSegmentPoints[pathSegmentPoints.Count - 2]), leftConnector);
            leftConnector.UpdateSegment(lastSegment);
            segmentPointsViewModels.Add(lastSegment);

            return segmentPointsViewModels;
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
            var sourceInfo = new PathFinder.ConnectorInfo
            {
                ConnectorPoint = this._sourceConnector.ConnectorPosition,
                Orientation = this._sourceConnector.Orientation,
                ConnectorParentX = this._sourceConnector.ParentViewModel.X,
                ConnectorParentY = this._sourceConnector.ParentViewModel.Y
            };

            _pathPoints.Clear();

            if (this._hitConnector == null)
            {
                _pathPoints.AddRange(PathFinder.GetConnectionLine(sourceInfo, position, ConnectorOrientation.NONE));
            }
            else
            {
                var hitInfo = new PathFinder.ConnectorInfo
                {
                    ConnectorPoint = this._hitConnector.ConnectorPosition,
                    Orientation = this._hitConnector.Orientation,
                    ConnectorParentX = this._hitConnector.ParentViewModel.X,
                    ConnectorParentY = this._hitConnector.ParentViewModel.Y
                };

                _pathPoints.AddRange(PathFinder.GetConnectionLine(sourceInfo, hitInfo));
            }

            if (_pathPoints.Count > 0)
            {
                var pathFigure = new PathFigure { StartPoint = _pathPoints[0] };
                for (int i = 1; i < _pathPoints.Count; i++)
                {
                    var lineSegment = new LineSegment(_pathPoints[i], true);
                    pathFigure.Segments.Add(lineSegment);
                }

                this._pathGeometry.Figures.Clear();
                this._pathGeometry.Figures.Add(pathFigure);
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

            this.HitConnector = cvm;
        }
    }
}