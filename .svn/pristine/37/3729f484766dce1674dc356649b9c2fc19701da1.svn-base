using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Oscilloscope.View.FrequencyChartItem
{
    /// <summary>
    /// Логика взаимодействия для FrequencyChart.xaml
    /// </summary>
    public partial class FrequencyChart : UserControl
    {
        private const int MAX_COUNT = 11;

        private Rectangle[] _columns;
        private Label[] _valueLabels;
        private Label[] _fLabels;
        private AnalogChannel _channel;

        public AnalogChannel Channel
        {
            get { return this._channel; }
            set
            {
                this._channel = value;
               var binding = new Binding();
                binding.Source = this._channel.Options;
                binding.Path = new PropertyPath("Frequency");
                binding.Converter = new BooleanToVisibilityConverter();
                this.SetBinding(UIElement.VisibilityProperty, binding);
                this.Changed();
                this.InfoLabel.Content = this._channel.Name;
                foreach (var column in this._columns)
                {
                    column.Stroke = this._channel.ChannelBrush;
                  
                    column.Fill = new SolidColorBrush(Color.Multiply(((SolidColorBrush)this._channel.ChannelBrush).Color,(float) 0.5)  );
                }
                this.ColorPanel.Background = this._channel.ChannelBrush;
            }
        }

        private FrequencyChartOptions _options = new FrequencyChartOptions();
        public FrequencyChartOptions Options
        {
            set
            {
                this._options = value;
                this.Changed();
            }
        }

        private double[][] _harmonics;
        private double[] _factor;
        private int _index;
 
        public FrequencyChart()
        {
            InitializeComponent();
            this._columns = new Rectangle[MAX_COUNT];
            this._valueLabels = new Label[MAX_COUNT];
            this._fLabels = new Label[MAX_COUNT];
            for (int i = 0; i < MAX_COUNT; i++)
            {
                this._columns[i] = new Rectangle()
                    {
                        Height = 5,
                        Fill = Brushes.DeepSkyBlue,
                        VerticalAlignment = VerticalAlignment.Bottom,
                        Margin = new Thickness(30+50*i,0,0,40),
                        Stroke = Brushes.Blue,
                        StrokeThickness = 1,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Width = 20
                    };
                this._valueLabels[i] = new Label()
                    {
                        Margin = new Thickness(10 + 50 * i, 0, 0, 30),
                        VerticalAlignment = VerticalAlignment.Bottom,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalContentAlignment = VerticalAlignment.Top,
                        Width = 60,
                        HorizontalContentAlignment = HorizontalAlignment.Center
                    };
                this._fLabels[i] = new Label()
                {
                    Margin = new Thickness(10 + 50 * i, 0, 0,20),
                    VerticalAlignment = VerticalAlignment.Bottom,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalContentAlignment = VerticalAlignment.Top,
                    Width = 60,
                    HorizontalContentAlignment = HorizontalAlignment.Center
                };
                this._fLabels[i].Content = i*50;
                this.MainGrid.Children.Add(this._columns[i]);
                this.MainGrid.Children.Add(this._valueLabels[i]);
                this.MainGrid.Children.Add(this._fLabels[i]);
            }
            this._options.Changed += this.Changed;
        

        }

        private void Changed()
        {
            this._harmonics = new double[MAX_COUNT][];
            for (int i = 0; i < MAX_COUNT; i++)
            {
                this._harmonics[i] = this.Channel.PrepareHarmonic(i, this._options.PeriodCount);
            }
            for (int i = 0; i < MAX_COUNT; i++)
            {
                bool flag = i < this._options.HarmonicCount +1;
                this._columns[i].Visibility = flag ? Visibility.Visible : Visibility.Collapsed;
                this._valueLabels[i].Visibility = flag ? Visibility.Visible : Visibility.Collapsed;
                this._fLabels[i].Visibility = flag ? Visibility.Visible : Visibility.Collapsed;
            }
            switch (this._options.FrequencyValue)
            {
                    case FrequencyValue.ABSOLUTE:
                    {
                        this._factor = null;
                        break;
                    }
                    case FrequencyValue.RMS:
                    {
                        this._factor = this._channel.RmsValues;
                        break;
                    }
                default:
                    {
                        this._factor = this._harmonics[(int) (this._options.FrequencyValue - 1)];
                        break;
                    }

            }

            this.Width = 30 + 50*this._options.HarmonicCount;
            this.Set(this._index);
        }

        public void Set(int index)
        {
            this._index = index;
            double[] values = new double[this._options.HarmonicCount + 1];
            for (int i = 0; i < this._options.HarmonicCount+1; i++)
            {
                double value = 0.0;
                if (this._options.FrequencyValue == FrequencyValue.ABSOLUTE)
                {
                    value =Math.Abs(Math.Round(this._harmonics[i][index])) ;
                }
                else
                {
                    if (this._factor[index] != 0)
                    {
                        value = Math.Round(Math.Abs(this._harmonics[i][index]/this._factor[index]*100), 1);
                    }
                }
                values[i] = value;
       
            }
            double factor = values.Max()/100;
            if (factor ==0)
            {
                factor = 1;
            }
            for (int i = 0; i <values.Length ; i++)
            {
                this._columns[i].Height = values[i]/factor+2;
                this._valueLabels[i].Content = values[i];
                this._valueLabels[i].Margin =new Thickness(this._valueLabels[i].Margin.Left,0,0,values[i] / factor+40);
            }
        
        }

       
    }
}
