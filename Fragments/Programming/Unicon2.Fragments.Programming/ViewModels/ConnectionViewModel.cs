using System;
using System.Collections.Generic;
using System.Linq;
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
        private PathGeometry _path;
        private Point _labelPosition;
        private DoubleCollection _strokeDashArray;
        private ushort _currentValue;
        private bool _gotValue;
        private bool _isSelected;
        private double _value;
        private double _x;
        private double _y;
        private IConnection _model;

        #endregion

        #region Constructors

        public ConnectionViewModel()
        {
            this._currentValue = 0;
            this._gotValue = false;
        }

        public ConnectionViewModel(IConnection model, PathGeometry path, IConnectorViewModel connector1, IConnectorViewModel connector2) : this()
        {
            this._model = model;
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

        public IConnector Source
        {
            get { return this._model.Connectors.First(c=>c.Orientation == ConnectorOrientation.RIGHT); }
            set
            {
                if(value.Orientation != ConnectorOrientation.RIGHT)
                    return;

                var source = this._model.Connectors.First(c=>c.Orientation == ConnectorOrientation.RIGHT);
                this._model.RemoveConnector(source);
                this._model.AddConnector(value);
                this.UpdatePathGeometry();
                RaisePropertyChanged();
            }
        }

        public IConnector Sink
        {
            get { return this._model.Connectors.First(c => c.Orientation == ConnectorOrientation.LEFT); }
            set
            {
                if (value.Orientation != ConnectorOrientation.LEFT)
                    return;

                var sink = this._model.Connectors.First(c => c.Orientation == ConnectorOrientation.LEFT);
                this._model.RemoveConnector(sink);
                this._model.AddConnector(value);
                this.UpdatePathGeometry();
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

        public string StrongName => ProgrammingKeys.CONNECTION +
                                    ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;
        public IConnection Model
        {
            get { return this._model; }
            set { this._model = value; }
        }
    }
}
