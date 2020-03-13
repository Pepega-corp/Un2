using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly IDialogCoordinator _dialogCoordinator;
        private readonly IApplicationSettingsService _applicationSettingsService;
        private readonly IDevicesContainerService _devicesContainerService;
        private readonly ITypesContainer _container;
        private readonly ILogService _logService;
        private readonly ISerializerService _serializerService;
        private object _dialogContext;


        public UniconProjectService(IUniconProject uniconProject, ILocalizerService localizerService,
            IDialogCoordinator dialogCoordinator, IApplicationSettingsService applicationSettingsService,
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
            if (CheckIfProjectSaved(this) != ProjectSaveCheckingResultEnum.CancelledByUser)
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
        public ProjectSaveCheckingResultEnum CheckIfProjectSaved(object dialogContext = null)
        {
            if (dialogContext != null)
                _dialogContext = dialogContext;
            if (_uniconProject.GetIsProjectChanged())
            {
                MessageDialogResult result = _dialogCoordinator.ShowModalMessageExternal(_dialogContext,
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

        public string CurrentProjectString => _uniconProject.Name;

        public List<string> GetLastProjectsList()
        {
            List<string> t = _applicationSettingsService.GetLastProjectStringCollection();
            return t;
        }

        public async void OpenProject(string lastProjectString = "", object dialogContext = null)
        {
            if (dialogContext != null)
                _dialogContext = dialogContext;
            if (CheckIfProjectSaved(_dialogContext) == ProjectSaveCheckingResultEnum.CancelledByUser) return;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = "Unicon Project file (*.uniproj)|*.uniproj";
            ofd.CheckFileExists = true;
            if (ofd.ShowDialog() == true)
            {
                try
                {
                    var deserialized = _serializerService.DeserializeFromFile<IUniconProject>(ofd.FileName);
                    _uniconProject.ConnectableItems = deserialized.ConnectableItems;
                    _uniconProject.LayoutString = deserialized.LayoutString;
                    _uniconProject.ProjectPath = Path.GetDirectoryName(ofd.FileName);
                    _uniconProject.Name = Path.GetFileNameWithoutExtension(ofd.FileName);
                    _devicesContainerService.Refresh();
                    foreach (IConnectable connectableItem in _uniconProject.ConnectableItems)
                    {
                        if (connectableItem.DeviceConnection != null)
                        {
                            await _devicesContainerService.ConnectDeviceAsync(connectableItem as IDevice,
                                connectableItem.DeviceConnection);
                        }
                    }

                    string message = string.Empty;
                    message += _localizerService.GetLocalizedString(ServicesKeys.PROJECT_OPENED);
                    message += " " + _uniconProject.ProjectPath + "\\" + _uniconProject.Name + ".uniproj";
                    _logService.LogMessage(message);
                }
                catch (Exception ex)
                {
                    _logService.LogMessage(ex.Message);
                }
            }
        }

        public void SetDialogContext(object obj)
        {
            _dialogContext = obj;
        }

        public string GetProjectPath()
        {
            return _uniconProject.ProjectPath;
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
        }
    }
}