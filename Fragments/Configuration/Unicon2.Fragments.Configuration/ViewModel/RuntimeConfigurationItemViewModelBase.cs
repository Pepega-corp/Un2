using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Base;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;

namespace Unicon2.Fragments.Configuration.ViewModel
{
    public abstract class RuntimeConfigurationItemViewModelBase : ConfigurationItemViewModelBase, IRuntimeConfigurationItemViewModel
    {
        private IRuntimeConfigurationItemViewModel _parent;
        


        public new IRuntimeConfigurationItemViewModel Parent
        {
            get { return _parent; }
            set
            {
                _parent = value;
                RaisePropertyChanged();
            }
        }
        
    }
}
