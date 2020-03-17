using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings.QuickMemoryAccess;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Model.FragmentSettings
{
	public class QuickAccessMemoryApplyingContext : IQuickAccessMemoryApplyingContext
	{
		public Func<IRange, Task> OnFillAddressRange { get; set; }
	}
}