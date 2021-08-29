using System;
using System.Windows;
using Unicon2.Fragments.Programming.Infrastructure;
using Unicon2.Fragments.Programming.Model;
using Unicon2.Fragments.Programming.ViewModels.ElementViewModels;

namespace Unicon2.Fragments.Programming.ViewModels
{
    public class ConnectorViewModel : SegmentPointViewModel
    {
        private bool _isDragConnection;
        private string _symbol;
        private Connector _modelConnector;
        private ConnectionViewModel _connectionViewModel;

        public event Action<Point> ConnectorPositionChanged;

        public ConnectorViewModel(LogicElementViewModel parent, Connector model) : base(model)
        {
            this._modelConnector = model;
            _modelConnector.ConnectionChanged += OnConnectionChanged;
            this.ParentViewModel = parent;
            this.IsDragConnection = false;
            this._symbol = string.Empty;
        }

        /// <summary>
        /// Точка расположения коннектора в DesignerCanvas
        /// </summary>
        public Point ConnectorPosition
        {
            get { return this._modelConnector.Position; }
            set
            {
                this._modelConnector.Position = value;
                RaisePropertyChanged();
                ConnectorPositionChanged?.Invoke(this._modelConnector.Position);
            }
        }

        public LogicElementViewModel ParentViewModel { get; }
        
        public Connector Model
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

        public ConnectionViewModel Connection
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
        public bool Connected => this._modelConnector.ConnectionSegment != null;

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

        private void OnConnectionChanged()
        {
            RaisePropertyChanged(nameof(Connected));
        }

        private Connector GetModel()
        {
            this._modelConnector.ConnectionNumber = this.ConnectionNumber;
            return this._modelConnector;
        }

        private void SetModel(Connector model)
        {
            if (model == null)
                return;
            
            this._modelConnector.Position = model.Position;
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
