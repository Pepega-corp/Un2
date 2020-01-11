using System;
using System.ComponentModel;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Oscilloscope.View.MainItem;

namespace Oscilloscope.View.AnalogChartItem
{


    /// <summary>
    /// Логика взаимодействия для AnalogChart.xaml
    /// </summary>
    public partial class AnalogChart : INotifyPropertyChanged
    {
        private AnalogChannel _channel;
        /// <summary>
        /// Высота линии графика
        /// </summary>
        private const int BASE_HEIGTH =120;

        private const int ADDED_HEIGTH = 10;
        private const int BORDER_HEIGTH = 0;
        private const int INFO_WIDTH = 150;
        private const int INFO_LINES_LENGHT = 10;
        /// <summary>
        /// Общая высота контрола
        /// </summary>
        private const int FULL_HEIGTH = BASE_HEIGTH + 2 * ADDED_HEIGTH + 2 * BORDER_HEIGTH;
        /// <summary>
        /// Ибщая высота графика
        /// </summary>
        private const int FULL_IMAGE_HEIGTH = BASE_HEIGTH + 2 * ADDED_HEIGTH ;
        /// <summary>
        /// Макс ширина между отсчётами по Х
        /// </summary>
        private const int COUNTDOWN_MAX_WIDTH = 100;

        /// <summary>
        /// Количество значений в 1 пиксиле
        /// </summary>
        private double _k;

        private Zoom _zoom = Zoom.Default;
        private Line _marker = new Line { Stroke = MainWindow.Marker1Brush };
        private Line _marker2 = new Line { Stroke = MainWindow.Marker2Brush };
        private Line _runMarker = new Line { Stroke = Brushes.Violet };
        public event Action<int> MoveMarker;
        public event Action<int> MoveMarker2;
        private static event Action ValueTypeChanged;
        private Pen diagramPen;
        private static Pen blackPen = new Pen(Brushes.Black, 1);
        private static Pen grayPen = new Pen(Brushes.Gray, 0.5);

        private double _scrollOffset;
        /// <summary>
        /// значение по которому стоится сетка oY
        /// </summary>
        private double _y;
        private int _markerPosition;
        private bool _runOscVisible;
        private int _markerPosition2;
        private Size _size;
        private double _xOffset;

        private bool _visible;
        private bool _changed;

        public event PropertyChangedEventHandler PropertyChanged;

        static AnalogChannel.GrafType _valueType = AnalogChannel.GrafType.ACTIV;
        
        private bool _visibilityMin;
        public bool VisibilityMin
        {
            get { return this._visibilityMin; }
            set
            {
                this._visibilityMin = value;
                this.RaisePropertyChanged("VisibilityMin");
            }
        }


        private bool _visibilityMax;
        public bool VisibilityMax
        {
            get { return this._visibilityMax; }
            set
            {
                this._visibilityMax = value;
                this.RaisePropertyChanged("VisibilityMax");
            }
        }
        
        private bool _visibilityRms;
        public bool VisibilityRms
        {
            get { return this._visibilityRms; }
            set
            {
                this._visibilityRms = value;
                this.RaisePropertyChanged("VisibilityRms");
            }
        }

        private bool _visibilityHarminy;
        public bool VisibilityHarminy
        {
            get { return this._visibilityHarminy; }
            set
            {
                this._visibilityHarminy = value;
                this.RaisePropertyChanged("VisibilityHarminy");
            }
        }

        private bool _visibilityFrequency;
        public bool VisibilityFrequency
        {
            get { return this._visibilityFrequency; }
            set
            {
                this._visibilityFrequency = value;
                this.RaisePropertyChanged("VisibilityFrequency");
            }
        }

        internal AnalogChartOptions Options
        {
            get
            {
                return this._channel.Options;
            }
        }

        private void RaisePropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        private void SetBindings()
        {
            CommonHelper.CreateVisibilityBinding(this, "VisibilityMin", this.MinTb);
            CommonHelper.CreateVisibilityBinding(this, "VisibilityMax", this.MaxTb);
            CommonHelper.CreateVisibilityBinding(this, "VisibilityRms", this.RmsTb);
            CommonHelper.CreateVisibilityBinding(this, "VisibilityHarminy", this.HarmonyTb);
            CommonHelper.CreateVisibilityBinding(this, "VisibilityFrequency", this.Frequency);

            CommonHelper.CreateVisibilityBinding(this._channel.Options, "Visibility", this);
            CommonHelper.CreateCheckedBinding(this._channel.Options, "Vector", this.VectorCheckBox);
            CommonHelper.CreateCheckedBinding(this._channel.Options, "Frequency", this.FreqCharCheckBox);
        }

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

        static AnalogChart()
        {
            blackPen.Freeze();
            grayPen.Freeze();
        }

        public AnalogChart()
        {
            this.InitializeComponent();
            this.ImageGrid.Children.Add(this._marker);
            this.ImageGrid.Children.Add(this._marker2);
            this.ImageGrid.Children.Add(this._runMarker);

            ValueTypeChanged += () => { this.CurrentValueType = ValueType; };
        }


        public AnalogChannel Channel
        {
            set
            {
                this._channel = value;
                this.diagramPen = new Pen(this._channel.ChannelBrush, 1);
                this.diagramPen.Freeze();              
                this.SetBindings();
                this.Draw();
                this._channel.NeedDraw += this.Draw;
                this.MinTb.Content = this._channel.Min;
                this.MaxTb.Content = this._channel.Max;
                this.CurrentValueType = ValueType;
            }
            get { return this._channel; }
        }

        private void Draw()
        {
            if (Visibility != Visibility.Visible)
            {
                return;
            }
            this.Prepare();
            this.DrawY();
            this._xOffset = (this._zoom.ZoomX == 1)
                ? 1
                : (this._size.Width/(double) (WorkplaceControl.BASE_CHANGE)* this._scrollOffset);
            this.DrawCurve();
            this.SetMarker(this._markerPosition, this._markerOffset);
            this.SetMarker2(this._markerPosition2, this._markerOffset2);

            this.RunOscVisible = this._runOscVisible;
        }

        private double _yMax = double.NaN;
        private void Prepare()
        {
            this._k = (double.IsNaN(this._yMax) ? this._channel.Amplitude(this.CurrentValueType) : this._yMax) / (BASE_HEIGTH * this._zoom.ZoomY / 2.0);
            this._y = this._channel.GetY(this.CurrentValueType);
            Height = FULL_IMAGE_HEIGTH * this._zoom.ZoomY + 2 * BORDER_HEIGTH;
        }

        /// <summary>
        /// Рисует кривую
        /// </summary>
        private void DrawCurve()
        {
            if ((this._size.Width < 1) || (this._size.Height < 1))
            {
                return;
            }
            Task.Factory.StartNew(this.DrawScroll, this.DrawImage.Source);
        }
        
        public void SetZoom(Zoom zoom)
        {
            this._zoom = zoom;
            this.Draw();
        }

        public bool RunOscVisible
        {
            set
            {
                this._runOscVisible = value;
                if (value)
                {
                    double ox = this._size.Width / (this._channel.Length - 1) * this._zoom.ZoomX;
                    this._runMarker.X1 = this._runMarker.X2 = (int)(this.Channel.Run * ox - this._xOffset);
                    this._runMarker.Y1 = 0;
                    this._runMarker.Y2 = ActualHeight * this._zoom.ZoomY;
                    this._runMarker.Visibility = Visibility.Visible;
                }
                else
                {
                    this._runMarker.X1 = this._runMarker.X2 = 0;
                    this._runMarker.Y1 = this._runMarker.Y2 = 0;
                    this._runMarker.Visibility = Visibility.Collapsed;
                }
            }
        }
        
        /// <summary>
        /// Строит сетку по У
        /// </summary>
        private void DrawY()
        {
            double oy = this._y / this._k;
            DrawingVisual drawingVisual = new DrawingVisual();
            int yBaseValue = (BASE_HEIGTH + 2*ADDED_HEIGTH)* this._zoom.ZoomY;
            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                //Заливка цветом
                drawingContext.DrawRectangle(Brushes.White, blackPen, new Rect(0, 0, INFO_WIDTH, FULL_IMAGE_HEIGTH * this._zoom.ZoomY));
                for (int i = -2; i <= 2; i++)
                {
                    double y = oy * i + (double)yBaseValue/2;
                    drawingContext.DrawLine(blackPen, new Point(INFO_WIDTH-INFO_LINES_LENGHT, y), new Point(INFO_WIDTH, y));

                    FormattedText formattedText = new FormattedText(
                        AnalogChannel.Normalize((-1)*i* this._y, this._channel.Measure),
                        CultureInfo.GetCultureInfo("en-us"),
                        FlowDirection.LeftToRight,
                        new Typeface("Verdana"),
                        12,
                        Brushes.Black);
                   
                    drawingContext.DrawText(formattedText, new Point(INFO_WIDTH - INFO_LINES_LENGHT - formattedText.Width, y - formattedText.Height/2));
                }
            }

            RenderTargetBitmap bmp = new RenderTargetBitmap(150, yBaseValue, 0, 0, PixelFormats.Pbgra32);
            bmp.Render(drawingVisual);
            this.InfoCanvas.Background = new ImageBrush(bmp);
        }
        
        private void DrowCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            double ox = this._size.Width/ this._channel.Length* this._zoom.ZoomX;
            Point position = e.GetPosition(this.DrawImage);
            if (position.X < 0)
            {
                return;
            }

            if (e.ChangedButton == MouseButton.Left)
            {
                this.RaiseMoveMarker((int) ((position.X + this._xOffset)/ox));
            }

            if (e.ChangedButton == MouseButton.Right)
            {
                this.RaiseMoveMarker2((int) ((position.X + this._xOffset)/ox));
            }
        }
        
        public static AnalogChannel.GrafType ValueType
        {
            get { return _valueType; }
            set
            {
                if (_valueType != value)
                {
                    _valueType = value;
                    if (ValueTypeChanged!= null)
                    {
                        ValueTypeChanged.Invoke();
                    }
                }
            }
        }
    
        private AnalogChannel.GrafType _currentValueType;
        private int _markerOffset;
        private int _markerOffset2;
        public AnalogChannel.GrafType CurrentValueType
        {
            get { return this._currentValueType; }
            set
            {
                this._currentValueType = value;
                this.Draw();
                //DrawY();
                switch (value)
                {
                    case AnalogChannel.GrafType.ACTIV :
                        {
                            this.Menu1.IsChecked = true;
                            break;
                        }
                    case AnalogChannel.GrafType.RMS:
                        {
                            this.Menu2.IsChecked = true;
                            break;
                        }
                    case AnalogChannel.GrafType.FIRST_HARMONIC:
                        {
                            this.Menu3.IsChecked = true;
                            break;
                        }
                }
            }
        }

        public void SetMarker(int position, int offset)
        {
            double[] values = this._channel.Values(this.CurrentValueType);
            double[] rmsValues = this._channel.RmsValues;
            if (position >= values.Length)
            {
                return;
            }
            string value = AnalogChannel.Normalize(values[position],this._channel.Measure);
            this.ChannelNameTb.Content = string.Format("{0} = {1}", this._channel.Name, value);

            value = AnalogChannel.Normalize(rmsValues[position],this._channel.Measure);
            this.RmsTb.Content = string.Format("{0}.ск = {1}", this._channel.Name, value);

            value = AnalogChannel.Normalize(this._channel.FirstHarmonicDouble[position], this._channel.Measure);
            this.HarmonyTb.Content = string.Format("{0}.1г = {1}", this._channel.Name, value);

            //TODO
            this._markerPosition = position;
            this._markerOffset = offset;
            this.MoveMarkers(this._marker, offset);
        }

        internal void SetMarker2(int obj, int offset)
        {
            this._markerPosition2 = obj;
            this._markerOffset2 = offset;
            string value = AnalogChannel.Normalize(this._channel.Values(this.CurrentValueType)[obj], this._channel.Measure);
            this.ChannelNameTb2.Content = string.Format("{0} = {1}", this._channel.Name, value);
            this.MoveMarkers(this._marker2, offset);
        }
        private void MoveMarkers(Line marker, int offset)
        {
            marker.X1 = marker.X2 = offset;
            marker.Y1 = 0;
            marker.Y2 = ActualHeight;
        }
        
        private void Grid_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            if ((e.NewSize.Height < 1.0) || (e.NewSize.Width < 1.0))
            {
                return;
            }
            this._changed = true;
            this._size = e.NewSize;

            this.DrawImage.Source = new WriteableBitmap(
                (int) this._size.Width,
                (int) this._size.Height,
                0,
                0,
                PixelFormats.Pbgra32,
                null);
            this.Draw();
        }

        public void VertivalScroll()
        {
            this.UserVisible();
        }

        private void UserVisible()
        {
            this._visible = IsUserVisible(this, (FrameworkElement) ((FrameworkElement) Parent).Parent);
            if (this._visible && this._changed)
            {
                this._changed = false;
                this.Draw();
            }
        }

        public static bool IsUserVisible(FrameworkElement element, FrameworkElement container)
        {
            if (!element.IsVisible)
                return false;

            Rect bounds = element.TransformToAncestor(container).TransformBounds(new Rect(0.0, 0.0, element.ActualWidth, element.ActualHeight));
            Rect rect = new Rect(0.0, 0.0, container.ActualWidth, container.ActualHeight);
            return rect.Contains(bounds.TopLeft) || rect.Contains(bounds.BottomRight) ||
                   bounds.Contains(rect.TopLeft) || bounds.Contains(rect.BottomRight);
        }

        internal void NewScroll(int p)
        {
            this._scrollOffset = p;
            this._changed = true;
            this.UserVisible();
        }

        private void DrawComplite(uint[] res, WriteableBitmap wbmp)
        {
            wbmp.Lock();
            unsafe
            {
                long pBackBuffer = (long) wbmp.BackBuffer;
                double lenght = Math.Min(wbmp.Height*wbmp.Width, res.Length);
                for (int i = 0; i < lenght - 1; i++)
                {
                    pBackBuffer += sizeof (uint);
                    *((uint*) pBackBuffer) = res[i];
                }
            }

            wbmp.AddDirtyRect(new Int32Rect(0, 0, (int) wbmp.Width, (int) wbmp.Height));
            wbmp.Unlock();
        }

        private void DrawScroll(object picture)
        {
            WriteableBitmap wbmp = (WriteableBitmap) picture;
            DrawingVisual drawingVisual = new DrawingVisual();

            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                //Заливка цветом
                drawingContext.DrawRectangle(Brushes.White, blackPen, new Rect(this._size));
                double oy = (this._y/ this._k);
                double ox = this._size.Width/(this._channel.Length-1)* this._zoom.ZoomX;
                //Видимая ширина в пикселях

                int xBaseValue = CommonHelper.CalcBaseXValue(COUNTDOWN_MAX_WIDTH, this._size.Width* this._zoom.ZoomX, this._channel.Length);
                double yBaseValue = (BASE_HEIGTH/2.0 + ADDED_HEIGTH)* this._zoom.ZoomY;

                int startX =
                    (int)Math.Round( ((this._channel.Length-1)/(double) this._zoom.ZoomX*(this._scrollOffset/(double) WorkplaceControl.BASE_CHANGE)));
                int endX =
                    (int)
                    Math.Round((this._channel.Length-1)/(double) this._zoom.ZoomX*(1 + this._scrollOffset/(double) WorkplaceControl.BASE_CHANGE));
                
                //Ось Х(Горизонтальные линии)
                for (int j = -2; j <= 2; j++)
                {
                    Point p1 = new Point(startX*ox - this._xOffset, oy*j + yBaseValue);
                    Point p2 = new Point(endX*ox - this._xOffset, oy*j + yBaseValue);
                    drawingContext.DrawLine(j == 0 ? blackPen : grayPen, p1, p2);
                }

                //Ось Y(Вертикальные линии)
                for (int i = (startX/xBaseValue)*xBaseValue; i < endX; i += xBaseValue)
                {
                    Point p1 = new Point(i*ox - this._xOffset, 0);
                    Point p2 = new Point(i*ox - this._xOffset, FULL_HEIGTH* this._zoom.ZoomY);
                    drawingContext.DrawLine(grayPen, p1, p2);
                }
                //График(Кривая)
                double[] values = this._channel.Values(this.CurrentValueType);
                for (int i = startX; i < endX ; i += 1)
                {
                    Point p1 = new Point(i*ox - this._xOffset, -(values[i]/ this._k) + yBaseValue);
                    Point p2 = new Point((i + 1)*ox - this._xOffset, -(values[i + 1]/ this._k) + yBaseValue);
                    drawingContext.DrawLine(this.diagramPen, p1, p2);
                }

                //Подписи оси оХ
                for (int i = 0; i < endX; i += xBaseValue)
                {
                    FormattedText formattedText = new FormattedText(
                        ((i)).ToString(CultureInfo.InvariantCulture),
                        CultureInfo.GetCultureInfo("en-us"),
                        FlowDirection.LeftToRight,
                        new Typeface("Verdana"),
                        12,
                        Brushes.Black);

                    double xPos = 0;
                    if ((i != 0) || (this._scrollOffset != 0))
                    {
                        xPos = i*ox - this._xOffset - formattedText.Width/2;
                    }

                    drawingContext.DrawText(formattedText,
                                            new Point(xPos, yBaseValue));
                }
            }

            RenderTargetBitmap bmp = new RenderTargetBitmap((int) (this._size.Width), (int) this._size.Height, 0, 0, PixelFormats.Pbgra32);

            bmp.Render(drawingVisual);
            bmp.Freeze();
            uint[] arrBits = new uint[bmp.PixelWidth*bmp.PixelHeight];
            bmp.CopyPixels(arrBits, 4*bmp.PixelWidth, 0);
        
            Dispatcher.Invoke(new Action<uint[], WriteableBitmap>(this.DrawComplite), arrBits, wbmp);
        }

        private void ResetYButton_Click(object sender, RoutedEventArgs e)
        {
            this._yMax = double.NaN;
            this.Draw();
        }

        private void SetYButton_Click(object sender, RoutedEventArgs e)
        {
            SetYWindow yWindow = new SetYWindow();
            yWindow.YMax = this._channel.Amplitude(this.CurrentValueType);
            if (yWindow.ShowDialog().GetValueOrDefault())
            {
                this._yMax = yWindow.YMax;
                this.Draw();
                //DrawY();
            }
        }

        private void Menu1_Checked(object sender, RoutedEventArgs e)
        {
            if (Equals(sender, this.Menu1))
            {
                this.Menu2.IsChecked = false;
                this.Menu3.IsChecked = false;
                this.CurrentValueType = AnalogChannel.GrafType.ACTIV;
            }

            if (Equals(sender, this.Menu2))
            {
                this.Menu1.IsChecked = false;
                this.Menu3.IsChecked = false;
                this.CurrentValueType = AnalogChannel.GrafType.RMS;
            }

            if (Equals(sender, this.Menu3))
            {
                this.Menu1.IsChecked = false;
                this.Menu2.IsChecked = false;
                this.CurrentValueType = AnalogChannel.GrafType.FIRST_HARMONIC;
            }
        }


    }
}
