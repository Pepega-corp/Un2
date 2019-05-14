using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Oscilloscope.View.FrequencyChartItem
{
    /// <summary>
    /// Логика взаимодействия для FrequencyChartPanel.xaml
    /// </summary>
    public partial class FrequencyChartPanel : UserControl
    {
        private AnalogChannel[] _channels;
        private FrequencyChart[] _charts;
        private FrequencyChartOptions _options = new FrequencyChartOptions();
        public AnalogChannel[] Channels
        {
            get { return _channels; }
            set
            {
                _channels = value;
                _charts = new FrequencyChart[_channels.Length];
                while (ChannelMenu.Items.Count>3)
                {
                    ChannelMenu.Items.RemoveAt(3);
                }
                MainPanel.Children.Clear();
                
                for (int i = 0; i < value.Length; i++)
                {
                    var analogChannel = value[i];
                    var chart = new FrequencyChart();
                    chart.Channel = analogChannel;
                    MainPanel.Children.Add(chart);
                    _charts[i] = chart; 
                    chart.HorizontalAlignment = HorizontalAlignment.Center;
                    MenuItem item = new MenuItem();
                    item.Header = analogChannel.Name;
                    item.IsCheckable = true;
                    CommonHelper.CreateCheckedBinding(analogChannel.Options, "Frequency", item);


                    ChannelMenu.Items.Add(item);
                }

            
            }
        }
        public void Set(int obj)
        {
            foreach (var frequencyChart in _charts)
            {
                frequencyChart.Set(obj);
            }
        }
        public FrequencyChartPanel()
        {
            InitializeComponent();
        }

        private void ActiveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            FrequencyChartOptionsForm _optionsForm = new FrequencyChartOptionsForm();
            _optionsForm.Options = _options;
            if (_optionsForm.ShowDialog() == true)
            {
                _options = _optionsForm.Options;
                foreach (var frequencyChart in _charts)
                {
                    frequencyChart.Options = _options;
                }
            }
            
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            foreach (var analogChannel in _channels)
            {
                analogChannel.Options.Frequency = true;
            }
        }



        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            foreach (var analogChannel in _channels)
            {
                analogChannel.Options.Frequency = false;
            }
        }


     

    }
}
