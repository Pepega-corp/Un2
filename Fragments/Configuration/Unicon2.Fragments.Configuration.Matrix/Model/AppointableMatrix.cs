using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Matrix.Helpers;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.Model;
using Unicon2.Fragments.Configuration.Model.Base;
using Unicon2.Fragments.Configuration.Model.Properties;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Configuration.Matrix.Model
{
    [DataContract(Namespace = "AppointableMatrixNS", Name = nameof(AppointableMatrix), IsReference = true)]

    public class AppointableMatrix : DefaultProperty, IAppointableMatrix
    {
        public AppointableMatrix(Func<IRange> rangeGetFunc, IMatrixTemplate matrixTemplate) : base(rangeGetFunc)
        {
            UshortsFormatter = new MatrixValueFormatter(this);
            this.MatrixTemplate = matrixTemplate;
        }

        public override Task<bool> Write()
        {
          return MatrixUshortLoadingHelper.WriteMatrixUshorts(this, _dataProvider,LocalUshortsValue);
        }

        public override string StrongName => ConfigurationKeys.APPOINTABLE_MATRIX;

        public override async Task Load()
        {
            await MatrixUshortLoadingHelper.FillMatrixUshorts(this, _dataProvider);
            this.ConfigurationItemChangedAction?.Invoke();
        }


        [DataMember]
        public IMatrixTemplate MatrixTemplate { get; set; }

        public override void InitializeFromContainer(ITypesContainer container)
        {
            UshortsFormatter=new MatrixValueFormatter(this);
            base.InitializeFromContainer(container);
        }
    }
}
