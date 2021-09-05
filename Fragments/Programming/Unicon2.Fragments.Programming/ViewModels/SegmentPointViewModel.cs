using System.Windows;
using Unicon2.Fragments.Programming.Model;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Programming.ViewModels
{
    public class SegmentPointViewModel : ViewModelBase
    {
        private readonly SegmentPoint _model;
        private ConnectionSegmentViewModel _segmentViewModel;

        public SegmentPointViewModel(SegmentPoint model)
        {
            _model = model;
        }

        public SegmentPointViewModel(SegmentPoint model, ConnectionSegmentViewModel segmentViewModel) : this(model)
        {
            UpdateSegment(segmentViewModel);
        }

        /// <summary>
        /// Точка расположения коннектора по Х в DesignerCanvas
        /// </summary>
        public double X
        {
            get => _model.Position.X;
            set
            {
                var p = _model.Position;
                p.X = value;
                _model.Position = p;
                RaisePropertyChanged();
                _segmentViewModel?.UpdatePositionX(this);
            }
        }

        /// <summary>
        /// Точка расположения коннектора по Y в DesignerCanvas
        /// </summary>
        public double Y
        {
            get => _model.Position.Y;
            set
            {
                var p = _model.Position;
                p.Y = value;
                _model.Position = p;
                RaisePropertyChanged();
                _segmentViewModel?.UpdatePositionY(this);
            }
        }
        
        public void UpdateSegment(ConnectionSegmentViewModel segmentViewModel)
        {
            _segmentViewModel = segmentViewModel;
        }
    }
}