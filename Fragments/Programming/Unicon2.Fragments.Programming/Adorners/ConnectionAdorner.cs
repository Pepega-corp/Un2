﻿using System.Collections.Generic;
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
                this._connection.Sink = this._hitConnector;
                this._hitConnector.IsDragConnection = false;
            }

            if (IsMouseCaptured) ReleaseMouseCapture();
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this._designerCanvas);
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

            List<Point> pathPoints = this._hitConnector == null
                ? PathFinder.GetConnectionLine(this._connection.Source, position, ConnectorOrientation.NONE)
                : PathFinder.GetConnectionLine(this._connection.Source, this._hitConnector);

            if (pathPoints.Count > 0)
            {
                this._currentPath.Figures.Clear();
                PathFigure figure = new PathFigure();
                figure.StartPoint = pathPoints[0];
                pathPoints.Remove(pathPoints[0]);
                figure.Segments.Add(new PolyLineSegment(pathPoints, true));
                this._currentPath.Figures.Add(figure);
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
            ConnectorViewModel cvm = fe.DataContext as ConnectorViewModel;
            if (cvm == null || cvm.Connector.Orientation == this._connection.Source.Connector.Orientation)
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