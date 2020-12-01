using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Measuring.Editor.ViewModel.PresentationSettings
{
	public class PresentationElementViewModel : ViewModelBase
	{
		private bool _isSelected;

		public PresentationElementViewModel(object templatedViewModelToShowOnCanvas)
		{
			TemplatedViewModelToShowOnCanvas = templatedViewModelToShowOnCanvas;
			PositioningInfoViewModel = new PositioningInfoViewModel(0,0,100,35);
		}

		public object TemplatedViewModelToShowOnCanvas { get; }
		public PositioningInfoViewModel PositioningInfoViewModel { get; set; }

		public bool IsSelected
		{
			get => _isSelected;
			set
			{
				_isSelected = value;
				RaisePropertyChanged();
			}
		}
	}
}