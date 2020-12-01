using System;
using System.Collections.Generic;
using System.Text;

namespace Unicon.Backend.Common.Modularity
{
	public interface IModule
	{
		void InitializeTypes(ITypesContainer typesContainer);
	}
}
