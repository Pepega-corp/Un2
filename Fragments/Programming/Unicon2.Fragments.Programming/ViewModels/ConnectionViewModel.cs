using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Unicon2.Fragments.Programming.Infrastructure;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;
using Unicon2.Fragments.Programming.Other;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Unity.Common;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Programming.ViewModels
{
    public class ConnectionViewModel : ViewModelBase, IConnectionViewModel
    {
        private IConnection _model;
        private IConnectorViewModel _source;
        private Point _labelPosition;
        private PathGeometry _path;
        private DoubleCollection _strokeDashArray;
        private ushort _currentValue;
        private bool _gotValue;
        private bool _debugMode;
        private bool _isSelected;
        private double _value;
        private double _x;
        private double _y;

        public event Action<IConnectionViewModel> NeedDelete;

        protected ConnectionViewModel(IConnection model, IConnectorViewModel source)
        {
            this.StrokeDashArray = new DoubleCollection();
            this._currentValue = 0;
            this._gotValue = false;

            this._model = model;
            _path = new PathGeometry();

            this._source = source;
            this._source.Connection = this;
            this._source.ConnectorPositionChanged += OnConnectorPositionChanged;
        }

        public ConnectionViewModel(IConnection model, IConnectorViewModel source, IConnectorViewModel sink) : this(model, source)
        {
            this.SinkConnectors = new ObservableCollection<IConnectorViewModel>();
            SinkConnectors.CollectionChanged += SinkCollectionChanged;
            SinkConnectors.Add(sink);
        }

        public ConnectionViewModel(IConnection model, IConnectorViewModel source, IConnectorViewModel[] sinks) : this(model, source)
        {
            this.SinkConnectors = new ObservableCollection<IConnectorViewModel>();
            SinkConnectors.CollectionChanged += SinkCollectionChanged;
            SinkConnectors.AddCollection(sinks);
        }
        
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
                if (Equals(this._path, value))
                    return;
                this._path = value;
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

                if (_source != null)
                {
                    _source.ConnectorPositionChanged -= OnConnectorPositionChanged;
                    _source.Connection = null;
                }

                this._source = value;

                if(_source != null)
                {
                    _source.ConnectorPositionChanged += OnConnectorPositionChanged;
                    _source.Connection = this;
                }
                else
                {
                    NeedDelete?.Invoke(this);
                }

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
                
        private IConnection GetModel()
        {
            return this._model;
        }

        private void SetModel(IConnection model)
        {
            this._model.ConnectionNumber = model.ConnectionNumber;
            this._model.Points = new List<Point>(model.Points);

            UpdatePathGeometry(_model.Points);
        }

        private void SinkCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                {
                    foreach (var item in args.NewItems.Cast<IConnectorViewModel>())
                    {
                        item.ConnectorPositionChanged += OnConnectorPositionChanged;
                        item.Connection = this;
                        UpdatePathGeometry();
                    }

                    break;
                }
                case NotifyCollectionChangedAction.Remove:
                {
                    foreach (var item in args.OldItems.Cast<IConnectorViewModel>())
                    {
                        item.ConnectorPositionChanged -= OnConnectorPositionChanged;
                        item.Connection = null;
                    }
                    if(SinkConnectors.Count == 0)
                    {
                        NeedDelete?.Invoke(this);
                    }
                    break;
                }
                case NotifyCollectionChangedAction.Reset:
                {
                    foreach (var item in args.OldItems.Cast<IConnectorViewModel>())
                    {
                        item.ConnectorPositionChanged -= OnConnectorPositionChanged;
                        item.Connection = null;
                    }
                    NeedDelete?.Invoke(this);
                    break;
                }
            }
        }

        private void OnConnectorPositionChanged(Point position)
        {
            UpdatePathGeometry();
        }
        
        private void UpdatePathGeometry()
        {
            if (this.SourceConnector == null || this.SinkConnectors.Count == 0)
                return;
            
            if (this.SinkConnectors.Count == 1)
            {
                var sourceInfo = new PathFinder.ConnectorInfo
                {
                    ConnectorPoint = this.SourceConnector.ConnectorPosition,
                    Orientation = this.SourceConnector.Orientation,
                    ConnectorParentX = this.SourceConnector.ParentViewModel.X,
                    ConnectorParentY = this.SourceConnector.ParentViewModel.Y
                };

                var sink = this.SinkConnectors[0];
                var sinkInfo = new PathFinder.ConnectorInfo
                {
                    ConnectorPoint = sink.ConnectorPosition,
                    Orientation = sink.Orientation,
                    ConnectorParentX = sink.ParentViewModel.X,
                    ConnectorParentY = sink.ParentViewModel.Y
                };

                _model.Points = PathFinder.GetConnectionLine(sourceInfo, sinkInfo);

                UpdatePathGeometry(_model.Points);
            }
        }

        private void UpdatePathGeometry(List<Point> points)
        {
            if (this.SourceConnector == null || this.SinkConnectors.Count == 0)
                return;

            if (points.Count > 1)
            {
                var pathFigure = new PathFigure { StartPoint = points[0] };
                for (int i = 1; i < points.Count; i++)
                {
                    var lineSegment = new LineSegment(points[i], true);
                    pathFigure.Segments.Add(lineSegment);
                }

                this.Path.Figures.Clear();
                this.Path.Figures.Add(pathFigure);
                RaisePropertyChanged(nameof(this.Path));
            }
            
        }

        public IConnectorViewModel GetNearConnector(IConnectorViewModel startConnector)
        {
            //TODO get near point in line segment
            return SinkConnectors.First();
        }
    }
}
