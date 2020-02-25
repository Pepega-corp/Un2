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
            get { return _connectionDescription; }
            set
            {
                _connectionDescription = value;
                RaisePropertyChanged();
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public string StrongName => nameof(DeviceDefinitionViewModel);

        public object Model
        {
            get { return _model; }
            set
            {
                _model = value as IDeviceCreator;
                Name = _model.DeviceName;
            }
        }
    }
}
