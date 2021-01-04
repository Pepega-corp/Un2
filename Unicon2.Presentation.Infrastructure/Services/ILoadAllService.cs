using System;
using System.Threading.Tasks;
using Unicon2.Infrastructure.Functional;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;

namespace Unicon2.Presentation.Infrastructure.Services
{
    public interface ILoadAllService
    {
        Result<Func<IFragmentViewModel, Task<Result<object>>>> LoadFragmentHandler(string fragmentName);

        void RegisterFragmentLoadHandler(string fragmentName, Func<IFragmentViewModel, Task<Result<object>>> handler);
    }
}