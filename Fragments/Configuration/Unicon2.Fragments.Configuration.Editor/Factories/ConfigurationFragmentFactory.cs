using System.Linq;
using Unicon2.Fragments.Configuration.Editor.Helpers;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Tree;
using Unicon2.Fragments.Configuration.Editor.ViewModels;
using Unicon2.Fragments.Configuration.Editor.Visitors;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings;

namespace Unicon2.Fragments.Configuration.Editor.Factories
{
	public class ConfigurationFragmentFactory
	{
		public static IDeviceConfiguration CreateConfiguration(
			IConfigurationEditorViewModel configurationEditorViewModel)
		{
			var deviceConfiguration = StaticContainer.Container.Resolve<IDeviceConfiguration>();

			deviceConfiguration.RootConfigurationItemList = configurationEditorViewModel.RootConfigurationItemViewModels
				.Select(model =>
					(model as IEditorConfigurationItemViewModel).Accept(
						new SaveEditorConfigurationItemViewModelVisitor(StaticContainer.Container))).ToList();
			deviceConfiguration.FragmentSettings =
				configurationEditorViewModel.FragmentSettingsViewModel.Model as IFragmentSettings;
			deviceConfiguration.BaseValues = StaticContainer.Container.Resolve<BaseValuesFillHelper>()
				.CreateConfigurationBaseValues(configurationEditorViewModel.BaseValuesViewModel);
			return deviceConfiguration;
		}
	}
}
