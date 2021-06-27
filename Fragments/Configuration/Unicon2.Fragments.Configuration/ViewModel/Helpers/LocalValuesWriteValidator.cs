using System.Collections.Generic;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Services;
using Unicon2.Infrastructure.Services.LogService;
using Unicon2.Presentation.Infrastructure.Extensions;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Presentation.Infrastructure.ViewModels;

namespace Unicon2.Fragments.Configuration.ViewModel.Helpers
{
	public static class LocalValuesWriteValidator
	{
		public static bool ValidateLocalValuesToWrite(IEnumerable<IConfigurationItemViewModel> configurationViewModels)
		{
			bool res = true;
			foreach (var configurationViewModel in configurationViewModels)
			{
				res = ValidateConfigItem(configurationViewModel, res);
			}

			return res;
		}

		public static bool ValidateConfigItem(IConfigurationItemViewModel configurationViewModel, bool initial)
		{
			bool res = initial;
            if (configurationViewModel is ICanBeHidden canBeHidden && !canBeHidden.IsHidden)
            {
                if (configurationViewModel is ILocalAndDeviceValueContainingViewModel
                        localAndDeviceValueContainingViewModel &&
                    localAndDeviceValueContainingViewModel.LocalValue != null &&
                    localAndDeviceValueContainingViewModel.LocalValue.HasErrors)
                {
                    var errorMes =
                        $"{StaticContainer.Container.Resolve<ILocalizerService>().GetLocalizedString(ApplicationGlobalNames.StatusMessages.ERROR)}: {configurationViewModel.BuildItemPath()}";
                    StaticContainer.Container.Resolve<ILogService>().LogMessage(errorMes, LogMessageTypeEnum.Error);
                    res = false;
                }
            }

            if (configurationViewModel.ChildStructItemViewModels.Count > 0)
			{
				foreach (var configurationItemViewModel in configurationViewModel.ChildStructItemViewModels)
				{
					res= ValidateConfigItem(configurationItemViewModel, res);
				}
			}
			return res;
		}

	}
}
