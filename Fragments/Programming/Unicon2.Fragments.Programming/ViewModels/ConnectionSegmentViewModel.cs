using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme;
using Unicon2.Fragments.Programming.Model;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Programming.ViewModels
{
    public class ConnectionSegmentViewModel : ViewModelBase, ISelectable
    {
        private ConnectionSegment _model;
        private SegmentPointViewModel _point1;
        private SegmentPointViewModel _point2;
        private bool _gotValue;
        private bool _debugMode;
        private bool _isSelected;
        
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
        public SegmentPointViewModel Point1 => _point1;
        public SegmentPointViewModel Point2 => _point2;

        public ConnectionSegmentViewModel(ConnectionSegment model)
        {
            _model = model;
            _point1 = new SegmentPointViewModel(_model.Point1);
            _point2 = new SegmentPointViewModel(_model.Point2);
        }
    }
}