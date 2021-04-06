using System;
using System.Windows;
using Unicon2.Fragments.Programming.Infrastructure;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;
using Unicon2.Fragments.Programming.Model;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Programming.ViewModels
{
    public class ConnectorViewModel : ViewModelBase, IConnectorViewModel
    {
        private bool _isDragConnection;
        private string _symbol;
        private IConnector _modelConnector;
        private IConnectionViewModel _connectionViewModel;

        public event Action<Point> ConnectorPositionChanged;

        public ConnectorViewModel(ILogicElementViewModel parent, IConnector model)
        {
            this.ParentViewModel = parent;
            this._modelConnector = model;
            this.IsDragConnection = false;
            this._symbol = string.Empty;
        }

        public ConnectorViewModel(ILogicElementViewModel parent, ConnectorOrientation orientation, ConnectorType type)
        {
            this.ParentViewModel = parent;
            this._modelConnector = new Connector(orientation, type);
            this.IsDragConnection = false;
            this._symbol = string.Empty;
        }
       
        /// <summary>
        /// Точка расположения коннектора в DesignerCanvas
        /// </summary>
        public Point ConnectorPosition
        {
            get { return this._modelConnector.ConnectorPosition; }
            set
            {
                this._modelConnector.ConnectorPosition = value;
                RaisePropertyChanged();
                ConnectorPositionChanged?.Invoke(this._modelConnector.ConnectorPosition);
            }
        }

        public ILogicElementViewModel ParentViewModel { get; }
        
        public IConnector Model
        {
            get => GetModel();
            set => SetModel(value);
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

        public IConnectionViewModel Connection
        {
            get => this._connectionViewModel;
            set
            {
                if(this._connectionViewModel == value)
                    return;

                this._connectionViewModel = value;

                RaisePropertyChanged();
                RaisePropertyChanged(nameof(this.Connected));

                _modelConnector.ConnectionNumber = _connectionViewModel == null ? -1 : _connectionViewModel.ConnectionNumber;
                RaisePropertyChanged(nameof(this.ConnectionNumber));
            }
        }

        public int ConnectionNumber
        {
            get => _modelConnector.ConnectionNumber;
            set
            {
                _modelConnector.ConnectionNumber = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Флаг того, что вывод подключен
        /// </summary>
        public bool Connected => this._connectionViewModel != null;

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

        private IConnector GetModel()
        {
            this._modelConnector.ConnectionNumber = this.ConnectionNumber;
            return this._modelConnector;
        }

        private void SetModel(IConnector model)
        {
            if (model == null)
                return;
            
            this._modelConnector.ConnectorPosition = model.ConnectorPosition;
            RaisePropertyChanged(nameof(ConnectorPosition));

            _modelConnector.Type = model.Type;
            RaisePropertyChanged(nameof(this.ConnectorType));
        }

        public void UpdateConnectorPosition(Point deltaPosition)
        {
            var position = this.ConnectorPosition;
            position.X += deltaPosition.X;
            position.Y += deltaPosition.Y;
            this.ConnectorPosition = position;
        }
    }
}
