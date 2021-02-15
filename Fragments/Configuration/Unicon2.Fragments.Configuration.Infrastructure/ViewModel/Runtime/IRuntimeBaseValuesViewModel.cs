using System.Collections.Generic;
using System.Windows.Input;
using Unicon2.Presentation.Infrastructure.ViewModels;

namespace Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime
{
    public interface IRuntimeBaseValuesViewModel
    {
        List<IRuntimeBaseValueViewModel> BaseValueViewModels { get; }
    }

    public interface IRuntimeBaseValueViewModel
    {
        string Name { get; }
        ICommand OnBaseValueApply { get; }
    }
}