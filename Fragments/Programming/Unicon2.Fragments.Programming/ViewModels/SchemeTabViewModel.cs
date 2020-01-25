using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Unicon2.Fragments.Programming.Behaviors;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;
using Unicon2.Infrastructure;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Programming.ViewModels
{
    // Вью модель одной схемы
    public class SchemeTabViewModel : ViewModelBase, ISchemeTabViewModel
    {
        public const int CELL_SIZE = 5;
        #region Events
        /// <summary>
        /// Событие закрытия вкладки схемы
        /// </summary>
        public event Action CloseTabEvent;
        #endregion

        #region Fields
        private string _schemeName;
        private double _schemeHeight;
        private double _schemeWidth;
        private double _scale;
        #endregion

        public SchemeTabViewModel(string name, Size size): this()
        {
            this.SchemeName = name;
            this.SchemeHeight = size.Height;
            this.SchemeWidth = size.Width;
        }

        public SchemeTabViewModel()
        {
            this.ElementCollection = new ObservableCollection<ISchemeElement>();

            this.ZoomIncrementCommand = new RelayCommand(this.IncrementZoom);
            this.ZoomDecrementCommand = new RelayCommand(this.DecrementZoom);
            this.CloseTabCommand = new RelayCommand(this.CloseTab);
            this.DeleteCommand = new RelayCommand(this.DeleteSelectedElements, this.CanDelete);
        }

        #region Properties
        /// <summary>
        /// Ссылка на поведение
        /// </summary>
        public DesignerCanvasBehavior SelfBehavior { get; set; }
        /// <summary>
        /// Список всех вью моделей эелементов, добавленных на схему
        /// </summary>
        public ObservableCollection<ISchemeElement> ElementCollection { get; }

        public string SchemeName
        {
            get { return this._schemeName; }
            set
            {
                if (this._schemeName == value) return;
                this._schemeName = value;
                RaisePropertyChanged();
            }
        }

        public double SchemeHeight
        {
            get { return this._schemeHeight; }
            set
            {
                if (Math.Abs(this._schemeHeight - value) < 0.0001) return;
                this._schemeHeight = value;
                RaisePropertyChanged();
            }
        }

        public double SchemeWidth
        {
            get { return this._schemeWidth; }
            set
            {
                if (Math.Abs(this._schemeWidth - value) < 0.0001) return;
                this._schemeWidth = value;
                RaisePropertyChanged();
            }
        }

        public double Scale
        {
            get => this._scale;
            set
            {
                this._scale = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(this.ScaleStr));
            }
        }

        public string ScaleStr => $"{this._scale * 100}%";

        public double RectHeight => (int) (this.SchemeHeight / this.RectY) * this.RectY- this.RectY;

        public double RectWidth => (int)(this.SchemeWidth / this.RectX) * this.RectX - this.RectX;

        public double RectX => CELL_SIZE;
        
        public double RectY => CELL_SIZE;

        #endregion Properties

        #region ZoomCommands
        public ICommand ZoomIncrementCommand { get; }

        private void IncrementZoom()
        {
            this.SelfBehavior.IncrementZoom();
        }

        public ICommand ZoomDecrementCommand { get; }

        private void DecrementZoom()
        {
            this.SelfBehavior.DecrementZoom();
        }
        #endregion

        #region CloseTabCommand
        public ICommand CloseTabCommand { get; }

        private void CloseTab()
        {
            this.CloseTabEvent?.Invoke();
        }
        #endregion

        #region DeleteCommand
        public ICommand DeleteCommand { get; }

        public void DeleteSelectedElements()
        {
            var selectedConnections = this.ElementCollection.Where(e => e is IConnectionViewModel && e.IsSelected).Cast<IConnectionViewModel>().ToList();
            foreach (IConnectionViewModel connectionViewModel in selectedConnections.Where(sc => this.ElementCollection.Contains(sc)))
            {
                ConnectionViewModel.RemoveConnectionWithNumber(connectionViewModel);
                this.ElementCollection.Remove(connectionViewModel);
            }

            var selectedElements = this.ElementCollection.Where(e =>e is ILogicElementViewModel && e.IsSelected).Cast<ILogicElementViewModel>().ToList();
            foreach (ILogicElementViewModel element in selectedElements)
            {
                var removingConnections = new List<IConnectionViewModel>();
                var connectedConnectors = element.Connectors.Where(c => c.Connected && c.Connections.Count != 0).ToList();
                foreach (var connector in connectedConnectors)
                {
                    removingConnections.AddRange(connector.Connections);
                }
                foreach (var removingConnection in removingConnections)
                {
                    ConnectionViewModel.RemoveConnectionWithNumber(removingConnection);
                    if (this.ElementCollection.Contains(removingConnection))
                    {
                        this.ElementCollection.Remove(removingConnection);
                    }
                }
                this.ElementCollection.Remove(element);
            }
        }

        public bool CanDelete()
        {
            List<ISchemeElement> selectedElements = this.ElementCollection.Where(e => e.IsSelected).ToList();
            return selectedElements.Count > 0;
        }
        #endregion

        #region IFragmentViewModel
        public string StrongName => ProgrammingKeys.SCHEME_TAB + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;
        public object Model { get; set; }
        #endregion IFragmentViewModel
    }
}
