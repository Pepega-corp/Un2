using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Oscilloscope.ComtradeFormat;
using Oscilloscope.View.AnalogChartItem;

namespace Oscilloscope.View
{
    public class AnalogChannel
    {
        private AnalogChartOptions _analogChartOptions;
        private int[] _data;
        private double[] _rmsValues;

        private double _min;
        private double _max;

        public Brush ChannelBrush { get; set; }
        public double[] ActivValues { get; }
        private double AmplitudeActiv { get; }
        public double[] RmsValues { get { return this._rmsValues.Select(o => o * this._analogChartOptions.Factor).ToArray(); } }
        private double RmsAmplitude { get; }
        private double FirstHarmonicAmplitude { get; }

        public event Action NeedDraw;
        
        public enum ChannelType
        {
            UNKNOWN,
            I,
            U
        }

        public ChannelType ThisType { get; set; }

        public static Brush[] DefaultBrushes = new Brush[]
            {
            new SolidColorBrush(Color.FromRgb(231,157,48)),  //  Brushes.Yellow,//5 
                Brushes.Green,//3
                Brushes.Red,//1
                Brushes.Blue,//2
                Brushes.Orange,//4              
                Brushes.Purple,//6
                Brushes.Brown,//7
                Brushes.DeepPink,//8
                Brushes.Indigo,//9
                Brushes.Coral //10
            };
        public static void SetBrushes(AnalogChannel[] channels)
        {
            int iCount = 0;
            int uCount = 0;
            for (int i = 0; i < channels.Length; i++)
            {
                if (channels[i].ThisType == ChannelType.I)
                {
                    channels[i].ChannelBrush = DefaultBrushes[iCount];
                    if (iCount < DefaultBrushes.Length - 1)
                    {
                        iCount++;
                    }

                }
                if (channels[i].ThisType == ChannelType.U)
                {
                    channels[i].ChannelBrush = DefaultBrushes[uCount];

                    if (uCount < DefaultBrushes.Length - 1)
                    {
                        uCount++;
                    }
                }
                if (channels[i].ThisType == ChannelType.UNKNOWN)
                {
                    channels[i].ChannelBrush = Brushes.Black;
                }
            }
        }

        private void RaiseNeedDraw()
        {
            if (this.NeedDraw != null)
            {
                this.NeedDraw.Invoke();
            }
        }
        public string Min
        {
            get
            {
                return string.Format("Min = {0}", Normalize(this._min * this._analogChartOptions.Factor, this.Measure));
            }
        }

        public string Max
        {
            get
            {
                return string.Format("Max = {0}", Normalize(this._max * this._analogChartOptions.Factor, this.Measure));
            }
        }
        public double[] Values(GrafType type)
        {

            double[] res = new double[1];
            switch (type)
            {
                case GrafType.ACTIV:
                    res = this.ActivValues;
                    break;
                case GrafType.RMS:
                    res = this._rmsValues;
                    break;
                case GrafType.FIRST_HARMONIC:
                    res = this.FirstHarmonicDouble;
                    break;
            }
            if (this._analogChartOptions == null)
            {
                return res;
            }
            return res.Select(o => o * this._analogChartOptions.Factor).ToArray();

        }

        public double Amplitude(GrafType type)
        {

            double res = 0;
            switch (type)
            {
                case GrafType.ACTIV:
                    res = this.AmplitudeActiv;
                    break;
                case GrafType.RMS:
                    res = this.RmsAmplitude;
                    break;
                case GrafType.FIRST_HARMONIC:
                    res = this.FirstHarmonicAmplitude;
                    break;
            }
            if (res == 0)
            {
                return 1.0;
            }
            return res * this._analogChartOptions.Factor;

        }

        public enum GrafType
        {
            ACTIV,
            RMS,
            FIRST_HARMONIC
        }


        public string Measure { get; }

        public string Name { get; }
        public int Length
        {
            get { return this.ActivValues == null ? 0 : this.ActivValues.Length; }
        }
        internal AnalogChartOptions Options
        {
            get
            {

                if (this._analogChartOptions == null)
                {
                    this._analogChartOptions = new AnalogChartOptions
                    {
                        ChannelName = this.Name
                    };
                    this._analogChartOptions.FactorChange += this.RaiseNeedDraw;

                }
                return this._analogChartOptions;
            }
        }

        public Point[] FirstHarmonic { get; private set; }
        public double[] FirstHarmonicDouble { get; private set; }

        public int[] Data
        {
            get { return this._data; }
        }

        public int Run
        {
            get { return this._run; }
        }

        private AnalogChannelConfiguration _analogChannelConfiguration;

        public AnalogChannel(AnalogChannelConfiguration configuration, int[] data, int run)
        {
            this._run = run;
            this._analogChannelConfiguration = configuration;
            this._data = data;
            this.Measure = configuration.Measure;
            this.Name = configuration.Name;
            this.ActivValues = data.Select(o => configuration.A * o + configuration.B).ToArray();
            this.AmplitudeActiv = this.ActivValues.Max(o => Math.Abs(o));
            this._min = this.ActivValues.Min();
            this._max = this.ActivValues.Max();

            var t = 20;
            this._rmsValues = new double[this.ActivValues.Length];
            for (int i = 0; i < this._rmsValues.Length; i++)
            {
                double temp = 0;


                for (int j = 0; j < t; j++)
                {
                    int index = i - j;
                    if ((index >= 0) && (index < this._rmsValues.Length))
                    {
                        temp += Math.Pow(this.ActivValues[index], 2);
                    }

                }
                temp = Math.Sqrt(temp / t);
                this._rmsValues[i] = temp;
            }
            this.RmsAmplitude = this._rmsValues.Max(o => Math.Abs(o));
            this.PrepareFirstHarmonic();


            this.FirstHarmonicAmplitude = this.FirstHarmonicDouble.Max(o => Math.Abs(o));
            switch (configuration.Measure)
            {
                case "A":
                    this.ThisType = ChannelType.I;
                    break;
                case "V":
                    this.ThisType = ChannelType.U;
                    break;
                default:
                    this.ThisType = ChannelType.UNKNOWN;
                    break;
            }
        }


        public AnalogChannel(string name, string measure, double[] data)
        {

            // _data = data;
            this.Measure = measure;
            this.Name = name;
            this.ActivValues = data;
            this.AmplitudeActiv = this.ActivValues.Max(o => Math.Abs(o));
            this._min = this.ActivValues.Min();
            this._max = this.ActivValues.Max();

            var t = 20;
            this._rmsValues = new double[this.ActivValues.Length];
            for (int i = 0; i < this._rmsValues.Length; i++)
            {
                double temp = 0;


                for (int j = 0; j < t; j++)
                {
                    int index = i - j;
                    if ((index >= 0) && (index < this._rmsValues.Length))
                    {
                        temp += Math.Pow(this.ActivValues[index], 2);
                    }

                }
                temp = Math.Sqrt(temp / (t));
                this._rmsValues[i] = temp;
            }
            this.RmsAmplitude = this._rmsValues.Max(o => Math.Abs(o));
            this.PrepareFirstHarmonic();
            this.ChannelBrush = Brushes.DarkBlue;

            this.FirstHarmonicAmplitude = this.FirstHarmonicDouble.Max(o => Math.Abs(o));
            this.ThisType = ChannelType.UNKNOWN;

        }

        public double[] PrepareHarmonic(int k, int periodCount)
        {
            var res = new double[this.ActivValues.Length];
            var factors = new double[20];
            int N = 20 * periodCount;
            double K = k;


            if (k == 0)
            {
                for (int j = N - 1; j < this.ActivValues.Length; j++)
                {
                    double sum = 0;
                    for (int i = 0; i < N; i++)
                    {
                        sum += this.ActivValues[j - N + 1 + i];
                    }
                    res[j] = sum / N;
                }
                return res;
            }




            for (int j = N - 1; j < this.ActivValues.Length; j++)
            {
                for (int i = 0; i < N; i++)
                {
                    if (i == 0)
                    {
                        factors[i] = this.ActivValues[j - N + 1 + i];
                        continue;
                    }
                    if (i == 1)
                    {
                        factors[i] = this.ActivValues[j - N + 1 + i] + 2 * Math.Cos(2 * Math.PI / N * K) * factors[i - 1];
                        continue;
                    }
                    factors[i] = this.ActivValues[j - N + 1 + i] + 2 * Math.Cos(2 * Math.PI / N * K) * factors[i - 1] - factors[i - 2];
                }
                var x = Math.Cos(2 * Math.PI / N * K) * factors[N - 1] - factors[N - 2];
                var y = Math.Sin(2 * Math.PI / N * K) * factors[N - 1];
                res[j] = Math.Sqrt(x * x + y * y) / (10.0 * Math.Sqrt(2));

            }
            return res;
        }

        private void PrepareFirstHarmonic()
        {
            this.FirstHarmonic = new Point[this.ActivValues.Length];
            var factors = new double[20];
            int N = 20;
            double K = 1;
            for (int j = N - 1; j < this.FirstHarmonic.Length; j++)
            {
                for (int i = 0; i < N; i++)
                {
                    if (i == 0)
                    {
                        factors[i] = this.ActivValues[j - N + 1 + i];
                        continue;
                    }
                    if (i == 1)
                    {
                        factors[i] = this.ActivValues[j - N + 1 + i] + 2 * Math.Cos(2 * Math.PI / N * K) * factors[i - 1];
                        continue;
                    }
                    factors[i] = this.ActivValues[j - N + 1 + i] + 2 * Math.Cos(2 * Math.PI / N * K) * factors[i - 1] - factors[i - 2];
                }

                this.FirstHarmonic[j].X = Math.Cos(2 * Math.PI / N * K) * factors[N - 1] - factors[N - 2];
                this.FirstHarmonic[j].Y = Math.Sin(2 * Math.PI / N * K) * factors[N - 1];
            }
            this.FirstHarmonicDouble = this.FirstHarmonic.Select(o => Math.Sqrt(o.X * o.X + o.Y * o.Y) / (10.0 * Math.Sqrt(2))).ToArray();
        }

        public double GetY(GrafType type)
        {
            return GetMiddleValue(this.Amplitude(type));
        }

        public static double GetMiddleValue(double max)
        {
            var tempMax = max;
            int exponent = 0;
            if (tempMax >= 1)
            {
                while (tempMax / 10.0 > 1.0)
                {
                    tempMax /= 10;
                    exponent++;
                }
            }
            else
            {
                while (tempMax * 10.0 < 10.0)
                {
                    tempMax *= 10;
                    exponent--;
                }
            }


            if (tempMax / 1.5 >= 1)
            {
                tempMax /= 3;
                if (tempMax >= 1)
                {
                    return Math.Truncate(tempMax) * Math.Pow(10, exponent) * 1.5;
                }
                else
                {
                    return Math.Truncate(tempMax * 10) * Math.Pow(10, exponent - 1) * 1.5;
                }
            }

            tempMax /= 2;

            if (tempMax >= 1)
            {
                return Math.Truncate(tempMax) * Math.Pow(10, exponent);
            }
            else
            {
                return Math.Truncate(tempMax * 10) * Math.Pow(10, exponent - 1);
            }
        }


        public static string Normalize(double value, string measure)
        {

            string mod = string.Empty;
            double res = 0;
            if (Math.Abs(value) >= 1000)
            {
                if (Math.Abs(value) >= 1000000)
                {
                    res = value / 1000000;
                    mod = "М";
                }
                else
                {
                    res = value / 1000;
                    mod = "к";
                }
            }
            if ((Math.Abs(value) >= 0.1) && (Math.Abs(value) <= 1))
            {
                res = value;
            }

            if (Math.Abs(value) < 0.1)
            {
                if (Math.Abs(value) <= 0.001)
                {
                    res = value * 1000000;
                    mod = "мк";
                }
                else
                {
                    res = value * 1000;
                    mod = "м";
                }
            }

            if (res == 0)
            {
                res = value;
                mod = string.Empty;
            }
            string str = string.Empty;
            if (res == 0)
            {
                str = "0";
            }
            else
            {
                if (Math.Abs(res) >= 100)
                {
                    str = Math.Round(res, 0).ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    if (Math.Abs(res) >= 10)
                    {
                        str = Math.Round(res, 1).ToString(CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        str = Math.Round(res, 2).ToString(CultureInfo.InvariantCulture);
                    }
                }
            }
            return string.Format("{0} {1}{2}", str, mod, measure);
        }

        private int _run;

        public override string ToString()
        {
            return this._analogChannelConfiguration.ToString();
        }
    }
}