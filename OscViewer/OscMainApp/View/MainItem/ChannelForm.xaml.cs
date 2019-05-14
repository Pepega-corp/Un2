using System.Windows;
using System.Windows.Controls;
using Oscilloscope.View.AnalogChartItem;

namespace Oscilloscope.View.MainItem
{
    /// <summary>
    /// Логика взаимодействия для ChannelForm.xaml
    /// </summary>
    public partial class ChannelForm : Window
    {
        private AnalogChartOptions[] _optionses;

        public ChannelForm()
        {
            InitializeComponent();
        }

        internal AnalogChartOptions[] Options
        {
            get { return _optionses; }
            set
            {
                _optionses = value;
                ChannelDataGrid.ItemsSource = _optionses;
            }
        }

        private void TestGrid_OnSelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            if (dataGrid == null) return;
            DataGridCellInfo selectedCell = dataGrid.CurrentCell;
            CheckBox cellContent = selectedCell.Column.GetCellContent(selectedCell.Item) as CheckBox;
            if (cellContent == null) return;
            cellContent.IsChecked = !cellContent.IsChecked;
        }

    }
}
