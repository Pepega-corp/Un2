using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Oscilloscope.View.MainItem
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class MarkersControl : UserControl
    {       
        public event Action MoveMarker1;
        private int _marker1;

        public Visibility Marker2Visibility
        {
            get { return Thumb2.Visibility; }
            set { Thumb2.Visibility = value; }
        }

        public int Marker1
        {
            get { return _marker1; }
            set
            {
                if (value > Maximum)
                {
                    value = Maximum;
                }
                _marker1 = value;
                SetMarker(Thumb1, value);
       
            }
        }

        public int Marker1Offset { get; private set; }
        public int Marker2Offset { get; private set; }


        public event Action MoveMarker2;
        private int _marker2;
        public int Marker2
        {
            get { return _marker2; }
            set
            {
                if (value > Maximum)
                {
                    value = Maximum;
                }
                _marker2 = value;
                SetMarker(Thumb2, value);
            }
        }

        internal void SetZoom(Zoom zoom)
        {
            this._zoom = zoom;
            this.UpdateValues();
        }

        private int _maximum = 1;

        private Zoom _zoom;
        private int _scrollOffset;
        private int xOffset;

        public int Maximum
        {
            get { return _maximum; }
            set { _maximum = value > 0 ? value : 1; }
        }

        public void UpdateValues()
        {
            this.SetMarker(Thumb1, _marker1);
            this.SetMarker(Thumb2, _marker2);
            RaiseMoveMarker1();
            RaiseMoveMarker2();
        }

        private void SetMarker(Thumb thumb, int value)
        {
            xOffset = (int) ((_zoom.ZoomX == 1)
                                 ? 1
                                 : ((this.ActualWidth - thumb.Width)/(WorkplaceControl.BASE_CHANGE)*_scrollOffset));
            var k = value*(this.ActualWidth - thumb.Width)/this._maximum*_zoom.ZoomX - xOffset;
            if (k<0)
            {
                k = 0;
            }
            if (k > this.ActualWidth - thumb.Width)
            {
                k = this.ActualWidth - thumb.Width;
            }
            var res = (int) Math.Round(k);
            if (thumb == Thumb1)
            {
                Marker1Offset = res;
            }
            else
            {
                Marker2Offset = res;
            }
            Canvas.SetLeft(thumb, res);
        }

        public MarkersControl()
        {
            InitializeComponent();
        }

        private void thumb1_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            var pos= CalcPos(Thumb1, e);
            Marker1 = (int) pos;
            this.RaiseMoveMarker1();
        }

        private void thumb2_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            var pos = CalcPos(Thumb2, e);
            Marker2 = (int) pos;
            this.RaiseMoveMarker2();
        }

        private void RaiseMoveMarker1()
        {
            if (MoveMarker1 != null)
            {
                MoveMarker1.Invoke();
            }
        }

        private void RaiseMoveMarker2()
        {
            if (MoveMarker2 != null)
            {
                MoveMarker2.Invoke();
            }
        }


        private double CalcPos(Control thumb, DragDeltaEventArgs e)
        {
            double pos = 0;

            if (Canvas.GetLeft(thumb) + e.HorizontalChange <= 0)
            {
                pos = 0;

            }
            else
            {
                double k = (this.ActualWidth - thumb.Width)/this._maximum*_zoom.ZoomX;
                var temp = Canvas.GetLeft(thumb) + e.HorizontalChange +
                           ((this.ActualWidth - thumb.Width)/(WorkplaceControl.BASE_CHANGE)*_scrollOffset);
                pos = Math.Round(temp/k);
            }
            return pos;
        }



        private void Thumb1_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Left)||(e.Key == Key.Down))
            {
                Marker1--;
                RaiseMoveMarker1();
            }
            if ((e.Key == Key.Right) || (e.Key == Key.Up))
            {
                Marker1++;
                RaiseMoveMarker1();
            }
            e.Handled = true;
        }

        private void Thumb2_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Left) || (e.Key == Key.Down))
            {
                Marker2--;
                RaiseMoveMarker2();
            }
            if ((e.Key == Key.Right) || (e.Key == Key.Up))
            {
                Marker2++;
                RaiseMoveMarker2();
            }
            e.Handled = true;
        }

        private void MarkerBar_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.UpdateValues();
        }


        internal void NewScroll(int p)
        {
            _scrollOffset = p;
            this.UpdateValues();
        }
    }
}
