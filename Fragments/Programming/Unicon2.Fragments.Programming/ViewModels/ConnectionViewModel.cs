using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using Unicon2.Fragments.Programming.Infrastructure;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;
using Unicon2.Infrastructure;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Programming.ViewModels
{
    public class ConnectionViewModel : ViewModelBase, IConnectionViewModel
    {
        public const string PATH_NAME = "ConnectionPath"; // должно полностью совпадать с Name у Path в XAML

        #region Private fields
        private IConnection _model;
        private IConnectorViewModel _source;
        private Point _labelPosition;
        private DoubleCollection _strokeDashArray;
        private ushort _currentValue;
        private bool _gotValue;
        private bool _debugMode;
        private bool _isSelected;
        private double _value;
        private double _x;
        private double _y;
        
        #endregion

        #region Constructors

        public ConnectionViewModel(IConnection model, IConnectorViewModel source, IConnectorViewModel sink)
        {
            this.StrokeDashArray = new DoubleCollection();
            this._currentValue = 0;
            this._gotValue = false;

            this._model = model;

            this._source = source;
            this._source.Connection = this;

            this.SinkConnectors = new ObservableCollection<IConnectorViewModel> {sink};
            sink.Connection = this;
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
            get { return this._model.Path; }
            set
            {
                if (Equals(this._model.Path, value))
                    return;
                this._model.Path = value;
                RaisePropertyChanged();
            }
        }

        public IConnectorViewModel SourceConnector
        {
            get { return this._source; }
            set
            {
                if(value.Orientation != ConnectorOrientation.RIGHT)
                    return;

                this._source = value;
                this.UpdatePathGeometry();
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<IConnectorViewModel> SinkConnectors { get; }

        public int ConnectionNumber => this._model.ConnectionNumber;

        /// <summary>
        /// Флаг того, что линия связи имеет значение
        /// </summary>
        public bool DebugMode
        {
            get { return this._debugMode; }
            set
            {
                this._debugMode = value;
                RaisePropertyChanged();
            }
        }

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
                if (Math.Abs(this._x - value) < 0.01)
                    return;

                this._x = value;

                RaisePropertyChanged();
            }
        }

        public double Y
        {
            get { return this._y; }
            set
            {
                if (Math.Abs(this._y - value) < 0.01)
                    return;

                this._y = value;

                RaisePropertyChanged();
            }
        }

        public string StrongName => ProgrammingKeys.CONNECTION +
                                    ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;
        public IConnection Model
        {
            get { return this.GetModel(); }
            set { this.SetModel(value); }
        }

        #endregion Properties

        #region Methods

        private IConnection GetModel()
        {
            this._model.Clear();

            foreach (var sinkConnector in this.SinkConnectors)
            {
                this._model.AddConnector(sinkConnector.Model);
            }

            this._model.AddConnector(this._source.Model);

            return this._model;
        }

        private void SetModel(IConnection model)
        {
            this._model = model;
        }
        
        private void UpdatePathGeometry()
        {
            if (this.SourceConnector == null || this.SinkConnectors.Count == 0)
                return;
            
            if (this.SinkConnectors.Count == 1)
            {
                var sourceInfo = new PathFinder.ConnectorInfo
                {
                    ConnectorPoint = this.SourceConnector.Model.ConnectorPoint,
                    Orientation = this.SourceConnector.Orientation,
                    ConnectorParentX = this.SourceConnector.ParentViewModel.X,
                    ConnectorParentY = this.SourceConnector.ParentViewModel.Y
                };

                var sink = this.SinkConnectors[0];
                var sinkInfo = new PathFinder.ConnectorInfo
                {
                    ConnectorPoint = sink.Model.ConnectorPoint,
                    Orientation = sink.Orientation,
                    ConnectorParentX = sink.ParentViewModel.X,
                    ConnectorParentY = sink.ParentViewModel.Y
                };

                var linePoints = PathFinder.GetConnectionLine(sourceInfo, sinkInfo);

                if (linePoints.Count > 1)
                {
                    var pathFigure = new PathFigure { StartPoint = linePoints[0] };
                    for (int i = 1; i < linePoints.Count; i++)
                    {
                        var lineSegment = new LineSegment(linePoints[i], true);
                        pathFigure.Segments.Add(lineSegment);
                    }
                    //var figure = new PathFigure();
                    //figure.StartPoint = pathPoints[0];
                    //pathPoints.Remove(pathPoints[0]);
                    //figure.Segments.Add(new PolyLineSegment(pathPoints, true));

                    this.Path.Figures.Clear();
                    this.Path.Figures.Add(pathFigure);
                    RaisePropertyChanged(nameof(this.Path));
                }
            }
        }

        #endregion
    }
}
