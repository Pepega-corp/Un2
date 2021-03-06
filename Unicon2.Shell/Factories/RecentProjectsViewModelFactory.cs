﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unicon2.Infrastructure.Services.ApplicationSettingsService;
using Unicon2.Shell.ViewModels;

namespace Unicon2.Shell.Factories
{
	public class RecentProjectsViewModelFactory
	{
		private readonly IApplicationSettingsService _applicationSettingsService;

		public RecentProjectsViewModelFactory(IApplicationSettingsService applicationSettingsService)
		{
			_applicationSettingsService = applicationSettingsService;
		}

		public List<RecentProjectViewModel> CreateProjectViewModels()
		{
			var result = _applicationSettingsService.GetRecentProjectStringCollection();


			return result.Select(s =>
					new RecentProjectViewModel($"{Path.GetFileNameWithoutExtension(s)} ({Path.GetDirectoryName(s)})",s))
				.ToList();
		}
	}
}
