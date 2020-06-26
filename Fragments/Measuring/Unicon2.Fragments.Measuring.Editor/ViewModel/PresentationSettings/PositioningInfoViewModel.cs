using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Measuring.Editor.Subscriptions;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Measuring.Editor.ViewModel.PresentationSettings
{
	public class PositioningInfoViewModel:ViewModelBase, IDisposable
	{
		private PresentationPositionChangedSubscription _presentationPositionChangedSubscription;
		private int _offsetLeft;
		private int _offsetTop;
		private int _sizeWidth;
		private int _sizeHeight;

		public PositioningInfoViewModel(int initialOffsetLeft, int initialOffsetTop, int initialSizeWidth,
			int initialsSizeHeight,
			PresentationPositionChangedSubscription presentationPositionChangedSubscription = null)
		{
			_presentationPositionChangedSubscription = presentationPositionChangedSubscription;
			_offsetLeft = initialOffsetLeft;
			_offsetTop = initialOffsetTop;
			_sizeWidth = initialSizeWidth;
			_sizeHeight = initialsSizeHeight;
		}

		public int OffsetLeft
		{
			get => _offsetLeft;
			set
			{
				if (value + SizeWidth > 1200) return;
				_presentationPositionChangedSubscription?.PositionChanged(value-_offsetLeft,OffsetKind.Left,this);
				_offsetLeft = value;
				RaisePropertyChanged();
			}
		}

		public int OffsetTop
		{
			get => _offsetTop;
			set
			{
				if (value + SizeHeight > 800) return;
				_presentationPositionChangedSubscription?.PositionChanged(value - _offsetTop, OffsetKind.Top, this);
				_offsetTop = value;
				RaisePropertyChanged();
			}
		}

		public int SizeWidth
		{
			get => _sizeWidth;
			set
			{
				if (value + OffsetLeft > 1200) return;
				_sizeWidth = value;
				RaisePropertyChanged();
			}
		}

		public int SizeHeight
		{
			get => _sizeHeight;
			set
			{
				if (value + OffsetTop > 800) return;
				_sizeHeight = value;
				RaisePropertyChanged();
			}
		}

		public void Dispose()
		{
			_presentationPositionChangedSubscription = null;
		}
	}
}
