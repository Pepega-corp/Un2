﻿using System;
using System.Collections.ObjectModel;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Programming.ViewModels.ElementViewModels
{
    public abstract class LogicElementViewModel : ViewModelBase, ILogicElementViewModel
    {
        protected LogicElementViewModel(string strongName)
        {
            this.StrongName = strongName;
            this.DebugMode = false;
            this.IsSelected = false;
            this.ValidationError = false;
        }

        private bool _isSelected;
        private ILogicElement _model;
        private bool _debugMode;
        private string _caption;
        private bool _validationError;
        private string _description;
        private double _x;
        private double _y;

        public string ElementName { get; protected set; }

        public bool IsSelected
        {
            get { return this._isSelected; }
            set
            {
                this._isSelected = value;
                this.RaisePropertyChanged();
            }
        }

        public string StrongName { get; protected set; }

        public object Model
        {
            get { return this._model; }
            set { this._model = value as ILogicElement; }
        }

        public string Symbol { get; protected set; }

        public string Caption
        {
            get { return this._caption; }
            set
            {
                this._caption = value;
                this.RaisePropertyChanged();
            }
        }

        public string Description
        {
            get { return this._description; }
            protected set
            {
                this._description = value;
                this.RaisePropertyChanged();
            }
        }
        public bool ValidationError
        {
            get { return this._validationError; }
            set
            {
                this._validationError = value;
                this.RaisePropertyChanged();
            }
        }
        public bool DebugMode
        {
            get { return this._debugMode; }
            set
            {
                this._debugMode = value;
                this.RaisePropertyChanged();
            }
        }

        public ObservableCollection<IConnectorViewModel> Connectors { get; protected set; }

        public abstract object Clone();

        public double X
        {
            get { return this._x; }
            set
            {
                if (Math.Abs(this._x - value) < 0.01) return;
                this._x = value;
                RaisePropertyChanged();
            }
        }

        public double Y
        {
            get { return this._y; }
            set
            {
                if (Math.Abs(this._y - value) < 0.01) return;
                this._y = value;
                RaisePropertyChanged();
            }
        }

        public virtual void OpenPropertyWindow() { }
    }
}
