using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Measuring.Editor.ViewModel.PresentationSettings
{
	public class PositioningInfoViewModel:ViewModelBase
	{
		private int _offsetLeft;
		private int _offsetTop;
		private int _sizeWidth;
		private int _sizeHeight;


		public int OffsetLeft
		{
			get => _offsetLeft;
			set
			{
				_offsetLeft = value;
				RaisePropertyChanged();
			}
		}

		public int OffsetTop
		{
			get => _offsetTop;
			set
			{
				_offsetTop = value;
				RaisePropertyChanged();
			}
		}

		public int SizeWidth
		{
			get => _sizeWidth;
			set
			{
				_sizeWidth = value;
				RaisePropertyChanged();
			}
		}

		public int SizeHeight
		{
			get => _sizeHeight;
			set
			{
				_sizeHeight = value;
				RaisePropertyChanged();
			}
		}
	}
}
