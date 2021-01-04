using System;
using System.Threading.Tasks;
using Unicon2.Infrastructure.Functional;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;

namespace Unicon2.Fragments.Configuration.ViewModel.Helpers
{
    public class LoadAllConfigurationHelper
    {
        public static Func<IFragmentViewModel, Task<Result<object>>> GetConFigurationLoadingHelper()
        {
            return async (fragment) =>
            {
                if (fragment is RuntimeConfigurationViewModel runtimeConfigurationViewModel)
                {
                    if (!runtimeConfigurationViewModel.DeviceContext.DataProviderContainer.DataProvider.IsSuccess)
                    {
                        return Result<object>.Create(false);
                    }
                    await runtimeConfigurationViewModel.Read();
                    return runtimeConfigurationViewModel.DeviceContext.DeviceMemory.DeviceMemoryValues;
                }
                return Result<object>.Create(false);
            };
        }
    }
}