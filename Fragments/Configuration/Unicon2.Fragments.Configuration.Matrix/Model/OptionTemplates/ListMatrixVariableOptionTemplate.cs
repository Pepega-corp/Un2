﻿using System.Collections.Generic;
using System.Runtime.Serialization;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Matrix.OptionTemplates;
using Unicon2.Fragments.Configuration.Matrix.Keys;

namespace Unicon2.Fragments.Configuration.Matrix.Model.OptionTemplates
{
    [DataContract(Namespace = "AppointableMatrixNS", Name = nameof(ListMatrixVariableOptionTemplate), IsReference = true)]

    public class ListMatrixVariableOptionTemplate : IMatrixVariableOptionTemplate
    {
        public ListMatrixVariableOptionTemplate()
        {
            this.OptionPossibleValues = new List<IOptionPossibleValue>();
        }



        public string StrongName => MatrixKeys.LIST_MATRIX_TEMPLATE;

        [DataMember]
        public List<IOptionPossibleValue> OptionPossibleValues { get; set; }

    }
}
