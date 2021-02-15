using System.Collections.Generic;
using System.Windows.Input;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;

namespace Unicon2.Fragments.Configuration.ViewModel
{
    public class RuntimeBaseValuesViewModel:IRuntimeBaseValuesViewModel
    {
        public RuntimeBaseValuesViewModel(List<IRuntimeBaseValueViewModel> baseValueViewModels)
        {
            BaseValueViewModels = baseValueViewModels;
        }

        public List<IRuntimeBaseValueViewModel> BaseValueViewModels { get; }
    }

    public class RuntimeBaseValueViewModel : IRuntimeBaseValueViewModel
    {
        public RuntimeBaseValueViewModel(string name, ICommand onBaseValueApply)
        {
            Name = name;
            OnBaseValueApply = onBaseValueApply;
        }

        public string Name { get; }
        public ICommand OnBaseValueApply { get; }
    }
}