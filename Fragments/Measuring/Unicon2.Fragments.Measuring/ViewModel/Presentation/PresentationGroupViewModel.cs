using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Measuring.ViewModel.Presentation
{
	public class PresentationGroupViewModel:ViewModelBase
	{
		public PresentationGroupViewModel(string header)
		{
			Header = header;
		}

		public string Header { get; }
	}
}
