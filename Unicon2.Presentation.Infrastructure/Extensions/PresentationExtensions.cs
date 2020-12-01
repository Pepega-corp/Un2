using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Unicon2.Presentation.Infrastructure.Extensions
{
    public static class PresentationExtensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumerable)
        {
            return new ObservableCollection<T>(enumerable);
        }
    }
}