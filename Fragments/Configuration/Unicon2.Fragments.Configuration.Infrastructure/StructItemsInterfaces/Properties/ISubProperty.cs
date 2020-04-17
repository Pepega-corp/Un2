using System;
using System.Collections.Generic;

namespace Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties
{
    public interface ISubProperty : IProperty
    {
        List<int> BitNumbersInWord { get; set; }
    }
}