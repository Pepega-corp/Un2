using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using Oscilloscope.View.MainItem;

namespace Oscilloscope.View.PieChartItem
{
    /// <summary>
    /// Логика взаимодействия для PieChart.xaml
    /// </summary>
    public partial class PieChart
    {
        private static readonly Pen BlackPen = new Pen(Brushes.Black, 0.5);
 
        private PieChartOptions _options;

        private List<PieChannelInfo> _infos = new List<PieChannelInfo>();
        private readonly object _syns = new object();
        private double _factor;
        private Point _realMid;
        private int _index;
        private int _zoom = 1;
        private Size _size;


        static PieChart()
        {
            BlackPen.Freeze();
        }

        private WorkplaceControl _currentWorkplace;
        public WorkplaceControl CurrentWorkplace
        {
            get { return _currentWorkplace; }
            set
            {
                _currentWorkplace = value;
                this.Options = _currentWorkplace.CurrentPieChartOptions;
            }
        }


        private List<MenuItem> _menuItems = new List<MenuItem>(); 

        private PieChartOptions Options
        {
            get { return _options; }
            set
            {
                _options = value;
                _infos = _options.Infos;
                _options.VisiblyOptions.NeedRedraw += Redraw;

                foreach (var menuItem in _menuItems)
                {
                    ChannelsMenuRoot.Items.Remove(menuItem);
                }

                foreach (var info in _infos)
                {
                    _menuItems.Add(info.ChannelMenuItem);
                    ChannelsMenuRoot.Items.Add(info.ChannelMenuItem);
                }

                this.Marker.Lenght = _options.Lenght-1;
                InfoGrid.ItemsSource = _infos.Where(o => o.IsVisibly).ToArray();
                this.Redraw();
                _currentWorkplace.AddedChannels = this.Options.AddedChannel.ToArray();
            }
        }


        private void Redraw()
        {

            if (MainGrid.ActualHeight - InfoGrid.ActualHeight > MainGrid.ActualWidth - InfoGrid.ActualWidth)
            {
                Grid.SetRow(InfoGrid, 2);
                Grid.SetColumn(InfoGrid, 0);
                _size = new Size(MainGrid.ActualWidth, MainGrid.ActualHeight - InfoGrid.ActualHeight - MarkerRow.ActualHeight);
            }
            else
            {
                Grid.SetRow(InfoGrid, 1);
                Grid.SetColumn(InfoGrid, 1);
                _size = new Size(MainGrid.ActualWidth - InfoGrid.ActualWidth, MainGrid.ActualHeight - MarkerRow.ActualHeight);
            }
            if (Options==null)
            {
                this.Marker.Visibility = Visibility.Hidden;
            }
            if ((_size.Height < 1.0) || (_size.Width < 1.0))
            {
                return;
            }
            var task = new Task<RenderTargetBitmap>(Rend);
            task.ContinueWith(
                o =>
                {

                    this.Dispatcher.Invoke(new Action(
                                               () =>
                                               {
                                                   image.Source = o.Result;
                                               }
                                               ));
                });
            task.Start();
        }

        public void SetGraph(int obj)
        {
            if (Options == null)
            {
                return;
            }
            var curInfos = _infos.Where(o => o.IsVisibly);
            InfoGrid.ItemsSource = curInfos.ToArray();
            _index = obj;
            foreach (var pieChannelInfo in curInfos)
            {
                pieChannelInfo.SetIndex(obj);
            }

            this.Marker.Value = obj;
            Redraw();
        }

        private void InfoGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.Redraw();
        }

        public PieChart()
        {
            InitializeComponent();
       
        }
        public Action<int> MoveMarker; 

        private void Marker_Move(int obj)
        {
            if (this.MoveMarker != null)
            {
                this.MoveMarker.Invoke(obj);
            }
        }

        /// <summary>
        /// Меню - Настройка
        /// </summary>
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            if (Options == null)
            {
                MessageBox.Show("Осциллограмма не загружена");
                return;
            }
            var optionsForm = new PieChartOptionsForm();
            optionsForm.Marker1 = CurrentWorkplace.MarkersBar.Markers.Marker1;
            optionsForm.Marker2 = CurrentWorkplace.MarkersBar.Markers.Marker2;
            optionsForm.ChartOptions = Options;

            if (optionsForm.ShowDialog().GetValueOrDefault())
            {
                this.Options = optionsForm.ChartOptions;
            }
            this.Redraw();
        }

        private void UserControl_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            this.Redraw();
        }


        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.Filter="Файл параметров круговой диаграммы|*.pco";

            if (dialog.ShowDialog().Value)
            {
                Options.Save(dialog.FileName);
            }
        }

 

        private void ShowAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (var info in _infos)
            {
                info.IsVisibly = true;
            }
        }

        private void HideAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (var info in _infos)
            {
                info.IsVisibly = false;
            }
        }

        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {

        }


        private bool _move;
        private Point _startPosition;
        private Point _offset = new Point(0,0);
        private Point _midTrans = new Point(0, 0);
        

        private void image_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _move = true;
            var temp = e.GetPosition(image);
            temp.Offset(-_midTrans.X, -_midTrans.Y);
            _startPosition = temp;

        }

        private void image_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _move = false;
        }

        private void image_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var newPosition = e.GetPosition(image);
            if ((_zoom > 1)&&(_move))
            {
               newPosition.Offset(- _startPosition.X,- _startPosition.Y);
                _midTrans = newPosition;
                this.Redraw();
            }
        }


        private void image_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {

            if ((e.Delta < 0) && (_zoom > 1))
            {
                var temp = e.GetPosition(image);
                var off = new Point((_realMid.X - temp.X), (_realMid.Y - temp.Y));
                _offset = new Point(_offset.X - (off.X / _zoom), _offset.Y - (off.Y / _zoom));
                _zoom--;
            }
            if (e.Delta > 0)
            {
               var temp = e.GetPosition(image);
                var off = new Point((_realMid.X - temp.X), (_realMid.Y - temp.Y));
                _offset = new Point(_offset.X +(off.X / _zoom)  ,_offset.Y + (off.Y / _zoom));
                  _zoom++;
                }
            if (_zoom == 1)
            {
                _offset = new Point(0, 0);
                _midTrans=new Point(0,0);
            }
            
            Redraw();
        }


        private RenderTargetBitmap Rend()
        {
            lock (_syns)
            {

                var drawingVisual = new DrawingVisual();
                const int border = 20;
                var radius = (int)((Math.Min(_size.Width, _size.Height) - border) / 2) * _zoom;

                _realMid = new Point(_size.Width / 2, _size.Height / 2);

                _realMid.Offset(_offset.X, _offset.Y);

                _realMid.Offset(_midTrans.X, _midTrans.Y);

                using (DrawingContext drawingContext = drawingVisual.RenderOpen())
                {

                    //Заливка цветом
                    drawingContext.DrawRectangle(Brushes.White, null,
                                                 new Rect(0, 0, _size.Width, _size.Height));
                    drawingContext.DrawLine(BlackPen, new Point(_realMid.X - radius, _realMid.Y), new Point(_realMid.X + radius, _realMid.Y));
                    drawingContext.DrawLine(BlackPen, new Point(_realMid.X, _realMid.Y - radius), new Point(_realMid.X, _realMid.Y + radius));

                    var formattedText = new FormattedText(
                        "-R",
                        CultureInfo.GetCultureInfo("en-us"),
                        FlowDirection.LeftToRight,
                        new Typeface("Verdana"),
                        12,
                        Brushes.Black);
                    drawingContext.DrawText(formattedText, new Point(_realMid.X - radius - border / 2.0, _realMid.Y - formattedText.Height));

                    formattedText = new FormattedText(
                        "R, Ом",
                        CultureInfo.GetCultureInfo("en-us"),
                        FlowDirection.RightToLeft,
                        new Typeface("Verdana"),
                        12,
                        Brushes.Black);
                    drawingContext.DrawText(formattedText, new Point(border / 2.0 + radius + _realMid.X, _realMid.Y - formattedText.Height));

                    formattedText = new FormattedText(
                        "X, Ом",
                        CultureInfo.GetCultureInfo("en-us"),
                        FlowDirection.LeftToRight,
                        new Typeface("Verdana"),
                        12,
                        Brushes.Black);
                    drawingContext.DrawText(formattedText, new Point(_realMid.X+10, _realMid.Y - radius));

                    formattedText = new FormattedText(
                        "-X",
                        CultureInfo.GetCultureInfo("en-us"),
                        FlowDirection.LeftToRight,
                        new Typeface("Verdana"),
                        12,
                        Brushes.Black);

                    drawingContext.DrawText(formattedText, new Point(_realMid.X+10, radius + _realMid.Y - formattedText.Height));
                    if (Options!= null &&Options.IsCorrect)
                    {
                        var charasteristicsMaxPoints = this.Options.CharacteristicsVisibly.Select(o => o.MaxPoint);
                        IEnumerable<Point> result = charasteristicsMaxPoints;
                        foreach (PieChannelInfo info in _infos)
                        {
                            if (info.IsVisibly)
                            {
                                result = result.Concat(info.Values.Where((e, index) => index >= _options.StartTime && index < _options.EndTime));
                            }
                            
                        }
                        double max = 0.0;
                        if (result.Count() > 0)
                        {
                            max = result.Max(o => Math.Max(Math.Abs(o.X), Math.Abs(o.Y)));





                            var midValue = AnalogChannel.GetMiddleValue(max);

                            _factor = radius/max;
                            double temp = 0;
                            double addedKoef = 1/(double) _zoom;
                            while (temp <= 2 - addedKoef)
                            {
                                temp += addedKoef;
                                double resultPOint = midValue*temp;
                                if (resultPOint < 10)
                                {
                                    resultPOint = Math.Round(resultPOint, 2);
                                }
                                else
                                {
                                    if (resultPOint < 100)
                                    {
                                        resultPOint = Math.Round(resultPOint, 1);
                                    }
                                    else
                                    {
                                        resultPOint = Math.Round(resultPOint);
                                    }
                                }
                                DrawAxis(drawingContext, resultPOint);
                            }



                            foreach (var info in _infos)
                            {
                                if (info.IsVisibly)
                                {
                                    var points =
                                        info.Values.Select(
                                            o => new Point(o.X*_factor + _realMid.X, _realMid.Y - o.Y*_factor))
                                            .ToArray();
                                    for (int i = _options.StartTime; i < _options.EndTime - 1; i++)
                                    {
                                        if (i == _index)
                                        {
                                            DrawPointer(drawingContext, points[i], info.ChannelPen);
                                        }
                                        if (((int) points[i].X == (int) points[i + 1].X) &&
                                            ((int) points[i].Y == (int) points[i + 1].Y))
                                        {
                                            continue;
                                        }
                                        drawingContext.DrawLine(info.ChannelPen, points[i], points[i + 1]);


                                    }
                                }
                            }

                            foreach (var characteristic in this.Options.CharacteristicsVisibly)
                            {
                                characteristic.Draw(drawingContext, _realMid, _factor, radius);
                            }
                        }
                    }
                }

                var bmp = new RenderTargetBitmap((int)_size.Width, (int)_size.Height, 0, 0,
                                                                PixelFormats.Pbgra32);
                bmp.Render(drawingVisual);

                bmp.Freeze();
                return bmp;
            }
        }
        private void DrawPointer(DrawingContext drawingContext, Point point, Pen pen)
        {
            const int r = 2;
            const int s = 2;
            const int l = 4;
            drawingContext.DrawEllipse(pen.Brush, BlackPen, point, r, r);
            drawingContext.DrawLine(pen, new Point(point.X, point.Y - s-r), new Point(point.X, point.Y - s-l-r));//1
            drawingContext.DrawLine(pen, new Point(point.X - s - r, point.Y), new Point(point.X - s - l - r, point.Y));//2
            drawingContext.DrawLine(pen, new Point(point.X, point.Y + s+r), new Point(point.X, point.Y + s + l+r));//3
            drawingContext.DrawLine(pen, new Point(point.X + s + r, point.Y), new Point(point.X + s + l + r, point.Y));//4
        }

        private void DrawAxis(DrawingContext drawingContext, double midValue)
        {
            var formattedText = new FormattedText(
                midValue.ToString(CultureInfo.InvariantCulture),
                CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight,
                new Typeface("Verdana"),
                12,
                Brushes.Black);

            drawingContext.DrawText(formattedText,
                                    new Point(_realMid.X + midValue*_factor - formattedText.Width/2, _realMid.Y + 5));
            drawingContext.DrawLine(BlackPen, new Point(_realMid.X + midValue*_factor, _realMid.Y + 5),
                                    new Point(_realMid.X + midValue*_factor, _realMid.Y - 5));



            drawingContext.DrawText(formattedText,
                                    new Point(_realMid.X - 5 - formattedText.Width,
                                              _realMid.Y - midValue*_factor - formattedText.Height/2));
            drawingContext.DrawLine(BlackPen, new Point(_realMid.X + 5, _realMid.Y - midValue*_factor),
                                    new Point(_realMid.X - 5, _realMid.Y - midValue*_factor));


            formattedText = new FormattedText(
                (-midValue).ToString(CultureInfo.InvariantCulture),
                CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight,
                new Typeface("Verdana"),
                12,
                Brushes.Black);

            drawingContext.DrawText(formattedText,
                                    new Point(_realMid.X - midValue*_factor - formattedText.Width/2, _realMid.Y + 5));
            drawingContext.DrawLine(BlackPen, new Point(_realMid.X - midValue*_factor, _realMid.Y + 5),
                                    new Point(_realMid.X - midValue*_factor, _realMid.Y - 5));

            drawingContext.DrawText(formattedText,
                                    new Point(_realMid.X - 5 - formattedText.Width,
                                              _realMid.Y + midValue*_factor - formattedText.Height/2));
            drawingContext.DrawLine(BlackPen, new Point(_realMid.X + 5, _realMid.Y + midValue*_factor),
                                    new Point(_realMid.X - 5, _realMid.Y + midValue*_factor));
        }


        private void MainGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.Redraw();
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
        /// <summary>
        /// Оптимизация масштаба
        /// </summary>
        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            this._zoom = 1;
            this._offset = new Point(0, 0);
            this._midTrans = new Point(0, 0);
            this.Redraw();
        }

        private void MenuItem_Click_6(object sender, RoutedEventArgs er)
        {
            this._zoom = 1;
            this._offset = new Point(0, 0);
            this._midTrans = new Point(0, 0);

            var cMax = 0.0;
            try
            {
                cMax = this.Options.CharacteristicsVisibly.Select(o => Math.Max(o.MaxPoint.X, o.MaxPoint.Y)).Max();
            }
            catch (Exception)
            {
                
         
            }
              

            var charasteristicsMaxPoints = this.Options.CharacteristicsVisibly.Select(o => o.MaxPoint);
            IEnumerable<Point> result = charasteristicsMaxPoints;
            foreach (PieChannelInfo info in _infos)
            {
                if (info.IsVisibly)
                {
                    result = result.Concat(info.Values.Where((e, index) => index >= _options.StartTime && index < _options.EndTime));
                }

            }
            var max = result.Max(o => Math.Max(Math.Abs(o.X), Math.Abs(o.Y)));
            if (max>cMax)
            {
                this._zoom = (int) Math.Ceiling(max/cMax);
            }
            this.Redraw();
        }
    }
}
