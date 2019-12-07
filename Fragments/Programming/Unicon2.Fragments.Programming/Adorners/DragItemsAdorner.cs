using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;
using Unicon2.Fragments.Programming.ViewModels;

namespace Unicon2.Fragments.Programming.Adorners
{
    //Украшатель, реализующий перетаскивание с визуальным отображением очертаний переносимых элементов
    public class DragItemsAdorner : Adorner
    {
        private Canvas _designerCanvas;
        private SchemeTabViewModel _tabViewModel;
        private Canvas _dragCanvas;
        private List<ContentPresenter> _contentPresenters;
        private List<Rect> _thumbRects;
        private Point _dragStartPoint;
        private double _leftOffset;
        private double _topOffset;

        public DragItemsAdorner(Canvas designerCanvas, List<ContentPresenter> contentPresenters, List<Rect> thumbRects, Point startPoint) : base(designerCanvas)
        {
            this._designerCanvas = designerCanvas;
            this._tabViewModel = this._designerCanvas.DataContext as SchemeTabViewModel;
            this._contentPresenters = contentPresenters;
            this._thumbRects = thumbRects;
            this._dragStartPoint = startPoint;

            this._dragCanvas = new Canvas
            {
                Height = this._designerCanvas.Height,
                Width = this._designerCanvas.Width,
                Opacity = 0.5
            };

            foreach (Rect rect in this._thumbRects)
            {
                Rectangle rectangle = new Rectangle();
                rectangle.Height = rect.Height;
                rectangle.Width = rect.Width;
                Canvas.SetLeft(rectangle, rect.Left);
                Canvas.SetTop(rectangle, rect.Top);
                rectangle.Stroke = Brushes.Gray;
                rectangle.Fill = Brushes.Transparent;
                this._dragCanvas.Children.Add(rectangle);
            }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            this._dragCanvas.Measure(constraint);
            return this._dragCanvas.DesiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            this._dragCanvas.Arrange(new Rect(finalSize));
            return finalSize;
        }

        protected override Visual GetVisualChild(int index)
        {
            return this._dragCanvas;
        }

        protected override int VisualChildrenCount
        {
            get { return 1; }
        }

        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {
            GeneralTransformGroup result = new GeneralTransformGroup();
            result.Children.Add(base.GetDesiredTransform(transform));
            result.Children.Add(new TranslateTransform(this._leftOffset * this._tabViewModel.Scale,
                this._topOffset * this._tabViewModel.Scale));
            return result;
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            dc.DrawRectangle(Brushes.Transparent, null, new Rect(RenderSize));
        }

        private void UpdatePosition()
        {
            if (Parent is AdornerLayer adornerLayer)
            {
                adornerLayer.Update(AdornedElement);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (!IsMouseCaptured) CaptureMouse();

                this._leftOffset = e.GetPosition(this._designerCanvas).X - this._dragStartPoint.X;//this._dragStartPoint.X - e.GetPosition(this._designerCanvas).X;
                this._topOffset = e.GetPosition(this._designerCanvas).Y - this._dragStartPoint.Y;//this._dragStartPoint.Y - e.GetPosition(this._designerCanvas).Y;
                // если перемещать нечего, то нет смысла проводить последний цикл
                if (Math.Abs(this._leftOffset) < 0.0001 && Math.Abs(this._topOffset) < 0.0001)
                {
                    e.Handled = true;
                    return;
                }

                double minLeft = double.MaxValue;
                double minTop = double.MaxValue;
                double maxRight = 0;
                double maxBottom = 0;
                // определение позиции контрола самого близкого к точке (0,0) Canvas
                foreach (Rect thumbRect in this._thumbRects)
                {
                    minLeft = Math.Min(thumbRect.X, minLeft);
                    minTop = Math.Min(thumbRect.Y, minTop);

                    maxRight = Math.Max(thumbRect.Right, maxRight);
                    maxBottom = Math.Max(thumbRect.Bottom, maxBottom);
                }

                // проверка на то,что перемещаемые элементы не выходят за рамки Canvas по горизонтали
                if (this._leftOffset > 0)
                {
                    if (maxRight + this._leftOffset > this._designerCanvas.Width)
                    {
                        this._leftOffset = this._designerCanvas.Width - maxRight;
                    }
                }
                else
                {
                    if (minLeft < Math.Abs(this._leftOffset))
                    {
                        this._leftOffset = -minLeft;
                    }
                }

                // проверка на то,что перемещаемые элементы не выходят за рамки Canvas по вертикали
                if (this._topOffset > 0)
                {
                    if (maxBottom + this._topOffset > this._designerCanvas.Height)
                    {
                        this._topOffset = this._designerCanvas.Height - maxBottom;
                    }
                }
                else
                {
                    if (minTop < Math.Abs(this._topOffset))
                    {
                        this._topOffset = -minTop;
                    }
                }

                this._leftOffset = this.UpdateValueMultiplicityFive(this._leftOffset);
                this._topOffset = this.UpdateValueMultiplicityFive(this._topOffset);

                this.UpdatePosition();
            }
        }

        private double UpdateValueMultiplicityFive(double value)
        {
            if (Math.Abs(value % 5) > 0)
            {
                int i = (int) value / 5;
                double delta = Math.Abs(value - i * 5);
                value = value > 0
                    ? (delta >= 2.5 ? value + 5 - delta : value - delta)
                    : (delta >= 2.5 ? value - 5 + delta : value + delta);
            }
            return value;
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            // перемещение всех элементов
            foreach (ContentPresenter control in this._contentPresenters.Where(control => control.Content is ILogicElementViewModel))
            {
                ILogicElementViewModel item = control.Content as ILogicElementViewModel;
                if(item == null) continue;
                item.X = item.X + this._leftOffset;
                item.Y = item.Y + this._topOffset;
                // Задание новых координат точек выводов
                foreach (IConnectorViewModel connector in item.Connectors)
                {
                    Point prevPoint = connector.ConnectorPoint;
                    connector.ConnectorPoint = new Point(prevPoint.X + this._leftOffset, prevPoint.Y + this._topOffset);
                }
            }

            if (IsMouseCaptured) ReleaseMouseCapture();
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this._designerCanvas);
            adornerLayer?.Remove(this);
        }
    }
}
