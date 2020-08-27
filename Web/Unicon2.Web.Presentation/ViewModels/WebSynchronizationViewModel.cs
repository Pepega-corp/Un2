using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Web.Presentation.ViewModels
{
	public class WebSynchronizationViewModel : ViewModelBase
	{
		public WebSynchronizationViewModel()
		{
			ServerDefinitions = new ObservableCollection<DefinitionInfoViewModel>();
			LocalDefinitions = new ObservableCollection<DefinitionInfoViewModel>();
			//RefreshAll = new RelayCommand();
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

		public ICommand RefreshAll { get; }

		public ObservableCollection<DefinitionInfoViewModel> ServerDefinitions { get; }
		public ObservableCollection<DefinitionInfoViewModel> LocalDefinitions { get; }

	}
}