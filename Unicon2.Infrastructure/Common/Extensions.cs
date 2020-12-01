using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unicon2.Infrastructure.Extensions;

namespace Unicon2.Infrastructure.Common
{
    public static class ExtensionsMVVM
    {
        public static Collection<T> AddCollection<T>(this ObservableCollection<T> collection, IEnumerable<T> range)
        {
            range.ForEach(collection.Add);
            return collection;
        }

    
    }
}
