using System;
using System.Collections.ObjectModel;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Infrastructure.Common;
using Unicon2.Presentation.Infrastructure.TreeGrid;

namespace Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Base
{
    public abstract class ConfigurationItemViewModelBase : ValidatableBindableBase, IConfigurationItemViewModel
    {
        private string _header;
        private int _level;
        private bool _isChecked;
        private string _description;
        private bool _isChekable;
        private IConfigurationItemViewModel _parent;
        private ObservableCollection<IConfigurationItemViewModel> _childStructItemViewModels;

        protected ConfigurationItemViewModelBase()
        {
            ChildStructItemViewModels = new ObservableCollection<IConfigurationItemViewModel>();
        }

        public string Header
        {
            get { return _header; }
            set
            {
                _header = value;
                RaisePropertyChanged();
            }
        }

        public int Level
        {
            get { return _level; }
            set
            {
                _level = value;
                RaisePropertyChanged();
            }
        }
        public Action<bool?> Checked { get; set; }
        public abstract string TypeName { get; }

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                RaisePropertyChanged();
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                RaisePropertyChanged();
            }
        }

        public bool IsCheckable
        {
            get { return _isChekable; }
            set
            {
                if (_isChekable == value) return;
                _isChekable = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<IConfigurationItemViewModel> ChildStructItemViewModels
        {
            get { return _childStructItemViewModels; }
            set
            {
                _childStructItemViewModels = value;
                RaisePropertyChanged();
            }
        }

        public IConfigurationItemViewModel Parent
        {
            get { return _parent; }
            set
            {
                _parent = value;
                RaisePropertyChanged();
            }
        }
        protected override void OnDisposing()
        {
	        Parent = null;
	        base.OnDisposing();
        }
	}
}