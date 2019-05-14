using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Unicon2.Unity.Common
{
    public static class ExtensionsMVVM
    {
        public static Collection<T> AddCollection<T>(this ObservableCollection<T> collection, IEnumerable<T> range)
        {
            return collection.AddRange(range);
        }
    }
}
