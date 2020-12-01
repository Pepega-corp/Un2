using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Measuring.Editor.ViewModel.PresentationSettings
{
	public class PresentationGroupViewModel:ViewModelBase
	{
		private string _header;
		private bool _isMoveWithChildren;

		public string Header
		{
			get => _header;
			set
			{
				_header = value;
				RaisePropertyChanged();
			}
		}

		public bool IsMoveWithChildren
		{
			get => _isMoveWithChildren;
			set
			{
				_isMoveWithChildren = value;
				RaisePropertyChanged();
			}
		}
	}
}
