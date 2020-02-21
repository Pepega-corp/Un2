using System.Collections.Generic;

namespace Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties
{
    public interface IComplexProperty : IProperty
    {
        List<ISubProperty> SubProperties { get; set; }
        bool IsGroupedProperty { get; set; }
    }
}