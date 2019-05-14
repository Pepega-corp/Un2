using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Xml.Linq;
using Oscilloscope.View.PieChartItem.Characteristics;

namespace Oscilloscope.View.PieChartItem
{
    [Serializable]
    public class PieChartOptions
    {
        private ChannelPieChartOptions[] _pieIChartOptions;
        private ChannelPieChartOptions[] _pieUChartOptions;
        private ChannelPieChartOptions[] _allChartOptions;
        private PieChartVisiblyOptions _visiblyOptions = new PieChartVisiblyOptions();
        private List<ICharacteristic> _characteristics = new List<ICharacteristic>();


        public ChannelPieChartOptions IaChannel { get; set; }
        public ChannelPieChartOptions IbChannel { get; set; }
        public ChannelPieChartOptions IcChannel { get; set; }
        public Point[] I0Channel { get; set; }

        public ChannelPieChartOptions UaChannel { get; set; }
        public ChannelPieChartOptions UbChannel { get; set; }
        public ChannelPieChartOptions UcChannel { get; set; }


       public PieChartOptions(){}

        public ZnOptions Z1 { get; set; }
        public ZnOptions Z2 { get; set; }
        public ZnOptions Z3 { get; set; }
        public ZnOptions Z4 { get; set; }
        public ZnOptions Z5 { get; set; }

        private List<PieChannelInfo> _infos = new List<PieChannelInfo>();

        private Point[] PrepareLineKz(Point[] If, Point[] uf, Point[] In, ZnOptions options)
        {

            // COS - X
            // SIN - Y
            Point[] zf = new Point[If.Length];
            for (int i = this.StartTime; i < this.EndTime; i++)
            {

                double cosf = Math.Cos(0);
                double sinf = Math.Sin(0);


                double a = If[i].X + options.Kr*In[i].X - options.Kx*In[i].Y;
                double b = If[i].Y + options.Kr*In[i].Y + options.Kx*In[i].X;
                /*  var c = uf[i].Y * cosf + uf[i].X * sinf;
                  var d = uf[i].X * cosf + uf[i].Y * sinf;
                  var r = (d * a + c * b) / (Math.Pow(a, 2) + Math.Pow(b, 2));
                  var x = (c * a - d * b) / (Math.Pow(a, 2) + Math.Pow(b, 2));*/

                double c = uf[i].Y*a - uf[i].X*b;
                double d = uf[i].X*a + uf[i].Y*b;

                double r = (d*cosf - c*sinf)/(a*a + b*b);
                double x = (c*cosf + d*sinf)/(a*a + b*b);

                if (double.IsInfinity(r) || double.IsNaN(r))
                {
                    r = 0;
                }
                if (double.IsInfinity(x) || double.IsNaN(x))
                {
                    x = 0;
                }
                zf[i] = new Point(r, x);
            }
            return zf;
        }


        private Point[] PrepareLineMf(Point[] if1, Point[] uf1, Point[] if2, Point[] uf2)
        {
            Point[] zmf = new Point[if1.Length];
            for (int i = this.StartTime; i < this.EndTime; i++)
            {
                double iabcos = if1[i].X - if2[i].X;
                double iabsin = if1[i].Y - if2[i].Y;
                double uabcos = uf1[i].X - uf2[i].X;
                double uabsin = uf1[i].Y - uf2[i].Y;

                double cosf = Math.Cos(0);
                double sinf = Math.Sin(0);

                double r = (cosf * (uabcos * iabcos + uabsin * iabsin) + sinf * (uabcos * iabsin - uabsin * iabcos)) /
                        (Math.Pow(iabcos, 2) + Math.Pow(iabsin, 2));
                double x = (sinf * (uabcos * iabcos + uabsin * iabsin) + cosf * (uabsin * iabcos - uabcos * iabsin)) /
                        (Math.Pow(iabcos, 2) + Math.Pow(iabsin, 2));
                if (double.IsInfinity(r) || double.IsNaN(r))
                {
                    r = 0;
                }
                if (double.IsInfinity(x) || double.IsNaN(x))
                {
                    x = 0;
                }
                zmf[i] = new Point(r, x);
            }
            return zmf;
        }
    

        public void Save(string fileName)
        {
            XElement characteristics = new XElement("Characteristics");
            foreach (ICharacteristic ch in this._characteristics)
            {
                characteristics.Add(ch.ToXml());
            }
            XElement mainElement = new XElement("PieOptions",
                                           new XElement("Channels",
                                                        new XAttribute("Ia", (object) this.IaChannel ?? string.Empty),
                                                        new XAttribute("Ib", (object) this.IbChannel ?? string.Empty),
                                                        new XAttribute("Ic", (object) this.IcChannel ?? string.Empty),
                                                      
                                                        new XAttribute("Ua", (object) this.UaChannel ?? string.Empty),
                                                        new XAttribute("Ub", (object) this.UbChannel ?? string.Empty),
                                                        new XAttribute("Uc", (object) this.UcChannel ?? string.Empty)
                                               ), this.VisiblyOptions.ToXml()
                );
           
            if (this.Z1 != null)
            {
                mainElement.Add(this.Z1.ToXml("Z1"));
            }
            if (this.Z2 != null)
            {
                mainElement.Add(this.Z2.ToXml("Z2"));
            }
            if (this.Z3 != null)
            {
                mainElement.Add(this.Z3.ToXml("Z3"));
            }
            if (this.Z4 != null)
            {
                mainElement.Add(this.Z4.ToXml("Z4"));
            }
            if (this.Z5 != null)
            {
                mainElement.Add(this.Z5.ToXml("Z5"));
            }
            mainElement.Add(characteristics);
            XDocument doc = new XDocument(mainElement);
            doc.Save(fileName);
        }

        private List<ICharacteristic> CharacteristicFromXml(XElement element)
        {
            List<ICharacteristic> res = new List<ICharacteristic>();
            Assembly assemply =     // нахождение сборки осциллографа
                AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName.Split(',')[0] == "Oscilloscope");
            if (assemply == null)
                throw new AppDomainUnloadedException("Невозможно найти Oscolloscope.exe");
            Type[] assemblyTypes = assemply.GetTypes();
            foreach (XElement fields in element.Elements())
            {
                Type type = assemblyTypes.FirstOrDefault(t => t.Name == fields.Name.LocalName.Split('.').Last());
                if(type == null)
                    throw new TypeLoadException("Невозможно найти тип "+fields.Name.LocalName);
                ICharacteristic item =   (ICharacteristic)Activator.CreateInstance(type, true);
                item.FromXml(fields);
                res.Add(item);
            }
            
            return res;
        }

         public void Load(string fileName)
         {
             XDocument doc = XDocument.Load(fileName);

             foreach (XElement element in doc.Root.Elements())
             {
                 if (element.Name == "Z1")
                 {
                     this.Z1 = new ZnOptions();
                     this.Z1.FromXml(element);
                 }
                 if (element.Name == "Z2")
                 {
                     this.Z2 = new ZnOptions();
                     this.Z2.FromXml(element);
                 }
                 if (element.Name == "Z3")
                 {
                     this.Z3 = new ZnOptions();
                     this.Z3.FromXml(element);
                 }
                 if (element.Name == "Z4")
                 {
                     this.Z4 = new ZnOptions();
                     this.Z4.FromXml(element);
                 }
                 if (element.Name == "Z5")
                 {
                     this.Z5 = new ZnOptions();
                     this.Z5.FromXml(element);
                 }
                 if (element.Name == "Characteristics")
                 {
                     this._characteristics= this.CharacteristicFromXml(element);
                 }
                 if (element.Name == "Visibly")
                 {
                     this.VisiblyOptions.FromXml(element);
                 }
                 if (element.Name == "Channels")
                 {
                     IEnumerable<XAttribute> attributes = element.Attributes();
                     IEnumerable<string> values = attributes.Select(o => o.Value).Where(o => !string.IsNullOrWhiteSpace(o));
                     if (this._allChartOptions != null)
                     {
                         IEnumerable<string> otherChannels = values.Except(this._allChartOptions.Select(o => o.Name));
                         if (otherChannels.Count() != 0)
                         {
                             MessageBox.Show("Неверный файл");
                             return;
                         }
                     }

                     string value = attributes.First(a => a.Name == "Ia").Value;
                     ChannelPieChartOptions option = this._pieIChartOptions.FirstOrDefault(o => o.Name == value);
                     if (option == null)
                     {
                         this.OnOptionOnAisTrue(null, this._pieIChartOptions);
                     }
                     else
                     {
                         option.A = true;
                     }

                     value = attributes.First(a => a.Name == "Ib").Value;
                     option = this._pieIChartOptions.FirstOrDefault(o => o.Name == value);
                     if (option == null)
                     {
                         this.OnOptionOnBisTrue(null, this._pieIChartOptions);
                     }
                     else
                     {
                         option.B = true;
                     }


                     value = attributes.First(a => a.Name == "Ic").Value;
                     option = this._pieIChartOptions.FirstOrDefault(o => o.Name == value);
                     if (option == null)
                     {
                         this.OnOptionOnCisTrue(null, this._pieIChartOptions);
                     }
                     else
                     {
                         option.C = true;
                     }

                     value = attributes.First(a => a.Name == "Ua").Value;
                     option = this._pieUChartOptions.FirstOrDefault(o => o.Name == value);
                     if (option == null)
                     {
                         this.OnOptionOnAisTrue(null, this._pieUChartOptions);
                     }
                     else
                     {
                         option.A = true;
                     }


                     value = attributes.First(a => a.Name == "Ub").Value;
                     option = this._pieUChartOptions.FirstOrDefault(o => o.Name == value);
                     if (option == null)
                     {
                         this.OnOptionOnBisTrue(null, this._pieUChartOptions);
                     }
                     else
                     {
                         option.B = true;
                     }


                     value = attributes.First(a => a.Name == "Uc").Value;
                     option = this._pieUChartOptions.FirstOrDefault(o => o.Name == value);
                     if (option == null)
                     {
                         this.OnOptionOnCisTrue(null, this._pieUChartOptions);
                     }
                     else
                     {
                         option.C = true;
                     }

                     this.ChannelAccept();
                 }
             }
         }

         
        public List<PieChannelInfo> Infos
        {
            get
            {
                this._infos = new List<PieChannelInfo>();
                this.AddedChannel = new List<AnalogChannel>();
               

 

                if (this.ZabCorrect)
                {
                    this.AddNewInfo(this.PrepareLineMf(this.IaChannel.Values, this.UaChannel.Values, this.IbChannel.Values, this.UbChannel.Values), this.VisiblyOptions.Zab);
                }

                if (this.ZbcCorrect)
                {
                    this.AddNewInfo(this.PrepareLineMf(this.IbChannel.Values, this.UbChannel.Values, this.IcChannel.Values, this.UcChannel.Values), this.VisiblyOptions.Zbc);
                }

                if (this.ZcaCorrect)
                {
                    this.AddNewInfo(this.PrepareLineMf(this.IcChannel.Values, this.UcChannel.Values, this.IaChannel.Values, this.UaChannel.Values), this.VisiblyOptions.Zca);
                }
                
                if (this.Z1ACorrect)
                {
                    this.AddNewInfo(this.PrepareLineKz(this.IaChannel.Values, this.UaChannel.Values, this.I0Channel, this.Z1), this.VisiblyOptions.Z1A);
                }

                if (this.Z1BCorrect)
                {
                    this.AddNewInfo(this.PrepareLineKz(this.IbChannel.Values, this.UbChannel.Values, this.I0Channel, this.Z1), this.VisiblyOptions.Z1B);
                }

                if (this.Z1CCorrect)
                {
                    this.AddNewInfo(this.PrepareLineKz(this.IcChannel.Values, this.UcChannel.Values, this.I0Channel, this.Z1), this.VisiblyOptions.Z1C);
                }
                
                if (this.Z2ACorrect)
                {
                    this.AddNewInfo(this.PrepareLineKz(this.IaChannel.Values, this.UaChannel.Values, this.I0Channel, this.Z2), this.VisiblyOptions.Z2A);
                }

                if (this.Z2BCorrect)
                {
                    this.AddNewInfo(this.PrepareLineKz(this.IbChannel.Values, this.UbChannel.Values, this.I0Channel, this.Z2), this.VisiblyOptions.Z2B);
                }

                if (this.Z2CCorrect)
                {
                    this.AddNewInfo(this.PrepareLineKz(this.IcChannel.Values, this.UcChannel.Values, this.I0Channel, this.Z2), this.VisiblyOptions.Z2C);
                }
                
                if (this.Z3ACorrect)
                {
                    this.AddNewInfo(this.PrepareLineKz(this.IaChannel.Values, this.UaChannel.Values, this.I0Channel, this.Z3), this.VisiblyOptions.Z3A);
                }

                if (this.Z3BCorrect)
                {
                    this.AddNewInfo(this.PrepareLineKz(this.IbChannel.Values, this.UbChannel.Values, this.I0Channel, this.Z3), this.VisiblyOptions.Z3B);
                }

                if (this.Z3CCorrect)
                {
                    this.AddNewInfo(this.PrepareLineKz(this.IcChannel.Values, this.UcChannel.Values, this.I0Channel, this.Z3), this.VisiblyOptions.Z3C);
                }


                if (this.Z4ACorrect)
                {
                    this.AddNewInfo(this.PrepareLineKz(this.IaChannel.Values, this.UaChannel.Values, this.I0Channel, this.Z4), this.VisiblyOptions.Z4A);
                }

                if (this.Z4BCorrect)
                {
                    this.AddNewInfo(this.PrepareLineKz(this.IbChannel.Values, this.UbChannel.Values, this.I0Channel, this.Z4), this.VisiblyOptions.Z4B);
                }

                if (this.Z4CCorrect)
                {
                    this.AddNewInfo(this.PrepareLineKz(this.IcChannel.Values, this.UcChannel.Values, this.I0Channel, this.Z4), this.VisiblyOptions.Z4C);
                }


                if (this.Z5ACorrect)
                {
                    this.AddNewInfo(this.PrepareLineKz(this.IaChannel.Values, this.UaChannel.Values, this.I0Channel, this.Z5), this.VisiblyOptions.Z5A);
                }

                if (this.Z5BCorrect)
                {
                    this.AddNewInfo(this.PrepareLineKz(this.IbChannel.Values, this.UbChannel.Values, this.I0Channel, this.Z5), this.VisiblyOptions.Z5B);
                }

                if (this.Z5CCorrect)
                {
                    this.AddNewInfo(this.PrepareLineKz(this.IcChannel.Values, this.UcChannel.Values, this.I0Channel, this.Z5), this.VisiblyOptions.Z5C);
                }

                return this._infos;
            }
        }
        public event Action ChannelChanged;
        public List<AnalogChannel> AddedChannel { get; set; }
        public int Lenght { get; private set; }

        private void AddNewInfo(/*Brush color,*/ Point[] values,VisibilityItem item)
        {
            PieChannelInfo info = new PieChannelInfo(/*color,*/ values, item);
            VisibilityItem a = item;
            if (a != null && a.R)
            {
                this.AddedChannel.Add(new AnalogChannel(item.Channel.Replace('Z', 'R'), "Ом", values.Select(o => o.X).ToArray()));
            }
            if (a != null && a.X)
            {
                this.AddedChannel.Add(new AnalogChannel(item.Channel.Replace('Z', 'X'), "Ом", values.Select(o => o.Y).ToArray()));
            }
            this._infos.Add(info);

        }

        public PieChartOptions(AnalogChannel[] channels)
        {
            this.Lenght = channels[0].Length;
            this._allChartOptions = channels.Select(o => new ChannelPieChartOptions(o)).ToArray();
            this._pieIChartOptions = this._allChartOptions.Where(o => o.Channel.Measure == "A").ToArray();
            this.StartTime = 20;
            this.EndTime = this.Lenght-1;
            
            for (int i = 0; i < this.PieIChartOptions.Length; i++)
            {
                ChannelPieChartOptions option = this.PieIChartOptions[i];

                switch (i)
                {
                    case 0:
                        {
                            option.A = true;
                            break;
                        }
                    case 1:
                        {
                            option.B = true;
                            break;
                        }
                    case 2:
                        {
                            option.C = true;
                            break;
                        }

                }


                option.AisTrue += o => this.OnOptionOnAisTrue(o, this.PieIChartOptions);
                option.BisTrue += o => this.OnOptionOnBisTrue(o, this.PieIChartOptions);
                option.CisTrue += o => this.OnOptionOnCisTrue(o, this.PieIChartOptions);
               // option.NisTrue += o => OnOptionOnNisTrue(o, PieIChartOptions);
                option.PropertyChanged += this.option_PropertyChanged;
            }


            this._pieUChartOptions = this._allChartOptions.Where(o => o.Channel.Measure == "V").ToArray(); 
            for (int i = 0; i < this.PieUChartOptions.Length; i++)
            {
                ChannelPieChartOptions option = this.PieUChartOptions[i];
                switch (i)
                {
                    case 0:
                        {
                            option.A = true;
                            break;
                        }
                    case 1:
                        {
                            option.B = true;
                            break;
                        }
                    case 2:
                        {
                            option.C = true;
                            break;
                        }
                   /* case 3:
                        {
                            option.N = true;
                            break;
                        }*/
                }
                option.AisTrue += o => this.OnOptionOnAisTrue(o, this.PieUChartOptions);
                option.BisTrue += o => this.OnOptionOnBisTrue(o, this.PieUChartOptions);
                option.CisTrue += o => this.OnOptionOnCisTrue(o, this.PieUChartOptions);
                option.PropertyChanged += this.option_PropertyChanged;
            }
        }

        public ChannelPieChartOptions[] PieIChartOptions
        {
            get { return this._pieIChartOptions; }
        }

        public ChannelPieChartOptions[] PieUChartOptions
        {
            get { return this._pieUChartOptions; }
        }

        public ChannelPieChartOptions[] AllChartOptions
        {
            get { return this._allChartOptions; }
        }


        void option_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
              if (this.ChannelChanged != null)
              {
                  this.ChannelChanged.Invoke();
              }
        }


        private void OnOptionOnAisTrue(ChannelPieChartOptions o, ChannelPieChartOptions[] collection)
        {
            foreach (ChannelPieChartOptions optionse in collection)
            {
                if (optionse != o)
                {
                    optionse.A = false;
                }
            }
        }

        private void OnOptionOnBisTrue(ChannelPieChartOptions o, ChannelPieChartOptions[] collection)
        {
            foreach (ChannelPieChartOptions optionse in collection)
            {
                if (optionse != o)
                {
                    optionse.B = false;
                }
            }
        }

        private void OnOptionOnCisTrue(ChannelPieChartOptions o, ChannelPieChartOptions[] collection)
        {
            foreach (ChannelPieChartOptions optionse in collection)
            {
                if (optionse != o)
                {
                    optionse.C = false;
                }
            }
        }
     
            public IEnumerable<ICharacteristic> CharacteristicsVisibly
        {
            get { return this._characteristics.Where(o=>o.Enabled); }
  
        }
        public List<ICharacteristic> Characteristics
        {
            get { return this._characteristics; }
            set { this._characteristics = value; }
        }

        public PieChartVisiblyOptions VisiblyOptions
        {
            get { return this._visiblyOptions; }
            set { this._visiblyOptions = value; }
        }



        public bool IsCorrect
        {
            get
            {
                return this.ZabCorrect || this.ZbcCorrect || this.ZcaCorrect || this.Z1ACorrect || this.Z1BCorrect || this.Z1CCorrect || this.Z2ACorrect || this.Z2BCorrect || this.Z2CCorrect || this.Z3ACorrect || this.Z3BCorrect || this.Z3CCorrect || this.Z4ACorrect || this.Z4BCorrect || this.Z4CCorrect || this.Z5ACorrect || this.Z5BCorrect || this.Z5CCorrect ;
            }
        }


        public void ChannelAccept()
        {
            this.IaChannel = this._pieIChartOptions.FirstOrDefault(o => o.A);
            this.IbChannel = this._pieIChartOptions.FirstOrDefault(o => o.B);
            this.IcChannel = this._pieIChartOptions.FirstOrDefault(o => o.C);

            this.UaChannel = this._pieUChartOptions.FirstOrDefault(o => o.A);
            this.UbChannel = this._pieUChartOptions.FirstOrDefault(o => o.B);
            this.UcChannel = this._pieUChartOptions.FirstOrDefault(o => o.C);

            if ((this.IaChannel!= null)&&(this.IbChannel!= null)&&(this.IcChannel!= null))
            {
                this.I0Channel = new Point[this.IaChannel.Values.Count()];
                for (int i = 0; i < this.IaChannel.Values.Count(); i++)
                {
                    double x = (this.IaChannel.Values[i].X + this.IbChannel.Values[i].X + this.IcChannel.Values[i].X)/3.0;
                    double y = (this.IaChannel.Values[i].Y + this.IbChannel.Values[i].Y + this.IcChannel.Values[i].Y) / 3.0;
                    this.I0Channel[i] = new Point(x,y);
                }
            }
            else
            {
                this.I0Channel = null;
            }
            
        }

       
        public bool ZabCorrect
        {
            get { return (this.IaChannel != null) && (this.IbChannel != null) && (this.UaChannel != null) && (this.UbChannel != null); }
        }

        public bool ZbcCorrect
        {
            get { return (this.IcChannel != null) && (this.IbChannel != null) && (this.UcChannel != null) && (this.UbChannel != null); }
        }

        public bool ZcaCorrect
        {
            get { return (this.IaChannel != null) && (this.IcChannel != null) && (this.UaChannel != null) && (this.UcChannel != null); }
        }

        public bool Z1ACorrect
        {
            get { return (this.IaChannel != null)  && (this.IbChannel != null) &&(this.IcChannel != null)  && (this.UaChannel != null) && (this.Z1 != null); }
        }

        public bool Z1BCorrect
        {
            get { return (this.IbChannel != null) && (this.IaChannel != null)  && (this.IcChannel != null) && (this.UbChannel != null) && (this.Z1 != null); }
        }

        public bool Z1CCorrect
        {
            get { return (this.IcChannel != null) && (this.IaChannel != null) && (this.IbChannel != null)  && (this.UcChannel != null) && (this.Z1 != null); }
        }

        public bool Z2ACorrect
        {
            get { return (this.IaChannel != null) && (this.IbChannel != null) && (this.IcChannel != null) && (this.UaChannel != null) && (this.Z2 != null); }
        }

        public bool Z2BCorrect
        {
            get { return (this.IbChannel != null) && (this.IaChannel != null) && (this.IcChannel != null) && (this.UbChannel != null) && (this.Z2 != null); }
        }

        public bool Z2CCorrect
        {
            get { return (this.IcChannel != null) && (this.IaChannel != null) && (this.IbChannel != null)  && (this.UcChannel != null) && (this.Z2 != null); }
        }


        public bool Z3ACorrect
        {
            get { return (this.IaChannel != null) && (this.IbChannel != null) && (this.IcChannel != null) && (this.UaChannel != null) && (this.Z3 != null); }
        }

        public bool Z3BCorrect
        {
            get { return (this.IbChannel != null) && (this.IaChannel != null) && (this.IcChannel != null) && (this.UbChannel != null) && (this.Z3 != null); }
        }

        public bool Z3CCorrect
        {
            get { return (this.IcChannel != null) && (this.IaChannel != null) && (this.IbChannel != null) && (this.UcChannel != null) && (this.Z3 != null); }
        }



        public bool Z4ACorrect
        {
            get { return (this.IaChannel != null) && (this.IbChannel != null) && (this.IcChannel != null) && (this.UaChannel != null) && (this.Z4 != null); }
        }

        public bool Z4BCorrect
        {
            get { return (this.IbChannel != null) && (this.IaChannel != null) && (this.IcChannel != null) && (this.UbChannel != null) && (this.Z4 != null); }
        }

        public bool Z4CCorrect
        {
            get { return (this.IcChannel != null) && (this.IaChannel != null) && (this.IbChannel != null) && (this.UcChannel != null) && (this.Z4 != null); }
        }


        public bool Z5ACorrect
        {
            get { return (this.IaChannel != null) && (this.IbChannel != null) && (this.IcChannel != null) && (this.UaChannel != null) && (this.Z5 != null); }
        }

        public bool Z5BCorrect
        {
            get { return (this.IbChannel != null) && (this.IaChannel != null) && (this.IcChannel != null) && (this.UbChannel != null) && (this.Z5 != null); }
        }

        public bool Z5CCorrect
        {
            get { return (this.IcChannel != null) && (this.IaChannel != null) && (this.IbChannel != null) && (this.UcChannel != null) && (this.Z5 != null); }
        }

        public int StartTime { get; set; }
        public int EndTime { get; set; }


 
    }
}