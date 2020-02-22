using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Unicon2.Fragments.Programming.Infrastructure;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;

namespace Unicon2.Fragments.Programming
{
    internal class PathFinder
    {
        /// <summary>
        /// Получает путь соединяющий 2 вывода
        /// </summary>
        /// <param name="source">Вывод-источник</param>
        /// <param name="sink">Целевой вывод</param>
        internal static List<Point> GetConnectionLine(IConnectorViewModel source, IConnectorViewModel sink)
        {
            List<Point> linePoints = new List<Point>();

            Rect sourceRect = GetElementRect(source);
            Rect sinkRect = GetElementRect(sink);

            linePoints.Add(source.ConnectorPoint);

            if (!sinkRect.Contains(source.ConnectorPoint) && !sourceRect.Contains(sink.ConnectorPoint))
            {
                AddOtherPoints(linePoints, source, sink, sourceRect, sinkRect);
            }
            else
            {
                linePoints.Add(sink.ConnectorPoint);
                OptimizeLine(linePoints, sourceRect);
            }

            return linePoints;
        }

        private static void AddOtherPoints(List<Point> linePoints, IConnectorViewModel source, IConnectorViewModel sink, Rect sourceRect, Rect sinkRect)
        {
            if (IsPointVisible(source, sink.ConnectorPoint, sourceRect))
            {
                linePoints.Add(sink.ConnectorPoint);
                OptimizeLine(linePoints, sourceRect);
                return;
            }
            if (TargetIsNotVisible(sourceRect, sinkRect, sink)) // когда нужно огибать элементы
            {
                AddLineAround(linePoints, source, sink, sourceRect, sinkRect);
            }
            else // когда целевой элемент над или под источником, при этом есть место для линии между ними
            {
                OptimizeLine(linePoints, source, sink, sourceRect, sinkRect);
            }
        }

        /// <summary>
        /// Добавление точек вокруг элементов для исключения пересечения элементов линией связи
        /// </summary>
        private static void AddLineAround(List<Point> linePoints, IConnectorViewModel source, IConnectorViewModel sink, Rect sourceRect, Rect sinkRect)
        {
            // ближайшая угловая точка рамки цели
            Point nearestSinkPoint = GetNearestNeighborSink(sink, sinkRect, source.ConnectorPoint);
            // ближе к верху
            if (Math.Abs(nearestSinkPoint.Y - sinkRect.Top) < 0.0001)
            {
                // верхние точки рамки источника
                Point sourceNeigbor = source.Model.Orientation == ConnectorOrientation.LEFT
                    ? sourceRect.TopLeft
                    : sourceRect.TopRight;
                Point sourceOpposite = source.Model.Orientation == ConnectorOrientation.LEFT
                    ? sourceRect.TopRight
                    : sourceRect.TopLeft;
                linePoints.Add(new Point(sourceNeigbor.X, source.ConnectorPoint.Y));

                if (nearestSinkPoint.Y <= sourceRect.Top)
                {
                    linePoints.Add(new Point(sourceNeigbor.X, nearestSinkPoint.Y));
                    linePoints.Add(nearestSinkPoint);
                    linePoints.Add(new Point(nearestSinkPoint.X, sink.ConnectorPoint.Y));
                }
                else
                {
                    linePoints.Add(sourceNeigbor);
                    if (sourceOpposite.X <= nearestSinkPoint.X)
                    {
                        linePoints.Add(sourceOpposite);
                        linePoints.Add(new Point(sourceOpposite.X, sink.ConnectorPoint.Y));
                    }
                    else
                    {
                        linePoints.Add(new Point(nearestSinkPoint.X, sourceOpposite.Y));
                        linePoints.Add(new Point(nearestSinkPoint.X, sink.ConnectorPoint.Y));
                    }
                }
            }
            else // ближе к низу
            {
                // Нижние точки рамки источника
                Point sourceNeigbor = source.Model.Orientation == ConnectorOrientation.LEFT
                    ? sourceRect.BottomLeft
                    : sourceRect.BottomRight;
                Point sourceOpposite = source.Model.Orientation == ConnectorOrientation.LEFT
                    ? sourceRect.BottomRight
                    : sourceRect.BottomLeft;
                linePoints.Add(new Point(sourceNeigbor.X, source.ConnectorPoint.Y));
                if (sourceNeigbor.Y >= nearestSinkPoint.Y)
                {
                    linePoints.Add(sourceNeigbor);
                    // RIGHT INPUT
                    if (nearestSinkPoint.X >= sourceOpposite.X)
                    {
                        linePoints.Add(sourceOpposite);
                        linePoints.Add(new Point(sourceOpposite.X, sink.ConnectorPoint.Y));
                    }
                    else
                    {
                        linePoints.Add(new Point(nearestSinkPoint.X, sourceOpposite.Y));
                        linePoints.Add(new Point(nearestSinkPoint.X, sink.ConnectorPoint.Y));
                    }
                }
                else
                {
                    linePoints.Add(new Point(sourceNeigbor.X, nearestSinkPoint.Y));
                    linePoints.Add(nearestSinkPoint);
                    linePoints.Add(new Point(nearestSinkPoint.X, sink.ConnectorPoint.Y));
                }
            }
            linePoints.Add(sink.ConnectorPoint);
        }

        internal static List<Point> GetConnectionLine(IConnectorViewModel source, Point sinkPoint, ConnectorOrientation preferredOrientation)
        {
            List<Point> linePoints = new List<Point>();
            Rect sourceRect = GetElementRect(source);
            Point startPoint = source.ConnectorPoint;
            linePoints.Add(startPoint);
            if (!sourceRect.Contains(sinkPoint))
            {
                AddOtherPoints(linePoints, source, sinkPoint, sourceRect);
            }
            else
            {
                linePoints.Add(sinkPoint);
            }
            // задание изломов линии на сегменты под 90 градусов
            OptimizeLine(linePoints, sourceRect);
            return linePoints;
        }

        // При необходимости, добавляем дополнительные точки, чтобы линия не пересекала элемент
        private static void AddOtherPoints(List<Point> linePoints, IConnectorViewModel source, Point sinkPoint, Rect sourceRect)
        {
            if (IsPointVisible(source, sinkPoint, sourceRect))
            {
                linePoints.Add(sinkPoint);
                return;
            }

            Point anglePoint = GetNearestNeighborSource(source, sinkPoint, sourceRect);
            linePoints.Add(new Point(anglePoint.X, source.ConnectorPoint.Y));
            linePoints.Add(anglePoint);
            linePoints.Add(sinkPoint);
        }

        /// <summary>
        /// Добавление изломов в линии
        /// </summary>
        /// <param name="linePoints">Начальные точки линии</param>
        /// <param name="sourceRect">Рамка элемента-источника</param>
        private static void OptimizeLine(List<Point> linePoints, Rect sourceRect)
        {
            Point endPoint = linePoints.Last();
            Point previous = linePoints[linePoints.Count - 2];
            // если точки расположены на одной оси координат, то нет изломов
            if (Math.Abs(previous.X - endPoint.X) < 0.0001 || Math.Abs(previous.Y - endPoint.Y) < 0.0001) return;
            // если 2 точки не на одной линии, надо сделать изломы под 90 градусов
            if (linePoints.Count == 2)
            {
                double middle = linePoints[1].X > linePoints[0].X
                    ? linePoints[0].X + (linePoints[1].X - linePoints[0].X) / 2
                    : linePoints[1].X + (linePoints[0].X - linePoints[1].X) / 2;
                Point subP1 = new Point(middle, linePoints[0].Y);
                Point subP2 = new Point(middle, linePoints[1].Y);
                linePoints.Insert(linePoints.Count - 1, subP1);
                linePoints.Insert(linePoints.Count - 1, subP2);
                return;
            }
            double middleRectY = sourceRect.Top + sourceRect.Height / 2;
            // проверка на то,что конечная точка находится позади элемента
            bool s = (endPoint.Y > previous.Y && endPoint.Y <= middleRectY) ||
                     (endPoint.Y >= middleRectY && endPoint.Y < previous.Y);
            if (s)  // точка находится позади
            {
                Point p = new Point(endPoint.X, previous.Y);
                linePoints.Insert(linePoints.Count - 1, p);
            }
            else    // точка находится сзади-сверху или сзади-снизу
            {
                double middle = previous.Y > endPoint.Y
                    ? endPoint.Y + (previous.Y - endPoint.Y) / 2
                    : previous.Y + (endPoint.Y - previous.Y) / 2;
                Point subP1 = new Point(previous.X, middle);
                Point subP2 = new Point(endPoint.X, middle);
                linePoints.Insert(linePoints.Count - 1, subP1);
                linePoints.Insert(linePoints.Count - 1, subP2);
            }
        }

        private static void OptimizeLine(List<Point> linePoints, IConnectorViewModel source, IConnectorViewModel sink, Rect sourceRect, Rect sinkRect)
        {
            if (source.ConnectorPoint.Y > sink.ConnectorPoint.Y)
            {
                double middleY = sink.ConnectorPoint.Y + (source.ConnectorPoint.Y - sink.ConnectorPoint.Y) / 2;
                Point rectAngle = source.Model.Orientation == ConnectorOrientation.LEFT
                    ? sourceRect.TopLeft
                    : sourceRect.TopRight;
                linePoints.Add(new Point(rectAngle.X, source.ConnectorPoint.Y));
                linePoints.Add(new Point(rectAngle.X, middleY));
                linePoints.Add(new Point(sink.ConnectorPoint.X, middleY));
            }
            else
            {
                Point rectAngle = sink.Model.Orientation == ConnectorOrientation.LEFT
                    ? sinkRect.TopLeft
                    : sinkRect.TopRight;
                if (sink.Model.Orientation == ConnectorOrientation.LEFT)
                {
                    if (rectAngle.X < source.ConnectorPoint.X)
                    {
                        linePoints.Add(new Point(source.ConnectorPoint.X, rectAngle.Y));
                        linePoints.Add(rectAngle);
                        linePoints.Add(new Point(rectAngle.X, sink.ConnectorPoint.Y));
                    }
                    else
                    {
                        double middleX = source.ConnectorPoint.X + (rectAngle.X - source.ConnectorPoint.X) / 2;
                        linePoints.Add(new Point(middleX, source.ConnectorPoint.Y));
                        linePoints.Add(new Point(middleX, sink.ConnectorPoint.Y));
                    }
                }
                else
                {
                    double middleY = sourceRect.Bottom + (sinkRect.Top - sourceRect.Bottom) / 2;
                    linePoints.Add(new Point(source.ConnectorPoint.X, middleY));
                    linePoints.Add(new Point(rectAngle.X, middleY));
                    linePoints.Add(new Point(rectAngle.X, sink.ConnectorPoint.Y));
                }
            }
            linePoints.Add(sink.ConnectorPoint);
        }

        /// <summary>
        /// Проверка на то, видна ли точка цели "напрямую"
        /// </summary>
        /// <param name="source">Источник с точкой</param>
        /// <param name="sinkPoint">Конечная точка</param>
        /// <param name="sourceRect">Рамка источника</param>
        private static bool IsPointVisible(IConnectorViewModel source, Point sinkPoint, Rect sourceRect)
        {
            return source.Model.Orientation == ConnectorOrientation.LEFT
                ? sourceRect.Left > sinkPoint.X
                : sourceRect.Right < sinkPoint.X;
        }

        /// <summary>
        /// Требуется ли огибать линию по рамке элемента
        /// </summary>
        /// <param name="sourceRect">Рамка элемента-источника</param>
        /// <param name="sinkRect">Рамка целевого элемента</param>
        /// <param name="sink">VM целевого элемента</param>
        /// <returns></returns>
        private static bool TargetIsNotVisible(Rect sourceRect, Rect sinkRect, IConnectorViewModel sink)
        {
            return sourceRect.IntersectsWith(sinkRect)
                || (sourceRect.Bottom >= sinkRect.Top && sourceRect.Bottom <= sinkRect.Bottom)
                || (sourceRect.Top <= sinkRect.Bottom && sourceRect.Top >= sinkRect.Top)
                || (sink.ConnectorPoint.Y >= sourceRect.Top && sink.ConnectorPoint.Y <= sourceRect.Bottom);
        }

        /// <summary>
        /// Получение вспомогательной точки на границе элемента, если линия уходит за сам элемент.
        /// Исключает пересечение границ элемента линией свзяи, если соединяемые элементы расположены
        /// на достаточном расстоянии друг от друга.
        /// </summary>
        /// <param name="source">Источник связи</param>
        /// <param name="endPoint">Точка конца линии связи</param>
        /// <param name="rectSource">Рамка, охватывающая элемент-источник</param>
        private static Point GetNearestNeighborSource(IConnectorViewModel source, Point endPoint, Rect rectSource)
        {
            Point n1, n2; // точки углов рамки
            GetNeighborCorners(source.Model.Orientation, rectSource, out n1, out n2);
            return Point.Subtract(n1, endPoint).Length <= Point.Subtract(n2, endPoint).Length ? n1 : n2;
        }

        /// <summary>
        /// Ближайшая угловая точка к источнику на рамке, охватывающей целевой элемент
        /// </summary>
        /// <param name="sink">Целевой элемент</param>
        /// <param name="sinkRect">Рамка, охватывающая целевой элемент</param>
        /// <param name="sourcePoint">Точка источника</param>
        private static Point GetNearestNeighborSink(IConnectorViewModel sink, Rect sinkRect, Point sourcePoint)
        {
            Point n1, n2; // точки углов рамки
            GetNeighborCorners(sink.Model.Orientation, sinkRect, out n1, out n2);
            return Point.Subtract(n1, sourcePoint).Length <= Point.Subtract(n2, sourcePoint).Length ? n1 : n2;
        }

        /// <summary>
        /// Получить точки углов охватывающей элемент рамки
        /// </summary>
        /// <param name="orientation">Левосторонный или правосторонний вывод</param>
        /// <param name="rect">Рамка, охватывающая элемент</param>
        /// <param name="n1">Точка угла 1</param>
        /// <param name="n2">Точка угла 2</param>
        private static void GetNeighborCorners(ConnectorOrientation orientation, Rect rect, out Point n1, out Point n2)
        {
            switch (orientation)
            {
                case ConnectorOrientation.LEFT:
                    n1 = rect.TopLeft; n2 = rect.BottomLeft;
                    break;
                case ConnectorOrientation.RIGHT:
                    n1 = rect.TopRight; n2 = rect.BottomRight;
                    break;
                default:
                    n1 = rect.TopRight; n2 = rect.BottomRight;
                    break;
            }
        }
        /// <summary>
        /// Получает прямоугольную область,которая охватывает элемент с его выводом-источником связи
        /// </summary>
        /// <param name="source">VM источника связи</param>
        /// <returns>Охватываемая область</returns>
        private static Rect GetElementRect(IConnectorViewModel source)
        {
            //TODO Get real size of ContentPresenter
            Rect rect = new Rect(source.ParentViewModel.X, source.ParentViewModel.Y, 30, 30);//source.ParentViewModel.Width, source.ParentViewModel.Height);
            if (rect.Contains(source.ConnectorPoint))
            {
                rect.Inflate(10, 10);
            }
            else
            {
                double margin = source.Model.Orientation == ConnectorOrientation.RIGHT
                    ? source.ConnectorPoint.X - rect.Right
                    : rect.Left - source.ConnectorPoint.X;
                rect.Inflate(margin, margin);
            }
            return rect;
        }
    }
}
