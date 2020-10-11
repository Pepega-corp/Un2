using System.Collections.ObjectModel;
using System.Linq;
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
        protected IApplicationGlobalCommands _globalCommands;
        protected bool _isSelected;
        protected ILogicElement _model;
        protected bool _debugMode;
        protected string _caption;
        protected bool _validationError;
        protected string _description;

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

        public abstract string StrongName { get; }

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

        public bool Connected => ConnectorViewModels.All(c=>c.Connected);

        private void UpdateConnectorsPosition(Point deltaPosition)
        {
            foreach (var connectorViewModel in this.ConnectorViewModels)
            {
                connectorViewModel.UpdateConnectorPosition(deltaPosition);
            }
        }

        protected virtual ILogicElement GetModel()
        {
            return this._model;
        }

        protected virtual void SetModel(ILogicElement model)
        {
            X = model.X;
            Y = model.Y;

            ConnectorViewModels.Clear();
            foreach (var c in model.Connectors)
            {
                var newConnector = new ConnectorViewModel(this, c.Orientation, c.Type);
                newConnector.ConnectionNumber = c.ConnectionNumber;
                newConnector.ConnectorPosition = c.ConnectorPosition;
                ConnectorViewModels.Add(newConnector);
            }
        }
        public abstract ILogicElementViewModel Clone();

        protected ILogicElementViewModel Clone<TR, T>() where TR : ILogicElementViewModel, new() where T : ILogicElement, new()
        {
            var newModel = new T();
            newModel.CopyValues(_model);

            var ret = new TR()
            {
                Model = newModel,
                Caption = this.Caption
            };

            return ret;
        }

        public virtual void OpenPropertyWindow()
        {
            this._globalCommands.ShowWindowModal(() => new LogicElementSettings(),
                new LogicElementSettingsViewModel(this));
        }
    }
}