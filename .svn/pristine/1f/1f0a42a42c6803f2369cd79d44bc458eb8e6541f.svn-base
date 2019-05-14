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
using System.Windows.Shapes;

namespace Oscilloscope.View.PieChartItem
{
    /// <summary>
    /// Логика взаимодействия для PolygonalCharacteristicOptionForm.xaml
    /// </summary>
    public partial class PolygonalCharacteristicOptionForm : Window
    {
       public PieChartCharacteristicOption Options { get; private set; }
        public PolygonalCharacteristicOptionForm()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            var res =new PieChartCharacteristicOption();
            res.F = double.Parse(FTb.Text);
            res.R = double.Parse(RTb.Text);
            res.X = double.Parse(XTb.Text);
            res.Enabled = true;
            Options = res;
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Options = new PieChartCharacteristicOption();
            this.DialogResult = false;
        }
    }
}
