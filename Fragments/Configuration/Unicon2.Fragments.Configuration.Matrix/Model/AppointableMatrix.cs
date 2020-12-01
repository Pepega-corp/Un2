using Newtonsoft.Json;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Fragments.Configuration.Model.Properties;
using Unicon2.Infrastructure.Values.Matrix;

namespace Unicon2.Fragments.Configuration.Matrix.Model
{
    [JsonObject(MemberSerialization.OptIn)]

    public class AppointableMatrix : DefaultProperty, IAppointableMatrix
    {
        public AppointableMatrix(IMatrixTemplate matrixTemplate) 
        {
            UshortsFormatter = new MatrixValueFormatter();
            MatrixTemplate = matrixTemplate;
        }

        [JsonProperty] public IMatrixTemplate MatrixTemplate { get; set; }

        public override T Accept<T>(IConfigurationItemVisitor<T> visitor)
        {
            return visitor.VisitMatrix(this);
        }
    }

}