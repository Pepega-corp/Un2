using System.Globalization;

namespace Oscilloscope.ComtradeFormat
{
    public class AnalogChannelConfiguration
    {
        private int _number;
        private string _name;
        private string _phase;
        private string _component;
        private string _measure;
        private double _a;
        private double _b;
        private double _skew;
        private int _min;
        private int _max;
        private int _samplingRate;

        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", this._name, this._phase,
                                 this._component, this._measure, this._a.ToString(CultureInfo.InvariantCulture), this._b.ToString(CultureInfo.InvariantCulture), this._skew.ToString(CultureInfo.InvariantCulture), this._min, this._max);
        }

        public AnalogChannelConfiguration Clone()
        {
            return (AnalogChannelConfiguration)this.MemberwiseClone();
        }
        public AnalogChannelConfiguration(string line)
        {
            var parameters = line.Split(new[] {','});
            this._number = int.Parse(parameters[0]);
            this._name = parameters[1];
            this._phase = parameters[2];
            this._component= parameters[3];
            this._measure= parameters[4];
            this._a = DoubleParse(parameters[5]);
            this._b = DoubleParse(parameters[6]);
            this._skew = DoubleParse(parameters[7]);
            this._min = int.Parse(parameters[8]);
            this._max = int.Parse(parameters[9]);
                 
        }

        public AnalogChannelConfiguration(string name, string measure, double a, double b, int min = -32768, int max = 32767)
        {
            this._name = name;
            this._measure = measure;
            this._a = a;
            this._b = b;
            this._min = min;
            this._max = max;
        }

     double DoubleParse(string value)
     {
         if (string.IsNullOrEmpty(value))
         {
             return 0;
         }
         return double.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
     }

        public string Measure
        {
            get { return this._measure; }
        }


        public string Name
        {
            get { return this._name; }
        }

        public double A
        {
            get { return _a; }
        }

        public double B
        {
            get { return _b; }
        }

        internal bool IsEqual(AnalogChannelConfiguration analogChannelConfiguration)
        {
            return analogChannelConfiguration._number == this._number &
                   analogChannelConfiguration._name == this._name &
                   analogChannelConfiguration._phase == this._phase &
                   analogChannelConfiguration._component == this._component &
                   analogChannelConfiguration._measure == this._measure &
                   analogChannelConfiguration._a == this._a &
                   analogChannelConfiguration._b == this._b &
                   analogChannelConfiguration._skew == this._skew &
                   analogChannelConfiguration._min == this._min &
                   analogChannelConfiguration._max == this._max &
                   analogChannelConfiguration._samplingRate == this._samplingRate;
        }
    }
}