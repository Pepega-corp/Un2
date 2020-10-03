using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Unicon2.Infrastructure.Common;

namespace Unicon2.Infrastructure
{
    public interface IApplicationGlobalCommands
    {
        void ShutdownApplication();
        void ExecuteByDispacther(Action action);
        void ShowWindowModal(Func<Window> getWindow, object dataContext);
        void ShowWindowModal(Func<Window> getWindow, object dataContext, object _owner);
        void ShowWindowModal(Func<Window> getWindow, object dataContext, bool isTopmost);
        bool AskUserToDeleteSelectedGlobal(object context);
        void ShowErrorMessage(string errorKey, object context);
        void SetToBuffer(object bufferObject);
        object GetFromBuffer();
        Task CallWaitingProgressWindow(object context, bool isToOpen);
        void OpenOscillogram(string oscillogramPath = null);
        
        Maybe<FileInfo> SelectFileToOpen(string windowTitle, string filters);

        Maybe<string> SelectFilePathToSave(string windowTitle, string defaultExtension, string filter,
            string initialName);
        Action ShellLoaded { get; set; }
    }
}