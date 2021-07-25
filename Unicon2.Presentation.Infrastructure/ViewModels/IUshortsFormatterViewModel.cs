﻿using System;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Presentation.Infrastructure.ViewModels
{
    public interface IUshortsFormatterViewModel : ICloneable, IStronglyNamed
    {
        
    }

    public interface IFormatterInfoService
    {
        bool ReturnsString(IUshortsFormatterViewModel formatterViewModel);
    }
}