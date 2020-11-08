using System.Collections.Generic;
using System.Linq;

namespace Unicon2.Infrastructure.Interfaces
{
	public interface ICloneable<out T>
	{
		T Clone();
	}

	public static class CloneableExtensions
	{
		public static IEnumerable<T> CloneCollection<T>(this IEnumerable<ICloneable<T>> cloneables)
		{
			return cloneables.Select(cloneable => cloneable.Clone());
		}
	}

}
