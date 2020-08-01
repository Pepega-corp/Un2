using org.mariuszgromada.math.mxparser;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Unicon2.Formatting.Infrastructure.Model;
using Unicon2.Formatting.Model.Base;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Interfaces.DataOperations;
using Unicon2.Infrastructure.Interfaces.Visitors;
using Unicon2.Infrastructure.Values;
using Unicon2.Unity.Interfaces;
using Expression = org.mariuszgromada.math.mxparser.Expression;

namespace Unicon2.Formatting.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class FormulaFormatter : UshortsFormatterBase, IFormulaFormatter
    {
   
        private ushort _numberOfSimbolsAfterComma;

        public FormulaFormatter()
        {
            UshortFormattableResources = new List<string>();
        }

        public override object Clone()
        {
            FormulaFormatter cloneFormulaFormatter = new FormulaFormatter();
            cloneFormulaFormatter.FormulaString = FormulaString;
            cloneFormulaFormatter.NumberOfSimbolsAfterComma = NumberOfSimbolsAfterComma;
            return cloneFormulaFormatter;
        }

        public override T Accept<T>(IFormatterVisitor<T> visitor)
        {
            return visitor.VisitFormulaFormatter(this);
        }

        [JsonProperty]
        public string FormulaString { get; set; }
        [JsonProperty]
        public List<string> UshortFormattableResources { get; set; }

       
       

        [JsonProperty]

        public ushort NumberOfSimbolsAfterComma
        {
            get { return _numberOfSimbolsAfterComma; }
            set
            {
                _numberOfSimbolsAfterComma = value;
            }
        }
    }
}