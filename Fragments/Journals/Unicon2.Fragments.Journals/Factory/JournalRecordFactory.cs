using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unicon2.Fragments.Journals.Infrastructure.Factories;
using Unicon2.Fragments.Journals.Infrastructure.Model;
using Unicon2.Fragments.Journals.Infrastructure.Model.JournalParameters;
using Unicon2.Fragments.Journals.Model.JournalParameters;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Interfaces.Dependancy;
using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Presentation.Infrastructure.Services.Formatting;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Journals.Factory
{
    public class JournalRecordFactory : IJournalRecordFactory
    {
        private readonly ITypesContainer _container;
        private readonly IFormattingService _formattingService;

        public JournalRecordFactory(ITypesContainer container, IFormattingService formattingService)
        {
            _container = container;
            _formattingService = formattingService;
        }

        public async Task<IJournalRecord> CreateJournalRecord(ushort[] values, IRecordTemplate recordTemplate,
            DeviceContext deviceContext)
        {
            if (values.All(o => o == 0))
                return null;
            IJournalRecord journalRecord = _container.Resolve<IJournalRecord>();

            foreach (IJournalParameter journalParameter in recordTemplate.JournalParameters)
            {
                journalRecord.FormattedValues.AddRange(
                    await GetValuesForRecord(journalParameter, values, deviceContext));
            }

            return journalRecord;
        }

        public async Task<List<IFormattedValue>> GetValuesForRecord(IJournalParameter parameter,
            ushort[] recordUshorts, DeviceContext deviceContext)
        {
            var formattedValues = new List<IFormattedValue>();
            switch (parameter)
            {
                case ComplexJournalParameter complexJournalParameter:
                    foreach (ISubJournalParameter childJournalParameter in complexJournalParameter
                        .ChildJournalParameters)
                    {
                        formattedValues.Add(await _formattingService.FormatValueAsync(
                            childJournalParameter.UshortsFormatter,
                            new[]
                            {
                                GetParameterUshortInRecord(recordUshorts, complexJournalParameter,
                                    childJournalParameter)
                            }, deviceContext));
                    }

                    break;
                case DependentJournalParameter dependentJournalParameter:
                    ushort[] dependentValuesToFormat = recordUshorts.Skip(dependentJournalParameter.StartAddress)
                        .Take(dependentJournalParameter.NumberOfPoints).ToArray();
                    foreach (IJournalCondition journalParameterDependancyCondition in dependentJournalParameter
                        .JournalParameterDependancyConditions)
                    {
                        if (GetConditionResult(recordUshorts, journalParameterDependancyCondition,
                            dependentJournalParameter))
                        {
                            //Task loadingTask = (journalParameterDependancyCondition.UshortsFormatter as ILoadable)?.Load();
                            //if (loadingTask != null)
                            //    await loadingTask;
                            formattedValues.Add(await _formattingService.FormatValueAsync(
                                journalParameterDependancyCondition.UshortsFormatter, dependentValuesToFormat,deviceContext));
                        }
                    }

                    break;
                case JournalParameter journalParameter:
                    ushort[] valuesToFormat = recordUshorts.Skip(journalParameter.StartAddress)
                        .Take(journalParameter.NumberOfPoints).ToArray();

                    formattedValues.Add(await _formattingService.FormatValueAsync(journalParameter.UshortsFormatter,
                        valuesToFormat,deviceContext));
                    break;
            }

            return formattedValues;
        }

        private bool GetConditionResult(ushort[] recordUshorts, IJournalCondition journalParameterDependancyCondition,
            DependentJournalParameter dependentJournalParameter)
        {
            if (journalParameterDependancyCondition.ConditionsEnum == ConditionsEnum.HaveTrueBitAt)
            {
                BitArray bitArray = new BitArray(new int[]
                    {recordUshorts[journalParameterDependancyCondition.BaseJournalParameter.StartAddress]});
                if (journalParameterDependancyCondition.BaseJournalParameter is ISubJournalParameter subJournalParameter
                )
                {
                    bitArray = new BitArray(new int[]
                        {GetParameterUshortInRecord(recordUshorts, dependentJournalParameter, subJournalParameter)});
                }

                return bitArray[journalParameterDependancyCondition.UshortValueToCompare];
            }

            if (journalParameterDependancyCondition.ConditionsEnum == ConditionsEnum.HaveFalseBitAt)
            {
                BitArray bitArray = new BitArray(new int[]
                    {recordUshorts[journalParameterDependancyCondition.BaseJournalParameter.StartAddress]});
                if (journalParameterDependancyCondition.BaseJournalParameter is ISubJournalParameter subJournalParameter
                )
                {
                    bitArray = new BitArray(new int[]
                        {GetParameterUshortInRecord(recordUshorts, dependentJournalParameter, subJournalParameter)});
                }

                return !bitArray[journalParameterDependancyCondition.UshortValueToCompare];
            }

            if (journalParameterDependancyCondition.ConditionsEnum == ConditionsEnum.Equal)
            {
                ushort resUshort = recordUshorts[journalParameterDependancyCondition.BaseJournalParameter.StartAddress];
                if (journalParameterDependancyCondition.BaseJournalParameter is ISubJournalParameter subJournalParameter
                )
                {
                    resUshort = GetParameterUshortInRecord(recordUshorts, dependentJournalParameter,
                        subJournalParameter);
                }

                return resUshort == journalParameterDependancyCondition.UshortValueToCompare;
            }

            return false;
        }


        private ushort GetParameterUshortInRecord(ushort[] recordUshorts,
            IJournalParameter parentJournalParameter, ISubJournalParameter childJournalParameter)
        {
            ushort valueToFormat = recordUshorts.Skip(parentJournalParameter.StartAddress).Take(1).ToArray()[0];
            BitArray parentBitArray = new BitArray(new[] {(int) valueToFormat});
            bool[] subParameterBools = new bool[childJournalParameter.BitNumbersInWord.Count];
            foreach (var numInWord in childJournalParameter.BitNumbersInWord)
            {
                if (parentBitArray.Count <= numInWord)
                {
                    subParameterBools[childJournalParameter.BitNumbersInWord.IndexOf(numInWord)] = false;
                }
                else
                {
                    subParameterBools[childJournalParameter.BitNumbersInWord.IndexOf(numInWord)] =
                        parentBitArray[numInWord];
                }
            }

            ushort res = (ushort) (new BitArray(subParameterBools).GetIntFromBitArray());
            return res;
        }
    }
}