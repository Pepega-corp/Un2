using System.Windows;
using System.Windows.Controls;
using Oscilloscope.ComtradeFormat;

namespace Oscilloscope.View.CommonDiscretChartItems
{
    /// <summary>
    /// Логика взаимодействия для InfoTable.xaml
    /// </summary>
    public partial class InfoTable : UserControl
    {

        private DiscretChannel _channel;

        public DiscretChannel Channel
        {
            set
            {
                this._channel = value;
                this.ChannelNameTb.Content = this._channel.Name;
            }
        }

        public bool Value
        {
            set
            {
                this.ChannelNameTb.Content = string.Format("{0} = {1}", this._channel.Name, value ? "1" : "0");
            }
        }


        public InfoTable()
        {
            InitializeComponent();
        }

        internal void Disable()
        {
            if (this._channel.IsEmpty)
            {
                this.Visibility = Visibility.Collapsed;
            }
        }

        internal void Enable()
        {
            this.Visibility = Visibility.Visible;
        }
    }
}
