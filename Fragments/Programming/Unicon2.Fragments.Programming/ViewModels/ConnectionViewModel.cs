using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme;
using Unicon2.Fragments.Programming.Model;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Programming.ViewModels
{
    public class ConnectionViewModel : ViewModelBase, ISchemeElementViewModel
    {
        private readonly Connection _model;
        private Point _labelPosition;
        private ushort _currentValue;
        private bool _gotValue;
        private bool _debugMode;
        private bool _isSelected;
        private double _value;
        private double _x;
        private double _y;

        public event Action<ConnectionViewModel> NeedDelete;

        public ConnectionViewModel(Connection model, List<ConnectionSegmentViewModel> segments)
        {
            this._model = model;
            this._currentValue = 0;
            this._gotValue = false;

            Segments.AddCollection(segments);
            Segments.CollectionChanged += OnCollectionChanged;
        }

        public ObservableCollection<ConnectionSegmentViewModel> Segments { get; } =
            new ObservableCollection<ConnectionSegmentViewModel>();

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

        public ConnectionSegmentViewModel SourceSegment
        {
            get => Segments.FirstOrDefault();
        }

        public ObservableCollection<ConnectionSegmentViewModel> SinkSegments { get; } =
            new ObservableCollection<ConnectionSegmentViewModel>();

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
                foreach (var segmentViewModel in Segments)
                {
                    segmentViewModel.DebugMode = value;
                }
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
                this._gotValue = value;
                foreach (var segmentViewModel in Segments)
                {
                    segmentViewModel.GotValue = value;
                }
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

        public Connection Model
        {
            get { return this.GetModel(); }
            set { this.SetModel(value); }
        }

        private Connection GetModel()
        {
            return this._model;
        }

        private void SetModel(Connection model)
        {
            this._model.ConnectionNumber = model.ConnectionNumber;
            this._model.Segments = new List<ConnectionSegment>(model.Segments);
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(SourceSegment));
        }

        // private void OnConnectorPositionChanged(Point position)
        // {
        //     UpdatePathGeometry();
        // }

        // private void UpdatePathGeometry()
        // {
        //     if (this.SourceConnector == null || this.SinkConnectors.Count == 0)
        //         return;
        //
        //     if (this.SinkConnectors.Count == 1)
        //     {
        //         var sourceInfo = new PathFinder.ConnectorInfo
        //         {
        //             ConnectorPoint = this.SourceConnector.ConnectorPosition,
        //             Orientation = this.SourceConnector.Orientation,
        //             ConnectorParentX = this.SourceConnector.ParentViewModel.X,
        //             ConnectorParentY = this.SourceConnector.ParentViewModel.Y
        //         };
        //
        //         var sink = this.SinkConnectors[0];
        //         var sinkInfo = new PathFinder.ConnectorInfo
        //         {
        //             ConnectorPoint = sink.ConnectorPosition,
        //             Orientation = sink.Orientation,
        //             ConnectorParentX = sink.ParentViewModel.X,
        //             ConnectorParentY = sink.ParentViewModel.Y
        //         };
        //
        //         var points = PathFinder.GetConnectionLine(sourceInfo, sinkInfo);
        //
        //         UpdatePathGeometry(points);
        //     }
        // }

        // private void UpdatePathGeometry(List<Point> points)
        // {
        //     if (this.SourceConnector == null || this.SinkConnectors.Count == 0)
        //         return;
        //
        //     if (points.Count > 1)
        //     {
        //         var pathFigure = new PathFigure { StartPoint = points[0] };
        //         for (int i = 1; i < points.Count; i++)
        //         {
        //             var lineSegment = new LineSegment(points[i], true);
        //             pathFigure.Segments.Add(lineSegment);
        //         }
        //
        //         this.Path.Figures.Clear();
        //         this.Path.Figures.Add(pathFigure);
        //         RaisePropertyChanged(nameof(this.Path));
        //     }
        // }

        // public ConnectorViewModel GetNearConnector(ConnectorViewModel startConnector)
        // {
        //     //TODO get near point in line segment
        //     return SinkConnectors.First();
        // }
    }
}