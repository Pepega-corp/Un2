using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace Oscilloscope.View.FrequencyChartItem
{
    /// <summary>
    /// Логика взаимодействия для FrequencyChartOptionsForm.xaml
    /// </summary>
    public partial class FrequencyChartOptionsForm : Window
    {
        private FrequencyChartOptions _options;

        public FrequencyChartOptions Options
        {
            get { return _options; }
            set
            {
                _options = value;
                this.Set();
            }
        }

        private static string[] FrequencyValueNames = new string[]
            {
                "Абсолютные",
                "Среднекв.",
                "G1",
                "G2",
                "G3",
                "G4",
                "G5",
                "G6",
                "G7",
                "G8",
                "G9",
                "G10"
            };



        public FrequencyChartOptionsForm()
        {
            InitializeComponent();
            PercentCb.ItemsSource = FrequencyValueNames;
        }

        private void Set()
        {
            HarmonyTb.Text = _options.HarmonicCount.ToString(CultureInfo.InvariantCulture);
            PeriodTb.Text = _options.PeriodCount.ToString(CultureInfo.InvariantCulture);
            PercentCb.SelectedIndex = (int) _options.FrequencyValue;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var harmony = 0;
            try
            {
                harmony = int.Parse(HarmonyTb.Text);
                if ((harmony < 1) || (harmony > 10))
                {
                    throw new ArgumentException();
                }
            }
            catch (Exception)
            {
                MessageBox.Show(string.Format("Поле \"{0}\" должно содержать целое число от 1 до 10", HarmonyLabel.Content));
                return;
            }

            var period = 0;
            try
            {
                period = int.Parse(PeriodTb.Text);
         
            }
            catch (Exception)
            {
                MessageBox.Show(string.Format("Поле \"{0}\" должно содержать целое число", PeriodLabel.Content));
                return;
            }
            _options.FrequencyValue = (FrequencyValue) PercentCb.SelectedIndex;
            _options.HarmonicCount = harmony;
            _options.PeriodCount = period;
            this.DialogResult = true;
            // _options.Update();

            //  this.Close();

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
           // this.Close();
        }
    }
}
