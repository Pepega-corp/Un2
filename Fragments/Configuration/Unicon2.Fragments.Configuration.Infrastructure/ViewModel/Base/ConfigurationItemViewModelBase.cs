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
        protected IConfigurationItem _model;
        private IConfigurationItemViewModel _parent;
        private ObservableCollection<IConfigurationItemViewModel> _childStructItemViewModels;

        protected ConfigurationItemViewModelBase()
        {
            this.ChildStructItemViewModels = new ObservableCollection<IConfigurationItemViewModel>();
        }

        public string Header
        {
            get { return this._header; }
            set
            {
                this._header = value;
                this.RaisePropertyChanged();
            }
        }

        public int Level
        {
            get { return this._level; }
            set
            {
                this._level = value;
                this.RaisePropertyChanged();
            }
        }

        public Action<bool?> Checked { get; set; }


        public abstract string TypeName { get; }


        public bool IsChecked
        {
            get { return this._isChecked; }
            set
            {
                this._isChecked = value;
                this.RaisePropertyChanged();
            }
        }

        public string Description
        {
            get { return this._description; }
            set
            {
                this._description = value;
                this.RaisePropertyChanged();
            }
        }

        public bool IsCheckable
        {
            get { return this._isChekable; }
            set
            {
                if (this._isChekable == value) return;
                this._isChekable = value;
                this.RaisePropertyChanged();
            }
        }

        public ObservableCollection<IConfigurationItemViewModel> ChildStructItemViewModels
        {
            get { return this._childStructItemViewModels; }
            set
            {
                this._childStructItemViewModels = value;
                this.RaisePropertyChanged();
            }
        }

        public IConfigurationItemViewModel Parent
        {
            get { return this._parent; }
            set
            {
                this._parent = value;
                this.RaisePropertyChanged();
            }
        }


        public abstract string StrongName { get; }

        public object Model
        {
            get { return this.GetModel(); }
            set
            {
                this.SetModel(value);

            }
        }

        protected virtual void SetModel(object model)
        {
            this._model = model as IConfigurationItem;
            this.Description = this._model.Description;
            this.Header = this._model.Name;

        }
        protected virtual object GetModel()
        {
            return this._model;
        }

        protected virtual void SaveModel()
        {
            this._model.Description = this.Description;
            this._model.Name = this.Header;
        }
    }
}