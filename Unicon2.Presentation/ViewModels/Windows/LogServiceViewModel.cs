using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Services;
using Unicon2.Infrastructure.Services.LogService;
using Unicon2.Presentation.Infrastructure.Enums;
using Unicon2.Presentation.Infrastructure.ViewModels.Windows;
using Unicon2.Unity.Commands;

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
            _allLogMessages = new List<ILogMessage>();
            _logService = logService;
            _logService.LoggersChangedAction += LoggersCollectionChanged;

            _localizerService = localizerService;
            _logService.NewMessageAction += OnNewMessage;
            InfoMessageCollectionToShow = new ObservableCollection<ILogMessage>();
            SetLogFilePathCommand = new RelayCommand(OnSetLogFilePathExecute);
            RefreshHeaderStringCommand = new RelayCommand(RefreshHeaderString);
            ClearLoggerCommand = new RelayCommand(OnExecuteClearLogger);
            RefreshHeaderString();
            IsVisible = true;
            WindowNameKey = ApplicationGlobalNames.WindowsStrings.NOTIFICATIONS_STRING_KEY;
            AnchorableDefaultPlacementEnum = PlacementEnum.Bottom;
            LoggersCollectionChanged();
            SelectedFilteringMessageSource = FilteringMessageSourceCollection.FirstOrDefault();
            Content = this;
        }

        private void LoggersCollectionChanged()
        {
            FilteringMessageSourceCollection = new ObservableCollection<string>(_logService.GetLoggersEnumerable().Select((logger => logger.SourceName)));
            FilteringMessageSourceCollection.Insert(0, _localizerService.GetLocalizedString(ApplicationGlobalNames.DefaultStringsForUi.ALL_STRING_KEY));
            RaisePropertyChanged(nameof(FilteringMessageSourceCollection));
        }

        public string HeaderString
        {
            get { return _headerString; }
            set
            {
                _headerString = value;
                RaisePropertyChanged();
            }
        }

        public string LastMessageString
        {
            get { return _lastMessageString; }
            set
            {
                _lastMessageString = value;
                RaisePropertyChanged();
            }
        }


        public ObservableCollection<object> LogSubjectCollection
        {
            get { return _logSubjectCollection; }
            set
            {
                _logSubjectCollection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<ILogMessage> InfoMessageCollectionToShow { get; }



        public bool IsInfoMessagesShowing
        {
            get { return _isInfoMessagesShowing; }
            set
            {
                _isInfoMessagesShowing = value;
                RaisePropertyChanged();
                FilterMessages();

            }
        }

        public bool IsErrorsShowing
        {
            get { return _isErrorsShowing; }
            set
            {
                _isErrorsShowing = value;
                RaisePropertyChanged();
                FilterMessages();

            }
        }

        public bool IsSuccessfulQueryShowing
        {
            get { return _isSuccessfulQueryShowing; }
            set
            {
                _isSuccessfulQueryShowing = value;
                RaisePropertyChanged();
                FilterMessages();

            }
        }

        public bool IsFailedQueryShowing
        {
            get { return _isFailedQueryShowing; }
            set
            {
                _isFailedQueryShowing = value;
                RaisePropertyChanged();
                FilterMessages();

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
            get { return _isLoggingToFileEnabled; }
            set
            {
                if (String.IsNullOrEmpty(_logService.FilePath))
                {
                    SetLogFilePathCommand.Execute(null);
                }
                if (!String.IsNullOrEmpty(_logService.FilePath))
                {
                    _isLoggingToFileEnabled = value;
                }
                _logService.IsLoggingToFileEnabled = _isLoggingToFileEnabled;
                RaisePropertyChanged();

            }
        }

        public ICommand RefreshHeaderStringCommand { get; set; }

        public string SelectedFilteringMessageSource
        {
            get { return _selectedFilteringMessageSource; }
            set
            {
                _selectedFilteringMessageSource = value;
                RaisePropertyChanged();
                FilterMessages();
            }
        }


        public ObservableCollection<string> FilteringMessageSourceCollection { get; private set; }


        private void OnExecuteClearLogger()
        {
            _allLogMessages.Clear();
            InfoMessageCollectionToShow.Clear();
            RaisePropertyChanged(nameof(InfoMessageCollectionToShow));
            ErrorMessagesCount = 0;
            InfoMessagesCount = 0;
            FailedQueryMessagesCount = 0;
            SuccessfulQueryMessagesCount = 0;
            RaisePropertyChanged(nameof(ErrorMessagesCount));
            RaisePropertyChanged(nameof(InfoMessageCollectionToShow));
            RaisePropertyChanged(nameof(SuccessfulQueryMessagesCount));
            RaisePropertyChanged(nameof(FailedQueryMessagesCount));
            RefreshHeaderString();
        }

        private void RefreshHeaderString()
        {
            HeaderString =
                _localizerService.GetLocalizedString(ApplicationGlobalNames.DefaultStringsForUi.RECENT_NOTIFICATIONS_STRING_KEY) + " 0";
            _newNotificationsCount = 0;
        }


        private void FilterMessages()
        {
            InfoMessageCollectionToShow.Clear();

            List<LogMessageTypeEnum> listTypesToHide = GetTypesToHide();
            IEnumerable<ILogMessage> messagesFilteredByType = _allLogMessages.Where((message => !listTypesToHide.Contains(message.LogMessageType)));

            if (FilteringMessageSourceCollection[0] != _selectedFilteringMessageSource)
            {
                InfoMessageCollectionToShow.AddCollection(messagesFilteredByType.Where((message => message.MessageSubject == _selectedFilteringMessageSource)));
            }
            else
            {
                InfoMessageCollectionToShow.AddCollection(messagesFilteredByType);
            }

            RaisePropertyChanged(nameof(InfoMessageCollectionToShow));
        }

        private List<LogMessageTypeEnum> GetTypesToHide()
        {
            List<LogMessageTypeEnum> typesToHide = new List<LogMessageTypeEnum>();
            if (!IsInfoMessagesShowing) typesToHide.Add(LogMessageTypeEnum.Info);
            if (!IsErrorsShowing) typesToHide.Add(LogMessageTypeEnum.Error);
            if (!IsFailedQueryShowing) typesToHide.Add(LogMessageTypeEnum.FailedQuery);
            if (!IsSuccessfulQueryShowing) typesToHide.Add(LogMessageTypeEnum.SuccsessfulQuery);

            return typesToHide;

        }


        private void OnNewMessage(ILogMessage logMessage)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                _allLogMessages.Insert(0, logMessage);
                _newNotificationsCount++;
                HeaderString = _localizerService.GetLocalizedString(ApplicationGlobalNames.DefaultStringsForUi
                                   .RECENT_NOTIFICATIONS_STRING_KEY) + " " + _newNotificationsCount;

                if (logMessage.LogMessageType != LogMessageTypeEnum.SuccsessfulQuery)
                    LastMessageString = logMessage.Description;
                if (!GetTypesToHide().Contains(logMessage.LogMessageType))
                {
                    if ((FilteringMessageSourceCollection[0] == _selectedFilteringMessageSource) ||
                        (logMessage.MessageSubject == _selectedFilteringMessageSource))
                    {
                        InfoMessageCollectionToShow.Insert(0, logMessage);
                    }
                }

                AddMessagesCount(logMessage.LogMessageType);
            });
        }

        private void AddMessagesCount(LogMessageTypeEnum logMessageType)
        {
            switch (logMessageType)
            {
                case LogMessageTypeEnum.Info:
                    InfoMessagesCount++;
                    RaisePropertyChanged(nameof(InfoMessagesCount));
                    break;
                case LogMessageTypeEnum.Error:
                    ErrorMessagesCount++;
                    RaisePropertyChanged(nameof(ErrorMessagesCount));

                    break;
                case LogMessageTypeEnum.SuccsessfulQuery:
                    SuccessfulQueryMessagesCount++;
                    RaisePropertyChanged(nameof(SuccessfulQueryMessagesCount));

                    break;
                case LogMessageTypeEnum.FailedQuery:
                    FailedQueryMessagesCount++;
                    RaisePropertyChanged(nameof(FailedQueryMessagesCount));
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
                _logService.FilePath = sfd.FileName;
            }

        }
    }
}