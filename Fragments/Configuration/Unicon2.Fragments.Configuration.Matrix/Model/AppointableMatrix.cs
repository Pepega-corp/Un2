using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Fragments.Configuration.Matrix.Helpers;
using Unicon2.Fragments.Configuration.Model.Properties;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Values.Matrix;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Configuration.Matrix.Model
{
    [JsonObject(MemberSerialization.OptIn)]

    public class AppointableMatrix : DefaultProperty, IAppointableMatrix
    {
        public AppointableMatrix(IMatrixTemplate matrixTemplate) 
        {
            UshortsFormatter = new MatrixValueFormatter();
            this.MatrixTemplate = matrixTemplate;
        }

        [JsonProperty] public IMatrixTemplate MatrixTemplate { get; set; }

        public override T Accept<T>(IConfigurationItemVisitor<T> visitor)
        {
            return visitor.VisitMatrix(this);
        }
    }

}