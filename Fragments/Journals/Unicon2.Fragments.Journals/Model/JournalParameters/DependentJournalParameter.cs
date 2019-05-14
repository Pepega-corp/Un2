using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Unicon2.Fragments.Journals.Infrastructure.Model.JournalParameters;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.DataOperations;
using Unicon2.Infrastructure.Values;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Journals.Model.JournalParameters
{
    [DataContract(Namespace = "DependentJournalParameterNS", IsReference = true)]
    public class DependentJournalParameter : JournalParameter, IDependentJournalParameter
    {
        public DependentJournalParameter()
        {
            this.JournalParameterDependancyConditions = new List<IJournalCondition>();
        }

        #region Implementation of IDependentJournalParameter
        [DataMember]
        public List<IJournalCondition> JournalParameterDependancyConditions { get; set; }
        #endregion

        #region Overrides of JournalParameter

        public override async Task<List<IFormattedValue>> GetFormattedValues(ushort[] recordUshorts)
        {
            List<IFormattedValue> formattedValues = new List<IFormattedValue>();
            ushort[] valuesToFormat = recordUshorts.Skip(this.StartAddress)
                .Take(this.NumberOfPoints).ToArray();

            foreach (IJournalCondition journalParameterDependancyCondition in this.JournalParameterDependancyConditions)
            {
                if (journalParameterDependancyCondition.GetConditionResult(recordUshorts))
                {

                    Task loadingTask = (journalParameterDependancyCondition.UshortsFormatter as ILoadable)?.Load();
                    if (loadingTask != null)
                        await loadingTask;
                    formattedValues.Add(journalParameterDependancyCondition.UshortsFormatter.Format(valuesToFormat));
                }
            }
            return formattedValues;
        }

        #region Overrides of JournalParameter

        public override void InitializeFromContainer(ITypesContainer container)
        {
            foreach (IJournalCondition journalParameterDependancyCondition in this.JournalParameterDependancyConditions)
            {
                (journalParameterDependancyCondition.UshortsFormatter as IInitializableFromContainer)?.InitializeFromContainer(container);
                journalParameterDependancyCondition.BaseJournalParameter.InitializeFromContainer(container);
            }
            base.InitializeFromContainer(container);
        }

        #endregion

        #endregion
    }
}
