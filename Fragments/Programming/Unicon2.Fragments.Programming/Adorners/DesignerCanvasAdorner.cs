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
        private readonly Pen _pen;

        public DesignerCanvasAdorner([NotNull] Canvas designerCanvas, SchemeTabViewModel schemeTabViewModel) : base(designerCanvas)
        {
            this._schemeTabViewModel = schemeTabViewModel;
            this._pen = new Pen(Brushes.Black, 1);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            var point = new Point(this._schemeTabViewModel.RectX, this._schemeTabViewModel.RectY);
            var size = new Size(this._schemeTabViewModel.RectWidth, this._schemeTabViewModel.RectHeight);
            drawingContext.DrawRectangle(Brushes.Transparent, this._pen, new Rect(point, size));
        }
    }
}
