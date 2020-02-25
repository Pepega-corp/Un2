using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings.QuickMemoryAccess
{
    public interface IQuickAccessMemoryApplyingContext : ISettingApplyingContext
    {
        Func<IRange, Task> OnFillAddressRange { get; set; }
    }
}