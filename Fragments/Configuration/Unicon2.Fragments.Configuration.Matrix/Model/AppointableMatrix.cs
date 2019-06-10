using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.Model;
using Unicon2.Fragments.Configuration.Model.Base;
using Unicon2.Fragments.Configuration.Model.Properties;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.Configuration.Matrix.Model
{
    [DataContract(Namespace = "AppointableMatrixNS", Name = nameof(AppointableMatrix), IsReference = true)]

    public class AppointableMatrix : DefaultProperty, IAppointableMatrix
    {
        public AppointableMatrix(Func<IRange> rangeGetFunc, IMatrixTemplate matrixTemplate) : base(rangeGetFunc)
        {
            this.MatrixTemplate = matrixTemplate;
        }

        #region Overrides of ConfigurationItemBase

        public override string StrongName => ConfigurationKeys.APPOINTABLE_MATRIX;
        protected override void FillAddressRanges(List<IRange> ranges)
        {
            throw new NotImplementedException();
        }

        protected override IConfigurationItem OnCloning()
        {
            throw new NotImplementedException();
        }

        #endregion


        #region Implementation of IAppointableMatrix

        [DataMember]
        public IMatrixTemplate MatrixTemplate { get; set; }

        #endregion
    }
}
