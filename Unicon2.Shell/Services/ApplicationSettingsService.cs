using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using Unicon2.Infrastructure.Services.ApplicationSettingsService;
using Unicon2.Shell.Properties;

namespace Unicon2.Shell.Services
{
	public class ApplicationSettingsService : IApplicationSettingsService
	{
		public List<string> GetRecentProjectStringCollection()
		{
			if (Settings.Default.RecentProjects != null)
			{
				var res= Settings.Default.RecentProjects.Cast<string>().ToList();
				return res.Where(File.Exists).ToList();
			}
			return new List<string>();
		}

		public void AddRecentProject(string projectPath)
		{
			var recentProjects = GetRecentProjectStringCollection();
			recentProjects.Insert(0, projectPath);
			recentProjects=recentProjects.Distinct().ToList();
			var recentProjectsToSave = new StringCollection();
			if (recentProjects.Count > 15)
			{
				recentProjectsToSave.AddRange(recentProjects.Take(15).ToArray());
			}
			else
			{
				recentProjectsToSave.AddRange(recentProjects.ToArray());
			}

			Settings.Default.RecentProjects = recentProjectsToSave;
			Settings.Default.Save();

		}

		public string UrlToServer
		{
			get => Settings.Default.UrlToServer;
			set
			{
				Settings.Default.UrlToServer = value;
				Settings.Default.Save();
			}
		}

		public string ClientSecret
		{
			get => Settings.Default.ClientSecret;
			set
			{
				Settings.Default.ClientSecret = value;
				Settings.Default.Save();
			}
		}
	}
}
