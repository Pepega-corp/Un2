using org.mariuszgromada.math.mxparser;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
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
    [DataContract(Name = nameof(FormulaFormatter), Namespace = "FormulaFormatterNS", IsReference = true)]
    public class FormulaFormatter : UshortsFormatterBase, IFormulaFormatter
    {
   
        private ushort _numberOfSimbolsAfterComma;

        public FormulaFormatter()
        {
            UshortFormattables = new List<IUshortFormattable>();
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

        [DataMember(Name = nameof(FormulaString))]
        public string FormulaString { get; set; }
        [DataMember]
        public List<IUshortFormattable> UshortFormattables { get; set; }

        [DataMember]

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