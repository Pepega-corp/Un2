using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Presentation.ViewModels.Windows;

namespace Unicon2.Presentation.Factories
{
    public class LoadAllFromDeviceWindowViewModelFactory
    {
        public LoadAllFromDeviceWindowViewModelFactory()
        {

        }

        public LoadAllFromDeviceWindowViewModel CreateLoadAllFromDeviceWindowViewModel(IDevice device)
        {
            var loadFragmentViewModels = new List<ILoadFragmentViewModel>()
            {
                new DefaultLoadFragmentViewModel()
                {
                    
                }
            };

            
            
            Func<string, Task> loader = async (pathToFolder) => { };


            return new LoadAllFromDeviceWindowViewModel(loadFragmentViewModels, loader);
        }
    }
}