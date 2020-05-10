﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Unicon2.Fragments.Programming.Behaviors;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;
using Unicon2.Infrastructure;
using Unicon2.Unity.Commands;
using Unicon2.Unity.Common;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Programming.ViewModels
{
    // Вью модель одной схемы
    public class SchemeTabViewModel : ViewModelBase, ISchemeTabViewModel
    {
        private readonly IProgrammingViewModel _programmingViewModel;
        private readonly ILogicElementFactory _factory;
        public const int CELL_SIZE = 5;
        private ISchemeModel _model;
        /// <summary>
        /// Событие закрытия вкладки схемы
        /// </summary>
        public event Action<ISchemeTabViewModel> CloseTabEvent;

        public ISchemeModel Model
        {
            get => this.GetModel();
            set => this.SetModel(value);
        }

        public string StrongName => ProgrammingKeys.SCHEME_TAB + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

        /// <summary>
        /// Ссылка на поведение
        /// </summary>
        public DesignerCanvasBehavior SelfBehavior { get; set; }
        /// <summary>
        /// Список всех вью моделей эелементов, добавленных на схему
        /// </summary>
        public ObservableCollection<ISchemeElementViewModel> ElementCollection { get; }

        public string SchemeName
        {
            get { return this._model.SchemeName; }
            set
            {
                if (this._model.SchemeName == value) return;
                this._model.SchemeName = value;
                RaisePropertyChanged();
            }
        }

        public double SchemeHeight => this._model.SchemeHeight;

        public double SchemeWidth => this._model.SchemeWidth;

        public double Scale
        {
            get => this._model.Scale;
            set
            {
                this._model.Scale = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(this.ScaleStr));
            }
        }

        public string ScaleStr => $"{this._model.Scale * 100}%";

        public double RectHeight => (int)(this.SchemeHeight / this.RectY) * this.RectY - this.RectY;

        public double RectWidth => (int)(this.SchemeWidth / this.RectX) * this.RectX - this.RectX;

        public double RectX => CELL_SIZE;

        public double RectY => CELL_SIZE;
        public ICommand ZoomIncrementCommand { get; }
        public ICommand ZoomDecrementCommand { get; }
        public ICommand CloseTabCommand { get; }
        public ICommand DeleteCommand { get; }

        public SchemeTabViewModel(ISchemeModel model, IProgrammingViewModel programmingViewModel, ILogicElementFactory factory) : this(factory)
        {
            this._programmingViewModel = programmingViewModel;
            this.Model = model;
        }

        private SchemeTabViewModel(ILogicElementFactory factory)
        {
            this._factory = factory;
            this.ElementCollection = new ObservableCollection<ISchemeElementViewModel>();
            this.ZoomIncrementCommand = new RelayCommand(this.IncrementZoom);
            this.ZoomDecrementCommand = new RelayCommand(this.DecrementZoom);
            this.CloseTabCommand = new RelayCommand(this.CloseTab);
            this.DeleteCommand = new RelayCommand(this.DeleteSelectedElements, this.CanDelete);
        }

        private ISchemeModel GetModel()
        {
            var logicElementViewModels = this.ElementCollection.Where(ec => ec is ILogicElementViewModel)
                .Cast<ILogicElementViewModel>().ToArray();
            this._model.LogicElements = logicElementViewModels.Select(lvm => lvm.Model).ToArray();

            var connectionsViewModels = this.ElementCollection.Where(ec => ec is IConnectionViewModel)
                .Cast<IConnectionViewModel>().ToArray();
            this._model.ConnectionNumbers = connectionsViewModels.Select(c => c.ConnectionNumber).ToArray();

            return this._model;
        }

        private void SetModel(ISchemeModel objModel)
        {
            this._model = objModel;
            var logicElementsViewModels = this._factory.GetAllElementsViewModels(this._model.LogicElements);
            this.ElementCollection.AddCollection(logicElementsViewModels);

            //get ConnectionViewModel from ProjectViewModel
        }

        public void AddConnectionToProgramm(IConnectionViewModel connectionViewModel)
        {
            this._programmingViewModel.AddConnection(connectionViewModel);
        }

        public int GetNextConnectionNumber()
        {
            return this._programmingViewModel.GetNewConnectionNumber();
        }

        private void IncrementZoom()
        {
            this.SelfBehavior.IncrementZoom();
        }
        
        private void DecrementZoom()
        {
            this.SelfBehavior.DecrementZoom();
        }

        private void CloseTab()
        {
            this.CloseTabEvent?.Invoke(this);
        }

        private void DeleteSelectedElements()
        {
            var selectedConnections = this.ElementCollection.Where(e => e is IConnectionViewModel && e.IsSelected).Cast<IConnectionViewModel>().ToList();
            foreach (var connectionViewModel in selectedConnections.Where(sc => this.ElementCollection.Contains(sc)))
            {
                this._programmingViewModel.RemoveConnection(connectionViewModel);
                this.ElementCollection.Remove(connectionViewModel);
            }

            var selectedElements = this.ElementCollection.Where(e =>e is ILogicElementViewModel && e.IsSelected).Cast<ILogicElementViewModel>().ToList();
            foreach (var element in selectedElements)
            {
                var removingConnections = new List<IConnectionViewModel>();
                var connectedConnectors = element.ConnectorViewModels.Where(c => c.Connected).ToList();
                foreach (var connector in connectedConnectors)
                {
                    if (!removingConnections.Contains(connector.Connection))
                    {
                        removingConnections.Add(connector.Connection);
                    }
                }
                foreach (var removingConnection in removingConnections)
                {
                    this._programmingViewModel.RemoveConnection(removingConnection);
                    if (this.ElementCollection.Contains(removingConnection))
                    {
                        this.ElementCollection.Remove(removingConnection);
                    }
                }
                this.ElementCollection.Remove(element);
            }

            OnSelectChanged();
        }

        private bool CanDelete()
        {
            var selectedElements = this.ElementCollection.Where(e => e.IsSelected).ToList();
            return selectedElements.Count > 0;
        }

        public void OnSelectChanged()
        {
            (DeleteCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }
    }
}
