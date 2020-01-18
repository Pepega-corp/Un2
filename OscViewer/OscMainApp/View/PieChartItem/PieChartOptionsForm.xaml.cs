using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;
using Oscilloscope.View.PieChartItem.Characteristics;

namespace Oscilloscope.View.PieChartItem
{
    /// <summary>
    /// Логика взаимодействия для PieChartOptionsForm.xaml
    /// </summary>
    public partial class PieChartOptionsForm : Window
    {
        private const int MIN_START_TIME = 20;

        public int Marker1 { get; set; }
        public int Marker2 { get; set; }

        private bool OldCalculateKoef
        {
            get { return this.k0RadioBtn.IsChecked.HasValue ? this.k0RadioBtn.IsChecked.Value : false; }
            set
            {
                this.k0RadioBtn.IsChecked = value;
                this.rxRadioBtn.IsChecked = !value;
                if (this._chartOptions != null)
                {
                    this._chartOptions.OldCalculate = value;
                }
            }
        }

        private PieChartOptions _chartOptions;

        public PieChartOptions ChartOptions
        {
            get { return this._chartOptions; }
            set
            {
                if (this._chartOptions != null)
                {
                    this.ChartOptions.ChannelChanged -= this.ChannelsChanged;
                }

                this._chartOptions = value;
                this.Characteristics = this._chartOptions.Characteristics;
                this.Open();
                this.Prepare();
            }
        }

        private void DataGridCell_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DataGridCell cell = sender as DataGridCell;
            if (cell == null)
            {
                return;
            }
            if (!cell.IsEditing)
            {
                // enables editing on single click
                if (!cell.IsFocused)
                    cell.Focus();
                if (!cell.IsSelected)
                    cell.IsSelected = true;
            }
        }

        public PieChartOptionsForm()
        {
            this.InitializeComponent();


            this.PolyGrid.DataContext = this._polyCharacteristicOption;
            this.RoundGrid.DataContext = this._roundCharacteristicOption;
            this.DirectionGrid.DataContext = this._directionCharacteristicOption;
            this.ChargeGrid.DataContext = this._chargeCharacteristicOption;

            this.OldCalculateKoef = true;
        }

        private void Prepare()
        {
            this.IOptions.ItemsSource = this.ChartOptions.PieIChartOptions;
            this.UOptions.ItemsSource = this.ChartOptions.PieUChartOptions;

            this.ChartOptions.ChannelChanged += this.ChannelsChanged;

            this.StartTimeTb.Text = this.ChartOptions.StartTime.ToString(CultureInfo.CurrentCulture);
            this.EndTimeTb.Text = this.ChartOptions.EndTime.ToString(CultureInfo.CurrentCulture);
            this.ChannelsChanged();
        }
        
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (this.ReadData())
            {
               DialogResult = true; 
            }
        }

        private bool ReadData()
        {
            int startTime;
            try
            {
                startTime = int.Parse(this.StartTimeTb.Text);
                if ((startTime < MIN_START_TIME) || (startTime > this._chartOptions.Lenght - 1))
                {
                    throw new ArgumentException();
                }
            }
            catch (Exception)
            {
                MessageBox.Show(string.Format("Значение начального времени должно быть целым числом от {0} до {1}", MIN_START_TIME, this._chartOptions.Lenght - 1));
                return false;
            }

            int endTime;
            try
            {
                endTime = int.Parse(this.EndTimeTb.Text);
                if ((endTime <= startTime) || (endTime > this._chartOptions.Lenght - 1))
                {
                    throw new ArgumentException();
                }
            }
            catch (Exception)
            {
                MessageBox.Show( string.Format("Значение конечного времени должно быть целым числом большим чем начальное время и меньшим {0}", this._chartOptions.Lenght - 1));
                return false;
            }

            ZnOptions z1;
            try
            {
                z1 = this.Z1Editor.Zn;
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка ввода коэффициентов компенсации по контуру Ф-N1");
                return false;
            }
            
            ZnOptions z2;
            try
            {
                z2 = this.Z2Editor.Zn;

            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка ввода коэффициентов компенсации по контуру Ф-N2");
                return false;
            }


            ZnOptions z3;
            try
            {
                z3 = this.Z3Editor.Zn;
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка ввода коэффициентов компенсации по контуру Ф-N3");
                return false;
            }


            ZnOptions z4;
            try
            {
                z4 = this.Z4Editor.Zn;
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка ввода коэффициентов компенсации по контуру Ф-N4");
                return false;
            }


            ZnOptions z5;
            try
            {
                z5 = this.Z5Editor.Zn;
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка ввода коэффициентов компенсации по контуру Ф-N5");
                return false;
            }

            this._chartOptions.Z1 = z1;
            this._chartOptions.Z2 = z2;
            this._chartOptions.Z3 = z3;
            this._chartOptions.Z4 = z4;
            this._chartOptions.Z5 = z5;
            this._chartOptions.StartTime = startTime;
            this._chartOptions.EndTime = endTime;
            this._chartOptions.Characteristics = this.Characteristics;
            this._chartOptions.ChannelAccept();
            this._chartOptions.OldCalculate = this.OldCalculateKoef;
            return true;
        }


        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }


        private void ChannelsChanged()
        {
            if (this.ChartOptions == null)
            {
                return;
            }

            bool ia = this.ChartOptions.PieIChartOptions.Any(o => o.A);
            bool ib = this.ChartOptions.PieIChartOptions.Any(o => o.B);
            bool ic = this.ChartOptions.PieIChartOptions.Any(o => o.C);
            bool i0 = ia & ib & ic;


            bool ua = this.ChartOptions.PieUChartOptions.Any(o => o.A);
            bool ub = this.ChartOptions.PieUChartOptions.Any(o => o.B);
            bool uc = this.ChartOptions.PieUChartOptions.Any(o => o.C);

            bool z1 = this.Z1Editor.Stage1Cb.IsChecked.GetValueOrDefault();
            bool z2 = this.Z2Editor.Stage1Cb.IsChecked.GetValueOrDefault();
            bool z3 = this.Z3Editor.Stage1Cb.IsChecked.GetValueOrDefault();
            bool z4 = this.Z4Editor.Stage1Cb.IsChecked.GetValueOrDefault();
            bool z5 = this.Z5Editor.Stage1Cb.IsChecked.GetValueOrDefault();

            List<VisibilityItem> res = new List<VisibilityItem>();


            if (ia & ib & ua & ub) res.Add(this.ChartOptions.VisiblyOptions.Zab);
            if (ic & ib & uc & ub) res.Add(this.ChartOptions.VisiblyOptions.Zbc);
            if (ic & ia & uc & ua) res.Add(this.ChartOptions.VisiblyOptions.Zca);

            if (ia & i0 & ua & z1) res.Add(this.ChartOptions.VisiblyOptions.Z1A);
            if (ib & i0 & ub & z1) res.Add(this.ChartOptions.VisiblyOptions.Z1B);
            if (ic & i0 & uc & z1) res.Add(this.ChartOptions.VisiblyOptions.Z1C);

            if (ia & i0 & ua & z2) res.Add(this.ChartOptions.VisiblyOptions.Z2A);
            if (ib & i0 & ub & z2) res.Add(this.ChartOptions.VisiblyOptions.Z2B);
            if (ic & i0 & uc & z2) res.Add(this.ChartOptions.VisiblyOptions.Z2C);

            if (ia & i0 & ua & z3) res.Add(this.ChartOptions.VisiblyOptions.Z3A);
            if (ib & i0 & ub & z3) res.Add(this.ChartOptions.VisiblyOptions.Z3B);
            if (ic & i0 & uc & z3) res.Add(this.ChartOptions.VisiblyOptions.Z3C);

            if (ia & i0 & ua & z4) res.Add(this.ChartOptions.VisiblyOptions.Z4A);
            if (ib & i0 & ub & z4) res.Add(this.ChartOptions.VisiblyOptions.Z4B);
            if (ic & i0 & uc & z4) res.Add(this.ChartOptions.VisiblyOptions.Z4C);

            if (ia & i0 & ua & z5) res.Add(this.ChartOptions.VisiblyOptions.Z5A);
            if (ib & i0 & ub & z5) res.Add(this.ChartOptions.VisiblyOptions.Z5B);
            if (ic & i0 & uc & z5) res.Add(this.ChartOptions.VisiblyOptions.Z5C);
            this.VisiblyOptionsGrid.ItemsSource = res;
        }

        List<ICharacteristic> _allCharacteristic = new List<ICharacteristic>();

        private PieChartPolyCharacteristicOption _polyCharacteristicOption = new PieChartPolyCharacteristicOption();
        private PieChartRoundCharacteristicOption _roundCharacteristicOption = new PieChartRoundCharacteristicOption();
        private PieChartChargeCharacteristicOption _chargeCharacteristicOption = new PieChartChargeCharacteristicOption();
        private PieChartDirectionCharacteristicOption _directionCharacteristicOption = new PieChartDirectionCharacteristicOption();
        private List<ICharacteristic> Characteristics
        {
            get { return this._allCharacteristic; }
            set
            {
                this.listBox.Items.Clear();
                if (value != null)
                {
                    this._allCharacteristic = value;
                    foreach (var characteristic in this._allCharacteristic)
                    {
                        this.listBox.Items.Add(characteristic);
                    }
                }
            }
        }

        private void PolyAddButton_Click(object sender, RoutedEventArgs e)
        {
            this._polyCharacteristicOption = new PieChartPolyCharacteristicOption();
            this._polyCharacteristicOption.F = double.Parse(this.PolyFTb.Text, CultureInfo.CurrentCulture);
            this._polyCharacteristicOption.R = double.Parse(this.PolyRTb.Text, CultureInfo.CurrentCulture);
            this._polyCharacteristicOption.X = double.Parse(this.PolyXTb.Text, CultureInfo.CurrentCulture);
            this._polyCharacteristicOption.Enabled = true;
            this._allCharacteristic.Add(this._polyCharacteristicOption);
            this.listBox.Items.Add(this._polyCharacteristicOption);
        }

        private void RoundAddButton_Click(object sender, RoutedEventArgs e)
        {
            this._roundCharacteristicOption = new PieChartRoundCharacteristicOption();
            this._roundCharacteristicOption.Radius = double.Parse(this.RoundRadiusTb.Text, CultureInfo.CurrentCulture);
            this._roundCharacteristicOption.R = double.Parse(this.RoundRTb.Text, CultureInfo.CurrentCulture);
            this._roundCharacteristicOption.X = double.Parse(this.RoundXTb.Text, CultureInfo.CurrentCulture);
            this._roundCharacteristicOption.Enabled = true;
            this._allCharacteristic.Add(this._roundCharacteristicOption);
            this.listBox.Items.Add(this._roundCharacteristicOption);
        }

        private void ChargeAddButton_Click(object sender, RoutedEventArgs e)
        {
            this._chargeCharacteristicOption = new PieChartChargeCharacteristicOption();
            this._chargeCharacteristicOption.R1 = double.Parse(this.ChargeR1Tb.Text, CultureInfo.CurrentCulture);
            this._chargeCharacteristicOption.R2 = double.Parse(this.ChargeR2Tb.Text, CultureInfo.CurrentCulture);
            this._chargeCharacteristicOption.F = double.Parse(this.ChargeFTb.Text, CultureInfo.CurrentCulture);
            this._chargeCharacteristicOption.Enabled = true;
            this._allCharacteristic.Add(this._chargeCharacteristicOption);
            this.listBox.Items.Add(this._chargeCharacteristicOption);
        }

        private void DirectionAddButton_Click(object sender, RoutedEventArgs e)
        {
            this._directionCharacteristicOption = new PieChartDirectionCharacteristicOption();
            this._directionCharacteristicOption.F1 = double.Parse(this.DirectionF1Tb.Text, CultureInfo.CurrentCulture);
            this._directionCharacteristicOption.F2 = double.Parse(this.DirectionF2Tb.Text, CultureInfo.CurrentCulture);

            this._directionCharacteristicOption.Enabled = true;
            this._allCharacteristic.Add(this._directionCharacteristicOption);
            this.listBox.Items.Add(this._directionCharacteristicOption);
        }

      

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.listBox.SelectedItem != null)
            {
                this._allCharacteristic.Remove((ICharacteristic) this.listBox.SelectedItem);
                this.listBox.Items.Remove(this.listBox.SelectedItem);
            }

        }

        SaveFileDialog saveDialog;
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.ReadData())
            {
                this.saveDialog = this.saveDialog ?? new SaveFileDialog();
                this.saveDialog.Filter = "Файл параметров круговой диаграммы|*.pco";
                if (this.saveDialog.ShowDialog().GetValueOrDefault())
                {
                    this._chartOptions.Save(this.saveDialog.FileName);
                }
            }
        }

        OpenFileDialog openFileDialog;
        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            this.openFileDialog = this.openFileDialog ?? new OpenFileDialog();
            this.openFileDialog.Filter = "Файл параметров круговой диаграммы|*.pco";
            if (this.openFileDialog.ShowDialog().GetValueOrDefault())
            {
                try
                {
                    this._chartOptions.Load(this.openFileDialog.FileName, this.OldCalculateKoef);
                }
                catch (Exception)
                {
                    MessageBox.Show("Ошибка загрузки файла");
                    return;
                }

                this.Characteristics = this._chartOptions.Characteristics;
                this.Open();
            }
        }


        private void Open()
        {
            this.Z1Editor.Zn = this._chartOptions.Z1;
            this.Z2Editor.Zn = this._chartOptions.Z2;
            this.Z3Editor.Zn = this._chartOptions.Z3;
            this.Z4Editor.Zn = this._chartOptions.Z4;
            this.Z5Editor.Zn = this._chartOptions.Z5;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            this.StartTimeTb.Text = Math.Min(this.Marker1, this.Marker2).ToString(CultureInfo.CurrentCulture);
            this.EndTimeTb.Text = Math.Max(this.Marker1, this.Marker2).ToString(CultureInfo.CurrentCulture);
        }


        private void ColorLabelDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var label = (Label) sender;
            var oldColor =  ((SolidColorBrush)label.Background).Color;
      
            System.Windows.Forms.ColorDialog dialog = new System.Windows.Forms.ColorDialog();
            
            dialog.Color = System.Drawing.Color.FromArgb(oldColor.A, oldColor.R, oldColor.G, oldColor.B);
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var visibilityItem = (VisibilityItem) this.VisiblyOptionsGrid.SelectedItem;
                visibilityItem.LineColor = new SolidColorBrush(Color.FromArgb(dialog.Color.A, dialog.Color.R, dialog.Color.G, dialog.Color.B));
            }
        }
    }
}
