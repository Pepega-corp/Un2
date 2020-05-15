using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
