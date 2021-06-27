using System.Collections.Generic;
using System.Linq;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Infrastructure.Extensions
{
    public static class StronglyNamedExtensions
    {
        public static List<T> ReplaceStronglyNamedInCollection<T>(this List<T> collection, T element)
            where T : IStronglyNamed
        {
            var toReplace=collection.FirstOrDefault(named => named.StrongName == element?.StrongName);
            if (toReplace != null)
            {
                collection[collection.IndexOf(toReplace)] = element;
            }
            return collection;
        }
    }
}