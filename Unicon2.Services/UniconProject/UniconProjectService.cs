using MahApps.Metro.Controls.Dialogs;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Services;
using Unicon2.Infrastructure.Services.ApplicationSettingsService;
using Unicon2.Infrastructure.Services.ItemChangingContext;
using Unicon2.Infrastructure.Services.LogService;
using Unicon2.Infrastructure.Services.UniconProject;
using Unicon2.Services.Keys;
using Unicon2.Unity.Interfaces;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace Unicon2.Services.UniconProject
{
    public class UniconProjectService : IUniconProjectService
    {
        private readonly IUniconProject _uniconProject;
        private readonly ILocalizerService _localizerService;
        private readonly Func<IDialogCoordinator> _dialogCoordinator;
        private readonly IApplicationSettingsService _applicationSettingsService;
        private readonly IDevicesContainerService _devicesContainerService;
        private readonly ITypesContainer _container;
        private readonly ILogService _logService;
        private readonly ISerializerService _serializerService;
        private object _dialogContext;


        
        public UniconProjectService(IUniconProject uniconProject, ILocalizerService localizerService,
            Func<IDialogCoordinator> dialogCoordinator, IApplicationSettingsService applicationSettingsService,
            IDevicesContainerService devicesContainerService, ITypesContainer container, ILogService logService,
            ISerializerService serializerService
        )
        {
            _uniconProject = uniconProject;
            _localizerService = localizerService;
            _dialogCoordinator = dialogCoordinator;
            _applicationSettingsService = applicationSettingsService;
            _devicesContainerService = devicesContainerService;
            _container = container;
            _logService = logService;
            _serializerService = serializerService;
        }


        public void CreateNewProject()
        {
            if (CheckIfProjectSaved() != ProjectSaveCheckingResultEnum.CancelledByUser)
            {
                _devicesContainerService.Refresh();
                _uniconProject.Dispose();
                _devicesContainerService.ConnectableItemChanged?.Invoke(
                    new ConnectableItemChangingContext(null, ItemModifyingTypeEnum.Refresh));
            }
        }

        public void SaveProject()
        {
            SaveProjectInFile(true);
        }

        public void SaveProjectAs()
        {
            SaveProjectInFile(false);
        }

        /// <summary>
        /// Проверить сохранен ли проект
        /// </summary>
        /// <returns>Было ли взаимодействие с пользователем по сохранению проекта</returns>
        public ProjectSaveCheckingResultEnum CheckIfProjectSaved()
        {
            if (GetIsProjectChanged(_uniconProject))
            {
                MessageDialogResult result = _dialogCoordinator().ShowModalMessageExternal(_dialogContext,
                    "Сохранение",
                    "Проект не был сохранен. Сохранить проект?",
                    MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary,
                    new MetroDialogSettings
                    {
                        FirstAuxiliaryButtonText =
                            _localizerService.GetLocalizedString(ApplicationGlobalNames.DialogStrings.CANCEL),
                        AffirmativeButtonText =
                            _localizerService.GetLocalizedString(ApplicationGlobalNames.DialogStrings.YES),
                        NegativeButtonText =
                            _localizerService.GetLocalizedString(ApplicationGlobalNames.DialogStrings.NO)
                    });
                if (result == MessageDialogResult.FirstAuxiliary)
                {
                    return ProjectSaveCheckingResultEnum.CancelledByUser;
                }

                if (result == MessageDialogResult.Affirmative)
                {
                    SaveProjectInFile(false);
                    return ProjectSaveCheckingResultEnum.ProjectSavedByUserInDialog;
                }

                if (result == MessageDialogResult.Negative)
                {
                    return ProjectSaveCheckingResultEnum.UserRejectedSaving;
                }
            }

            return ProjectSaveCheckingResultEnum.ProjectAlreadySaved;
        }
        private bool GetIsProjectChanged(IUniconProject uniconProject)
        {
	        if ((uniconProject.ProjectPath != null) && (uniconProject.Name != null))
	        {
		        if (File.Exists(uniconProject.ProjectPath + "\\" + uniconProject.Name + ".uniproj"))
		        {
			        try
			        {
				        string existing = _serializerService.SerializeInString(_uniconProject);
				        string xmlString = File.ReadAllText(uniconProject.ProjectPath + "\\" + uniconProject.Name + ".uniproj");

				        string existing1 = existing.Remove(0, existing.IndexOf("UniconProject"));
				        string xmlString1 = xmlString.Remove(0, xmlString.IndexOf("UniconProject"));
				        //var t = existing1.Length==xmlString1.Length;
				        if (xmlString1 == existing1)
				        {
					        return false;
				        }
			        }
			        catch
			        {
				        return true;
			        }
		        }
	        }

	        return true;
        }
        public string CurrentProjectString => _uniconProject.Name;

     

        public async void OpenProject()
        {
            if (CheckIfProjectSaved() == ProjectSaveCheckingResultEnum.CancelledByUser) return;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = "Unicon Project file (*.uniproj)|*.uniproj";
            ofd.CheckFileExists = true;
            if (ofd.ShowDialog() == true)
            {
               await LoadProject(ofd.FileName);
            }
        }
        public async Task LoadProject(string path)
        {
	        try
	        {
		        var deserialized = _serializerService.DeserializeFromFile<IUniconProject>(path);
		        _uniconProject.ConnectableItems = deserialized.ConnectableItems;
		        _uniconProject.LayoutString = deserialized.LayoutString;
		        _uniconProject.ProjectPath = Path.GetDirectoryName(path);
		        _uniconProject.Name = Path.GetFileNameWithoutExtension(path);
		        _devicesContainerService.Refresh();
		        foreach (IConnectable connectableItem in _uniconProject.ConnectableItems)
		        {
			        if (connectableItem.DeviceConnection != null)
			        {
				        var res = await _devicesContainerService.ConnectDeviceAsync(connectableItem as IDevice,
					        connectableItem.DeviceConnection);
				        if (!res.IsSuccess)
				        {
					        _devicesContainerService.ConnectDeviceAsync(connectableItem as IDevice,
						        _container.Resolve<IDeviceConnectionFactory>(ApplicationGlobalNames
							        .OFFLINE_CONNECTION_FACTORY_NAME).CreateDeviceConnection());
				        }

				        if (!_devicesContainerService.ConnectableItems.Contains(connectableItem as IDevice))
				        {
					        _devicesContainerService.AddConnectableItem(connectableItem as IDevice);
				        }
			        }
		        }

		        string message = string.Empty;
		        message += _localizerService.GetLocalizedString(ServicesKeys.PROJECT_OPENED);
		        message += " " + _uniconProject.ProjectPath + "\\" + _uniconProject.Name + ".uniproj";

		        _logService.LogMessage(message);
		        _applicationSettingsService.AddRecentProject(path);
	        }
	        catch (Exception ex)
	        {
		        _logService.LogMessage(ex.Message);
	        }

        }

        public void SetDialogContext(object obj)
        {
            _dialogContext = obj;
        }

        public string GetProjectTitle()
        {
            return $"{_localizerService.GetLocalizedString("Project")}: {_uniconProject.Name} ({_uniconProject.ProjectPath})";
        }

        public async void LoadDefaultProject()
        {
	        var projectPath=_applicationSettingsService.GetRecentProjectStringCollection().FirstOrDefault();
	        if (projectPath != null)
	        {
                await LoadProject(projectPath);
	        }
        }


        private void SaveProjectInFile(bool isDefaultSaving)
        {
            _uniconProject.ConnectableItems = _devicesContainerService.ConnectableItems;
            string projectPath = Path.Combine(_uniconProject.ProjectPath, _uniconProject.Name + ".uniproj");
            if (isDefaultSaving && _uniconProject.IsProjectSaved)
            {
                _serializerService.SerializeInFile(_uniconProject, projectPath);
            }
            else
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Unicon Project file (*.uniproj)|*.uniproj";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    _uniconProject.ProjectPath = projectPath = Path.GetDirectoryName(sfd.FileName);
                    _uniconProject.Name = Path.GetFileNameWithoutExtension(sfd.FileName);
                    _serializerService.SerializeInFile(_uniconProject,
                        _uniconProject.ProjectPath + "\\" + _uniconProject.Name + ".uniproj");
                }
                else return;
            }

            string message = string.Empty;
            message += _localizerService.GetLocalizedString(ServicesKeys.PROJECT_SAVED);
            message += " " + projectPath;
            _logService.LogMessage(message);
            _applicationSettingsService.AddRecentProject(Path.Combine(_uniconProject.ProjectPath, _uniconProject.Name + ".uniproj"));
        }
    }
}