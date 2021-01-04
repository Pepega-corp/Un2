using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Unicon2.Infrastructure.Extensions;

namespace Unicon2.Infrastructure.Common
{
    public static class CollectionExtensions
    {
        public static Collection<T> AddCollection<T>(this ObservableCollection<T> collection, IEnumerable<T> range)
        {
            range.ForEach(collection.Add);
            return collection;
        }

        public static Dictionary<TKey, TVal> AddElement<TKey, TVal>(this Dictionary<TKey, TVal> dictionary, TKey key,
            TVal value, bool replaceIfExist = true)
        {
            if (dictionary.ContainsKey(key))
            {
                if (replaceIfExist)
                {
                    dictionary[key] = value;
                }
            }
            else
            {
                dictionary.Add(key, value);
            }

            return dictionary;
        }
        
        

    }
}
