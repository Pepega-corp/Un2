using System.Collections.ObjectModel;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Base;

namespace Unicon2.Fragments.Configuration.ViewModel
{
    public abstract class RuntimeConfigurationItemViewModelBase : ConfigurationItemViewModelBase, IRuntimeConfigurationItemViewModel
    {
        private IRuntimeConfigurationItemViewModel _parent;

        protected RuntimeConfigurationItemViewModelBase()
        {
        }


        public new IRuntimeConfigurationItemViewModel Parent
        {
            get { return this._parent; }
            set
            {
                this._parent = value;
                this.RaisePropertyChanged();
            }
        }
    }
}
