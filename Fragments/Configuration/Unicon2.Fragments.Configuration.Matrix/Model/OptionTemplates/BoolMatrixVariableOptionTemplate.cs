using System.Runtime.Serialization;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.Model.OptionTemplates;
using Unicon2.Fragments.Configuration.Matrix.Keys;

namespace Unicon2.Fragments.Configuration.Matrix.Model.OptionTemplates
{
    [DataContract(Namespace = "AppointableMatrixNS", Name = nameof(BoolMatrixVariableOptionTemplate), IsReference = true)]

    public class BoolMatrixVariableOptionTemplate : IMatrixVariableOptionTemplate
    {
        public string StrongName => MatrixKeys.BOOL_MATRIX_TEMPLATE;
    }
}
