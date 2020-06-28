using System.Collections.ObjectModel;
using System.Windows;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;
using Unicon2.Fragments.Programming.Views;
using Unicon2.Infrastructure;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Programming.ViewModels.ElementViewModels
{
    public abstract class LogicElementViewModel : ViewModelBase, ILogicElementViewModel
    {
        protected readonly IApplicationGlobalCommands _globalCommands;
        protected bool _isSelected;
        protected ILogicElement _model;
        protected bool _debugMode;
        protected string _caption;
        protected bool _validationError;
        protected string _description;

        protected LogicElementViewModel(string strongName, IApplicationGlobalCommands globalCommands)
        {
            this.StrongName = strongName;
            this._globalCommands = globalCommands;
        }

        public string ElementName { get; protected set; }

        public bool IsSelected
        {
            get { return this._isSelected; }
            set
            {
                this._isSelected = value;
                RaisePropertyChanged();
            }
        }

        public string StrongName { get; protected set; }

        public ILogicElement Model
        {
            get => this.GetModel();
            set => this.SetModel(value);
        }

        public string Symbol { get; protected set; }

        public string Caption
        {
            get { return this._caption; }
            set
            {
                this._caption = value;
                RaisePropertyChanged();
            }
        }

        public string Description
        {
            get { return this._description; }
            protected set
            {
                this._description = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<IConnectorViewModel> ConnectorViewModels { get; protected set; }

        private Point _deltaPosition;
        private bool xChanged;
        private bool yChanged;

        public double X
        {
            get { return this._model.X; }
            set
            {
                this._deltaPosition.X = value - this._model.X;
                if (this.yChanged)
                {
                    this.yChanged = false;
                    this.xChanged = false;
                    this.UpdateConnectorsPosition(this._deltaPosition);
                }
                else
                {
                    this.xChanged = true;
                }

                this._model.X = value;
                RaisePropertyChanged();
            }
        }

        public double Y
        {
            get { return this._model.Y; }
            set
            {
                this._deltaPosition.Y = value - this._model.Y;
                if (this.xChanged)
                {
                    this.yChanged = false;
                    this.xChanged = false;
                    this.UpdateConnectorsPosition(this._deltaPosition);
                }
                else
                {
                    this.yChanged = true;
                }

                this._model.Y = value;
                RaisePropertyChanged();
            }
        }

        private void UpdateConnectorsPosition(Point deltaPosition)
        {
            foreach (var connectorViewModel in this.ConnectorViewModels)
            {
                connectorViewModel.UpdateConnectorPosition(deltaPosition);
            }
        }

        protected abstract ILogicElement GetModel();
        protected abstract void SetModel(object modelObj);
        public abstract object Clone();

        public virtual void OpenPropertyWindow()
        {
            this._globalCommands.ShowWindowModal(() => new LogicElementSettings(),
                new LogicElementSettingsViewModel(this));
        }
    }
}