using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Oscilloscope.View.VectorChartItem
{
    /// <summary>
    /// Логика взаимодействия для PieChart.xaml
    /// </summary>
    public partial class VectorChart : UserControl
    {
        private static Pen DashBlackPen = new Pen(Brushes.Black, 0.5){DashStyle = new DashStyle(new[]{2.0,5.0},1)};
        private static Pen BlackPen = new Pen(Brushes.Black, 0.5) ;
        private static readonly Dictionary<Brush,Pen> PensI;
        private static readonly Dictionary<Brush, Pen> PensU;
        private Size _size;
        private const int CIRCLE_COUNT = 5;
        private AnalogChannel[] _channels = new AnalogChannel[0];
        private int _index;



        private VectorChannelInfo[] _infos;
        private RadioButton _absoluteRadioButton;
        public AnalogChannel[] Channels
        {
            get { return _channels; }
            set
            {
                _channels = value;
                this.Marker.Lenght = _channels[0].Length-1;
                //Очистка меню
                while (ChannelMenu.Items.Count > 3)
                {
                    ChannelMenu.Items.RemoveAt(3);
                }
                StartPointMenu.Items.Clear();
                
                //Абсолютный угол
                MenuItem menuItem = new MenuItem();
                RadioButton itemRadio = new RadioButton();
                _absoluteRadioButton = itemRadio;
                menuItem.Click += OnMenuItemOnClick;
                itemRadio.GroupName = "Group1";
                itemRadio.Checked +=itemRadio_Checked;
                itemRadio.IsChecked = true;
                menuItem.Icon = itemRadio;
                menuItem.Header = "Абсолютный угол";
                
                StartPointMenu.Items.Add(menuItem);
                StartPointMenu.Items.Add(new Separator());
                foreach (var analogChannel in _channels)
                {
                    analogChannel.Options.PropertyChanged += (sender, args) =>
                    {
                        if (args.PropertyName == "Vector")
                        {
                            Redraw();
                        }
                    };
                    MenuItem item = new MenuItem();
                    item.Header = analogChannel.Name;
                    //Меню каналы
                    Binding binding = new Binding();
                    binding.Source = analogChannel.Options;
                    binding.Path = new PropertyPath("Vector");
                    item.IsCheckable = true;
                    item.SetBinding(MenuItem.IsCheckedProperty, binding);
                    item.Unchecked += item_Unchecked;
                    ChannelMenu.Items.Add(item);
                    //Меню отсчёты 
                    menuItem = new MenuItem();
                    itemRadio = new RadioButton();
                    menuItem.Click += OnMenuItemOnClick;
                    itemRadio.GroupName = "Group1";
                    menuItem.Icon = itemRadio;
                    menuItem.Header = item.Header;
                    itemRadio.Tag = menuItem;
                    StartPointMenu.Items.Add(menuItem);
                    binding = new Binding();
                    //binding.Converter = new BooleanToVisibilityConverter();
                    binding.Source = item;
                    binding.Path = new PropertyPath("IsChecked");
                    menuItem.SetBinding(UIElement.IsEnabledProperty, binding);
                    itemRadio.Checked += itemRadio_Checked;

                    item.Tag = itemRadio;
                }
                Redraw();
            }
        }

        void item_Unchecked(object sender, RoutedEventArgs e)
        {
            var radioButton = ((RadioButton) ((MenuItem) sender).Tag);
            if (radioButton.IsChecked.GetValueOrDefault())
            {
                _absoluteRadioButton.IsChecked = true;
            }
        }

        private string _startingPoint = string.Empty;
        private void itemRadio_Checked(object sender, RoutedEventArgs e)
        {
            var radioButton = (RadioButton) sender;

            if (Equals(radioButton, _absoluteRadioButton))
            {
                _startingPoint = string.Empty;
            }
            else
            {
                _startingPoint = ((MenuItem) radioButton.Tag).Header.ToString();
            }
            this.Redraw();
        }

 

        private void OnMenuItemOnClick(object sender, RoutedEventArgs args)
        {
            var radioButton = (RadioButton) ((MenuItem) sender).Icon;
  
            radioButton.IsChecked = true;
        }

        public Action<int> MoveMarker; 

        public void SetGraph(int index)
        {
            _index = index;
            this.Marker.Value = index;
            Redraw();
        }

        static VectorChart()
        {
            DashBlackPen.Freeze();

            BlackPen.Freeze();
            PensI = new Dictionary<Brush, Pen>();
            PensU = new Dictionary<Brush, Pen>();
            foreach (var brush in AnalogChannel.DefaultBrushes)
            {
                var penI = new Pen(brush, 1);
                penI.Freeze();
                var penU = new Pen(brush, 1) {DashStyle = new DashStyle(new[] {2.0, 5.0}, 1)};
                penU.Freeze();
                PensI.Add(brush, penI);
                PensU.Add(brush, penU);
            }
        }

        public VectorChart()
        {
            InitializeComponent();
        }

        private bool _isMaximized;

        public bool IsMaximized
        {
            get { return _isMaximized; }
            set
            {
                _isMaximized = value;
                if (_isMaximized)
                {
                    Marker.Visibility = Visibility.Visible;
                }
                else
                {
                    Marker.Visibility = Visibility.Collapsed;
                }
            }
        }

        /// <summary>
        /// Отрисовка контрола
        /// </summary>
        /// <returns>Текущее изображение</returns>
        private RenderTargetBitmap Render()
        {
            DrawingVisual drawingVisual = new DrawingVisual();
            var border = 20;
            var radius = (int) ((Math.Min(_size.Width, _size.Height) - border)/2);
            var mid = radius + border/2;
            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                //Заливка цветом
                drawingContext.DrawRectangle(Brushes.White, null,
                                             new Rect(0, 0, _size.Width, _size.Height));
                Pen pen;
                for (int i = 0; i < CIRCLE_COUNT; i++)
                {
                    pen = i == 0 ? BlackPen : DashBlackPen;
                    var currentRadius = radius/(double) CIRCLE_COUNT*(CIRCLE_COUNT - i);
                    drawingContext.DrawEllipse(null, pen, new Point(mid, mid), currentRadius, currentRadius);
                }

                for (int i = 0; i < 360; i += 30)
                {
                    pen = (i%90) == 0 ? BlackPen : DashBlackPen;
                    var х1 = mid + (Math.Cos(i*Math.PI/180.0)*radius);
                    var у1 = mid - (Math.Sin(i*Math.PI/180.0)*radius);
                    drawingContext.DrawLine(pen, new Point(mid, mid), new Point(х1, у1));
                }


                var formattedText = new FormattedText(
                    "180",
                    CultureInfo.GetCultureInfo("en-us"),
                    FlowDirection.LeftToRight,
                    new Typeface("Verdana"),
                    12,
                    Brushes.Black);
                drawingContext.DrawText(formattedText, new Point(border/2.0, mid));

                formattedText = new FormattedText(
                    "0",
                    CultureInfo.GetCultureInfo("en-us"),
                    FlowDirection.RightToLeft,
                    new Typeface("Verdana"),
                    12,
                    Brushes.Black);
                drawingContext.DrawText(formattedText, new Point(border/2.0 + radius*2, mid));

                formattedText = new FormattedText(
                    "90",
                    CultureInfo.GetCultureInfo("en-us"),
                    FlowDirection.LeftToRight,
                    new Typeface("Verdana"),
                    12,
                    Brushes.Black);
                drawingContext.DrawText(formattedText, new Point(mid, border/2.0));

                formattedText = new FormattedText(
                    "270",
                    CultureInfo.GetCultureInfo("en-us"),
                    FlowDirection.LeftToRight,
                    new Typeface("Verdana"),
                    12,
                    Brushes.Black);
                drawingContext.DrawText(formattedText, new Point(mid, border/2.0 + radius*2 - formattedText.Height));

                if (Channels != null)
                {
                    var baseChannel = this.Channels.FirstOrDefault(o => o.Name == _startingPoint);
                    var baseAngle = 0.0;
                    if (baseChannel != null)
                    {
                        baseAngle = GetAngle(baseChannel.FirstHarmonic[_index]);
                    }
                    var enabledVectors = this.Channels.Where(o => o.Options.Vector).ToArray();
                    this._infos = new VectorChannelInfo[enabledVectors.Length];
                    double maxValueI = enabledVectors.Where(o => o.ThisType == AnalogChannel.ChannelType.I).Select(channel => channel.FirstHarmonicDouble[_index]).Concat(new[] { 0.0 }).Max();
                    double maxValueU = enabledVectors.Where(o => o.ThisType== AnalogChannel.ChannelType.U).Select(channel => channel.FirstHarmonicDouble[_index]).Concat(new[] { 0.0 }).Max();
                    double maxValueUnknown = enabledVectors.Where(o=>o.ThisType== AnalogChannel.ChannelType.UNKNOWN).Select(channel => channel.FirstHarmonicDouble[_index]).Concat(new[] { 0.0 }).Max();
                    for (int i = 0; i < enabledVectors.Length; i++)
                    {
                        var max = 0.0;
                        Pen currentPen;
                        switch (enabledVectors[i].ThisType)
                        {
                            case  AnalogChannel.ChannelType.I:
                                {
                                    max = maxValueI;
                                    currentPen = PensI[enabledVectors[i].ChannelBrush];
                                    break;
                                }
                            case AnalogChannel.ChannelType.U:
                                {
                                    max = maxValueU;
                                    currentPen = PensU[enabledVectors[i].ChannelBrush];
                                    break;
                                }
                            default:
                                {
                                    max = maxValueUnknown;
                                    currentPen = BlackPen;
                                    break;
                                }
                        }
                        double factor = enabledVectors[i].FirstHarmonicDouble[_index] / max;
                        Point point = enabledVectors[i].FirstHarmonic[_index];
                        double a = this.GetAngle(point) - baseAngle;
                        if (a < 0)
                        {
                            a += 360;
                        }
                        double x1 = mid + (Math.Cos(-a * Math.PI / 180.0) * radius) * factor;
                        double y1 = mid + (Math.Sin(-a * Math.PI / 180.0) * radius) * factor;
                        drawingContext.DrawLine(currentPen, new Point(mid, mid), new Point(x1, y1));

                        this._infos[i] = new VectorChannelInfo(enabledVectors[i].Name, currentPen.Brush, 
                            AnalogChannel.Normalize(enabledVectors[i].FirstHarmonicDouble[_index], enabledVectors[i].Measure), Math.Round(a).ToString(CultureInfo.InvariantCulture));

                        if (Math.Sqrt(Math.Pow(x1 - mid, 2) + Math.Pow(y1 - mid, 2))<10)
                        {
                            continue; 
                        }

                        PathGeometry sad = new PathGeometry(new[]
                            {
                                new PathFigure(this.Rotate(new Point(0, 0), a, new Point(x1, y1)), new[]
                                    {
                                        new LineSegment(this.Rotate(new Point(-10, 3), -a, new Point(x1, y1)), false),
                                        new LineSegment(this.Rotate(new Point(-10, -3), -a, new Point(x1, y1)), false)
                                    }
                                               , true)
                            });

                        drawingContext.DrawGeometry(currentPen.Brush, currentPen, sad);
                    }
                }
            }

            RenderTargetBitmap bmp = new RenderTargetBitmap(radius*2 + border, radius*2 + border, 0, 0,
                                                            PixelFormats.Pbgra32);
            bmp.Render(drawingVisual);
            bmp.Freeze();
            return bmp;
        }

        private double GetAngle(Point point)
        {
            double angle = 0.0;
            if (Math.Abs(point.X) <= Math.Abs(point.Y))
            {
                angle = Math.Acos(point.X / Math.Sqrt((Math.Pow(point.X, 2) + Math.Pow(point.Y, 2)))) / Math.PI * 180.0;
                if (point.Y < 0)
                {
                    angle *= -1;
                }

            }
            else
                if (Math.Abs(point.X) > Math.Abs(point.Y))
            {
                angle = Math.Asin(point.Y / (Math.Sqrt(Math.Pow(point.X, 2) + Math.Pow(point.Y, 2)))) / Math.PI * 180.0;
                if (point.X < 0)
                {
                    angle = 180 - angle;
                }
            }
            if (angle < 0)
            {
                angle += 360;
            }
            //int mod = point.Y <= 0 ? -1 : 1;
            //double angle = Math.Acos(point.X / (1 + Math.Sqrt(Math.Pow(point.X, 2) + Math.Pow(point.Y, 2)))) / Math.PI * 180.0 * mod;
            return angle;
        }

        /// <summary>
        /// Поворот точки относительно центра
        /// </summary>
        /// <param name="point">Начальные координаты</param>
        /// <param name="angle">Угол поворота</param>
        /// <param name="mid">Точка начала координат</param>
        /// <returns>Координаты после поворота</returns>
        private Point Rotate(Point point, double angle, Point mid)
        {
            double x1 = point.X * Math.Cos(angle * Math.PI / 180.0) - point.Y * Math.Sin(angle * Math.PI / 180.0) + mid.X;
            double y1 = point.Y * Math.Cos(angle * Math.PI / 180.0) + point.X * Math.Sin(angle * Math.PI / 180.0) + mid.Y;
            return new Point(x1, y1);
        }

        private void UserControl_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            this.Redraw();
        }

  
        /// <summary>
        /// Запуск перерисовки
        /// </summary>
        private void Redraw()
        {
            if (MainGrid.ActualHeight -InfoGrid.ActualHeight> MainGrid.ActualWidth - InfoGrid.ActualWidth)
            {
                Grid.SetRow(InfoGrid,2);
                Grid.SetColumn(InfoGrid,0); 
                _size = new Size(MainGrid.ActualWidth , MainGrid.ActualHeight - InfoGrid.ActualHeight-MarkerRow.ActualHeight);
            }
            else
            {
                Grid.SetRow(InfoGrid, 1);
                Grid.SetColumn(InfoGrid, 1);
                _size = new Size(MainGrid.ActualWidth - InfoGrid.ActualWidth, MainGrid.ActualHeight - MarkerRow.ActualHeight);
            }

            if (!this.Channels.Any())
            {
                this.Marker.Visibility = Visibility.Hidden;
            }

            if ((_size.Height == 0.0)&&(_size.Width == 0.0))
            {
                return;
            }
            
            Task<RenderTargetBitmap> task = new Task<RenderTargetBitmap>(Render);
            task.ContinueWith(o =>
            {
                this.Dispatcher.Invoke(new Action(
                    () =>
                    {
                        try
                        {
                            image.Source = o.Result;
                            InfoGrid.ItemsSource = _infos;
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                    ));
            });
            task.Start();
        }

      
        /// <summary>
        /// Изменение размеров таблицы
        /// </summary>
        private void InfoGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.Redraw();
        }

        /// <summary>
        /// Отобразить все каналы
        /// </summary>
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            foreach (AnalogChannel analogChannel in _channels)
            {
                analogChannel.Options.Vector = true;
            }
        }
   
        /// <summary>
        /// Скрыть все каналы
        /// </summary>
        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            foreach (var analogChannel in _channels)
            {
                analogChannel.Options.Vector = false;
            }
        }
        /// <summary>
        /// Включение маркера
        /// </summary>
        private void MarkerMenuItem_Checked(object sender, RoutedEventArgs e)
        {
            this.Marker.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Отключение маркера
        /// </summary>
        private void MarkerMenuItem_Unchecked(object sender, RoutedEventArgs e)
        {
            this.Marker.Visibility = Visibility.Hidden;
            
        }

        private void Marker_Move(int obj)
        {
            if (this.MoveMarker != null)
            {
                this.MoveMarker.Invoke(obj);
            }
        }

        private void MainGrid_LayoutUpdated(object sender, EventArgs e)
        {
            
        }

  
    }
}
