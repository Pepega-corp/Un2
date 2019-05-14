using System.Collections.Generic;
using System.Runtime.Serialization;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.Model;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.Model.Helpers;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.Model.OptionTemplates;
using Unicon2.Fragments.Configuration.Matrix.Model.OptionTemplates;
using Unicon2.Infrastructure.Common;

namespace Unicon2.Fragments.Configuration.Matrix.Model
{
    [DataContract(Namespace = "AppointableMatrixNS", Name = nameof(DefaultMatrixTemplate), IsReference = true)]

    public class DefaultMatrixTemplate : IMatrixTemplate
    {
        public DefaultMatrixTemplate()
        {
            this.ResultBitOptions = new List<IBitOption>();
            this.MatrixMemoryVariables = new List<IMatrixMemoryVariable>();
            this.VariableOptionSignatures = new List<IVariableSignature>();
            this.MatrixVariableOptionTemplate = new BoolMatrixVariableOptionTemplate();
        }

        #region Implementation of IMatrixTemplate
        [DataMember]
        public int NumberOfBitsOnEachVariable { get; set; }
        [DataMember]
        public List<IMatrixMemoryVariable> MatrixMemoryVariables { get; set; }
        [DataMember]
        public List<IVariableSignature> VariableOptionSignatures { get; set; }
        [DataMember]
        public IMatrixVariableOptionTemplate MatrixVariableOptionTemplate { get; set; }
        public void UpdateResultBitOptions()
        {
            this.ResultBitOptions =
            (StaticContainer.Container.Resolve(typeof(IResultBitOptionSeedingStrategy)) as
                IResultBitOptionSeedingStrategy).UpdateBitOptions(this, this.ResultBitOptions);
        }
        [DataMember]
        public List<IBitOption> ResultBitOptions { get; set; }

        #endregion
    }
}
