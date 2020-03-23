using System;

namespace Unicon2.Infrastructure.Interfaces
{
	public interface IUniqueId
	{
		Guid Id { get; }
	}

	public interface IUniqueIdWithSet : IUniqueId
	{
		void SetId(Guid id);
	}
}