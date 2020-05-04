using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Measuring.Editor.ViewModel.PresentationSettings
{
	public class PresentationElementViewModel : ViewModelBase
	{
		private bool _isSelected;

		public PresentationElementViewModel(object templatedViewModelToShowOnCanvas)
		{
			TemplatedViewModelToShowOnCanvas = templatedViewModelToShowOnCanvas;
			PositioningInfoViewModel = new PositioningInfoViewModel();
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