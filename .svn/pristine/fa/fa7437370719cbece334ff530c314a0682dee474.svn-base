using System.Collections.ObjectModel;
using System.Windows.Input;
using Unicon2.Infrastructure.Services.LogService;
using Unicon2.Presentation.Infrastructure.ViewModels.DockingManagerWindows;

namespace Unicon2.Presentation.Infrastructure.ViewModels.Windows
{
    public interface ILogServiceViewModel : IAnchorableWindow
    {

        string HeaderString { get; set; }
        string LastMessageString { get; set; }
        ObservableCollection<ILogMessage> InfoMessageCollectionToShow { get; }

        bool IsInfoMessagesShowing { get; set; }
        bool IsErrorsShowing { get; set; }
        bool IsSuccessfulQueryShowing { get; set; }
        bool IsFailedQueryShowing { get; set; }
        int InfoMessagesCount { get; }
        int ErrorMessagesCount { get; }
        int SuccessfulQueryMessagesCount { get; }
        int FailedQueryMessagesCount { get; }
        ICommand ClearLoggerCommand { get; set; }

        ICommand SetLogFilePathCommand { get; set; }
        bool IsLoggingToFileEnabled { get; set; }
        ICommand RefreshHeaderStringCommand { get; set; }
        string SelectedFilteringMessageSource { get; set; }

        ObservableCollection<string> FilteringMessageSourceCollection { get; }
    }
}