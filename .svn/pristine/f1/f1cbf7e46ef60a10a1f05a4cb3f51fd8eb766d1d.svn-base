using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Oscilloscope.ComtradeFormat;
using Oscilloscope.View.AnalogChartItem;
using Oscilloscope.View.MainItem;

namespace Oscilloscope.View.CommonDiscretChartItems
{
    /// <summary>
    /// Логика взаимодействия для AnalogChart.xaml
    /// </summary>
    public partial class CommonDiscretChart : UserControl
    {
        public class DiscretChannelsVisibleOption
        {
            private States _state = States.ALL;


            public States State
            {
                get { return this._state; }
                set {
                    this._state = value;
                    this.RaiseStateChanged();}
            }

            public event Action StateChanged;
        

            private void RaiseStateChanged()
            {
                if (this.StateChanged!= null)
                {
                    this.StateChanged.Invoke();
                }
            }
        }

        public enum States
        {
            ALL,
            ENABLED,
            HIDE
        }
        private void Grid_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Height < 1.0 || e.NewSize.Width < 1.0)
            {
                return;
            }
            this._changed = true;
            this._size = e.NewSize;

         
            for (int i = 0; i < this._channels.Length; i++)
            {
                this._gridImageCanvas[i].Width = this._size.Width;
                this._gridImageCanvas[i].Source = new WriteableBitmap(
                    (int) this._size.Width,
                    BASE_HEIGTH,
                    0,
                    0,
                    PixelFormats.Pbgra32,
                    null);
            }
            this.Draw();
            this.SetMarker(this._markerPosition, this._markerOffset);
            this.SetMarker2(this._markerPosition2, this._markerOffset2);
        }

        static DiscretChannelsVisibleOption _visibleOption = new DiscretChannelsVisibleOption();
        public static DiscretChannelsVisibleOption VisibleOption
        {
            get { return _visibleOption; }
        }

        private DiscretChannel[] _channels;
        private DiscretChannel[] _enabledChannels;
        private const int BASE_HEIGTH =30;
        private Line _marker = new Line { Stroke = MainWindow.Marker1Brush };
        private Line _marker2 = new Line { Stroke = MainWindow.Marker2Brush };
        static Pen NavyPen = new Pen(Brushes.Navy, 1.5);
        static Pen BluePen = new Pen(Brushes.DeepSkyBlue, 3);
        static Pen BlackPen = new Pen(Brushes.Black, 1);
        public event Action<int> MoveMarker;
        public event Action<int> MoveMarker2;
        private InfoTable[] _infoTables;
        private Size _size;

        private void RaiseMoveMarker(int position)
        {
            if (this.MoveMarker != null)
            {
                this.MoveMarker.Invoke(position);
            }
        }
        private void RaiseMoveMarker2(int position)
        {
            if (this.MoveMarker2 != null)
            {
                this.MoveMarker2.Invoke(position);
            }
        }
        public CommonDiscretChart()
        {
            this.InitializeComponent();
            this.ImageGrid.Children.Add(this._marker);
            this.ImageGrid.Children.Add(this._marker2);
            this.ImageGrid.Children.Add(this._runMarker);
            Panel.SetZIndex(this._marker,5);
            Panel.SetZIndex(this._marker2, 5);
            Panel.SetZIndex(this._runMarker, 5);
            BluePen.Freeze();
            NavyPen.Freeze();
            BlackPen.Freeze();
            _visibleOption.StateChanged += this._visibleOption_StateChanged;
            IsVisibleChanged += this.CommonDiscretChart_IsVisibleChanged;
        }
        public void VerticalScroll()
        {
            this.UserVisible();
        }
        void CommonDiscretChart_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (_visibleOption.State == States.HIDE&&Visibility!= Visibility.Collapsed)
            {
                Visibility = Visibility.Collapsed;
            }
        }

        private void _visibleOption_StateChanged()
        {
            switch (_visibleOption.State)
            {
                    case States.ALL:
                    {
                        Visibility = Visibility.Visible;
                        break;
                    }

                    case States.ENABLED:
                    {
                        Visibility = Visibility.Visible;
                        break;
                    }

                    case States.HIDE:
                    {
                        Visibility = Visibility.Collapsed;
                        break;
                    }
            }
            this.Draw();
        }

        private Image[] _gridImageCanvas;
        public DiscretChannel[] Channels
        {
            get { return this._channels; }
            set
            {
                this._channels = value;
                this._enabledChannels = this._channels.Where(o => !o.IsEmpty).ToArray();
                this._infoTables= new InfoTable[this._channels.Length];
                for (int i = 0; i < this._infoTables.Length; i++)
                {
                    this._infoTables[i] = new InfoTable();
                    this._infoTables[i].Channel = this._channels[i];
                    this.InfoStack.Children.Add(this._infoTables[i]);
                }
                Height = BASE_HEIGTH*this._channels.Length;

                this._gridImageCanvas = new Image[this._channels.Length];
                for (int i = 0; i < this._channels.Length; i++)
                {
                    Image ni = new Image();
                    ni.Height = BASE_HEIGTH;

                    ni.Width = ActualWidth;
                    ni.Stretch = Stretch.None;

                    this.ImageStack.Children.Add(ni);

                    this._gridImageCanvas[i] = ni;

                }
                this.Draw();
            }
        }

        private int _zoomX = 1;

        private void Draw()
        {
            if (this._size.Height < 1.0 || this._size.Width < 1.0)
            {
                return;
            }
            //GC.WaitForPendingFinalizers();
            //GC.Collect();
            this._xOffset = this._zoomX == 1
                  ? 1
                  : this._size.Width / WorkplaceControl.BASE_CHANGE * this._scrollOffset;

            this.SetMarker(this._markerPosition, this._markerOffset);
            this.SetMarker2(this._markerPosition2, this._markerOffset2);
            if (_visibleOption.State == States.HIDE)
            {
                return;
            }
            if (_visibleOption.State == States.ALL)
            {
                foreach (var infoTable in this._infoTables)
                {
                    infoTable.Enable();
                }
                this.DrawChannels(this._channels);
            }

            if (_visibleOption.State == States.ENABLED)
            {


                foreach (var infoTable in this._infoTables)
                {
                    infoTable.Disable();
                }
                this.DrawChannels(this._enabledChannels);
            }

            this.RunOscVisible = this._runOscVisible;
        }

        private class ImageItem
        {
            private DiscretChannel _discretChannel;
            private Image _image;

            public ImageItem(Image image, DiscretChannel discretChannel)
            {
                this._discretChannel = discretChannel;
                this._image = image;
                this.Width = (int) this._image.Width;
                this.Height = (int) this._image.Height;
            }

            public int Width { get; private set; }
            public int Height { get; private set; }
            public DiscretChannel DiscretChannel
            {
                get { return this._discretChannel; }
            }



            public Image Image
            {
                get { return this._image; }
            }

      
        }

        private void DrawChannels(DiscretChannel[] discretChannels)
        {
            Height = BASE_HEIGTH*discretChannels.Length;
            for (int i = 0; i < discretChannels.Length; i++)
            {
                Task.Factory.StartNew(this.DrawOneChannel, new ImageItem(this._gridImageCanvas[i], discretChannels[i]));
            }
        }





        private void DrawOneChannel(object  item1 )
        {
           var item = (ImageItem) item1;
            DrawingVisual drawingVisual = new DrawingVisual();
            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {

                //Заливка цветом
                drawingContext.DrawRectangle(Brushes.White, null,
                                             new Rect(0, 0, item.Width, item.Height));

                this.DrawOneDiscret(drawingContext, item.DiscretChannel.Values);
                drawingContext.DrawLine(BlackPen, new Point(this._size.Width, 0), new Point(item.Width, item.Height));
            }
            var bmp = new RenderTargetBitmap(item.Width, item.Height, 0, 0,
                                             PixelFormats.Pbgra32);
            bmp.Render(drawingVisual);
            bmp.Freeze();

            uint[] arrBits = new uint[bmp.PixelWidth * bmp.PixelHeight];
            bmp.CopyPixels(arrBits, 4 * bmp.PixelWidth, 0);
            Dispatcher.Invoke(new Action<ImageItem,uint[]>(this.DrawComplite),item1, arrBits);

        }

        private void DrawComplite(ImageItem arg1, uint[] res)
        {
            var writeableBitmap = (WriteableBitmap) arg1.Image.Source;
            writeableBitmap.Lock();

            unsafe
            {
                var lenght = Math.Min(writeableBitmap.Height * writeableBitmap.Width, res.Length);
                long pBackBuffer = (long)writeableBitmap.BackBuffer;
                for (int i = 0; i < lenght-1; i++)
                {
                    pBackBuffer += sizeof(uint);
                    *((uint*)pBackBuffer) = res[i];
                }

            }

            writeableBitmap.AddDirtyRect(new Int32Rect(0, 0, (int) writeableBitmap.Width, (int) writeableBitmap.Height));
            writeableBitmap.Unlock();
        }

        private void UserVisible()
        {
            this._visible = AnalogChart.IsUserVisible(this, (FrameworkElement) ((FrameworkElement) Parent).Parent);
         
            if (this._visible)
                if (this._visible && this._changed)
                {
                    this._changed = false;
                    this.Draw();
                }
        }


        internal void NewScroll(int p)
        {
            this._scrollOffset = p;
            this._changed = true;
            this.UserVisible();
            this.SetMarker(this._markerPosition, this._markerOffset);
            this.SetMarker2(this._markerPosition2, this._markerOffset2);
        }
        private void DrawOneDiscret(DrawingContext drawingContext, bool[] values)
        {
            double ox = this._size.Width / (values.Length-1) * this._zoomX;
            List<DiscretLine> d = DiscretLine.CreateLines(values);
            foreach (DiscretLine discretLine in d)
            {
                if (discretLine.Value)
                {
                    drawingContext.DrawLine(BluePen, new Point(discretLine.Start * ox - this._xOffset, BASE_HEIGTH / 2),
                                            new Point(discretLine.End * ox - this._xOffset, BASE_HEIGTH / 2));
                    drawingContext.DrawLine(NavyPen, new Point(discretLine.Start * ox - this._xOffset, BASE_HEIGTH / 2 - 3),
                                            new Point(discretLine.End * ox - this._xOffset, BASE_HEIGTH / 2 - 3));
                    drawingContext.DrawLine(NavyPen, new Point(discretLine.Start * ox - this._xOffset, BASE_HEIGTH / 2 + 3),
                                            new Point(discretLine.End * ox - this._xOffset, BASE_HEIGTH / 2 + 3));
                }
                else
                {
                    drawingContext.DrawLine(NavyPen, new Point(discretLine.Start * ox - this._xOffset, BASE_HEIGTH / 2),
                                            new Point(discretLine.End * ox - this._xOffset, BASE_HEIGTH / 2));
                }
            }
            drawingContext.DrawLine(BlackPen, new Point(0, 0), new Point(this._size.Width, 0));
            drawingContext.DrawLine(BlackPen, new Point(0, BASE_HEIGTH), new Point(this._size.Width, BASE_HEIGTH));
        }

        private class DiscretLine
        {
            private int _start;
            private int _end;
            private bool _value;

            private DiscretLine(int start, int end, bool value)
            {
                this._start = start;
                this._end = end;
                this._value = value;
            }

            public int Start
            {
                get { return this._start; }
            }

            public int End
            {
                get { return this._end; }
            }

            public bool Value
            {
                get { return this._value; }
            }

            public static List<DiscretLine> CreateLines(bool[] values)
            {
                List<DiscretLine> res = new List<DiscretLine>();

                bool flag = values[0];
                int startIndex = 0;


                for (int i = 0; i < values.Length; i++)
                {
                    if (values[i] == flag)
                    {
                        continue;
                    }
                    if (!flag)
                    {
                        res.Add(new DiscretLine(startIndex, i - 1, flag));
                        startIndex = i - 1;
                    }
                    else
                    {
                        res.Add(new DiscretLine(startIndex, i, flag));
                        startIndex = i;
                    }

                    flag = values[ i];
                }
                res.Add(new DiscretLine(startIndex, values.Length, flag));
                return res;
            }
        }

        private void DrowCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var ox = this._size.Width / this._channels[0].Length * this._zoomX;
            if (e.ChangedButton == MouseButton.Left)
            {
                var position = e.GetPosition(this.ImageStack);
                if (position.X < 0)
                {
                    return;
                }

                this.RaiseMoveMarker((int)((position.X + this._xOffset) / ox));
            }

            if (e.ChangedButton == MouseButton.Right)
            {
                var position = e.GetPosition(this.ImageStack);
                if (position.X < 0)
                {
                    return;
                }

                this.RaiseMoveMarker2((int)((position.X + this._xOffset) / ox));
            }
        }

        public void SetMarker(int position, int offset)
        {
            this._markerPosition = position;
            this._markerOffset = offset;
            this._marker.Y1 = 0;
            this._marker.Y2 = BASE_HEIGTH * this._infoTables.Length;
            this._marker.X1 = this._marker.X2 = offset;

            for (int i = 0; i < this._infoTables.Length; i++)
            {
                this._infoTables[i].Value = this._channels[i].Values[(int) position];
            }
        }

        internal void SetMarker2(int position, int offset)
        {
            this._markerPosition2 = position;
            this._markerOffset2 = offset;

            this._marker2.X1 = this._marker2.X2 = offset;
            this._marker2.Y1 = 0;
            this._marker2.Y2 = BASE_HEIGTH * this._infoTables.Length;
        }

        internal void SetZoom(Zoom zoom)
        {
            //Сделать 0 и 1 блоки
            this._zoomX = zoom.ZoomX;
            this.Draw();
            this.SetMarker(this._markerPosition, this._markerOffset);
            this.SetMarker2(this._markerPosition2, this._markerOffset2);
            this.RunOscVisible = this._runOscVisible;
        }

        private bool _runOscVisible;
        public bool RunOscVisible
        {
            set
            {
                this._runOscVisible = value;
                if (value)
                {
                    var ox = this._size.Width / (this._channels[0].Length - 1) * this._zoomX;
                    this._runMarker.X1 = this._runMarker.X2 = (int)(this._channels[0].Run * ox - this._xOffset);
                    //_runMarker.X1 = _channels[0].Run * _zoomX;
                    //_runMarker.X2 = _channels[0].Run * _zoomX;
                    this._runMarker.Y1 = 0;
                    this._runMarker.Y2 = BASE_HEIGTH * this._infoTables.Length;
                    this._runMarker.Visibility = Visibility.Visible;
                }
                else
                {
                    this._runMarker.X1 = 0;
                    this._runMarker.X2 = 0;
                    this._runMarker.Y1 = 0;
                    this._runMarker.Y2 = 0;
                    this._runMarker.Visibility = Visibility.Collapsed;
                }
            }
        }

        private Line _runMarker = new Line { Stroke = Brushes.Violet };
        private int _markerPosition;
        private int _markerPosition2;
        private int _scrollOffset;
        private double _xOffset;
        private bool _changed;
        private bool _visible;
        private int _markerOffset2;
        private int _markerOffset;



    }
}
