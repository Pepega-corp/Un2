using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Functional;

namespace Unicon2.Tests.Utils.Mocks
{
    public static class ApplicationGlobalCommandsMockExtensions
    {
        public static ApplicationGlobalCommandsMock WithAskUserGlobalResult(this ApplicationGlobalCommandsMock applicationGlobalCommands, bool result)
        {
            applicationGlobalCommands.AskUserGlobalResult = result;
            return applicationGlobalCommands;
        }
        
        public static ApplicationGlobalCommandsMock WithSelectFileToOpenResult(this ApplicationGlobalCommandsMock applicationGlobalCommands, string result)
        {
            applicationGlobalCommands.SelectFileToOpenResult = result;
            return applicationGlobalCommands;
        }
    }


  public  class ApplicationGlobalCommandsMock: IApplicationGlobalCommands
    {
        public bool AskUserGlobalResult { get; set; }
        public bool IsAskUserGlobalTriggered { get; set; }
        public string SelectFileToOpenResult { get; set; }
        public bool IsSelectFileToOpenTriggered { get; set; }

        
        public ApplicationGlobalCommandsMock()
        {
            
        }

        public static ApplicationGlobalCommandsMock Create()
        {
            return new ApplicationGlobalCommandsMock();
        }


        public void ShutdownApplication()
        {
            throw new NotImplementedException();
        }

        public void ExecuteByDispacther(Action action)
        {
            throw new NotImplementedException();
        }

        public void ShowWindowModal(Func<Window> getWindow, object dataContext)
        {
            throw new NotImplementedException();
        }

        public void ShowWindowModal(Func<Window> getWindow, object dataContext, object _owner)
        {
            throw new NotImplementedException();
        }

        public void ShowWindowModal(Func<Window> getWindow, object dataContext, bool isTopmost)
        {
            throw new NotImplementedException();
        }

        public bool AskUserToDeleteSelectedGlobal(object context)
        {
            throw new NotImplementedException();
        }

        public bool AskUserGlobal(string message, string title, object context = null, string yesText=null, string noText=null)
        {
            IsAskUserGlobalTriggered = true;
            return AskUserGlobalResult;
        }

        public void ShowErrorMessage(string errorKey, object context)
        {
            throw new NotImplementedException();
        }

        public void SetToBuffer(object bufferObject)
        {
            throw new NotImplementedException();
        }

        public object GetFromBuffer()
        {
            throw new NotImplementedException();
        }

        public Task CallWaitingProgressWindow(object context, bool isToOpen)
        {
            throw new NotImplementedException();
        }

        public void OpenOscillogram(string oscillogramPath = null)
        {
            throw new NotImplementedException();
        }

        public Result<FileInfo> SelectFileToOpen(string windowTitle, string filters)
        {
            IsSelectFileToOpenTriggered = true;
            return new FileInfo(SelectFileToOpenResult);
        }

        public Maybe<string> SelectFilePathToSave(string windowTitle, string defaultExtension, string filter, string initialName)
        {
            throw new NotImplementedException();
        }

        public Action ShellLoaded { get; set; }
        public void SetGlobalDialogContext(object context)
        {
            throw new NotImplementedException();
        }
    }
}
