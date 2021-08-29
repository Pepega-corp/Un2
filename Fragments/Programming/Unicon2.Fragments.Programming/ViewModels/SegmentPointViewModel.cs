using System.Windows;
using Unicon2.Fragments.Programming.Model;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Programming.ViewModels
{
    public class SegmentPointViewModel : ViewModelBase
    {
        private readonly SegmentPoint _model;

        public SegmentPointViewModel(SegmentPoint model)
        {
            _model = model;
        }

        public double X
        {
            get => _model.Position.X;
            set
            {
                var p = _model.Position;
                p.X = value;
                _model.Position = p;
                RaisePropertyChanged();
            }
        }

        public double Y
        {
            get => _model.Position.Y;
            set
            {
                var p = _model.Position;
                p.Y = value;
                _model.Position = p;
                RaisePropertyChanged();
            }
        }
    }
}