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
using Unicon2.Infrastructure.Functional;
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
        private object _globalDialogContext;

        public ApplicationGlobalCommands(ITypesContainer container, 
            IDialogCoordinator dialogCoordinator, 
            ILocalizerService localizerService)
        {
            _container = container;
            _dialogCoordinator = dialogCoordinator;
            _localizerService = localizerService;
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
            GetWindow(getWindow, dataContext);
        }

        public void ShowWindowModal(Func<Window> getWindow, object dataContext, object _owner)
        {
            GetWindow(getWindow, dataContext, _owner);
        }

        public void ShowWindowModal(Func<Window> getWindow, object dataContext, bool isTopmost)
        {
            Window windowToShow = GetWindow(getWindow, dataContext);
            windowToShow.Topmost = isTopmost;
        }

        private Window GetWindow(Func<Window> getWindow, object dataContext, object _owner = null)
        {
            Window windowToShow = getWindow.Invoke();
            windowToShow.Owner = _owner==null ? Application.Current.MainWindow : (_owner as Window);
           // windowToShow.Topmost = true;
            windowToShow.DataContext = dataContext;
            windowToShow.ShowDialog();
            return windowToShow;
        }

        public bool AskUserToDeleteSelectedGlobal(object context)
        {
            if (_dialogCoordinator.ShowModalMessageExternal(context, _localizerService.GetLocalizedString(ApplicationGlobalNames.DialogStrings.DELETE),
                   _localizerService.GetLocalizedString(ApplicationGlobalNames.DialogStrings.DELETE_SELECTED_ITEM_QUESTION),
                    MessageDialogStyle.AffirmativeAndNegative) == MessageDialogResult.Affirmative)
            {
                return true;
            }
            return false;
        }

        public bool AskUserGlobal(string message, string title, object context = null, string yesText = null,
            string noText = null)
        {
            var contextToUse = _globalDialogContext;
            if (context != null)
            {
                contextToUse = context;
            }

            var settings = new MetroDialogSettings();
            if (yesText != null)
            {
                settings.AffirmativeButtonText = yesText;
            }

            if (noText != null)
            {
                settings.NegativeButtonText = noText;
            }

            if (_dialogCoordinator.ShowModalMessageExternal(contextToUse, title,
                message,
                MessageDialogStyle.AffirmativeAndNegative, settings) == MessageDialogResult.Affirmative)
            {
                return true;
            }

            return false;
        }

        public void ShowErrorMessage(string errorKey, object context)
        {
            _dialogCoordinator.ShowModalMessageExternal(context,
                _localizerService.GetLocalizedString(ApplicationGlobalNames.StatusMessages.ERROR),
                _localizerService.GetLocalizedString(errorKey));
        }



        public void SetToBuffer(object bufferObject)
        {
            if (bufferObject is ICloneable)
            {
                _bufferObject = (bufferObject as ICloneable).Clone();

            }
            else
            {
                throw new ArgumentException();
            }
        }

        public object GetFromBuffer()
        {
            ICloneable cloneable = _bufferObject as ICloneable;
            if (cloneable != null) return cloneable.Clone();
            throw new ArgumentException();
        }

        public async Task CallWaitingProgressWindow(object context, bool isToOpen)
        {
            if (isToOpen)
            {
                _progressDialogController = await _dialogCoordinator.ShowProgressAsync(context,
                    _localizerService.GetLocalizedString(ApplicationGlobalNames.DialogStrings.PROCESSING_VALUES), "",
                    false);
            }
            else
            {
                if (_progressDialogController != null)
                {
                    await _progressDialogController.CloseAsync();
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

        public Result<FileInfo> SelectFileToOpen(string windowTitle, string filter)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Title = windowTitle;
            ofd.Filter = filter;
            ofd.CheckFileExists = true;
            if (ofd.ShowDialog() == true)
            {
                return Result<FileInfo>.Create(new FileInfo(ofd.FileName),true);
            }
            return Result<FileInfo>.Create(false);
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

        public Action ShellLoaded { get; set; }
        public void SetGlobalDialogContext(object context)
        {
            _globalDialogContext = context;
        }
    }
}