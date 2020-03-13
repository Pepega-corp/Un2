using System.Runtime.Serialization;
using Newtonsoft.Json;
using Unicon2.Fragments.Configuration.Matrix.Keys;
using Unicon2.Infrastructure.Values.Matrix.OptionTemplates;

namespace Unicon2.Fragments.Configuration.Matrix.Model.OptionTemplates
{
    [JsonObject(MemberSerialization.OptIn)]

    public class BoolMatrixVariableOptionTemplate : IMatrixVariableOptionTemplate
    {
        public string StrongName => MatrixKeys.BOOL_MATRIX_TEMPLATE;
    }
}
