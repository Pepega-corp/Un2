using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Matrix;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Fragments.Configuration.Matrix.Helpers;
using Unicon2.Fragments.Configuration.Model.Properties;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Configuration.Matrix.Model
{
    [DataContract(Namespace = "AppointableMatrixNS", Name = nameof(AppointableMatrix), IsReference = true)]

    public class AppointableMatrix : DefaultProperty, IAppointableMatrix
    {
        public AppointableMatrix(IMatrixTemplate matrixTemplate) 
        {
            UshortsFormatter = new MatrixValueFormatter(this);
            this.MatrixTemplate = matrixTemplate;
        }

        [DataMember] public IMatrixTemplate MatrixTemplate { get; set; }

        public override T Accept<T>(IConfigurationItemVisitor<T> visitor)
        {
            return visitor.VisitMatrix(this);
        }
    }

}