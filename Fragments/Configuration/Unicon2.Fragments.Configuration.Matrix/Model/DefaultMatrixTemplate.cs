using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Unicon2.Fragments.Configuration.Matrix.Model.OptionTemplates;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Values.Matrix;
using Unicon2.Infrastructure.Values.Matrix.OptionTemplates;

namespace Unicon2.Fragments.Configuration.Matrix.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class DefaultMatrixTemplate : IMatrixTemplate
    {
        public DefaultMatrixTemplate()
        {
            this.ResultBitOptions = new List<IBitOption>();
            this.MatrixMemoryVariables = new List<IMatrixMemoryVariable>();
            this.VariableColumnSignatures = new List<IVariableColumnSignature>();
            this.MatrixVariableOptionTemplate = new BoolMatrixVariableOptionTemplate();
        }

        [JsonProperty]
        public int NumberOfBitsOnEachVariable { get; set; }
        [JsonProperty]
        public List<IMatrixMemoryVariable> MatrixMemoryVariables { get; set; }
        [JsonProperty]
        public List<IVariableColumnSignature> VariableColumnSignatures { get; set; }
        [JsonProperty]
        public IMatrixVariableOptionTemplate MatrixVariableOptionTemplate { get; set; }

        [JsonProperty]
        public List<IBitOption> ResultBitOptions { get; set; }

        public object Clone()
        {
            return new DefaultMatrixTemplate();
        }
    }
}
