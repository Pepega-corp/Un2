using System;
using System.ComponentModel;

namespace Oscilloscope.View.AnalogChartItem
{
    internal class AnalogChartOptions : INotifyPropertyChanged
    {
        private string _name;
        private bool _visibility =true;


        private bool _enabledFrequency;
        private bool _enabledVector;
        private double _primaryFactor=1.0;
        private bool _isPrimaryFactor = true;

        public event Action FactorChange;

        public double Factor
        {
            get { return this._primaryFactor; }
        }

        public bool Vector
        {
            get { return this._enabledVector; }
            set
            {
                this._enabledVector = value;
                this.RaisePropertyChanged("Vector");
            }
        }

        public bool Frequency
        {
            get { return this._enabledFrequency; }
            set
            {
                this._enabledFrequency = value;
                this.RaisePropertyChanged("Frequency");
            }
        }
        public string ChannelName
        {
            get { return this._name; }
            set { this._name = value; }
        }

        public bool Visibility
        {
            get { return this._visibility; }
            set
            {
                this._visibility = value;
                this.RaisePropertyChanged("Visibility");
            }
        }
        
        public double PrimaryFactor
        {
            get { return this._primaryFactor; }
            set
            {
                this._primaryFactor = value;
                this.RaisePropertyChanged("PrimaryFactor");
            }
        }

        public bool IsPrimaryFactor
        {
            get { return this._isPrimaryFactor; }
            set
            {
                if (this._isPrimaryFactor == value)
                {
                    return;
                }
                this._isPrimaryFactor = value;
                this.RaiseFactorChange();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


        public void RaiseFactorChange()
        {
            if (this.FactorChange != null)
                this.FactorChange.Invoke();
        }

        public AnalogChartOptions Clone()
        {
            return (AnalogChartOptions)MemberwiseClone();
        }

        public static void ApplyFactors(AnalogChartOptions[] sourse, AnalogChartOptions[] desteny)
        {
            if (sourse.Length != desteny.Length)
            {
                throw new ArgumentException();
            }
            for (int i = 0; i < sourse.Length; i++)
            {
                desteny[i]._primaryFactor = sourse[i]._primaryFactor;
                desteny[i].RaiseFactorChange();
            }
        }
    }
}
