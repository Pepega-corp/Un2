using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Oscilloscope.View.PieChartItem
{
    [Serializable]
    public class ChannelPieChartOptions : INotifyPropertyChanged
    {
        private string _name;
        private bool _a;
        private bool _b;
        private bool _c;
        private bool _n;

        public override string ToString()
        {
            return this._name;
        }
        public AnalogChannel Channel { get; }

        public ChannelPieChartOptions(string name)
        {
            this._name = name;
        }

        public ChannelPieChartOptions(AnalogChannel channel)
        {
            this.Channel = channel;
            this._name = channel.Name;
        }

        public Point[] Values { get { return this.Channel.FirstHarmonic; } }

        public string Name
        {
            get { return this._name; }
            set
            {
                this._name = value;
                this.RaisePropertyChanged();
            }
        }

        public bool A
        {
            get { return this._a; }
            set
            {
                this._a = value;
                if (value)
                {
                    this.B = false;
                    this.C = false;
                    this.OnAisTrue(this);
                }
                this.RaisePropertyChanged();
            }
        }

        public bool B
        {
            get { return this._b; }
            set
            {
                this._b = value;
                if (value)
                {
                    this.A = false;
                    this.C = false;
                    this.OnBisTrue(this);
                }
                this.RaisePropertyChanged();
            }
        }

        public bool C
        {
            get { return this._c; }
            set
            {
                this._c = value;
                if (value)
                {
                    this.B = false;
                    this.A = false;
                    this.OnCisTrue(this);
                }
                this.RaisePropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event Action<ChannelPieChartOptions> AisTrue;
        public event Action<ChannelPieChartOptions> BisTrue;
        public event Action<ChannelPieChartOptions> CisTrue;

        protected virtual void OnBisTrue(ChannelPieChartOptions obj)
        {
            this.BisTrue?.Invoke(obj);
        }


        protected virtual void OnCisTrue(ChannelPieChartOptions obj)
        {
            this.CisTrue?.Invoke(obj);
        }


        protected virtual void OnAisTrue(ChannelPieChartOptions obj)
        {
            this.AisTrue?.Invoke(obj);
        }

        private void RaisePropertyChanged([CallerMemberName] string name = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}