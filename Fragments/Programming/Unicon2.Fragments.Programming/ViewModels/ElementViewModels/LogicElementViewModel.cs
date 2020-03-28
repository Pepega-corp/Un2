using System;
using System.Collections.ObjectModel;
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

        public double X
        {
            get { return this._model.X; }
            set
            {
                if (Math.Abs(this._model.X - value) < 0.01)
                    return;

                var delta = value - this._model.X;
                this._model.X = value;

                this.UpdateConnectorsPositionX(delta);
                RaisePropertyChanged();
            }
        }

        public double Y
        {
            get { return this._model.Y; }
            set
            {
                if (Math.Abs(this._model.Y - value) < 0.01)
                    return;

                var delta = value - this._model.Y;
                this._model.Y = value;

                this.UpdateConnectorsPositionY(delta);
                RaisePropertyChanged();
            }
        }

        private void UpdateConnectorsPositionX(double deltaX)
        {
            foreach (var connectorViewModel in this.ConnectorViewModels)
            {
                connectorViewModel.UpdateConnectorPositionX(deltaX);
            }
        }

        private void UpdateConnectorsPositionY(double deltaY)
        {
            foreach (var connectorViewModel in this.ConnectorViewModels)
            {
                connectorViewModel.UpdateConnectorPositionY(deltaY);
            }
        }

        protected abstract ILogicElement GetModel();
        protected abstract void SetModel(object modelObj);
        public abstract object Clone();

        public virtual void OpenPropertyWindow()
        {
            this._globalCommands.ShowWindowModal(() => new LogicElementSettings(), new LogicElementSettingsViewModel(this));
        }
    }
}
