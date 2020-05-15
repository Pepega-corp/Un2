using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Measuring.Editor.ViewModel;
using Unicon2.Fragments.Measuring.Editor.ViewModel.PresentationSettings;

namespace Unicon2.Fragments.Measuring.Editor.Subscriptions
{
	public class PresentationPositionChangedSubscription
	{
		private readonly PresentationSettingsViewModel _presentationSettingsViewModel;
		private readonly PresentationGroupViewModel _presentationGroupViewModel;

		public PresentationPositionChangedSubscription(PresentationGroupViewModel presentationGroupViewModel, PresentationSettingsViewModel presentationSettingsViewModel)
		{
			_presentationGroupViewModel = presentationGroupViewModel;
			_presentationSettingsViewModel = presentationSettingsViewModel;
		}

		public void PositionChanged(int offset, OffsetKind offsetKind,PositioningInfoViewModel basePositioning)
		{
			if (_presentationGroupViewModel.IsMoveWithChildren)
			{
				foreach (var presentationElementViewModel in _presentationSettingsViewModel.PresentationElementViewModels)
				{
					if (presentationElementViewModel.IsSelected || !IsElementInAnotherElement(basePositioning,
						    presentationElementViewModel.PositioningInfoViewModel)) continue;

					if (offsetKind == OffsetKind.Left)
						presentationElementViewModel.PositioningInfoViewModel.OffsetLeft += offset;
					else if (offsetKind == OffsetKind.Top) presentationElementViewModel.PositioningInfoViewModel.OffsetTop += offset;
				}



			}
		}


		private bool IsElementInAnotherElement(PositioningInfoViewModel basePositioning,PositioningInfoViewModel elementPositioningToCheck)
		{
			var isHorizontalMatch = elementPositioningToCheck.OffsetLeft > basePositioning.OffsetLeft &&
			                        elementPositioningToCheck.OffsetLeft <
			                        basePositioning.OffsetLeft + basePositioning.SizeWidth;
			var isVerticalMatch = elementPositioningToCheck.OffsetTop > basePositioning.OffsetTop &&
			                        elementPositioningToCheck.OffsetTop <
			                        basePositioning.OffsetTop + basePositioning.SizeHeight;
			return isVerticalMatch && isHorizontalMatch;
		}
	}




	public enum OffsetKind
	{
		Left,Top
	}
}
