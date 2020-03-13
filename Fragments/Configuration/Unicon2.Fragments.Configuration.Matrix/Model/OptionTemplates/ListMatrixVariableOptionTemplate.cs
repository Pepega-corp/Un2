using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Unicon2.Fragments.Configuration.Matrix.Keys;
using Unicon2.Infrastructure.Values.Matrix.OptionTemplates;

namespace Unicon2.Fragments.Configuration.Matrix.Model.OptionTemplates
{
    [JsonObject(MemberSerialization.OptIn)]

    public class ListMatrixVariableOptionTemplate : IMatrixVariableOptionTemplate
    {
        public ListMatrixVariableOptionTemplate()
        {
            OptionPossibleValues = new List<IOptionPossibleValue>();
        }



        public string StrongName => MatrixKeys.LIST_MATRIX_TEMPLATE;

        [JsonProperty]
        public List<IOptionPossibleValue> OptionPossibleValues { get; set; }

    }
}
