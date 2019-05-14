using System;
using System.ComponentModel;
using System.Configuration;
using System.Xml.Linq;
using System.Xml.Serialization;
using Oscilloscope.View.MainItem;

namespace Oscilloscope.View.PieChartItem
{

    [Serializable]
    public class PieChartVisiblyOptions 
    {
        public PieChartVisiblyOptions()
        {
            
            foreach (var visibilityItem in VisibleItems)
            {
                visibilityItem.PropertyChanged += (sender, args) =>
                    {
                        if (args.PropertyName == "Visible")
                        {
                            if (NeedRedraw != null)
                            {
                                NeedRedraw.Invoke();
                            }
                        }
                    };
            }
        }

        public VisibilityItem[] VisibleItems
        {
            get
            {
                return new VisibilityItem[]
                    {
                        _zab, _zbc, _zca,
                        _z1a, _z1b, _z1c,
                        _z2a, _z2b, _z2c,
                        _z3a, _z3b, _z3c,
                        _z4a, _z4b, _z4c,
                        _z5a, _z5b, _z5c
                    };
            }
            set
            {
                _zab = value[0];
                _zbc = value[1];
                _zca = value[2];
                _z1a = value[3];
                _z1b = value[4];
                _z1c = value[5];
                _z2a = value[6];
                _z2b = value[7];
                _z2c = value[8];
                _z3a = value[9];
                _z3b = value[10];
                _z3c = value[11];
                _z4a = value[12];
                _z4b = value[13];
                _z4c = value[14];
                _z5a = value[15];
                _z5b = value[16];
                _z5c = value[17];
            }
        }

        private VisibilityItem _zab = new VisibilityItem("Zab", MainWindow.PieChartColors.Colors[0]);
        private VisibilityItem _zbc = new VisibilityItem("Zbc", MainWindow.PieChartColors.Colors[1]);
        private VisibilityItem _zca = new VisibilityItem("Zca", MainWindow.PieChartColors.Colors[2]);
        private VisibilityItem _z1a = new VisibilityItem("Z1A", MainWindow.PieChartColors.Colors[3]);
        private VisibilityItem _z1b = new VisibilityItem("Z1B", MainWindow.PieChartColors.Colors[4]);
        private VisibilityItem _z1c = new VisibilityItem("Z1C", MainWindow.PieChartColors.Colors[5]);
        private VisibilityItem _z2a = new VisibilityItem("Z2A", MainWindow.PieChartColors.Colors[6]);
        private VisibilityItem _z2b = new VisibilityItem("Z2B", MainWindow.PieChartColors.Colors[7]);
        private VisibilityItem _z2c = new VisibilityItem("Z2C", MainWindow.PieChartColors.Colors[8]);
        private VisibilityItem _z3a = new VisibilityItem("Z3A", MainWindow.PieChartColors.Colors[9]);
        private VisibilityItem _z3b = new VisibilityItem("Z3B", MainWindow.PieChartColors.Colors[10]);
        private VisibilityItem _z3c = new VisibilityItem("Z3C", MainWindow.PieChartColors.Colors[11]);
        private VisibilityItem _z4a = new VisibilityItem("Z4A", MainWindow.PieChartColors.Colors[12]);
        private VisibilityItem _z4b = new VisibilityItem("Z4B", MainWindow.PieChartColors.Colors[13]);
        private VisibilityItem _z4c = new VisibilityItem("Z4C", MainWindow.PieChartColors.Colors[14]);
        private VisibilityItem _z5a = new VisibilityItem("Z5A", MainWindow.PieChartColors.Colors[15]);
        private VisibilityItem _z5b = new VisibilityItem("Z5B", MainWindow.PieChartColors.Colors[16]);
        private VisibilityItem _z5c = new VisibilityItem("Z5C", MainWindow.PieChartColors.Colors[17]);


        public XElement ToXml()
        {
            return new XElement("Visibly",
                                _zab.ToXml(),
                                _zbc.ToXml(),
                                _zca.ToXml(),
                                _z1a.ToXml(),
                                _z1b.ToXml(),
                                _z1c.ToXml(),
                                _z2a.ToXml(),
                                _z2b.ToXml(),
                                _z2c.ToXml(),
                                _z3a.ToXml(),
                                _z3b.ToXml(),
                                _z3c.ToXml(),
                                _z4a.ToXml(),
                                _z4b.ToXml(),
                                _z4c.ToXml(),
                                _z5a.ToXml(),
                                _z5b.ToXml(),
                                _z5c.ToXml()
                );
        }


        public void FromXml(XElement element)
        {
            _zab.FromXml(element);
            _zbc.FromXml(element);
            _zca.FromXml(element);
            _z1a.FromXml(element);
            _z1b.FromXml(element);
            _z1c.FromXml(element);
            _z2a.FromXml(element);
            _z2b.FromXml(element);
            _z2c.FromXml(element);
            _z3a.FromXml(element);
            _z3b.FromXml(element);
            _z3c.FromXml(element);
            _z4a.FromXml(element);
            _z4b.FromXml(element);
            _z4c.FromXml(element);
            _z5a.FromXml(element);
            _z5b.FromXml(element);
            _z5c.FromXml(element);
        }

         [XmlIgnore]
        public VisibilityItem Zab
        {
            get { return _zab; }
        }
         [XmlIgnore]
        public VisibilityItem Zbc
        {
            get { return _zbc; }
        }
         [XmlIgnore]
        public VisibilityItem Zca
        {
            get { return _zca; }
        }
         [XmlIgnore]
        public VisibilityItem Z1A
        {
            get { return _z1a; }
        }
         [XmlIgnore]
        public VisibilityItem Z1B
        {
            get { return _z1b; }
        }
         [XmlIgnore]
        public VisibilityItem Z1C
        {
            get { return _z1c; }
        }
         [XmlIgnore]
        public VisibilityItem Z2A
        {
            get { return _z2a; }
        }
         [XmlIgnore]
        public VisibilityItem Z2B
        {
            get { return _z2b; }
        }
         [XmlIgnore]
        public VisibilityItem Z2C
        {
            get { return _z2c; }
        }
         [XmlIgnore]
        public VisibilityItem Z3A
        {
            get { return _z3a; }
        }
         [XmlIgnore]
        public VisibilityItem Z3B
        {
            get { return _z3b; }
        }
         [XmlIgnore]
        public VisibilityItem Z3C
        {
            get { return _z3c; }
        }
         [XmlIgnore]
        public VisibilityItem Z4A
        {
            get { return _z4a; }
        }
         [XmlIgnore]
        public VisibilityItem Z4B
        {
            get { return _z4b; }
        }
         [XmlIgnore]
        public VisibilityItem Z4C
        {
            get { return _z4c; }
        }
         [XmlIgnore]
        public VisibilityItem Z5A
        {
            get { return _z5a; }
        }
         [XmlIgnore]
        public VisibilityItem Z5B
        {
            get { return _z5b; }
        }
         [XmlIgnore]
        public VisibilityItem Z5C
        {
            get { return _z5c; }
        }
        [field: NonSerialized]
        public event Action NeedRedraw;

    }
}