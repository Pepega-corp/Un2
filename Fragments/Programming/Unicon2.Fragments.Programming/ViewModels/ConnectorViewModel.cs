using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using Unicon2.Fragments.Programming.Behaviors;
using Unicon2.Fragments.Programming.Infrastructure;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Programming.ViewModels
{
    public class ConnectorViewModel : ViewModelBase, IConnectorViewModel, IDisposable
    {
        private bool _isDragConnection;
        private string _symbol;
        private IConnector _modelConnector;

        public ConnectorViewModel(ILogicElementViewModel parent, IConnector model)
        {
            this.ParentViewModel = parent;
            this.Model = model;
            this.IsDragConnection = false;
            this._symbol = string.Empty;
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
        public Point ConnectorPosition
        {
            get { return this._modelConnector.ConnectorPoint; }
            set
            {
                this._modelConnector.ConnectorPoint = value;
                RaisePropertyChanged();
            }
        }

        public ILogicElementViewModel ParentViewModel { get; }

        public ObservableCollection<IConnectionViewModel> Connections { get; }

        
        public IConnector Model
        {
            get { return this._modelConnector; }
            set
            {
                if (value == null)
                    return;
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
        public bool Connected => this.Connections.Count > 0;

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

        public void UpdateConnectorPositionX(double deltaX)
        {
            var position =  this.ConnectorPosition;
            position.X += deltaX;
            this.ConnectorPosition = position;
        }

        public void UpdateConnectorPositionY(double deltaY)
        {
            var position = this.ConnectorPosition;
            position.Y += deltaY;
            this.ConnectorPosition = position;
        }

        private void ConnectionsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            RaisePropertyChanged(nameof(this.Connected));
        }
    }
}
