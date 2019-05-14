using System;
using System.Collections.Generic;
using System.Linq;



namespace Oscilloscope.ComtradeFormat
{
    public class CountingStruct
    {
        private int[][] _countingArray;

        public CountingStruct(int countingSize, ushort[] pageValue, CfgOptions cfgOptions):
            this(countingSize,pageValue.Select(o=>(int)o).ToArray(),cfgOptions)
        {}

        public CountingStruct(int countingSize, int[] pageValue, CfgOptions cfgOptions)
        {
            _cfgOptions = cfgOptions;
            this._countingArray = new int[countingSize][];
            int m = 0;
            int n = 0;
            //Общее количество отсчётов
            _count = pageValue.Length / countingSize;
            //Инициализация массива
            for (int i = 0; i < countingSize; i++)
            {
                this._countingArray[i] = new int[_count];
            }
            foreach (var value in pageValue)
            {
                this._countingArray[n][m] = value;
                n++;
                if (n == countingSize)
                {
                    m++;
                    n = 0;
                }
            }
        }

        class AnalogChannelDescription
        {
            private AnalogChannelConfiguration _configuration;
            private int[] _data;
            public AnalogChannelDescription(AnalogChannelConfiguration configuration, int[] data)
            {
                _configuration = configuration;
                _data = data;
            }

            public AnalogChannelConfiguration Configuration
            {
                get { return _configuration; }
            }

            public int[] Data
            {
                get { return _data; }
            }
        }
        class DiscetChannelDescription
        {
            private DiscretChannelConfiguration _configuration;
            private ushort[] _data;
            public DiscetChannelDescription(DiscretChannelConfiguration configuration, ushort[] data)
            {
                _configuration = configuration;
                _data = data;
            }

            public DiscretChannelConfiguration Configuration
            {
                get { return _configuration; }
            }

            public ushort[] Data
            {
                get { return _data; }
            }
        }
        List<AnalogChannelDescription> _analogChannelDescriptions = new List<AnalogChannelDescription>();
        List<DiscetChannelDescription> _discetChannelDescriptions = new List<DiscetChannelDescription>();
        private CfgOptions _cfgOptions;
        private int _count;
        public void AddAnalog(int wordIndex, string name, string measure, double a, double b)
        {
            var conf = new AnalogChannelConfiguration(name, measure, a, b);
            _analogChannelDescriptions.Add(new AnalogChannelDescription(conf, _countingArray[wordIndex]));
        }

        public void AddDiscret(int wordIndex, int byteIndex, string name)
        {
            var conf = new DiscretChannelConfiguration(name, 0);
            _discetChannelDescriptions.Add(new DiscetChannelDescription(conf, GetDiscretArray(_countingArray[wordIndex], byteIndex)));
        }


        private ushort[] GetDiscretArray(int[] sourseArray, int byteIndex)
        {

            var result = new ushort[16][];
            for (int i = 0; i < 16; i++)
            {
                result[i] = new ushort[sourseArray.Length];
            }

            for (int i = 0; i < sourseArray.Length; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    result[j][i] = (ushort)(GetBit((ushort) sourseArray[i], j) ? 1 : 0);
                }
            }

            return result[byteIndex];
        }


        public ComTrade ToComTrade()
        {
            CfgFile cfgFile = new CfgFile(_cfgOptions, _count,
                                          _analogChannelDescriptions.Select(o => o.Configuration).ToList(),
                                          _discetChannelDescriptions.Select(o => o.Configuration).ToList());
            DatFile dat = new DatFile(cfgFile,_analogChannelDescriptions.Select(o=>o.Data).ToList(),_discetChannelDescriptions.Select(o=>o.Data).ToList());
            ComTrade res = new ComTrade(cfgFile,dat);
            return res;
        }
        public static bool GetBit(ushort value, int index)
        {
            return 0 != (value & (ushort)(Math.Pow(2, index)));
        }
    }
}