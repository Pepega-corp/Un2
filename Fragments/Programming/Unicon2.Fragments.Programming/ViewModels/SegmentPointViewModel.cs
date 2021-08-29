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

        public Point Position
        {
            get => _model.Position;
            set
            {
                _model.Position = value;
                RaisePropertyChanged();
            }
        }
    }
}