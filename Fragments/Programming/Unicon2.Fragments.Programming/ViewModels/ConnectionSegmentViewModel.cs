using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Programming.ViewModels
{
    public class ConnectionSegmentViewModel : ViewModelBase, ISelectable
    {
        private SegmentPointViewModel _point1;
        private SegmentPointViewModel _point2;
        private bool _gotValue;
        private bool _debugMode;
        private bool _isSelected;

        public SegmentPointViewModel Point1 => _point1;
        public SegmentPointViewModel Point2 => _point2;
        
        public bool IsSelected
        {
            get => this._isSelected;
            set
            {
                this._isSelected = value;
                RaisePropertyChanged();
            }
        }
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

        public double X1
        {
            get => _point1.X;
            set
            {
                _point1.X = value;
                RaisePropertyChanged();
            }
        }
        
        public double Y1
        {
            get => _point1.Y;
            set
            {
                _point1.Y = value;
                RaisePropertyChanged();
            }
        }
        
        public double X2
        {
            get => _point2.X;
            set
            {
                _point2.X = value;
                RaisePropertyChanged();
            }
        }
        
        public double Y2
        {
            get => _point2.Y;
            set
            {
                _point2.Y = value;
                RaisePropertyChanged();
            }
        }

        public bool IsSink => _point2 is ConnectorViewModel;
        public bool IsSource => _point1 is ConnectorViewModel;

        public ConnectionSegmentViewModel(SegmentPointViewModel point1, SegmentPointViewModel point2)
        {
            _point1 = point1;
            _point2 = point2;
        }

        public void UpdatePositionX(SegmentPointViewModel point)
        {
            if (point == _point1)
            {
                RaisePropertyChanged(nameof(X1));
            }

            if (point == _point2)
            {
                RaisePropertyChanged(nameof(X2));
            }
        }

        public void UpdatePositionY(SegmentPointViewModel point)
        {
            if (point == _point1)
            {
                RaisePropertyChanged(nameof(Y1));
            }

            if (point == _point2)
            {
                RaisePropertyChanged(nameof(Y2));
            }
        }
    }
}