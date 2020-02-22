using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using Unicon2.Fragments.Programming.Infrastructure;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;
using Unicon2.Fragments.Programming.Model;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Programming.ViewModels
{
    public class ConnectorViewModel : ViewModelBase, IConnectorViewModel, IDisposable
    {
        private bool _isDragConnection;
        private bool _connected;
        private Point _connPoint;
        private string _symbol;
        private IConnector _modelConnector;

        public ConnectorViewModel(ILogicElementViewModel parent, ConnectorOrientation orientation, ConnectorType type)
        {
            this.ParentViewModel = parent;
            this.Connected = false;
            this.Model = new Connector(orientation, type);
            this.IsDragConnection = false;
            this._symbol = string.Empty;
            this.Connections = new ObservableCollection<IConnectionViewModel>();
            this.Connections.CollectionChanged += this.ConnectionsOnCollectionChanged;
        }

        public ConnectorViewModel(ILogicElementViewModel parent, ConnectorOrientation orientation, ConnectorType type, string symbol) : this(parent, orientation, type)
        {
            this.Symbol = symbol;
        }

        public ConnectorViewModel(ILogicElementViewModel newParent, IConnectorViewModel copied)
        {
            this.CopyWithoutConnection(newParent, copied);
            this.Connections = new ObservableCollection<IConnectionViewModel>();
            this.Connections.CollectionChanged += this.ConnectionsOnCollectionChanged;
        }

        public void Dispose()
        {
            this.Connections.CollectionChanged -= this.ConnectionsOnCollectionChanged;
        }

        /// <summary>
        /// Точка расположения коннектора в DesignerCanvas
        /// </summary>
        public Point ConnectorPoint
        {
            get { return this._connPoint; }
            set
            {
                if (this._connPoint == value) return;
                this._connPoint = value;
                RaisePropertyChanged();
            }
        }

        public ILogicElementViewModel ParentViewModel { get; private set; }

        public ObservableCollection<IConnectionViewModel> Connections { get; }

        
        public IConnector Model
        {
            get { return this._modelConnector; }
            set
            {
                if (value == null || this._modelConnector != null && this._modelConnector.Equals(value)) return;
                this._modelConnector = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(this.ConnectorType));
            }
        }
        /// <summary>
        /// Тип вывода: прямой или инверсный
        /// </summary>
        public ConnectorType ConnectorType
        {
            get { return this.Model.Type; }
            set
            {
                if (this.Model.Type == value) return;
                this.Model.Type = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Направление вывода
        /// </summary>
        public ConnectorOrientation Orientation
        {
            get { return this.Model.Orientation; }
        }
        /// <summary>
        /// Номер связи
        /// </summary>
        public int ConnectionNumber
        {
            get { return this.Model.ConnectionNumber; }
            set
            {
                if (this.Model.ConnectionNumber == value) return;
                this.Model.ConnectionNumber = value;
                if (this.Model.ConnectionNumber == -1 && this.Connections.Count > 0)
                {
                    foreach (var connection in this.Connections)
                    {
                        if (connection.Sink.Equals(this))
                        {
                            connection.Sink = null;
                        }
                        else
                        {
                            connection.Source = null;
                        }
                    }
                    this.Connections.Clear();
                }
                RaisePropertyChanged(nameof(this.Connected));
            }
        }

        /// <summary>
        /// Флаг того,что в процессе соединения линией двух коннекторов,
        /// данный вывод находится в допустимом радиусе для соединения 
        /// </summary>
        public bool IsDragConnection
        {
            get { return this._isDragConnection; }
            set
            {
                if (this._isDragConnection == value) return;
                this._isDragConnection = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Флаг того, что вывод подключен
        /// </summary>
        public bool Connected
        {
            get { return this._connected; }
            set
            {
                if (this._connected == value) return;
                this._connected = value;
                if (!this._connected)
                {
                    foreach (var connection in this.Connections)
                    {
                        if (connection.Sink.Equals(this))
                        {
                            connection.Sink = null;
                        }
                        else
                        {
                            connection.Source = null;
                        }
                    }
                    this.Connections.Clear();
                    this.Model.ConnectionNumber = -1;
                }
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// Символ-подпись вывода
        /// </summary>
        public string Symbol
        {
            get { return this._symbol; }
            set
            {
                if (value == null) return;
                this._symbol = value;
                RaisePropertyChanged();
            }
        }

        private void ConnectionsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            this.Connected = this.Connections.Count > 0;
        }

        /// <summary>
        /// Копирование общих свойств вывода, без информации о подключении и линии связи
        /// </summary>
        /// <param name="parent">Элемент, в котором находится данный вывод</param>
        /// <param name="cvm">Копируемый объект вывода</param>
        public void CopyWithoutConnection(ILogicElementViewModel parent, IConnectorViewModel cvm)
        {
            this.ParentViewModel = parent;
            this.Model = new Connector(cvm.Model.Orientation, cvm.Model.Type);
            this.IsDragConnection = false;
            this.Symbol = cvm.Symbol;
        }
    }
}
