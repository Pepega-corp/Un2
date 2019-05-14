using Unicon2.Infrastructure.Services.LogService;
using Unicon2.Presentation.Infrastructure.ViewModels.Device;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Presentation.ViewModels.Device
{
    public class DeviceLoggerViewModel : ViewModelBase, IDeviceLoggerViewModel
    {
        private IDeviceLogger _model;


        #region Implementation of IDeviceLoggerViewModel

        public bool IsInfoMessagesLoggingEnabled
        {
            get { return this._model.IsInfoMessagesLoggingEnabled; }
            set
            {
                this._model.IsInfoMessagesLoggingEnabled = value;
                this.RaisePropertyChanged();
            }
        }

        public bool IsFailedQueriesLoggingEnabled
        {
            get { return this._model.IsFailedQueriesLoggingEnabled; }
            set
            {
                this._model.IsFailedQueriesLoggingEnabled = value;
                this.RaisePropertyChanged();

            }
        }

        public bool IsSuccessfulQueriesLoggingEnabled
        {
            get { return this._model.IsSuccessfulQueriesLoggingEnabled; }
            set
            {
                this._model.IsSuccessfulQueriesLoggingEnabled = value;
                this.RaisePropertyChanged();

            }
        }

        public bool IsErrorsLoggingEnabled
        {
            get { return this._model.IsErrorsLoggingEnabled; }
            set
            {
                this._model.IsErrorsLoggingEnabled = value;
                this.RaisePropertyChanged();

            }
        }

        #endregion

        #region Implementation of IStronglyNamed

        public string StrongName => nameof(DeviceLoggerViewModel);


        #endregion

        #region Implementation of IViewModel

        public object Model
        {
            get { return this._model; }
            set
            {
                this._model = value as IDeviceLogger;
                this.RaisePropertyChanged(nameof(this.IsErrorsLoggingEnabled));
                this.RaisePropertyChanged(nameof(this.IsSuccessfulQueriesLoggingEnabled));
                this.RaisePropertyChanged(nameof(this.IsFailedQueriesLoggingEnabled));
                this.RaisePropertyChanged(nameof(this.IsInfoMessagesLoggingEnabled));
            }
        }

        #endregion
    }
}