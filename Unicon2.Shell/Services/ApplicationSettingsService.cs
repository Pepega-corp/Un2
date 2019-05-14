using System.Collections.Generic;
using System.Linq;
using Unicon2.Infrastructure.Services.ApplicationSettingsService;
using Unicon2.Shell.Properties;

namespace Unicon2.Shell.Services
{
    public class ApplicationSettingsService : IApplicationSettingsService
    {
        #region Implementation of IApplicationSettingsService

        public List<string> GetLastProjectStringCollection()
        {
            return Settings.Default.LastProjectsStringCollection.Cast<string>().ToList();
        }

        #endregion
    }
}
