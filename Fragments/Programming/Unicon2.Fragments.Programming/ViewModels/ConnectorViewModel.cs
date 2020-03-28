using System.Windows;
using Unicon2.Fragments.Programming.Infrastructure;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Programming.ViewModels
{
    public class ConnectorViewModel : ViewModelBase, IConnectorViewModel
    {
        private bool _isDragConnection;
        private string _symbol;
        private IConnector _modelConnector;
        private IConnectionViewModel _connectionViewModel;

        public ConnectorViewModel(ILogicElementViewModel parent, IConnector model)
        {
            this.ParentViewModel = parent;
            this.Model = model;
            this.IsDragConnection = false;
            this._symbol = string.Empty;
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
        
        public IConnector Model
        {
            get
            {
                this._modelConnector.ConnectionNumber = this.ConnectionNumber;
                return this._modelConnector;
            }
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
                RaisePropertyChanged(nameof(this.ConnectionNumber));
            }
        }

        public int ConnectionNumber => this._connectionViewModel?.ConnectionNumber ?? -1;

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

        public void UpdateConnectorPosition(Point deltaPosition)
        {
            var position = this.ConnectorPosition;
            position.X += deltaPosition.X;
            position.Y += deltaPosition.Y;
            this.ConnectorPosition = position;
        }
    }
}
