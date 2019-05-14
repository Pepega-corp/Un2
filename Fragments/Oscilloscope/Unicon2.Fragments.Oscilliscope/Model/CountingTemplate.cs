using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unicon2.Formatting.Infrastructure.Model;
using Unicon2.Fragments.Journals.Infrastructure.Model;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Model.CountingTemplate;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Oscilliscope.Model
{
    [DataContract(Namespace = "CountingTemplateNS")]
    public class CountingTemplate : ICountingTemplate
    {
        #region Implementation of ICountingTemplate
        [DataMember]
        public IRecordTemplate RecordTemplate { get; set; }

        public List<string> GetCountingNames()
        {
            throw new NotImplementedException();
        }

        public ushort[] GetCountingValuesUshorts()
        {
            return new ushort[0];
        }

        public int GetNumberOfAnalogs()
        {
            int numOfAnalogs = 0;
            foreach (IJournalParameter journalParameter in this.RecordTemplate.JournalParameters)
            {
                if (!(journalParameter.UshortsFormatter is IBitMaskFormatter))
                {
                    numOfAnalogs++;
                }
            }
            return numOfAnalogs;
        }

        public List<string> GetAnalogsNames()
        {
            List<string> analogNames = new List<string>();
            foreach (IJournalParameter journalParameter in this.RecordTemplate.JournalParameters)
            {
                if (!(journalParameter.UshortsFormatter is IBitMaskFormatter))
                {
                    analogNames.Add(journalParameter.Name);
                }
            }
            return analogNames;
        }

        public string GetMeasureUnit(int numberOfElement)
        {
            if (this.RecordTemplate.JournalParameters.Count <= numberOfElement) return string.Empty;
            if (this.RecordTemplate.JournalParameters[numberOfElement].IsMeasureUnitEnabled)
            {
                return this.RecordTemplate.JournalParameters[numberOfElement].MeasureUnit;
            }
            return string.Empty;
        }

        public int GetNumberOfDiscrets()
        {
            int numOfDiscrets = 0;
            foreach (IJournalParameter journalParameter in this.RecordTemplate.JournalParameters)
            {
                if ((journalParameter.UshortsFormatter is IBitMaskFormatter))
                {
                    numOfDiscrets += journalParameter.NumberOfPoints * 16;
                }
            }
            return numOfDiscrets;
        }

        public List<string> GetDiscretsNames()
        {
            List<string> discretNames = new List<string>();
            foreach (IJournalParameter journalParameter in this.RecordTemplate.JournalParameters)
            {
                if ((journalParameter.UshortsFormatter is IBitMaskFormatter))
                {
                    int index = 0;
                    foreach (string bitSignature in (journalParameter.UshortsFormatter as IBitMaskFormatter).BitSignatures)
                    {
                        if (index <= journalParameter.NumberOfPoints * 16)
                        {
                            discretNames.Add(bitSignature);
                        }
                        index++;
                    }

                }
            }
            return discretNames;
        }

        public int GetAllChannels()
        {
            int numOfChannels = 0;
            foreach (IJournalParameter journalParameter in this.RecordTemplate.JournalParameters)
            {
                if ((journalParameter.UshortsFormatter is IBitMaskFormatter))
                {
                    numOfChannels += journalParameter.NumberOfPoints * 16;
                }
                else
                {
                    numOfChannels++;
                }
            }
            return numOfChannels;
        }

        #endregion

        #region Implementation of IDataProviderContaining

        public void SetDataProvider(IDataProvider dataProvider)
        {
            this.RecordTemplate.JournalParameters.ForEach((parameter =>
           {
               (parameter.UshortsFormatter as IDataProviderContaining)?.SetDataProvider(dataProvider);
           }));
        }

        #endregion

        #region Implementation of IInitializableFromContainer

        public bool IsInitialized { get; }

        public void InitializeFromContainer(ITypesContainer container)
        {
            this.RecordTemplate.JournalParameters.ForEach((parameter =>
            {
                (parameter.UshortsFormatter as IInitializableFromContainer)?.InitializeFromContainer(container);
            }));
        }

        #endregion
    }
}
