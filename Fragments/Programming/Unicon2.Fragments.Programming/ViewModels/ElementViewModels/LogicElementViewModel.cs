﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Unicon2.Fragments.Programming.Infrastructure.Enums;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme;
using Unicon2.Fragments.Programming.Model.Elements;
using Unicon2.Fragments.Programming.Views;
using Unicon2.Infrastructure;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Programming.ViewModels.ElementViewModels
{
    public abstract class LogicElementViewModel : ViewModelBase, ISchemeElementViewModel
    {
        protected IApplicationGlobalCommands _globalCommands;
        protected bool isSelected;
        protected LogicElement _logicElementModel;
        protected bool debugMode;
        protected string caption = "";
        protected bool validationError;
        protected string description;
        private Point _deltaPosition;
        private bool _xChanged;
        private bool _yChanged;

        public string ElementName { get; protected set; }
        public ElementType ElementType => this._logicElementModel.ElementType;
        public bool IsSelected
        {
            get => this.isSelected;
            set
            {
                this.isSelected = value;
                RaisePropertyChanged();
            }
        }
        public abstract string StrongName { get; }
        public LogicElement Model => this.GetModel();
        public string Symbol { get; protected set; }
        public string Caption
        {
            get => this.caption;
            set
            {
                this.caption = value;
                RaisePropertyChanged();
            }
        }
        public string Description
        {
            get => this.description;
            protected set
            {
                this.description = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<ConnectorViewModel> ConnectorViewModels { get; protected set; }
        public double X
        {
            get => this._logicElementModel.X;
            set
            {
                this._deltaPosition.X = value - this._logicElementModel.X;
                if (this._yChanged)
                {
                    this._yChanged = false;
                    this._xChanged = false;
                    this.UpdateConnectorsPosition(this._deltaPosition);
                }
                else
                {
                    this._xChanged = true;
                }

                this._logicElementModel.X = value;
                RaisePropertyChanged();
            }
        }
        public double Y
        {
            get => this._logicElementModel.Y;
            set
            {
                this._deltaPosition.Y = value - this._logicElementModel.Y;
                if (this._xChanged)
                {
                    this._yChanged = false;
                    this._xChanged = false;
                    this.UpdateConnectorsPosition(this._deltaPosition);
                }
                else
                {
                    this._yChanged = true;
                }

                this._logicElementModel.Y = value;
                RaisePropertyChanged();
            }
        }
        public bool Connected => ConnectorViewModels.All(c=>c.Connected);
        public int CompilePriority { get; set; }

        private void UpdateConnectorsPosition(Point deltaPosition)
        {
            foreach (var connectorViewModel in this.ConnectorViewModels)
            {
                connectorViewModel.UpdateConnectorPosition(deltaPosition);
            }
        }

        protected virtual LogicElement GetModel()
        {
            this._logicElementModel.Connectors.Clear();
            this._logicElementModel.Connectors.AddRange(this.ConnectorViewModels.Select(cvm => cvm.Model));
            return this._logicElementModel;
        }

        protected virtual void SetModel(LogicElement model)
        {
            X = model.X;
            Y = model.Y;
            ConnectorViewModels.Clear();
            foreach (var connector in model.Connectors)
            {
                ConnectorViewModels.Add(new ConnectorViewModel(this, connector));
            }
        }
        
        public virtual void ResetSettingsTo(LogicElement model){}

        public abstract LogicElementViewModel Clone();

        public virtual void OpenPropertyWindow()
        {
            this._globalCommands.ShowWindowModal(() => new LogicElementSettings(), new LogicElementSettingsViewModel(this));
        }
    }
}