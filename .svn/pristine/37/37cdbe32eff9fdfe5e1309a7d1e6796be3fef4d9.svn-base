using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Unicon2.Fragments.Journals.Infrastructure.Model;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Values;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Journals.Model.JournalParameters
{
    [DataContract(Namespace = "JournalParameterNS", IsReference = true)]
    public class JournalParameter : IJournalParameter
    {
        #region Implementation of IStronglyNamed

        public string StrongName { get; }

        #endregion

        #region Implementation of INameable
        [DataMember]
        public string Name { get; set; }

        #endregion


        #region Implementation of IUshortFormattable
        [DataMember]
        public IUshortsFormatter UshortsFormatter { get; set; }

        #endregion

        #region Implementation of IMeasurable
        [DataMember]
        public string MeasureUnit { get; set; }
        [DataMember]
        public bool IsMeasureUnitEnabled { get; set; }

        #endregion

        #region Implementation of IJournalParameter
        [DataMember]
        public ushort StartAddress { get; set; }
        [DataMember]
        public ushort NumberOfPoints { get; set; }

        public virtual async Task<List<IFormattedValue>> GetFormattedValues(ushort[] recordUshorts)
        {
            List<IFormattedValue> formattedValues = new List<IFormattedValue>();
            ushort[] valuesToFormat = recordUshorts.Skip(this.StartAddress)
                .Take(this.NumberOfPoints).ToArray();
            formattedValues.Add(this.UshortsFormatter.Format(valuesToFormat));
            return formattedValues;
        }

        #endregion

        #region Implementation of IInitializableFromContainer

        public bool IsInitialized { get; private set; }

        public virtual void InitializeFromContainer(ITypesContainer container)
        {
            this.IsInitialized = true;
            (this.UshortsFormatter as IInitializableFromContainer)?.InitializeFromContainer(container);
        }

        #endregion

    }
}
