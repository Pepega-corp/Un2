using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace Oscilloscope.View.PieChartItem
{
    /// <summary>
    /// Логика взаимодействия для ZEditControl.xaml
    /// </summary>
    public partial class ZEditControl : UserControl
    {
        public ZEditControl()
        {
            InitializeComponent();
        }

        public event Action ChannelsChanged;

        private void Stage1Cb_Checked(object sender, RoutedEventArgs e)
        {
            Z1R0Tb.IsEnabled = Z1R1Tb.IsEnabled = Z1X0Tb.IsEnabled = Z1X1Tb.IsEnabled = true;
            ChannelsChangedRaise();
        }

        private void ChannelsChangedRaise()
        {
            if (ChannelsChanged!= null)
            {
                ChannelsChanged.Invoke();
            }
        }

        private void Stage1Cb_Unchecked(object sender, RoutedEventArgs e)
        {
            Z1R0Tb.IsEnabled = Z1R1Tb.IsEnabled = Z1X0Tb.IsEnabled = Z1X1Tb.IsEnabled = false;
            ChannelsChangedRaise();
        }

        private ZnOptions _zn;
        public ZnOptions Zn
        {
            get
            {
                if (Stage1Cb.IsChecked.GetValueOrDefault())
                {
                    var z1 = new ZnOptions();

                    z1.R0 = double.Parse(this.Z1R0Tb.Text, CultureInfo.CurrentCulture);
                    z1.R1 = double.Parse(this.Z1R1Tb.Text, CultureInfo.CurrentCulture);
                    z1.X0 = double.Parse(this.Z1X0Tb.Text, CultureInfo.CurrentCulture);
                    z1.X1 = double.Parse(this.Z1X1Tb.Text, CultureInfo.CurrentCulture);
                    return z1;
                }
                return null;
            }
            set
            {

                if (value != null)
                {
                    Stage1Cb.IsChecked = true;
                    this.Z1R0Tb.Text = value.R0.ToString(CultureInfo.CurrentCulture);
                    this.Z1R1Tb.Text = value.R1.ToString(CultureInfo.CurrentCulture);
                    this.Z1X0Tb.Text = value.X0.ToString(CultureInfo.CurrentCulture);
                    this.Z1X1Tb.Text = value.X1.ToString(CultureInfo.CurrentCulture);
                }
                else
                {
                    Stage1Cb.IsChecked = false;
                    this.Z1R0Tb.Text = "1";
                    this.Z1R1Tb.Text = "1";
                    this.Z1X0Tb.Text = "1";
                    this.Z1X1Tb.Text = "1";

                }

            }
        }
        public int Index
        {
            get { return 0; }
            set { Group1.Header += value.ToString(); }
        }
    }
}
