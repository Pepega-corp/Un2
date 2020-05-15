using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Measuring.Infrastructure.ViewModel.Dependencies;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Measuring.Editor.ViewModel.Dependencies
{
	public class BoolToAddressDependencyViewModel : ViewModelBase, IDependencyViewModel
	{
		private string _relatedResourceName;
		private ushort _resultingAddressIfTrue;
		private ushort _resultingAddressIfFalse;

		public string RelatedResourceName
		{
			get => _relatedResourceName;
			set
			{

				_relatedResourceName = value;
				RaisePropertyChanged();
			}
		}

		public ushort ResultingAddressIfTrue
		{
			get => _resultingAddressIfTrue;
			set
			{
				_resultingAddressIfTrue = value;
				RaisePropertyChanged();
			}
		}

		public ushort ResultingAddressIfFalse
		{
			get => _resultingAddressIfFalse;
			set
			{
				_resultingAddressIfFalse = value;
				RaisePropertyChanged();
			}
		}
	}
}