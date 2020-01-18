using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Services;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Services
{
    public class ApplicationGlobalCommands : IApplicationGlobalCommands
    {
        private readonly ITypesContainer _container;
        private readonly IDialogCoordinator _dialogCoordinator;
        private readonly ILocalizerService _localizerService;
        private object _bufferObject;
        private ProgressDialogController _progressDialogController;

        public ApplicationGlobalCommands(ITypesContainer container, IDialogCoordinator dialogCoordinator, ILocalizerService localizerService)
        {
            this._container = container;
            this._dialogCoordinator = dialogCoordinator;
            this._localizerService = localizerService;
        }

        public void ShutdownApplication()
        {
            Application.Current.Shutdown();
        }

        public void ExecuteByDispacther(Action action)
        {
            Dispatcher dispatchObject = Application.Current.Dispatcher;
            if (dispatchObject == null || dispatchObject.CheckAccess())
            {
                action();
            }
            else
            {
                dispatchObject.Invoke(action);
            }
        }

        public void ShowWindowModal(Func<Window> getWindow, object dataContext)
        {
            this.GetWindow(getWindow, dataContext);
        }

        public void ShowWindowModal(Func<Window> getWindow, object dataContext, object _owner)
        {
            this.GetWindow(getWindow, dataContext, _owner);
        }

        public void ShowWindowModal(Func<Window> getWindow, object dataContext, bool isTopmost)
        {
            Window windowToShow = this.GetWindow(getWindow, dataContext);
            windowToShow.Topmost = isTopmost;
        }

        private Window GetWindow(Func<Window> getWindow, object dataContext, object _owner = null)
        {
            Window windowToShow = getWindow.Invoke();
            windowToShow.Owner = _owner==null ? Application.Current.MainWindow : (_owner as Window);
            windowToShow.Topmost = true;
            windowToShow.DataContext = dataContext;
            windowToShow.ShowDialog();
            return windowToShow;
        }

        public bool AskUserToDeleteSelectedGlobal(object context)
        {
            if (this._dialogCoordinator.ShowModalMessageExternal(context, this._localizerService.GetLocalizedString(ApplicationGlobalNames.DialogStrings.DELETE),
                   this._localizerService.GetLocalizedString(ApplicationGlobalNames.DialogStrings.DELETE_SELECTED_ITEM_QUESTION),
                    MessageDialogStyle.AffirmativeAndNegative) == MessageDialogResult.Affirmative)
            {
                return true;
            }
            return false;
        }

        public void ShowErrorMessage(string errorKey, object context)
        {
            this._dialogCoordinator.ShowModalMessageExternal(context,
                this._localizerService.GetLocalizedString(ApplicationGlobalNames.StatusMessages.ERROR),
                this._localizerService.GetLocalizedString(errorKey));
        }



        public void SetToBuffer(object bufferObject)
        {
            if (bufferObject is ICloneable)
            {
                if (bufferObject is IInitializableFromContainer)
                {
                    (bufferObject as IInitializableFromContainer).InitializeFromContainer(this._container);
                }
                this._bufferObject = (bufferObject as ICloneable).Clone();

            }
            else
            {
                throw new ArgumentException();
            }
        }

        public object GetFromBuffer()
        {
            ICloneable cloneable = this._bufferObject as ICloneable;
            if (cloneable != null) return cloneable.Clone();
            throw new ArgumentException();
        }

        public async Task CallWaitingProgressWindow(object context, bool isToOpen)
        {
            if (isToOpen)
            {
                this._progressDialogController = await this._dialogCoordinator.ShowProgressAsync(context,
                    this._localizerService.GetLocalizedString(ApplicationGlobalNames.DialogStrings.PROCESSING_VALUES), "",
                    false);
            }
            else
            {
                if (this._progressDialogController != null)
                {
                    await this._progressDialogController.CloseAsync();
                }
            }
        }

        public void OpenOscillogram(string oscillogramPath = null)
        {

            string fileName = "";
            if (oscillogramPath == null)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Multiselect = false;
                ofd.Filter = " ComTrade Header File (*.hdr)|*.hdr" + "|Все файлы (*.*)|*.* ";
                ofd.CheckFileExists = true;
                if (ofd.ShowDialog() == true)
                {
                    fileName = ofd.FileName;
                }
                else
                {
                    return;
                }
            }
            else
            {
                fileName = oscillogramPath;
            }
            System.Diagnostics.Process.Start(
                Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "OscApp", "Oscilloscope.exe"),
                fileName);
        }

        public Maybe<FileInfo> SelectFileToOpen(string windowTitle, string filter)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            Maybe<FileInfo> fileInfoMaybe = new Maybe<FileInfo>();
            ofd.Multiselect = false;
            ofd.Title = windowTitle;
            ofd.Filter = filter;
            ofd.CheckFileExists = true;
            if (ofd.ShowDialog() == true)
            {
                fileInfoMaybe.AddValue(new FileInfo(ofd.FileName));
            }
            return fileInfoMaybe;
        }

        public Maybe<string> SelectFilePathToSave(string windowTitle, string defaultExtension, string filter, string initialName)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            Maybe<string> filepathMaybe = new Maybe<string>();
            dlg.Title = windowTitle;
            dlg.FilterIndex = 0;
            dlg.DefaultExt = defaultExtension;
            dlg.CheckPathExists = true;
            dlg.Filter = filter;
            dlg.OverwritePrompt = true;
            dlg.ValidateNames = true;
            dlg.FileName = initialName;
            bool? result = dlg.ShowDialog();
            if (result == true)
            {
                filepathMaybe.AddValue(dlg.FileName);
            }

            return filepathMaybe;
        }


    }
}