using Unicon2.Infrastructure.Services.LogService;
using Unicon2.Presentation.Infrastructure.ViewModels.Device;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Presentation.ViewModels.Device
{
    public class DeviceLoggerViewModel : ViewModelBase, IDeviceLoggerViewModel
    {
        private IDeviceLogger _model;


        public bool IsInfoMessagesLoggingEnabled
        {
            get { return _model.IsInfoMessagesLoggingEnabled; }
            set
            {
                _model.IsInfoMessagesLoggingEnabled = value;
                RaisePropertyChanged();
            }
        }

        public bool IsFailedQueriesLoggingEnabled
        {
            get { return _model.IsFailedQueriesLoggingEnabled; }
            set
            {
                _model.IsFailedQueriesLoggingEnabled = value;
                RaisePropertyChanged();

            }
        }

        public bool IsSuccessfulQueriesLoggingEnabled
        {
            get { return _model.IsSuccessfulQueriesLoggingEnabled; }
            set
            {
                _model.IsSuccessfulQueriesLoggingEnabled = value;
                RaisePropertyChanged();

            }
        }

        public bool IsErrorsLoggingEnabled
        {
            get { return _model.IsErrorsLoggingEnabled; }
            set
            {
                _model.IsErrorsLoggingEnabled = value;
                RaisePropertyChanged();

            }
        }

        public string StrongName => nameof(DeviceLoggerViewModel);


        public object Model
        {
            get { return _model; }
            set
            {
                _model = value as IDeviceLogger;
                RaisePropertyChanged(nameof(IsErrorsLoggingEnabled));
                RaisePropertyChanged(nameof(IsSuccessfulQueriesLoggingEnabled));
                RaisePropertyChanged(nameof(IsFailedQueriesLoggingEnabled));
                RaisePropertyChanged(nameof(IsInfoMessagesLoggingEnabled));
            }
        }
    }
}