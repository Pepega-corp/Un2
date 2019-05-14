using System;
using System.Collections.Generic;

namespace Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties
{
    public interface ISubProperty : IProperty
    {
        void SetParent(IComplexProperty complexProperty);
        List<int> BitNumbersInWord { get; set; }
        Action LocalValueChanged { get; set; }
    }
}