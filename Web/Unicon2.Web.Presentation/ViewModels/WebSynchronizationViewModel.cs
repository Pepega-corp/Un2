using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Web.Presentation.ViewModels
{
	public class WebSynchronizationViewModel: ViewModelBase
	{
		public WebSynchronizationViewModel()
		{
			
		}





		private bool _canConnectToServer;

		public bool CanConnectToServer
		{
			get => _canConnectToServer;
			set
			{
				_canConnectToServer = value;
				RaisePropertyChanged();
			}
		}
	}
}
