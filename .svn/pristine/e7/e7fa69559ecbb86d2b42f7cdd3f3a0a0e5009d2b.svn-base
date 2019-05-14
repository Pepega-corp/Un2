using System.ComponentModel;
using System.Windows.Media;

namespace Oscilloscope.View.VectorChartItem
{
    class VectorChannelInfo : INotifyPropertyChanged
    {
        private string _channelName = string.Empty;
        private Brush _lineColor = Brushes.Black;
        private string _currentValue = string.Empty;
        private string _arc = string.Empty;

        public event PropertyChangedEventHandler PropertyChanged;

        public VectorChannelInfo()
        {
            
        }
        public VectorChannelInfo(string name, Brush color, string value, string arc)
        {
            _channelName = name;
            _lineColor = color;
            _currentValue = value;
            _arc = arc;
        }

        public string ChannelName
        {
            get { return _channelName; }
            set
            {
                _channelName = value;
                RaisePropertyChanged("ChannelName");
            }
        }

        public Brush LineColor
        {
            get { return _lineColor; }
            set
            {
                _lineColor = value;
                RaisePropertyChanged("LineColor");
            }
        }

        public string CurrentValue
        {
            get { return _currentValue; }
            set
            {
                _currentValue = value;
                RaisePropertyChanged("CurrentValue");
            }
        }

        public string Arc
        {
            get { return _arc; }
            set
            {
                _arc = value;
                RaisePropertyChanged("Arc");
            }
        }
        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}