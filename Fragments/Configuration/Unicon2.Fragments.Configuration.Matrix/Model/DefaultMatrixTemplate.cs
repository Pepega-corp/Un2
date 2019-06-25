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
            this.VariableColumnSignatures = new List<IVariableColumnSignature>();
            this.MatrixVariableOptionTemplate = new BoolMatrixVariableOptionTemplate();
        }

        #region Implementation of IMatrixTemplate
        [DataMember]
        public int NumberOfBitsOnEachVariable { get; set; }
        [DataMember]
        public List<IMatrixMemoryVariable> MatrixMemoryVariables { get; set; }
        [DataMember]
        public List<IVariableColumnSignature> VariableColumnSignatures { get; set; }
        [DataMember]
        public IMatrixVariableOptionTemplate MatrixVariableOptionTemplate { get; set; }

        [DataMember]
        public List<IBitOption> ResultBitOptions { get; set; }

        #endregion

        #region Implementation of ICloneable

        public object Clone()
        {
            return new DefaultMatrixTemplate();
        }

        #endregion

    }
}
