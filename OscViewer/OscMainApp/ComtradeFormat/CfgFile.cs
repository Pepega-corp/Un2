using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Oscilloscope.ComtradeFormat
{
    public class CfgOptions
    {
        /// <summary>
        /// Название устройства
        /// </summary>
        private string _deviceName;
        /// <summary>
        /// Номер устройства
        /// </summary>
        private string _deviceId;
        /// <summary>
        /// Частота сети
        /// </summary>
        private int _frequency;
        /// <summary>
        /// Количество различных скоростей дискретизации 
        /// </summary>
        private int _samplingCount;
        /// <summary>
        /// Частота дискретизации в Гц 
        /// </summary>
        private int _samplingRate;
        /// <summary>
        /// Время начала осц.
        /// </summary>
        private string _start;
        /// <summary>
        /// Время аварии
        /// </summary>
        private string _run;

      

        public CfgOptions(string name, string id,string start, string run, int frequency = 50, int samplingCount = 1, int samplingRate = 1000)
        {
            this._deviceName = name;
            this._deviceId = id;
            this._start = start;
            this._run = run;
            this._frequency = frequency;
            this._samplingCount = samplingCount;
            this._samplingRate = samplingRate;
        }

        /// <summary>
        /// Название устройства
        /// </summary>
        public string DeviceName
        {
            get { return this._deviceName; }
        }

        /// <summary>
        /// Номер устройства
        /// </summary>
        public string DeviceId
        {
            get { return this._deviceId; }
        }

        /// <summary>
        /// Частота сети
        /// </summary>
        public int Frequency
        {
            get { return this._frequency; }
        }

        /// <summary>
        /// Количество различных скоростей дискретизации 
        /// </summary>
        public int SamplingCount
        {
            get { return this._samplingCount; }
        }

        /// <summary>
        /// Частота дискретизации в Гц 
        /// </summary>
        public int SamplingRate
        {
            get { return this._samplingRate; }
        }

        /// <summary>
        /// Время начала осц.
        /// </summary>
        public string Start
        {
            get { return this._start; }
        }

        /// <summary>
        /// Время аварии
        /// </summary>
        public string Run
        {
            get { return this._run; }
        }
    }
    class CfgFile
    {
        private const char ANALOG_CHANNEL = 'A';
        private const char DISCRET_CHANNEL = 'D';


        /// <summary>
        /// Название устройства
        /// </summary>
        private string _deviceName;
        /// <summary>
        /// Номер устройства
        /// </summary>
        private string _deviceId;
        /// <summary>
        /// Общее количество каналов
        /// </summary>
        private int _allChannelsCount;
        /// <summary>
        /// Количество аналоговых каналов
        /// </summary>
        private int _analogChannels;
        /// <summary>
        /// Количество дискретных каналов
        /// </summary>
        private int _discretChannels;
        /// <summary>
        /// Частота сети
        /// </summary>
        private int _frequency;
        /// <summary>
        /// Количество различных скоростей дискретизации 
        /// </summary>
        private int _samplingCount;
        /// <summary>
        /// Частота дискретизации в Гц 
        /// </summary>
        private int _samplingRate;
        /// <summary>
        /// Номер последней выборки для данной скорости
        /// </summary>
        private int _size;
        /// <summary>
        /// Время начала осц.
        /// </summary>
        private string _start;
        /// <summary>
        /// Время аварии
        /// </summary>
        private string _run;
        /// <summary>
        /// Кодировка
        /// </summary>
        private string _encodingStr;

        private int _runOsc;

        public CfgFile()
        {
            this._encodingStr = "ASCII";
        }
        public CfgFile(CfgOptions options, int size, List<AnalogChannelConfiguration> analogChannelConfigurations, List<DiscretChannelConfiguration> discretChannelConfigurations):this()
        {
            this._deviceId = options.DeviceId;
            this._deviceName = options.DeviceName;
            this._frequency = options.Frequency;
            this._run = options.Run;
            this._samplingCount = options.SamplingCount;
            this._samplingRate = options.SamplingRate;
            this._start = options.Start;
            this._analogChannelConfigurations = analogChannelConfigurations;
            this._discretChannelConfigurations = discretChannelConfigurations;
            this._size = size;
            this._analogChannels = analogChannelConfigurations.Count;
            this._discretChannels = discretChannelConfigurations.Count;
            this._allChannelsCount = this._analogChannels + this._discretChannels;
            this.CalcRunOsc();
        }

        private void CalcRunOsc()
        {
            try
            {
                string startTime = this._start.Split(',')[1];
                string runTime = this._run.Split(',')[1];

                TimeSpan a = DateTime.Parse(runTime) - DateTime.Parse(startTime);
                this._runOsc = (int) a.TotalMilliseconds;
            }
            catch (Exception)
            {
                this._runOsc = 0;
            }
        }

        private List<AnalogChannelConfiguration> _analogChannelConfigurations = new List<AnalogChannelConfiguration>();
        private List<DiscretChannelConfiguration> _discretChannelConfigurations = new List<DiscretChannelConfiguration>();

        private CfgFile Clone()
        {
            CfgFile res = (CfgFile) MemberwiseClone();
            res._analogChannelConfigurations = this._analogChannelConfigurations.Select(o => o.Clone()).ToList();
            res._discretChannelConfigurations = this._discretChannelConfigurations.Select(o => o.Clone()).ToList();
            return res;

        }

        public CfgFile Copy(int start , int end )
        {
            CfgFile res = this.Clone();
            res._size = end - start;
            return res;
        }

        public CfgFile Copy(int size)
        {
            CfgFile res = this.Clone();
            res._size = size;
            return res;
        }

        /// <summary>
        /// Общее количество каналов
        /// </summary>
        public int AllChannelsCount
        {
            get { return this._allChannelsCount; }
        }

        /// <summary>
        /// Номер последней выборки для данной скорости
        /// </summary>
        public int Size
        {
            get { return this._size; }
        }

        public AnalogChannelConfiguration[] AnalogChannelConfigurations
        {
            get { return this._analogChannelConfigurations.ToArray(); }
        }

        public DiscretChannelConfiguration[] DiscretChannelConfigurations
        {
            get { return this._discretChannelConfigurations.ToArray(); }
        }

        /// <summary>
        /// Количество аналоговых каналов
        /// </summary>
        public int AnalogChannels
        {
            get { return this._analogChannels; }
        }

        /// <summary>
        /// Количество дискретных каналов
        /// </summary>
        public int DiscretChannels
        {
            get { return this._discretChannels; }
        }

        /// <summary>
        /// Время начала осц.
        /// </summary>
        public string Start
        {
            get { return this._start; }
        }

        public int RunOsc
        {
            get { return this._runOsc; }
        }

        public void Load(string fileName, int encoding)
        {
            fileName = Path.ChangeExtension(fileName, "cfg");
            string[] strings = File.ReadAllLines(fileName);

            if (encoding != 0)
            {
                strings = File.ReadAllLines(fileName, Encoding.GetEncoding(encoding));
            }

            string[] head = strings[0].Split(new[] {','}, int.MaxValue, StringSplitOptions.RemoveEmptyEntries);
            if (head.Length >= 2)
            {
                this._deviceName = head[0];
                this._deviceId = head[1];
            }

            string[] channels = strings[1].Split(new[] { ',' }, int.MaxValue);

            if (channels.Length == 3)
            {
                this._allChannelsCount = int.Parse(channels[0]);
                if (channels[1].Last() == ANALOG_CHANNEL)
                {
                    string analog = channels[1].TrimEnd(new[] {ANALOG_CHANNEL});
                    this._analogChannels = int.Parse(analog);
                }

                if (channels[2].Last() == DISCRET_CHANNEL)
                {
                    string discret = channels[2].TrimEnd(new[] {DISCRET_CHANNEL});
                    this._discretChannels = int.Parse(discret);
                }
                if (this.AllChannelsCount != this.AnalogChannels + this.DiscretChannels)
                {
                    throw new FileLoadException(string.Format("Ошибка в файле {0}, строка 2", fileName));
                }
            }
            else
            {
                throw new FileLoadException(string.Format("Ошибка в файле {0}, строка 2", fileName));
            }
            int i = 2;
            for (; i < this.AnalogChannels+2; i++)
            {
                AnalogChannelConfiguration a = new AnalogChannelConfiguration(strings[i]);
                this._analogChannelConfigurations.Add(a);
            }

            for (; i < this.AnalogChannels + this.DiscretChannels +2; i++)
            {
                DiscretChannelConfiguration a = new DiscretChannelConfiguration(strings[i]);
                this._discretChannelConfigurations.Add(a);
            }

            this._frequency = (int) double.Parse(strings[i], CultureInfo.InvariantCulture);
            i++;
            this._samplingCount = int.Parse(strings[i]);
            i++;
            for (int j = 0; j < this._samplingCount; j++)
            {
                channels = strings[i].Split(new[] {','});
                this._samplingRate = (int)double.Parse(channels[0], CultureInfo.InvariantCulture);
                this._size = int.Parse(channels[1]);
                i++;
            }
            this._start = strings[i];
            i++;
            this._run = strings[i];
            i++;
            this._encodingStr = strings[i];
            this.CalcRunOsc();
        }

        public void Save(string fileName, int encoding)
        {
            List<string> res = new List<string>();

            res.Add(string.Format("{0},{1}", this._deviceName, this._deviceId)); //0
            res.Add(string.Format("{0},{1}A,{2}D", this._allChannelsCount, this._analogChannels, this._discretChannels)); //1

            int i = 1;
            foreach (AnalogChannelConfiguration analogChannelConfiguration in this._analogChannelConfigurations)
            {
                res.Add(string.Format("{0},{1}", i, analogChannelConfiguration));
                i++;
            }
            foreach (DiscretChannelConfiguration discretChannelConfigurations in this._discretChannelConfigurations)
            {
                res.Add(string.Format("{0},{1}", i, discretChannelConfigurations));
                i++;
            }
            res.Add(this._frequency.ToString(CultureInfo.InvariantCulture));
            res.Add(this._samplingCount.ToString(CultureInfo.InvariantCulture));

            res.Add(string.Format("{0},{1}", this._samplingRate, this._size));
            res.Add(this.Start);
            res.Add(this._run);
            res.Add(this._encodingStr);

            File.WriteAllLines(fileName, res, Encoding.GetEncoding(encoding));
        }

        internal bool IsEqual(CfgFile cfgFile)
        {
            bool res = this._deviceName == cfgFile._deviceName &
                      this._deviceId == cfgFile._deviceId &
                      this._allChannelsCount == cfgFile._allChannelsCount &
                      this._analogChannels == cfgFile._analogChannels &
                      this._discretChannels == cfgFile._discretChannels &
                      this._frequency == cfgFile._frequency &
                      this._samplingCount == cfgFile._samplingCount &
                      this._samplingRate == cfgFile._samplingRate &
                      this._encodingStr == cfgFile._encodingStr;
            if (!res)
            {
                return res;
            }
            for (int i = 0; i < this._analogChannelConfigurations.Count; i++)
            {
              res&=  this._analogChannelConfigurations[i].IsEqual(cfgFile._analogChannelConfigurations[i]);
            }

            for (int i = 0; i < this._discretChannelConfigurations.Count; i++)
            {
                res &= this._discretChannelConfigurations[i].IsEqual(cfgFile._discretChannelConfigurations[i]);
            }
            return res;
        }
    }
}
