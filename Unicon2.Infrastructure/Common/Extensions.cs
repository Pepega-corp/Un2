using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Interfaces;

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
