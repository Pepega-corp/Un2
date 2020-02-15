﻿using System;
using System.Collections.Generic;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings.QuickMemoryAccess;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Model.FragmentSettings
{
    public class QuickAccessMemoryApplyingContext : IQuickAccessMemoryApplyingContext
    {
        public QuickAccessModeEnum QuickAccessMode { get; set; }
        public Action<IRange> OnFillAddressRange { get; set; }
    }
}