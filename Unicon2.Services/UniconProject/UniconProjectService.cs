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
        private object _dialogContext;


        public UniconProjectService(IUniconProject uniconProject, ILocalizerService localizerService,
            IDialogCoordinator dialogCoordinator, IApplicationSettingsService applicationSettingsService,
            IDevicesContainerService devicesContainerService, ITypesContainer container, ILogService logService)
        {
            this._uniconProject = uniconProject;
            this._localizerService = localizerService;
            this._dialogCoordinator = dialogCoordinator;
            this._applicationSettingsService = applicationSettingsService;
            this._devicesContainerService = devicesContainerService;
            this._container = container;
            this._logService = logService;
        }



        #region Implementation of IUniconProjectService


        public void CreateNewProject()
        {
            if (this.CheckIfProjectSaved() != ProjectSaveCheckingResultEnum.CancelledByUser)
            {
                this._uniconProject.Dispose();
                this._devicesContainerService.ConnectableItemChanged?.Invoke(new ConnectableItemChangingContext(null, ItemModifyingTypeEnum.Refresh));
            }
        }

        public void SaveProject()
        {
            this.SaveProjectInFile(true);
        }

        public void SaveProjectAs()
        {
            this.SaveProjectInFile(false);
        }

        /// <summary>
        /// Проверить сохранен ли проект
        /// </summary>
        /// <returns>Было ли взаимодействие с пользователем по сохранению проекта</returns>
        public ProjectSaveCheckingResultEnum CheckIfProjectSaved()
        {
            if (this._uniconProject.GetIsProjectChanged())
            {
                MessageDialogResult result = this._dialogCoordinator.ShowModalMessageExternal(this._dialogContext,
                    "Сохранение",
                    "Проект не был сохранен. Сохранить проект?",
                    MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary,
                    new MetroDialogSettings
                    {
                        FirstAuxiliaryButtonText =
                            this._localizerService.GetLocalizedString(ApplicationGlobalNames.DialogStrings.CANCEL),
                        AffirmativeButtonText =
                            this._localizerService.GetLocalizedString(ApplicationGlobalNames.DialogStrings.YES),
                        NegativeButtonText =
                            this._localizerService.GetLocalizedString(ApplicationGlobalNames.DialogStrings.NO)
                    });
                if (result == MessageDialogResult.FirstAuxiliary)
                {
                    return ProjectSaveCheckingResultEnum.CancelledByUser;
                }
                if (result == MessageDialogResult.Affirmative)
                {
                    this.SaveProjectInFile(true);
                    return ProjectSaveCheckingResultEnum.ProjectSavedByUserInDialog;
                }
                if (result == MessageDialogResult.Negative)
                {
                    return ProjectSaveCheckingResultEnum.UserRejectedSaving;
                }
            }
            return ProjectSaveCheckingResultEnum.ProjectAlreadySaved;
        }

        public string CurrentProjectString => this._uniconProject.Name;

        public List<string> GetLastProjectsList()
        {
            List<string> t = this._applicationSettingsService.GetLastProjectStringCollection();
            return t;
        }

        public async void OpenProject(string lastProjectString = "")
        {
            if (this.CheckIfProjectSaved() == ProjectSaveCheckingResultEnum.CancelledByUser) return;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = "Unicon Project file (*.uniproj)|*.uniproj";
            ofd.CheckFileExists = true;
            if (ofd.ShowDialog() == true)
            {
                this._uniconProject.DeserializeFromFile(ofd.FileName);
                this._uniconProject.ProjectPath = Path.GetDirectoryName(ofd.FileName);
                this._uniconProject.Name = Path.GetFileName(this._uniconProject.ProjectPath);
                this._devicesContainerService.Refresh();
                foreach (IConnectable connectableItem in this._uniconProject.ConnectableItems)
                {
                    if (connectableItem is IInitializableFromContainer)
                    {
                        (connectableItem as IInitializableFromContainer).InitializeFromContainer(this._container);
                    }
                    if (connectableItem.DeviceConnection != null)
                    {
                        await this._devicesContainerService.ConnectDeviceAsync(connectableItem as IDevice,
                               connectableItem.DeviceConnection);
                    }
                }
                string message = string.Empty;
                message += this._localizerService.GetLocalizedString(ServicesKeys.PROJECT_OPENED);
                message += " " + this._uniconProject.ProjectPath;
                this._logService.RaiseInfoMessage(message);
            }
        }

        public void SetDialogContext(object obj)
        {
            this._dialogContext = obj;
        }

        public string GetProjectPath()
        {
            return this._uniconProject.ProjectPath;
        }


        private void SaveProjectInFile(bool isDefaultSaving)
        {
            this._uniconProject.ConnectableItems = this._devicesContainerService.ConnectableItems;
            string projectPath = Path.Combine(this._uniconProject.ProjectPath, this._uniconProject.Name + ".uniproj");
            if (isDefaultSaving && this._uniconProject.IsProjectSaved)
            {
                this._uniconProject.SerializeInFile(projectPath, false);
            }
            else
            {
                FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
                folderBrowserDialog.Description = "Выберите папку для сохранения";
                if (!string.IsNullOrEmpty(this._uniconProject.ProjectPath))
                {
                    folderBrowserDialog.SelectedPath = this._uniconProject.ProjectPath;
                }

                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    string folderPath = folderBrowserDialog.SelectedPath;
                    this._uniconProject.Name = Path.GetFileName(folderPath);
                    this._uniconProject.SerializeInFile(projectPath, false);
                    this._uniconProject.ProjectPath = folderPath;
                }
            }
            string message = string.Empty;
            message += this._localizerService.GetLocalizedString(ServicesKeys.PROJECT_SAVED);
            message += " " + projectPath;
            this._logService.RaiseInfoMessage(message);
        }


        #endregion
    }
}