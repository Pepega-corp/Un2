﻿using System.Collections.Generic;

namespace Unicon2.Infrastructure.Services.ApplicationSettingsService
{
    public interface IApplicationSettingsService
    {
        List<string> GetRecentProjectStringCollection();
        void AddRecentProject(string projectPath);
    }
}