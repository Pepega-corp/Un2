using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Programming.ViewModels
{
    public class ConnectionViewModel : ViewModelBase, IConnectionViewModel
    {
        private static Dictionary<IConnectionViewModel, int> ConnectionNumbers = new Dictionary<IConnectionViewModel, int>();

        private const string NAME_PATTERN = "var{0}";
        public const string PATH_NAME = "ConnectionPath"; // должно полностью совпадать с Name у Path в XAML

        #region Private fields

        private IConnectorViewModel _source;
        private IConnectorViewModel _sink;
        private PathGeometry _path;
        private Point _labelPosition;
        private DoubleCollection _strokeDashArray;
        private ushort _currentValue;
        private bool _gotValue;
        private bool _isSelected;
        private double _value;
        private double _x;
        private double _y;

        #endregion

        #region Constructors

        public ConnectionViewModel()
        {
            this._currentValue = 0;
            this._gotValue = false;
        }

        public ConnectionViewModel(IConnectorViewModel source, IConnectorViewModel sink, PathGeometry path) : this()
        {
            this.Source = source;
            this.Sink = sink;
            this.Path = path;
            this.StrokeDashArray = new DoubleCollection();
        }

        #endregion

        #region Properties

        public string Name { get; private set; }

        public bool IsSelected
        {
            get => this._isSelected;
            set
            {
                this._isSelected = value;
                RaisePropertyChanged();
            }
        }

        public double Value
        {
            get => this._value;
            set
            {
                this._value = value;
                RaisePropertyChanged();
            }
        }

        public PathGeometry Path
        {
            get { return this._path; }
            set
            {
                if (Equals(this._path, value)) return;
                this._path = value;
                RaisePropertyChanged();
            }
        }

        public IConnectorViewModel Source
        {
            get { return this._source; }
            set
            {
                if (this._source != null && this._source.Equals(value)) return;
                if (this._source != null)
                {
                    if (this._source.Connections.Contains(this))
                    {
                        this._source.Connections.Remove(this);
                    }
                }
                this._source = value;
                if (this._source != null)
                {
                    this._source.Connections.Add(this);
                    this.UpdatePathGeometry();
                }

                RaisePropertyChanged();
            }
        }

        public IConnectorViewModel Sink
        {
            get { return this._sink; }
            set
            {
                if (this._sink != null && this._sink.Equals(value)) return;
                if (this._sink != null)
                {
                    if (this._sink.Connections.Contains(this))
                    {
                        this._sink.Connections.Remove(this);
                    }
                }
                this._sink = value;
                if (this._sink != null)
                {
                    this._sink.Connections.Add(this);
                    this.UpdatePathGeometry();
                }

                RaisePropertyChanged();
            }
        }

        public int ConnectionNumber { get; private set; }

        /// <summary>
        /// Значение сигнала, проходящего по данной связи
        /// </summary>
        public ushort CurrentValue
        {
            get { return this._currentValue; }
            set
            {
                if (this._currentValue == value) return;
                this._currentValue = value;
                this.GotValue = this._currentValue != 0;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Флаг того, что линия связи имеет значение
        /// </summary>
        public bool GotValue
        {
            get { return this._gotValue; }
            set
            {
                if (this._gotValue == value) return;
                this._gotValue = value;
                RaisePropertyChanged();
            }
        }

        public Point LabelPosition
        {
            get { return this._labelPosition; }
            set
            {
                if (this._labelPosition == value) return;
                this._labelPosition = value;
                RaisePropertyChanged();
            }
        }

        public DoubleCollection StrokeDashArray
        {
            get { return this._strokeDashArray; }
            set
            {
                if (Equals(this._strokeDashArray, value)) return;
                this._strokeDashArray = value;
                RaisePropertyChanged();
            }
        }
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
        #endregion Properties

        #region Methods

        public void UpdateConnector(IConnectorViewModel connector)
        {
            if (ReferenceEquals(connector, this.Source))
            {
                RaisePropertyChanged(nameof(this.Source));
                this.UpdatePathGeometry();
            }

            if (ReferenceEquals(connector, this.Sink))
            {
                RaisePropertyChanged(nameof(this.Sink));
                this.UpdatePathGeometry();
            }
        }

        private void UpdatePathGeometry()
        {
            if (this.Source == null || this.Sink == null || this._path == null) return;

            this._path.Figures.Clear();
            List<Point> linePoints = PathFinder.GetConnectionLine(this.Source, this.Sink);
            if (linePoints.Count > 1)
            {
                PathFigure figure = new PathFigure();
                figure.StartPoint = linePoints[0];
                linePoints.Remove(linePoints[0]);
                figure.Segments.Add(new PolyLineSegment(linePoints, true));
                this._path.Figures.Add(figure);
                RaisePropertyChanged(nameof(this.Path));
            }
        }

        #endregion

        #region Static methods

        public static void AddNewConnectionNumber(ConnectionViewModel connection)
        {
            if (ConnectionNumbers.ContainsKey(connection))
                return;

            if (connection.Source.ConnectionNumber != -1)
            {
                ConnectionNumbers.Add(connection, connection.Source.ConnectionNumber);
                connection.ConnectionNumber = connection.Source.ConnectionNumber;
                connection.Sink.ConnectionNumber = connection.Source.ConnectionNumber;
            }
            else
            {
                int i = 0;
                while (ConnectionNumbers.ContainsValue(i))
                {
                    i++;
                }
                ConnectionNumbers.Add(connection, i);
                connection.ConnectionNumber = i;
                connection.Source.ConnectionNumber = i;
                connection.Sink.ConnectionNumber = i;
                connection.Name = string.Format(NAME_PATTERN, i);
            }
        }

        /// <summary>
        /// Удаление связи, а так же из списка используемых номеров
        /// </summary>
        /// <param name="viewModel">Связь</param>
        public static void RemoveConnectionWithNumber(IConnectionViewModel viewModel)
        {
            if (ConnectionNumbers.ContainsKey(viewModel))
            {
                ConnectionNumbers.Remove(viewModel);
            }
            //при удалении связи нужно проверять, подключен ли вывод еще с одной связью
            viewModel.Source.Connections.Remove(viewModel);
            viewModel.Source.Connected = viewModel.Source.Connections.Count > 0;
            viewModel.Source = null;

            viewModel.Sink.Connections.Remove(viewModel);
            viewModel.Sink.Connected = viewModel.Sink.Connections.Count > 0;
            viewModel.Sink = null;
        }

        #endregion
    }
}
