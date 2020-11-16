using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicon2.Infrastructure.Functional
{
  public  static class ResultExtensions
    {
        public static IEnumerable<T> Choose<T>(this IEnumerable<Result<T>> results)
        {
            return results.Where(result => result.IsSuccess).Select(result => result.Item);
        }
    }
}
