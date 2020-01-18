using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Services;
using Unicon2.Infrastructure.Services.LogService;
using Unicon2.Presentation.Infrastructure.Enums;
using Unicon2.Presentation.Infrastructure.ViewModels.Windows;
using Unicon2.Unity.Commands;
using Unicon2.Unity.Common;

namespace Unicon2.Presentation.ViewModels.Windows
{
    public class LogServiceViewModel : AnchorableWindowBase, ILogServiceViewModel
    {
        private readonly ILogService _logService;
        private readonly ILocalizerService _localizerService;
        private readonly List<ILogMessage> _allLogMessages;
        private bool _isInfoMessagesShowing;
        private bool _isErrorsShowing;
        private bool _isSuccessfulQueryShowing;
        private bool _isFailedQueryShowing;
        private bool _isLoggingToFileEnabled;
        private ObservableCollection<object> _logSubjectCollection;
        private string _headerString;
        private int _newNotificationsCount;
        private string _lastMessageString;
        private string _selectedFilteringMessageSource;

        public LogServiceViewModel(ILogService logService, ILocalizerService localizerService)
        {
            this._allLogMessages = new List<ILogMessage>();
            this._logService = logService;
            this._logService.LoggersChangedAction += this.LoggersCollectionChanged;

            this._localizerService = localizerService;
            this._logService.NewMessageAction += this.OnNewMessage;
            this.InfoMessageCollectionToShow = new ObservableCollection<ILogMessage>();
            this.SetLogFilePathCommand = new RelayCommand(this.OnSetLogFilePathExecute);
            this.RefreshHeaderStringCommand = new RelayCommand(this.RefreshHeaderString);
            this.ClearLoggerCommand = new RelayCommand(this.OnExecuteClearLogger);
            this.RefreshHeaderString();
            this.IsVisible = true;
            this.WindowNameKey = ApplicationGlobalNames.WindowsStrings.NOTIFICATIONS_STRING_KEY;
            this.AnchorableDefaultPlacementEnum = PlacementEnum.Bottom;
            this.LoggersCollectionChanged();
            this.SelectedFilteringMessageSource = this.FilteringMessageSourceCollection.FirstOrDefault();
        }

        private void LoggersCollectionChanged()
        {
            this.FilteringMessageSourceCollection = new ObservableCollection<string>(this._logService.GetLoggersEnumerable().Select((logger => logger.SourceName)));
            this.FilteringMessageSourceCollection.Insert(0, this._localizerService.GetLocalizedString(ApplicationGlobalNames.DefaultStringsForUi.ALL_STRING_KEY));
            this.RaisePropertyChanged(nameof(this.FilteringMessageSourceCollection));
        }

        public string HeaderString
        {
            get { return this._headerString; }
            set
            {
                this._headerString = value;
                this.RaisePropertyChanged();
            }
        }

        public string LastMessageString
        {
            get { return this._lastMessageString; }
            set
            {
                this._lastMessageString = value;
                this.RaisePropertyChanged();
            }
        }


        public ObservableCollection<object> LogSubjectCollection
        {
            get { return this._logSubjectCollection; }
            set
            {
                this._logSubjectCollection = value;
                this.RaisePropertyChanged();
            }
        }

        public ObservableCollection<ILogMessage> InfoMessageCollectionToShow { get; }



        public bool IsInfoMessagesShowing
        {
            get { return this._isInfoMessagesShowing; }
            set
            {
                this._isInfoMessagesShowing = value;
                this.RaisePropertyChanged();
                this.FilterMessages();

            }
        }

        public bool IsErrorsShowing
        {
            get { return this._isErrorsShowing; }
            set
            {
                this._isErrorsShowing = value;
                this.RaisePropertyChanged();
                this.FilterMessages();

            }
        }

        public bool IsSuccessfulQueryShowing
        {
            get { return this._isSuccessfulQueryShowing; }
            set
            {
                this._isSuccessfulQueryShowing = value;
                this.RaisePropertyChanged();
                this.FilterMessages();

            }
        }

        public bool IsFailedQueryShowing
        {
            get { return this._isFailedQueryShowing; }
            set
            {
                this._isFailedQueryShowing = value;
                this.RaisePropertyChanged();
                this.FilterMessages();

            }
        }

        public int InfoMessagesCount { get; private set; }

        public int ErrorMessagesCount { get; private set; }

        public int SuccessfulQueryMessagesCount { get; private set; }

        public int FailedQueryMessagesCount { get; private set; }

        public ICommand ClearLoggerCommand { get; set; }


        public ICommand SetLogFilePathCommand { get; set; }



        public bool IsLoggingToFileEnabled
        {
            get { return this._isLoggingToFileEnabled; }
            set
            {
                if (String.IsNullOrEmpty(this._logService.FilePath))
                {
                    this.SetLogFilePathCommand.Execute(null);
                }
                if (!String.IsNullOrEmpty(this._logService.FilePath))
                {
                    this._isLoggingToFileEnabled = value;
                }
                this._logService.IsLoggingToFileEnabled = this._isLoggingToFileEnabled;
                this.RaisePropertyChanged();

            }
        }

        public ICommand RefreshHeaderStringCommand { get; set; }

        public string SelectedFilteringMessageSource
        {
            get { return this._selectedFilteringMessageSource; }
            set
            {
                this._selectedFilteringMessageSource = value;
                this.RaisePropertyChanged();
                this.FilterMessages();
            }
        }


        public ObservableCollection<string> FilteringMessageSourceCollection { get; private set; }


        private void OnExecuteClearLogger()
        {
            this._allLogMessages.Clear();
            this.InfoMessageCollectionToShow.Clear();
            this.RaisePropertyChanged(nameof(this.InfoMessageCollectionToShow));
            this.ErrorMessagesCount = 0;
            this.InfoMessagesCount = 0;
            this.FailedQueryMessagesCount = 0;
            this.SuccessfulQueryMessagesCount = 0;
            this.RaisePropertyChanged(nameof(this.ErrorMessagesCount));
            this.RaisePropertyChanged(nameof(this.InfoMessageCollectionToShow));
            this.RaisePropertyChanged(nameof(this.SuccessfulQueryMessagesCount));
            this.RaisePropertyChanged(nameof(this.FailedQueryMessagesCount));
            this.RefreshHeaderString();
        }

        private void RefreshHeaderString()
        {
            this.HeaderString =
                this._localizerService.GetLocalizedString(ApplicationGlobalNames.DefaultStringsForUi.RECENT_NOTIFICATIONS_STRING_KEY) + " 0";
            this._newNotificationsCount = 0;
        }


        private void FilterMessages()
        {
            this.InfoMessageCollectionToShow.Clear();

            List<LogMessageTypeEnum> listTypesToHide = this.GetTypesToHide();
            IEnumerable<ILogMessage> messagesFilteredByType = this._allLogMessages.Where((message => !listTypesToHide.Contains(message.LogMessageType)));

            if (this.FilteringMessageSourceCollection[0] != this._selectedFilteringMessageSource)
            {
                this.InfoMessageCollectionToShow.AddCollection(messagesFilteredByType.Where((message => message.MessageSubject == this._selectedFilteringMessageSource)));
            }
            else
            {
                this.InfoMessageCollectionToShow.AddCollection(messagesFilteredByType);
            }

            this.RaisePropertyChanged(nameof(this.InfoMessageCollectionToShow));
        }

        private List<LogMessageTypeEnum> GetTypesToHide()
        {
            List<LogMessageTypeEnum> typesToHide = new List<LogMessageTypeEnum>();
            if (!this.IsInfoMessagesShowing) typesToHide.Add(LogMessageTypeEnum.Info);
            if (!this.IsErrorsShowing) typesToHide.Add(LogMessageTypeEnum.Error);
            if (!this.IsFailedQueryShowing) typesToHide.Add(LogMessageTypeEnum.FailedQuery);
            if (!this.IsSuccessfulQueryShowing) typesToHide.Add(LogMessageTypeEnum.SuccsessfulQuery);

            return typesToHide;

        }


        private void OnNewMessage(ILogMessage logMessage)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                this._allLogMessages.Insert(0, logMessage);
                this._newNotificationsCount++;
                this.HeaderString = this._localizerService.GetLocalizedString(ApplicationGlobalNames.DefaultStringsForUi
                                   .RECENT_NOTIFICATIONS_STRING_KEY) + " " + this._newNotificationsCount;

                if (logMessage.LogMessageType != LogMessageTypeEnum.SuccsessfulQuery)
                    this.LastMessageString = logMessage.Description;
                if (!this.GetTypesToHide().Contains(logMessage.LogMessageType))
                {
                    if ((this.FilteringMessageSourceCollection[0] == this._selectedFilteringMessageSource) ||
                        (logMessage.MessageSubject == this._selectedFilteringMessageSource))
                    {
                        this.InfoMessageCollectionToShow.Insert(0, logMessage);
                    }
                }

                this.AddMessagesCount(logMessage.LogMessageType);
            });
        }

        private void AddMessagesCount(LogMessageTypeEnum logMessageType)
        {
            switch (logMessageType)
            {
                case LogMessageTypeEnum.Info:
                    this.InfoMessagesCount++;
                    this.RaisePropertyChanged(nameof(this.InfoMessagesCount));
                    break;
                case LogMessageTypeEnum.Error:
                    this.ErrorMessagesCount++;
                    this.RaisePropertyChanged(nameof(this.ErrorMessagesCount));

                    break;
                case LogMessageTypeEnum.SuccsessfulQuery:
                    this.SuccessfulQueryMessagesCount++;
                    this.RaisePropertyChanged(nameof(this.SuccessfulQueryMessagesCount));

                    break;
                case LogMessageTypeEnum.FailedQuery:
                    this.FailedQueryMessagesCount++;
                    this.RaisePropertyChanged(nameof(this.FailedQueryMessagesCount));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logMessageType), logMessageType, null);
            }
        }

        private void OnSetLogFilePathExecute()
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = " TXT файл (*.txt)|*.txt" + "|Все файлы (*.*)|*.* ",
                DefaultExt = ".txt",
                FileName = "logfile." + DateTime.Today.ToShortDateString()
            };
            if (sfd.ShowDialog() == true)
            {
                this._logService.FilePath = sfd.FileName;
            }

        }
    }
}