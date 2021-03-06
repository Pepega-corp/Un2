﻿using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.ViewModels;

namespace Unicon2.Presentation.Infrastructure.Services
{
    public interface ISaveFormatterService
    {
        IUshortsFormatter CreateUshortsParametersFormatter(IFormatterParametersViewModel formatterParametersViewModel);
        IUshortsFormatter CreateUshortsParametersFormatter(IUshortsFormatterViewModel ushortsFormatterViewModel);

    }
}