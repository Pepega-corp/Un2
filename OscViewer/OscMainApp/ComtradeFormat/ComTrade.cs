using System.IO;
using System.Windows;
using Oscilloscope.View;

namespace Oscilloscope.ComtradeFormat
{
    public class ComTrade
    {
        public string DateTime { get { return this._cfgFile.Start; } }
        //private HdrFile _hdrFile;
        private CfgFile _cfgFile;
        private DatFile _datFile;

        public string FileName { get; private set; }

        internal AnalogChannel[] AnalogChannels
        {
            get { return this._datFile.AnalogChannels; }
        }

        internal DiscretChannel[] DiscretChannels
        {
            get { return this._datFile.DiscretChannels; }
        }

        internal CfgFile Cfg
        {
            get { return this._cfgFile; }
        }

        public static ComTrade Load(string fileName, int encodding)
        {
            ComTrade res = new ComTrade();  
            res.FileName = Path.GetFileNameWithoutExtension(fileName);
            res._cfgFile = new CfgFile();
            res._cfgFile.Load(fileName, encodding);
            res._datFile = DatFile.Load(res._cfgFile, fileName);
           
            return res;
        }

        public void Save(string filename, int encoding)
        {
            string cfgPathPath = Path.ChangeExtension(filename, "cfg");
            this._cfgFile.Save(cfgPathPath, encoding);
            this._datFile.Save(cfgPathPath);
        }

        internal ComTrade()
        {
        }

        internal ComTrade(CfgFile cfgFile, DatFile datFile)
        {
            this._cfgFile = cfgFile;
            this._datFile = datFile;
        }

        public ComTrade Copy(string name,int start, int end)
        {
            CfgFile cfg = this._cfgFile.Copy(start, end);
            DatFile dat = this._datFile.Copy(start, end, cfg);
            ComTrade res = new ComTrade(cfg, dat);
            res.FileName = name;
            return res;
        }

        public ComTrade Add(ComTrade add)
        {
            CfgFile cfg = this._cfgFile.Copy(this.Cfg.Size+add.Cfg.Size);
            DatFile dat = this._datFile.Add(cfg,add._datFile);
            ComTrade res = new ComTrade(cfg, dat);
            res.FileName = string.Format("{0}+{1}",this.FileName,add.FileName);
            return res;
        }
        public bool IsEqual(ComTrade file)
        {
            return file.Cfg.IsEqual(this.Cfg);
        }
    }
}
