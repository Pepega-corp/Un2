using System.Linq;
using System.Windows;
using Oscilloscope.View.AnalogChartItem;

namespace Oscilloscope.View.MainItem
{
    /// <summary>
    /// Логика взаимодействия для FactorForm.xaml
    /// </summary>
    public partial class FactorForm : Window
    {
        private AnalogChartOptions[] _optionses;
        private AnalogChartOptions[] _currentOptionses;
        public FactorForm()
        {
            this.InitializeComponent();
        }

        internal AnalogChartOptions[] Options
        {
            get { return this._optionses; }
            set
            {
                this._optionses = value;
                this._currentOptionses = this._optionses.Select(o => o.Clone()).ToArray();
                this.ChannelDataGrid.ItemsSource = this._currentOptionses;
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            AnalogChartOptions.ApplyFactors(this._currentOptionses, this._optionses);
            DialogResult = true;
        }

        private void CanselButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
