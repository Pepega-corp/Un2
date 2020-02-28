using System;
using System.Runtime.Serialization;
using Unicon2.Formatting.Infrastructure.Model;
using Unicon2.Formatting.Model.Base;
using Unicon2.Infrastructure.Interfaces.Visitors;
using Unicon2.Infrastructure.Values;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Formatting.Model
{
    [DataContract(Namespace = "DefaultTimeFormatterNS", IsReference = true)]
    public class DefaultTimeFormatter : UshortsFormatterBase, IDefaultTimeFormatter
    {

        public DefaultTimeFormatter()
        {
        }

        public override object Clone()
        {
            DefaultTimeFormatter cloneFormatter = new DefaultTimeFormatter();
            cloneFormatter.DayInMonthPointNumber = this.DayInMonthPointNumber;
            cloneFormatter.YearPointNumber = this.YearPointNumber;
            cloneFormatter.MonthPointNumber = this.MonthPointNumber;
            cloneFormatter.HoursPointNumber = this.HoursPointNumber;
            cloneFormatter.MinutesPointNumber = this.MinutesPointNumber;
            cloneFormatter.SecondsPointNumber = this.SecondsPointNumber;
            cloneFormatter.MillisecondsDecimalsPlaces = this.MillisecondsDecimalsPlaces;
            cloneFormatter.MillisecondsPointNumber = this.MillisecondsPointNumber;
            cloneFormatter.NumberOfPointsInUse = this.NumberOfPointsInUse;

            return cloneFormatter;
        }

        public override T Accept<T>(IFormatterVisitor<T> visitor)
        {
            return visitor.VisitTimeFormatter(this);
        }

        [DataMember]
        public int MillisecondsDecimalsPlaces { get; set; }
        [DataMember]
        public int NumberOfPointsInUse { get; set; }
        [DataMember]
        public int YearPointNumber { get; set; }
        [DataMember]
        public int MonthPointNumber { get; set; }
        [DataMember]
        public int DayInMonthPointNumber { get; set; }
        [DataMember]
        public int HoursPointNumber { get; set; }
        [DataMember]
        public int MinutesPointNumber { get; set; }
        [DataMember]
        public int SecondsPointNumber { get; set; }
        [DataMember]
        public int MillisecondsPointNumber { get; set; }
    }
}
