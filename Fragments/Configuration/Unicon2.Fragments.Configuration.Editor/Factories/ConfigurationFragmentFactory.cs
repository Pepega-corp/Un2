using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Tree;
using Unicon2.Fragments.Configuration.Editor.ViewModels;
using Unicon2.Fragments.Configuration.Editor.Visitors;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings;
using Unicon2.Presentation.Infrastructure.Factories;

namespace Unicon2.Fragments.Configuration.Editor.Factories
{
	public class ConfigurationFragmentFactory
	{
		public static IDeviceConfiguration CreateConfiguration(IConfigurationEditorViewModel configurationEditorViewModel)
		{
			var deviceConfiguration = StaticContainer.Container.Resolve<IDeviceConfiguration>();

			deviceConfiguration.RootConfigurationItemList = configurationEditorViewModel.RootConfigurationItemViewModels
				.Select(model =>
					(model as IEditorConfigurationItemViewModel).Accept(
						new SaveEditorConfigurationItemViewModelVisitor(StaticContainer.Container))).ToList();
            deviceConfiguration.FragmentSettings=configurationEditorViewModel.FragmentSettingsViewModel.Model as IFragmentSettings;
			return deviceConfiguration;
		}
	}
}
