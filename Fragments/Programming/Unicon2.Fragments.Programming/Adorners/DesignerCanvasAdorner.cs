using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Unicon2.Fragments.Programming.ViewModels;
using Unicon2.Unity.Annotations;

namespace Unicon2.Fragments.Programming.Adorners
{
    public class DesignerCanvasAdorner : Adorner
    {
        private readonly SchemeTabViewModel _schemeTabViewModel;
        private readonly Pen _rectPen;
        private readonly Pen _gridPen;

        public DesignerCanvasAdorner([NotNull] Canvas designerCanvas, SchemeTabViewModel schemeTabViewModel) : base(designerCanvas)
        {
            this._schemeTabViewModel = schemeTabViewModel;
            this._rectPen = new Pen(Brushes.Black, 1);
            this._gridPen = new Pen(Brushes.Black, 0.1);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            var point = new Point(this._schemeTabViewModel.RectX, this._schemeTabViewModel.RectY);
            var size = new Size(this._schemeTabViewModel.RectWidth, this._schemeTabViewModel.RectHeight);
            drawingContext.DrawRectangle(Brushes.Transparent, this._rectPen, new Rect(point, size));

            for (int i = 0; i < this._schemeTabViewModel.SchemeWidth; i = i + SchemeTabViewModel.CELL_SIZE)
            {
                drawingContext.DrawLine(this._gridPen, new Point(i, 0), new Point(i, this._schemeTabViewModel.SchemeHeight));
            }

            for (int j = 0; j < this._schemeTabViewModel.SchemeHeight; j = j + SchemeTabViewModel.CELL_SIZE)
            {
                drawingContext.DrawLine(this._gridPen, new Point(0, j), new Point(this._schemeTabViewModel.SchemeWidth, j));
            }
        }
    }
}
