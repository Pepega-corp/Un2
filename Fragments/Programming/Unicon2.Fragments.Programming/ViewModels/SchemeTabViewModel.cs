using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
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
        /// <summary>
        /// Событие закрытия вкладки схемы
        /// </summary>
        public event Action CloseTabEvent;

        private string _schemeName;
        private double _schemeHeight;
        private double _schemeWidth;
        private double _scale;

        //public SchemeTabViewModel(string name, Size size)
        //{
        //    this.SchemeName = name;
        //    this.SchemeHeight = size.Height;
        //    this.SchemeWidth = size.Width;
        //    this.ElementList = new ObservableCollection<IAbstractViewModel>();
        //}

        public SchemeTabViewModel()
        {
            //this.SchemeName = name;
            //this.SchemeHeight = size.Height;
            //this.SchemeWidth = size.Width;
            this.ElementList = new ObservableCollection<ILogicElementViewModel>();
            this.ZoomIncrementCommand = new RelayCommand(this.IncrementZoom);
            this.ZoomDecrementCommand = new RelayCommand(this.DecrementZoom);
            this.CloseTabCommand = new RelayCommand(this.CloseTab);
            this.DeleteCommand = new RelayCommand(this.DeleteSelectedElements, this.CanDelete);
        }

        /// <summary>
        /// Ссылка на поведение
        /// </summary>
        //public DesignerCanvasBehavior SelfBehavior { get; set; }
        /// <summary>
        /// Список всех вью моделей эелементов, добавленных на схему
        /// </summary>
        public ObservableCollection<ILogicElementViewModel> ElementList { get; } //для заглушки

        public string SchemeName
        {
            get { return this._schemeName; }
            set
            {
                if (this._schemeName == value) return;
                this._schemeName = value;
                this.RaisePropertyChanged();
            }
        }

        public double SchemeHeight
        {
            get { return this._schemeHeight; }
            set
            {
                if (Math.Abs(this._schemeHeight - value) < 0.0001) return;
                this._schemeHeight = value;
                this.RaisePropertyChanged();
            }
        }

        public double SchemeWidth
        {
            get { return this._schemeWidth; }
            set
            {
                if (Math.Abs(this._schemeWidth - value) < 0.0001) return;
                this._schemeWidth = value;
                this.RaisePropertyChanged();
            }
        }

        public double Scale
        {
            get { return this._scale; }
            set
            {
                if (this._scale == value) return;
                this._scale = value;
                this.RaisePropertyChanged();
            }
        }

        public ICommand ZoomIncrementCommand { get; }

        private void IncrementZoom()
        {
            //this.SelfBehavior.IncrementZoom();
        }

        public ICommand ZoomDecrementCommand { get; }

        private void DecrementZoom()
        {
            //this.SelfBehavior.DecrementZoom();
        }

        public ICommand CloseTabCommand { get; }

        private void CloseTab()
        {
            this.CloseTabEvent?.Invoke();
        }

        public ICommand DeleteCommand { get; }

        public void DeleteSelectedElements()
        {
            //List<AbstractElementViewModel> selectedElements = this.ElementList.Where(e => e.IsSelected).ToList();
            //foreach (AbstractElementViewModel element in selectedElements)
            //{
            //    if (element is ConnectionViewModel && this.ElementList.Contains(element))
            //    {
            //        ConnectionViewModel.RemoveConnectionWithNumber(element as ConnectionViewModel);
            //        continue;
            //    }
            //    if (element is BaseElementViewModel)
            //    {
            //        List<ConnectionViewModel> removingConnections = new List<ConnectionViewModel>();
            //        List<ConnectorViewModel> connectedConnectors =
            //            (element as BaseElementViewModel).ConnectorViewModels
            //            .Where(c => c.Connected && c.Connections.Count != 0).ToList();
            //        foreach (ConnectorViewModel connector in connectedConnectors)
            //        {
            //            removingConnections.AddRange(connector.Connections);
            //        }
            //        foreach (ConnectionViewModel connection in removingConnections)
            //        {
            //            ConnectionViewModel.RemoveConnectionWithNumber(connection);
            //        }
            //        this.ElementList.Remove(element);
            //    }
            //}
        }

        public bool CanDelete()
        {
            List<ILogicElementViewModel> selectedElements = this.ElementList.Where(e => e.IsSelected).ToList();
            return selectedElements.Count > 0;
        }

        /// <summary>
        /// Удаление связи со схемы
        /// </summary>
        /// <param name="connection">Удаляемая связь</param>
        public void OnDeleteConnection(/*ConnectionViewModel connection*/)
        {
            //if (this.ElementList.Contains(connection))
            //{
            //    this.ElementList.Remove(connection);
            //    connection.DeleteConnection -= this.OnDeleteConnection;
            //}
        }

        public string StrongName => ProgrammingKeys.SCHEME_TAB + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;
        public object Model { get; set; }
    }
}
