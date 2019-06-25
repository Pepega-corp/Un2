﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.Model;
using Unicon2.Fragments.Configuration.Matrix.Keys;
using Unicon2.Infrastructure.Values.Base;

namespace Unicon2.Fragments.Configuration.Matrix.Model
{
    public class MatrixValue: FormattedValueBase,IMatrixValue
    {
        #region Overrides of FormattedValueBase

        public override string StrongName => MatrixKeys.MATRIX_VALUE;
        public override string AsString()
        {
            return "Matrix";
        }

        #endregion

        #region Implementation of IMatrixValue

        public ushort[] ValueUshorts { get; set; }
        public IMatrixTemplate MatrixTemplate { get; set; }

        #endregion
    }
}