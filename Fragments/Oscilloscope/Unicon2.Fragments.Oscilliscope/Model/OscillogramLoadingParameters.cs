using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Unicon2.Fragments.Journals.Infrastructure.Model;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Keys;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Model;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Model.OscillogramLoadingParameters;
using Unicon2.Infrastructure.Values;

namespace Unicon2.Fragments.Oscilliscope.Model
{
    [DataContract(Namespace = "OscillogramLoadingParametersNS")]
    public class OscillogramLoadingParameters : IOscillogramLoadingParameters
    {
        private List<IFormattedValue> _formattedValues;
        private IRecordTemplate _recordTemplate;
        private int _oscillogramCountingsNumber;
        private int _sizeOfCountingInWords;
        private int _pointOfStart;
        private int _sizeAfter;

        private int _beginAddresInData;
        private ITimeValue _dataTimeJournalValue;
        private string _alarm;

        public OscillogramLoadingParameters()
        {
            this.OscilloscopeTags = new List<IOscilloscopeTag>();
        }


        [DataMember]
        public List<IOscilloscopeTag> OscilloscopeTags { get; set; }
        [DataMember]
        public ushort AddressOfOscillogram { get; set; }
        [DataMember]
        public ushort MaxSizeOfRewritableOscillogramInMs { get; set; }

        [DataMember]
        public bool IsFullPageLoading { get; set; }

        public void Initialize(List<IFormattedValue> formattedValues, IRecordTemplate recordTemplate)
        {
            this._formattedValues = formattedValues;
            this._recordTemplate = recordTemplate;
            int numberOfValue = 0;
            numberOfValue = this._recordTemplate.JournalParameters.IndexOf(this.OscilloscopeTags
                .First((tag => tag.TagKey == OscilloscopeKeys.LEN)).RelatedJournalParameter);
            this._oscillogramCountingsNumber = (int)Math.Ceiling((this._formattedValues[numberOfValue] as INumericValue).NumValue);

            numberOfValue = this._recordTemplate.JournalParameters.IndexOf(this.OscilloscopeTags
               .First((tag => tag.TagKey == OscilloscopeKeys.REZ)).RelatedJournalParameter);
            this._sizeOfCountingInWords = (int)Math.Ceiling((this._formattedValues[numberOfValue] as INumericValue).NumValue);

            numberOfValue = this._recordTemplate.JournalParameters.IndexOf(this.OscilloscopeTags
               .First((tag => tag.TagKey == OscilloscopeKeys.POINT)).RelatedJournalParameter);
            this._pointOfStart = (int)Math.Ceiling((this._formattedValues[numberOfValue] as INumericValue).NumValue);



            numberOfValue = this._recordTemplate.JournalParameters.IndexOf(this.OscilloscopeTags
               .First((tag => tag.TagKey == OscilloscopeKeys.AFTER)).RelatedJournalParameter);
            this._sizeAfter = (int)Math.Ceiling((this._formattedValues[numberOfValue] as INumericValue).NumValue);


            numberOfValue = this._recordTemplate.JournalParameters.IndexOf(this.OscilloscopeTags
               .First((tag => tag.TagKey == OscilloscopeKeys.BEGIN)).RelatedJournalParameter);
            this._beginAddresInData = (int)Math.Ceiling((this._formattedValues[numberOfValue] as INumericValue).NumValue);


            numberOfValue = this._recordTemplate.JournalParameters.IndexOf(this.OscilloscopeTags
               .First((tag => tag.TagKey == OscilloscopeKeys.DATATIME)).RelatedJournalParameter);
            if (!(this._formattedValues[numberOfValue] is ITimeValue)) throw new ArgumentException("Time value not on right place");
            this._dataTimeJournalValue = (this._formattedValues[numberOfValue] as ITimeValue);

            numberOfValue = this._recordTemplate.JournalParameters.IndexOf(this.OscilloscopeTags
                .First((tag => tag.TagKey == OscilloscopeKeys.ALM)).RelatedJournalParameter);
            this._alarm = (this._formattedValues[numberOfValue]).AsString();
        }

        public int GetOscillogramCountingsNumber()
        {
            return this._oscillogramCountingsNumber;

        }

        public int GetSizeOfCountingInWords()
        {
            return this._sizeOfCountingInWords;
        }

        public int GetPointOfStart()
        {
            return this._pointOfStart;


        }

        public int GetSizeAfter()
        {
            return this._sizeAfter;
        }

        public int GetReadyPointAddress()
        {
            IJournalParameter val = this.OscilloscopeTags
                .First((tag => tag.TagKey == OscilloscopeKeys.READY)).RelatedJournalParameter;
            return val.StartAddress;
        }

        public string GetDataTimeofAlarm()
        {
            DateTime dateTime = this._dataTimeJournalValue.GetFullDateTime();

            DateTime alarmDateTime = dateTime.AddMilliseconds(this._oscillogramCountingsNumber - this._sizeAfter);
            return
                $"{alarmDateTime.Month:D2}/{alarmDateTime.Day:D2}/{this._dataTimeJournalValue.YearValue:D2},{alarmDateTime.Hour:D2}:{alarmDateTime.Minute:D2}:{alarmDateTime.Second:D2}.{alarmDateTime.Millisecond:D3}";

        }

        public int GetBeginAddresInData()
        {
            return this._beginAddresInData;

        }

        public string GetDateTime()
        {
            return this._dataTimeJournalValue.GetPindosFullFormatDateTime();
        }
        public string GetAlarm()
        {
            return this._alarm;

        }

        public ushort GetReadyPointNumberOfPoints()
        {
            IJournalParameter val = this.OscilloscopeTags
                .First((tag => tag.TagKey == OscilloscopeKeys.READY)).RelatedJournalParameter;
            return val.NumberOfPoints;

        }
    }
}
