using System.Threading.Tasks;
using MahApps.Metro.Controls.Dialogs;

namespace Unicon2.Tests.Utils.Mocks
{
    public class DialogCoordinatorMock:IDialogCoordinator
    {
        public Task<string> ShowInputAsync(object context, string title, string message, MetroDialogSettings settings = null)
        {
            throw new System.NotImplementedException();
        }

        public string ShowModalInputExternal(object context, string title, string message, MetroDialogSettings settings = null)
        {
            throw new System.NotImplementedException();
        }

        public Task<LoginDialogData> ShowLoginAsync(object context, string title, string message, LoginDialogSettings settings = null)
        {
            throw new System.NotImplementedException();
        }

        public LoginDialogData ShowModalLoginExternal(object context, string title, string message,
            LoginDialogSettings settings = null)
        {
            throw new System.NotImplementedException();
        }

        public Task<MessageDialogResult> ShowMessageAsync(object context, string title, string message, MessageDialogStyle style = MessageDialogStyle.Affirmative,
            MetroDialogSettings settings = null)
        {
            throw new System.NotImplementedException();
        }

        public MessageDialogResult ShowModalMessageExternal(object context, string title, string message,
            MessageDialogStyle style = MessageDialogStyle.Affirmative, MetroDialogSettings settings = null)
        {
            return MessageDialogResult.Negative;

        }

        public Task<ProgressDialogController> ShowProgressAsync(object context, string title, string message, bool isCancelable = false,
            MetroDialogSettings settings = null)
        {
            throw new System.NotImplementedException();
        }

        public Task ShowMetroDialogAsync(object context, BaseMetroDialog dialog, MetroDialogSettings settings = null)
        {
            throw new System.NotImplementedException();
        }

        public Task HideMetroDialogAsync(object context, BaseMetroDialog dialog, MetroDialogSettings settings = null)
        {
            throw new System.NotImplementedException();
        }

        public Task<TDialog> GetCurrentDialogAsync<TDialog>(object context) where TDialog : BaseMetroDialog
        {
            throw new System.NotImplementedException();
        }
    }
}