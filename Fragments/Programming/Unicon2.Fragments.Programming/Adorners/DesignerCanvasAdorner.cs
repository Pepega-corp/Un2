using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Unicon2.Fragments.Programming.ViewModels;

namespace Unicon2.Fragments.Programming.Adorners
{
    public class DesignerCanvasAdorner : Adorner
    {
        private readonly SchemeTabViewModel _schemeTabViewModel;
        private readonly Pen _rectPen;
        private readonly Pen _gridPen;

        //Размер физического пикселя в виртуальных единицах
        public static double PixelSize { get; }

        //Текущее разрешение
        public static int Dpi { get; }

        static DesignerCanvasAdorner()
        {
            var flags = BindingFlags.NonPublic | BindingFlags.Static;
            var dpiProperty = typeof(SystemParameters).GetProperty("Dpi", flags);

            Dpi = (int)dpiProperty.GetValue(null, null);
            PixelSize = 96.0 / Dpi;
        }
        
        public DesignerCanvasAdorner(Canvas designerCanvas, SchemeTabViewModel schemeTabViewModel) : base(designerCanvas)
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

            for (int i = 0; i < this._schemeTabViewModel.SchemeWidth; i += SchemeTabViewModel.CELL_SIZE)
            {
                var iSnapped = this.SnapToPixels(i);
                var heightSnapped = this.SnapToPixels(this._schemeTabViewModel.SchemeHeight);
                drawingContext.DrawLine(this._gridPen, new Point(iSnapped, 0), new Point(i,heightSnapped));
            }

            for (int j = 0; j < this._schemeTabViewModel.SchemeHeight; j = j + SchemeTabViewModel.CELL_SIZE)
            {
                var jSnapped = this.SnapToPixels(j);
                var widthSnapped = this.SnapToPixels(this._schemeTabViewModel.SchemeWidth);
                drawingContext.DrawLine(this._gridPen, new Point(0, jSnapped), new Point(widthSnapped, j));
            }
        }

        private double SnapToPixels(double value)
        {
            value += PixelSize / 2;

            //На нестандартных DPI размер пикселя в WPF-единицах дробный.
            //Перемножение на 1000 нужно из-за потерь точности
            //при представлении дробных чисел в double
            //2.4 / 0.4 = 5.9999999999999991
            //240.0 / 40.0 = 6.0

            var div = (value * 1000) / (PixelSize * 1000);

            return (int)div * PixelSize;
        }
    }
}
