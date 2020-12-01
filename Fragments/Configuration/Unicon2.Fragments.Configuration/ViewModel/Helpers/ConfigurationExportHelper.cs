using System;
using System.IO;
using Microsoft.Win32;
using Unicon2.Fragments.Configuration.Infrastructure.Export;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;
using Unicon2.Fragments.Configuration.Views;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Interfaces.DataOperations;
using Unicon2.Infrastructure.Services;
using Unicon2.Infrastructure.Services.LogService;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Configuration.ViewModel.Helpers
{
    public class ConfigurationExportHelper
    {
	    public static void ExportConfiguration(IRuntimeConfigurationViewModel deviceConfiguration,
		    ITypesContainer typesContainer, string deviceName, string nameForUI)
	    {
		    var viewModel = typesContainer.Resolve<ExportSelectionViewModel>();
		    var logger = typesContainer.Resolve<ILogService>();
		    var localizer = typesContainer.Resolve<ILocalizerService>();
		    var appCommands = typesContainer.Resolve<IApplicationGlobalCommands>();
		    var nameForUiLocalized = nameForUI;
		    localizer.TryGetLocalizedString(nameForUI, out nameForUiLocalized);
		    ExportSelectionWindow window = new ExportSelectionWindow();
		    viewModel.Initialize((async (exportSelector) =>
		    {
			    var sfd = new SaveFileDialog
			    {
				    Filter = " HTML файл (*html)|*html" + "|Все файлы (*.*)|*.* ",
				    DefaultExt = ".html",
				    FileName = $"{nameForUiLocalized} {deviceName}"
			    };
			    if (sfd.ShowDialog() == true)
			    {
				    try
				    {
					    viewModel.IsSavingInProcess = true;
					    File.WriteAllText(sfd.FileName,
						    await typesContainer
							    .Resolve<IHtmlRenderer<IRuntimeConfigurationViewModel, ConfigurationExportSelector>>()
							    .RenderHtmlString(deviceConfiguration, exportSelector));
					    logger.LogMessage(ApplicationGlobalNames.StatusMessages.FILE_EXPORT_SUCCESSFUL);
					    viewModel.IsSavingInProcess = false;
					    window.Close();
				    }
				    catch (Exception e)
				    {
					    logger.LogMessage(e.Message + Environment.NewLine + e.StackTrace, LogMessageTypeEnum.Error);
				    }

			    }
		    }), deviceConfiguration);
		    appCommands.ShowWindowModal(() => window, viewModel);
	    }
    }
}