using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Oscilloscope.View;
using System.Linq;

namespace Oscilloscope.ComtradeFormat
{
    class DatFile
    {
        private AnalogChannel[] _analogChannels;

        public AnalogChannel[] AnalogChannels
        {
            get { return _analogChannels; }
        }

        private DiscretChannel[] _discretChannels;

        public DiscretChannel[] DiscretChannels
        {
            get { return _discretChannels; }
        }

        internal DatFile()
        {}

        public static DatFile Load(CfgFile cfgFile, string cfgFileName)
        {
            DatFile res = new DatFile();
            string fileName = Path.ChangeExtension(cfgFileName, "dat");
            string[] strings = File.ReadAllLines(fileName);
            res.Load(cfgFile, strings);
            return res;
        }

        public DatFile(CfgFile cfgFile, List<int[]> analogData ,List<ushort[]>discretData )
      {
          _analogChannels = new AnalogChannel[cfgFile.AnalogChannels];
          for (int i = 0; i < cfgFile.AnalogChannelConfigurations.Length; i++)
          {

              AnalogChannels[i] = new AnalogChannel(cfgFile.AnalogChannelConfigurations[i], analogData[i].ToArray(),cfgFile.RunOsc); ;
          }
          AnalogChannel.SetBrushes(AnalogChannels);
          _discretChannels = new DiscretChannel[cfgFile.DiscretChannels];

          for (int i = 0; i < cfgFile.DiscretChannelConfigurations.Length; i++)
          {
              DiscretChannels[i] = new DiscretChannel(cfgFile.DiscretChannelConfigurations[i], discretData[i].Select(o => (int)o).ToArray(), cfgFile.RunOsc);
          }
      }

        private void Load(CfgFile cfgFile, string[] strings)
        {
            int[][] channelsData = new int[cfgFile.AllChannelsCount][];

            for (int index = 0; index < channelsData.Length; index++)
            {
                channelsData[index] = new int[strings.Length];
            }

            for (int i = 0; i < strings.Length; i++)
            {
                var parameters = strings[i].Split(',');
                for (int j = 0; j < cfgFile.AllChannelsCount; j++)
                {  
                    channelsData[j][i] = (int)double.Parse(parameters[j + 2], CultureInfo.InvariantCulture);
                }
            }
   
            _analogChannels = new AnalogChannel[cfgFile.AnalogChannels];
            for (int i = 0; i < cfgFile.AnalogChannelConfigurations.Length; i++)
            {
                AnalogChannels[i] = new AnalogChannel(cfgFile.AnalogChannelConfigurations[i], channelsData[i],cfgFile.RunOsc); ;
            }
            AnalogChannel.SetBrushes(AnalogChannels);
            _discretChannels= new DiscretChannel[cfgFile.DiscretChannels];

            for (int i = 0; i < cfgFile.DiscretChannelConfigurations.Length; i++)
            {
                DiscretChannels[i] = new DiscretChannel(cfgFile.DiscretChannelConfigurations[i], channelsData[i + cfgFile.AnalogChannelConfigurations.Length], cfgFile.RunOsc);
            }
        }

        public DatFile Copy(int start, int end, CfgFile cfgFile)
        {
            var data = GetSaveData();
           data= data.GetRange(start, end - start);
            var res = new DatFile();
            res.Load(cfgFile, data.ToArray());
            return res;
        }


        public DatFile Add(CfgFile cfgFile, DatFile add)
        {
            var data = AddSaveData(add);
            var res = new DatFile();
            res.Load(cfgFile, data.ToArray());
            return res;
        }


        private List<string> AddSaveData(DatFile add)
        {
            List<string> res = new List<string>();
            string line = string.Empty;
            for (int i = 0; i <add._analogChannels[0].Length+ _analogChannels[0].Length; i++)
            {
                line = string.Format("{0:D6},{1}000", i + 1, i + 1);
                if (i < _analogChannels[0].Length)
                {
                    foreach (var analogChannel in _analogChannels)
                    {
                        line = line + "," + analogChannel.Data[i];
                    }
                    foreach (var discretChannel in _discretChannels)
                    {
                        line = line + "," + discretChannel.Data[i];
                    }
                }
                else
                {
                    foreach (var analogChannel in add._analogChannels)
                    {
                        line = line + "," + analogChannel.Data[i-_analogChannels[0].Length];
                    }
                    foreach (var discretChannel in add._discretChannels)
                    {
                        line = line + "," + discretChannel.Data[i - _analogChannels[0].Length];
                    }
                }

                res.Add(line);
            }
            return res;
        }
        
        private List<string> GetSaveData()
        {
            List<string> res = new List<string>();
            string line = string.Empty;
            for (int i = 0; i < _analogChannels[0].Length; i++)
            {
                line = string.Format("{0:D6},{1}000", i+1, i+1);
                foreach (var analogChannel in _analogChannels)
                {
                    line = line + "," + analogChannel.Data[i];
                }
                foreach (var discretChannel in _discretChannels)
                {
                    line = line + "," + discretChannel.Data[i];
                }
                res.Add(line);
            }
            return res;
        }

        public void Save(string cfgFileName)
        {
            var fileName = Path.ChangeExtension(cfgFileName, "dat");
            var res = GetSaveData();
            File.WriteAllLines(fileName, res);
        }
    }
}
