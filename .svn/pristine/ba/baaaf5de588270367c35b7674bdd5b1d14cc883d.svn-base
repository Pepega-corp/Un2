using System;

namespace Oscilloscope.View.FrequencyChartItem
{
    public class FrequencyChartOptions
    {
        private int _periodCount = 1;
        private int _harmonicCount = 10;
        private FrequencyValue _frequencyValue = FrequencyValue.RMS;
        public event Action Changed;
        private void RaiseChanged()
        {
            if (Changed!= null)
            {
                Changed.Invoke();
            }
        }
        public int PeriodCount
        {
            get { return _periodCount; }
            set { _periodCount = value; }
        }

        public int HarmonicCount
        {
            get { return _harmonicCount; }
            set { _harmonicCount = value; }
        }

        public FrequencyValue FrequencyValue
        {
            get { return _frequencyValue; }
            set { _frequencyValue = value; }
        }

        internal void Update()
        {
            this.RaiseChanged();
        }
    }
}
