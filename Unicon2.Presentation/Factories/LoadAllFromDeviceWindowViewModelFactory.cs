using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Services;
using Unicon2.Infrastructure.Services.LogService;
using Unicon2.Presentation.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.ViewModels.Device;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Presentation.ViewModels.Device;
using Unicon2.Presentation.ViewModels.Windows;

namespace Unicon2.Presentation.Factories
{
    public class LoadAllFromDeviceWindowViewModelFactory
    {
        private readonly ILoadAllService _loadAllService;
        private readonly ISerializerService _serializerService;

        public LoadAllFromDeviceWindowViewModelFactory(ILoadAllService loadAllService,
            ISerializerService serializerService)
        {
            _loadAllService = loadAllService;
            _serializerService = serializerService;
        }
        
        private async Task<bool> LoadFragment(ILoadFragmentViewModel loadFragmentViewModel,
            IFragmentViewModel fragmentViewModel,
            Func<IFragmentViewModel, Task<Result<object>>> handler, string pathToFolder)
        {
            if (loadFragmentViewModel.IsSelectedForLoading)
            {
                try
                {
                    loadFragmentViewModel.IsFragmentLoadingInProgress = true;
                    var result = await handler(fragmentViewModel);
                    var path = pathToFolder + "\\" + loadFragmentViewModel.UiName;
                    if (fragmentViewModel is IFragmentFileExtension fragmentFileExtension)
                    {
                        path =path + "." + fragmentFileExtension.FileExtension;
                    }
                    result.OnSuccess(o => _serializerService.SerializeInFile(o, path));
                    return result.IsSuccess;
                }
                catch (Exception e)
                {
                    StaticContainer.Container.Resolve<ILogService>().LogMessage(e.Message,LogMessageTypeEnum.Error);
                    return false;
                }
                finally
                {
                    loadFragmentViewModel.IsFragmentLoadingInProgress = false;
                }
            }

            return true;
        }

        public LoadAllFromDeviceWindowViewModel CreateLoadAllFromDeviceWindowViewModel(IDevice device,
            IDeviceViewModel deviceViewModel)
        {
            var loadFragmentViewModels = new List<ILoadFragmentViewModel>();

            List<(ILoadFragmentViewModel loadFragmentViewModel, Func<string, Task<bool>> loadFragment)> loadFragmentsInfo =
                new List<(ILoadFragmentViewModel loadFragmentViewModel, Func<string, Task<bool>> loadFragment)>();

            foreach (var fragmentViewModel in deviceViewModel.FragmentViewModels)
            {
                var loadHandler = _loadAllService.LoadFragmentHandler(
                    fragmentViewModel.StrongName);

                loadHandler.OnSuccess(handler =>
                {
                    var loadFragmentViewModel = new DefaultLoadFragmentViewModel()
                    {
                        FragmentName = StaticContainer.Container.Resolve<ILocalizerService>().GetLocalizedString(fragmentViewModel.NameForUiKey),
                        UiName = StaticContainer.Container.Resolve<ILocalizerService>().GetLocalizedString(fragmentViewModel.NameForUiKey)
                    };

                    Task<bool> GetLoadFragmentTask(string pathToFolder) => LoadFragment(loadFragmentViewModel, fragmentViewModel, handler, pathToFolder);

                    loadFragmentsInfo.Add((loadFragmentViewModel,
                        GetLoadFragmentTask));
                    loadFragmentViewModels.Add(loadFragmentViewModel);
                });

            }


            Func<string, Task<LoadingResult>> loader = async (pathToFolder) =>
            {
                Directory.CreateDirectory(pathToFolder + "\\" + device.DeviceSignature);
                var results = new List<bool>();
                foreach (var loadFragmentInfo in loadFragmentsInfo)
                {
                  results.Add(await loadFragmentInfo.loadFragment(pathToFolder + "\\" + deviceViewModel.DeviceSignature));
                }
                if (results.All(b => b))
                {
                    return LoadingResult.Success;
                }
                if (results.All(b => !b))
                {
                    return LoadingResult.Fail;
                }
                if (results.Any(b => !b))
                {
                    return LoadingResult.SuccessWithIssues;
                }
                
                return LoadingResult.Fail;
            };


            return new LoadAllFromDeviceWindowViewModel(loadFragmentViewModels, loader, deviceViewModel.DeviceSignature);
        }
    }
}