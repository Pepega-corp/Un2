using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Unicon2.Connections.MockConnection.Model;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.ViewModel;
using Unicon2.Fragments.Measuring.ViewModel;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Services;
using Unicon2.Model.Memory;
using Unicon2.Presentation.Values;
using Unicon2.Presentation.ViewModels.Fragment;
using Unicon2.Shell.ViewModels;
using Unicon2.Tests.Utils;
using Unicon2.Unity.Commands;
using Unicon2.Unity.Common;
using Unicon2.Unity.Interfaces;
using Unity;

namespace Unicon2.Tests.Measuring
{
    [TestFixture]

    public class MeasuringTests
    {
        private ITypesContainer _typesContainer;
        private IDevice _device;
        private IDeviceConfiguration _configuration;
        private RuntimeConfigurationViewModel _configurationFragmentViewModel;
        private RelayCommand _readCommand;
        private ShellViewModel _shell;
        private MeasuringMonitorViewModel _measuringMonitorViewModel;

        [SetUp]
        public async Task OnSetup()
        {
            Program.RefreshProject();
            _typesContainer =
                new TypesContainer(Program.GetApp().Container.Resolve(typeof(IUnityContainer)) as IUnityContainer);

            _device = Program.GetDevice();
            _configuration =

                _device.DeviceFragments.First(fragment => fragment.StrongName == "Configuration") as
                    IDeviceConfiguration;

            _shell = _typesContainer.Resolve<ShellViewModel>();
            var deviceMemory = new DeviceMemory();


            _device.DeviceMemory = deviceMemory;
            _configurationFragmentViewModel = null;

            _configurationFragmentViewModel = _shell.ProjectBrowserViewModel.DeviceViewModels[0].FragmentViewModels
                    .First(model => model.NameForUiKey == "Configuration") as
                RuntimeConfigurationViewModel;
            _readCommand = _configurationFragmentViewModel.FragmentOptionsViewModel.FragmentOptionGroupViewModels
                .First(model => model.NameKey == "Device").FragmentOptionCommandViewModels
                .First(model => model.TitleKey == ApplicationGlobalNames.UiCommandStrings.READ_STRING_KEY)
                .OptionCommand as RelayCommand;
            await _typesContainer.Resolve<IDevicesContainerService>()
                .ConnectDeviceAsync(_device, new MockConnection());
            _shell.ActiveFragmentViewModel = new FragmentPaneViewModel()
            {
                FragmentViewModel = _configurationFragmentViewModel
            };
            await _configurationFragmentViewModel.SetFragmentOpened(true);
            _measuringMonitorViewModel=_shell.ProjectBrowserViewModel.DeviceViewModels[0].FragmentViewModels
                    .First(model => model.NameForUiKey == "MeasuringMonitor") as
                MeasuringMonitorViewModel;
        }

        [Test]
        public async Task MeasuringMonitorLoad()
        {
            _device.DeviceMemory.DeviceMemoryValues.Clear();

            var loadCommand = _measuringMonitorViewModel.FragmentOptionsViewModel.FragmentOptionGroupViewModels
                .First(model => model.NameKey == "Loading").FragmentOptionCommandViewModels
                .First(model => model.TitleKey == "Load")
                .OptionCommand as RelayCommand;

            loadCommand.Execute(null);

            var analogSignal =
                _measuringMonitorViewModel.MeasuringElementViewModels.First(model => model.Header == "analogSignal");
            Assert.True(await TestsUtils.WaitUntil(() => analogSignal.FormattedValueViewModel != null));

            Assert.True((analogSignal.FormattedValueViewModel as NumericValueViewModel).NumValue=="0");
                
        }
    }

}