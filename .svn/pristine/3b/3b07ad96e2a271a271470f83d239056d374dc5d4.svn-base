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
using System.Windows.Shapes;

namespace Oscilloscope.View.AnalogChartItem
{
    /// <summary>
    /// Логика взаимодействия для SetYWindow.xaml
    /// </summary>
    public partial class SetYWindow : Window
    {
        public SetYWindow()
        {
            InitializeComponent();
        }

        private double _yMax;
        public double YMax
        {
            get { return _yMax; }
            set
            {
                _yMax = value;
                ValueTextBox.Text = _yMax.ToString(CultureInfo.InvariantCulture);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                YMax = double.Parse(ValueTextBox.Text, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
                if (YMax<= 0)
                {
                    throw new Exception();
                }
                DialogResult = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Проверте правильность ввода");
            }
            
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
