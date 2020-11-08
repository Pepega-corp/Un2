using System;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;

namespace Unicon2.Fragments.Configuration.Infrastructure.Factories
{
	public interface
		IRuntimeConfigurationItemViewModelFactory : IConfigurationItemVisitor<FactoryResult<IRuntimeConfigurationItemViewModel>>
	{
	}

	public class FactoryResult<T>
	{
		private FactoryResult(T item, bool needAdding)
		{
			Item = item;
			NeedAdding = needAdding;
		}

		public T Item { get;  }
		public bool NeedAdding { get; }

		public static FactoryResult<T> Create(T item)
		{
			return new FactoryResult<T>(item,true);
		}
		public static FactoryResult<T> Create(T item,bool needAdding)
		{
			return new FactoryResult<T>(item, needAdding);
		}

		public FactoryResult<T> OnAddingNeeded(Action<T> actionWhenAddingNeeded)
		{
			if (NeedAdding)
			{
				actionWhenAddingNeeded(Item);
			}
			return this;
		}
	}


}