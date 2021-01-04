using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Functional;
using Unicon2.Presentation.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;

namespace Unicon2.Presentation.Services
{
    public class LoadAllService : ILoadAllService
    {
        private Dictionary<string, Func<IFragmentViewModel, Task<Result<object>>>> _loadFragmentHandlers =
            new Dictionary<string, Func<IFragmentViewModel, Task<Result<object>>>>();


        public Result<Func<IFragmentViewModel, Task<Result<object>>>> LoadFragmentHandler(string fragmentName)
        {
            return _loadFragmentHandlers.GetElement(fragmentName);
        }

        public void RegisterFragmentLoadHandler(string fragmentName, Func<IFragmentViewModel, Task<Result<object>>> handler)
        {
            _loadFragmentHandlers.AddElement(fragmentName, handler);
        }
    }
}