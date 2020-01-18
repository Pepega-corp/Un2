using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Presentation.ViewModels
{
    public class DeviceDefinitionViewModel : ViewModelBase, IDeviceDefinitionViewModel
    {
        private IDeviceCreator _model;
        private string _connectionDescription;
        public string Name { get; set; }

        public string ConnectionDescription
        {
            get { return this._connectionDescription; }
            set
            {
                this._connectionDescription = value;
                this.RaisePropertyChanged();
            }
        }

        public override string ToString()
        {
            return this.Name;
        }

        public string StrongName => nameof(DeviceDefinitionViewModel);

        public object Model
        {
            get { return this._model; }
            set
            {
                this._model = value as IDeviceCreator;
                this.Name = this._model.DeviceName;
            }
        }
    }
}
