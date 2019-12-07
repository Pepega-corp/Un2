using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Unicon2.Fragments.Programming.Behaviors;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme;

namespace Unicon2.Fragments.Programming.Adorners
{
    public class RubberbandAdorner : Adorner
    {
        private Point? _startPoint;
        private Point? _endPoint;
        private Pen _rubberbandPen;
        private DesignerCanvasBehavior _designerCanvasBehavior;

        public RubberbandAdorner(DesignerCanvasBehavior dcb): base(dcb.DesignerCanvas)
        {
            this._designerCanvasBehavior = dcb;
            this._startPoint = dcb.RubberbandSelectionStartPoint;
            this._rubberbandPen = new Pen(Brushes.LightSlateGray, 1);
            this._rubberbandPen.DashStyle = new DashStyle(new double[] { 2 }, 1);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (!IsMouseCaptured)
                    CaptureMouse();

                this._endPoint = e.GetPosition(this);
                this.UpdateSelection();
                InvalidateVisual();
            }
            else
            {
                if (IsMouseCaptured) ReleaseMouseCapture();
            }

            e.Handled = true;
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            // release mouse capture
            if (IsMouseCaptured) ReleaseMouseCapture();
            // remove this adorner from adorner layer
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this._designerCanvasBehavior.DesignerCanvas);
            if (adornerLayer != null)
                adornerLayer.Remove(this);
            e.Handled = true;
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            dc.DrawRectangle(Brushes.Transparent, null, new Rect(RenderSize));

            if (this._startPoint.HasValue && this._endPoint.HasValue)
                dc.DrawRectangle(Brushes.Transparent, this._rubberbandPen, new Rect(this._startPoint.Value, this._endPoint.Value));
        }

        private void UpdateSelection()
        {
            foreach (ISelectable item in this._designerCanvasBehavior.TabViewModel.ElementCollection)
                item.IsSelected = false;
            if(!this._startPoint.HasValue || !this._endPoint.HasValue) return;
            
            Rect rubberBand = new Rect(this._startPoint.Value, this._endPoint.Value);
            foreach (ContentPresenter item in this._designerCanvasBehavior.DesignerCanvas.Children.OfType<ContentPresenter>().Where(cp=>cp.Content is ISelectable))
            {
                Rect itemRect = VisualTreeHelper.GetDescendantBounds(item);
                Rect itemBounds =
                    item.TransformToAncestor(this._designerCanvasBehavior.DesignerCanvas).TransformBounds(itemRect);
                // чтобы элемент уже выделялся, когда рамка охватывает хотя бы четверть элемента
                Point point = new Point(itemBounds.X + (itemBounds.Right-itemBounds.X)/2, itemBounds.Y + (itemBounds.Bottom-itemBounds.Y)/2);
                if (rubberBand.Contains(point))
                {
                    ISelectable selectableItem = (ISelectable) item.Content;
                    selectableItem.IsSelected = true;
                }
            }
        }
    }
}
