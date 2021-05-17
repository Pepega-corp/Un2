using System;
using Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel.Elements;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Measuring.Editor.ViewModel.PresentationSettings
{
	public class PresentationElementViewModel : ViewModelBase
	{
		private bool _isSelected;

		public PresentationElementViewModel(object templatedViewModelToShowOnCanvas, Guid? relatedIdOfMeasuringElement=null)
		{
			TemplatedViewModelToShowOnCanvas = templatedViewModelToShowOnCanvas;
            RelatedIdOfMeasuringElement = relatedIdOfMeasuringElement;
            PositioningInfoViewModel = new PositioningInfoViewModel(0,0,100,35);
		}

		public object TemplatedViewModelToShowOnCanvas { get; }
		public PositioningInfoViewModel PositioningInfoViewModel { get; set; }

		public Guid? RelatedIdOfMeasuringElement { get; }
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