using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Oscilloscope.ComtradeFormat;
using Oscilloscope.View.AnalogChartItem;
using Oscilloscope.View.PieChartItem;

namespace Oscilloscope.View.MainItem
{
    /// <summary>
    /// Логика взаимодействия для WorkplaceControl.xaml
    /// </summary>
    public partial class WorkplaceControl : UserControl
    {
        private AnalogChart[] _analogCharts;
        private CommonDiscretChartItems.CommonDiscretChart _commonDiscretChart;
        private Zoom _zoom = Zoom.Default;
        private AnalogChartOptions[] _analogChartOptionses;

        public const int BASE_CHANGE = 20;

        private AnalogChart[] _addedCharts = new AnalogChart[0];
        private AnalogChartOptions[] _addedChartOptionses;
        private AnalogChannel[] _addedChannels;

        private PieChartOptions _options;

        private bool _runOscVisible;

        public MarkerStatusBar MarkerStatusBarControl
        {
            get { return this._markerStatusBarControl; }
            set
            {
                this._markerStatusBarControl = value;
                if (this._analogCharts.Length != 0)
                    this._markerStatusBarControl.Maximum = this._analogCharts[0].Channel.Length - 1;
                else
                    this._markerStatusBarControl.Maximum = this._commonDiscretChart.Channels[0].Length-1;
                this._markerStatusBarControl.MoveMarker1 += this.MainWindow_MoveMarker;
                this._markerStatusBarControl.MoveMarker2 += this.MainWindow_MoveMarker2;
                this._markerStatusBarControl.Marker1 = this.MarkersBar.Markers.Marker1;
                this._markerStatusBarControl.Marker2 = this.MarkersBar.Markers.Marker2;
            }
        }

        public AnalogChart[] AnalogCharts
        {
            get { return this._analogCharts; }
        }

        public AnalogChart[] AddedCharts
        {
            get { return this._addedCharts; }
        }

        public bool RunOscVisible
        {
            get { return this._runOscVisible; }
            set
            {
                this._runOscVisible = value;
                foreach (AnalogChart analogChart in this._analogCharts)
                {
                    analogChart.RunOscVisible = this._runOscVisible;
                }
                foreach (AnalogChart addedChart in this._addedCharts)
                {
                    addedChart.RunOscVisible = this._runOscVisible;
                }
                if (this._commonDiscretChart != null)
                { 
                    this._commonDiscretChart.RunOscVisible = this._runOscVisible;
                }
            }
        }

        public PieChartOptions CurrentPieChartOptions
        {
            get { return this._options; }
        }

        public AnalogChannel[] AddedChannels
        {
            get { return this._addedChannels; }
            set
            {
                if (this._addedCharts != null)
                {
                    foreach (AnalogChart addedChart in this._addedCharts)
                    {
                        this.ChartsPanel.Children.Remove(addedChart);
                        addedChart.MoveMarker -= this.MainWindow_MoveMarker;
                        addedChart.MoveMarker2 -= this.MainWindow_MoveMarker2;
                    }
                }

                this._addedChannels = value;

                this._addedCharts = new AnalogChart[this._addedChannels.Length];
                this._addedChartOptionses = new AnalogChartOptions[this._addedChannels.Length];
                for (int i = 0; i < this._addedChannels.Length; i++)
                {
                    this._addedCharts[i] = new AnalogChart();
                    this._addedCharts[i].HorizontalAlignment = HorizontalAlignment.Stretch;
                    int discretIndex = this.ChartsPanel.Children.IndexOf(this._commonDiscretChart);
                    this.ChartsPanel.Children.Insert(discretIndex, this._addedCharts[i]);
                    this._addedCharts[i].MoveMarker += this.MainWindow_MoveMarker;
                    this._addedCharts[i].MoveMarker2 += this.MainWindow_MoveMarker2;
                    this._addedCharts[i].Channel = this._addedChannels[i];
                    this._addedChartOptionses[i] = this._addedCharts[i].Options;
                    this._addedCharts[i].SetZoom(this._zoom);
                    this._addedCharts[i].SetMarker(this.MarkersBar.Markers.Marker1, this.MarkersBar.Markers.Marker1Offset);
                    this._addedCharts[i].SetMarker2(this.MarkersBar.Markers.Marker2, this.MarkersBar.Markers.Marker2Offset);
                }
            }
        }

        private ComTrade _data;
        public event Action<int> MoveMarker;
        private MarkerStatusBar _markerStatusBarControl;

        public ComTrade Data
        {
            get { return this._data; }
            set
            {
                this._data = value;
                if(this._data.AnalogChannels.Length != 0)
                    this._options = new PieChartOptions(this._data.AnalogChannels);
                this.Display();
            }
        }
        
        public WorkplaceControl()
        {
            this.InitializeComponent();
            this.MarkersBar.Move += this.MainWindow_MoveMarker;
            this.MarkersBar.MoveSecond += this.MainWindow_MoveMarker2;
        }
        
        public void Squeeze()
        {
            this.SetZoom();

        }

        private void ScrollViewer_ScrollChanged_1(object sender, ScrollChangedEventArgs e)
        {

            if ((Math.Abs(e.VerticalChange) <= 1.0) && (Math.Abs(e.ViewportHeightChange) <= 1.0))
            {
                return;
            }
            if (this._analogCharts != null)
            {
                foreach (AnalogChart analogChart in this._analogCharts)
                {
                    analogChart.VertivalScroll();
                }
            }

            if (this._addedCharts != null)
            {
                foreach (AnalogChart addedChart in this._addedCharts)
                {
                    addedChart.VertivalScroll();
                }
            }
            if (this._commonDiscretChart != null)
            {
                this._commonDiscretChart.VerticalScroll();
            }
                
        }

        private void Display()
        {
            this.ChartsPanel.Children.Clear();

            this._analogCharts = new AnalogChart[this.Data.AnalogChannels.Length];
            this._analogChartOptionses = new AnalogChartOptions[this._analogCharts.Length];
            for (int i = 0; i < this._analogCharts.Length; i++)
            {
                this._analogCharts[i] = new AnalogChart();
                this._analogCharts[i].HorizontalAlignment = HorizontalAlignment.Stretch;

                this.ChartsPanel.Children.Add(this._analogCharts[i]);
                this._analogCharts[i].MoveMarker += this.MainWindow_MoveMarker;
                this._analogCharts[i].MoveMarker2 += this.MainWindow_MoveMarker2;
                this._analogCharts[i].Channel = this.Data.AnalogChannels[i];
                this._analogChartOptionses[i] = this._analogCharts[i].Options;
            }
                
            if (this.Data.DiscretChannels.Length > 0)
            {
                this._commonDiscretChart = new CommonDiscretChartItems.CommonDiscretChart();
                this._commonDiscretChart.Channels = this.Data.DiscretChannels;
                this._commonDiscretChart.MoveMarker += this.MainWindow_MoveMarker;
                this._commonDiscretChart.MoveMarker2 += this.MainWindow_MoveMarker2;
                this._commonDiscretChart.HorizontalAlignment = HorizontalAlignment.Stretch;
                this.ChartsPanel.Children.Add(this._commonDiscretChart);
            }


            this.MarkersBar.Lenght = this.Data.Cfg.Size - 1;
            this.MainWindow_MoveMarker(0);
            this.MainWindow_MoveMarker2(this.MarkersBar.Markers.Maximum);
        }

        public void ShowFactorForm()
        {
            FactorForm _factorForm = new FactorForm();
            _factorForm.Options = this._analogChartOptionses;
            _factorForm.ShowDialog();
        }
        
        private void MainWindow_MoveMarker2(int obj)
        {
            this.MarkersBar.Markers.Marker2 = obj;
            foreach (AnalogChart analogChart in this._analogCharts)
            {
                analogChart.SetMarker2(this.MarkersBar.Markers.Marker2, this.MarkersBar.Markers.Marker2Offset);
            }
            if (this._commonDiscretChart != null)
            {
                this._commonDiscretChart.SetMarker2(obj, this.MarkersBar.Markers.Marker2Offset);
            }
                

            foreach (AnalogChart addedChart in this._addedCharts)
            {
                addedChart.SetMarker2(this.MarkersBar.Markers.Marker2, this.MarkersBar.Markers.Marker2Offset);
            }

            if (this.MarkerStatusBarControl != null)
            {
                this.MarkerStatusBarControl.Marker2 = obj;
            }
        }

        public void MainWindow_MoveMarker(int obj)
        {
            this.MarkersBar.Markers.Marker1 = obj;
            foreach (AnalogChart analogChart in this._analogCharts)
            {
                analogChart.SetMarker(this.MarkersBar.Markers.Marker1, this.MarkersBar.Markers.Marker1Offset);
            }
            if (this._commonDiscretChart != null)
            {
                this._commonDiscretChart.SetMarker(obj, this.MarkersBar.Markers.Marker1Offset);
            }
            

            foreach (AnalogChart analogChart in this._addedCharts)
            {
                analogChart.SetMarker(this.MarkersBar.Markers.Marker1, this.MarkersBar.Markers.Marker1Offset);
            }

            if (this.MarkerStatusBarControl != null)
            {
                this.MarkerStatusBarControl.Marker1 = obj;
            }
            if (this.MoveMarker != null)
            {
                this.MoveMarker.Invoke(obj);
            }
        }

        public void IncreaseX()
        {
            this._zoom.IncreaseX();
            this.SetZoom();
        }

        public void DecreaseX()
        {
            this._zoom.DecreaseX();
            this.SetZoom();
        }

        public void IncreaseY()
        {
            this._zoom.IncreaseY();
            this.SetZoom();
        }

        public void DecreaseY()
        {
            this._zoom.DecreaseY();
            this.SetZoom();
        }

        private void SetZoom()
        {
            int markerPos = (int)((this.HorizontalScroll.Maximum + BASE_CHANGE)*this.MarkersBar.Value/this.MarkersBar.Lenght);
            double oldMax = (this.HorizontalScroll.Maximum + BASE_CHANGE);
            bool flag = this.HorizontalScroll.Value <= markerPos &&
                        markerPos < this.HorizontalScroll.Value + BASE_CHANGE;
            
            this.HorizontalScroll.Maximum = BASE_CHANGE*(this._zoom.ZoomX - 1);
            foreach (AnalogChart analogChart in this._analogCharts)
            {
                analogChart.SetZoom(this._zoom);
            }
            foreach (AnalogChart analogChart in this._addedCharts)
            {
                analogChart.SetZoom(this._zoom);
            }
            if (this._commonDiscretChart != null)
            {
                this._commonDiscretChart.SetZoom(this._zoom);
            }
                
            this.MarkersBar.SetZoom(this._zoom);
            if (flag)
            {
                this.HorizontalScroll.Value =
                    (int) (this.HorizontalScroll.Maximum*this.MarkersBar.Value/this.MarkersBar.Lenght);
            }
            else
            {
                this.HorizontalScroll.Value = (this.HorizontalScroll.Maximum)*(this.HorizontalScroll.Value + BASE_CHANGE/2.0)/oldMax;
            }
        }

        public void ShowChannelOptions()
        {
            ChannelForm _channelForm = new ChannelForm();
            _channelForm.Options = this._analogChartOptionses;

            _channelForm.ShowDialog();
        }

        public void OneZoom()
        {
            this._zoom = Zoom.Default;
            this.SetZoom();
        }

        public bool PrimaryFactor
        {
            set
            {
                if (this._analogCharts != null)
                {
                    foreach (AnalogChart analogChart in this._analogCharts)
                    {
                        analogChart.Options.IsPrimaryFactor = value;
                    }
                }
            }
        }
        
        private void UserControl_PreviewMouseWheel_1(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftShift))
            {
                if (e.Delta > 0)
                {
                    this._zoom.IncreaseX();
                }
                if (e.Delta < 0)
                {
                    this._zoom.DecreaseX();
                }
                this.SetZoom();
                e.Handled = true;
            }
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                if (e.Delta > 0)
                {
                    this._zoom.IncreaseY();
                }
                if (e.Delta < 0)
                {
                    this._zoom.DecreaseY();
                }
                this.SetZoom();
                e.Handled = true;
            }
        }

        private void ScrollBar_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.HorizontalScroll.Value = Math.Round(e.NewValue);

            foreach (AnalogChart analogChart in this._analogCharts)
            {
                analogChart.NewScroll((int) this.HorizontalScroll.Value);
            }
            foreach (AnalogChart addedChart in this._addedCharts)
            {
                addedChart.NewScroll((int) this.HorizontalScroll.Value);
            }
            if (this._commonDiscretChart != null)
            {
                this._commonDiscretChart.NewScroll((int) this.HorizontalScroll.Value);
            }                
            this.MarkersBar.Markers.NewScroll((int) this.HorizontalScroll.Value);
        }
    }
}
