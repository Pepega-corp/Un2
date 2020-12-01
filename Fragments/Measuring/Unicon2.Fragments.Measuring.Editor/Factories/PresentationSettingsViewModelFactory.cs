using System;
using System.Collections.Generic;
using Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel;
using Unicon2.Fragments.Measuring.Editor.Subscriptions;
using Unicon2.Fragments.Measuring.Editor.ViewModel;
using Unicon2.Fragments.Measuring.Editor.ViewModel.PresentationSettings;
using Unicon2.Fragments.Measuring.Infrastructure.Model;
using Unicon2.Fragments.Measuring.Infrastructure.Model.PresentationSettings;

namespace Unicon2.Fragments.Measuring.Editor.Factories
{
	public class PresentationSettingsViewModelFactory
	{
		public PresentationSettingsViewModel CreatePresentationSettingsViewModel(IMeasuringGroup measuringGroup,
			IMeasuringGroupEditorViewModel measuringGroupEditorViewModel)
		{
			var res = new PresentationSettingsViewModel(measuringGroupEditorViewModel,
				CreatePositioningInfoViewModels(measuringGroup.PresentationSettings));
			if (measuringGroup?.PresentationSettings != null)
				foreach (var groupsOfElement in measuringGroup?.PresentationSettings?.GroupsOfElements)
				{
					var presentationGroupViewModel = new PresentationGroupViewModel()
					{
						Header = groupsOfElement.Name
					};
					res.PresentationElementViewModels.Add(new PresentationElementViewModel(
						presentationGroupViewModel)
					{
						PositioningInfoViewModel = InitializePositioningInfo(groupsOfElement.PositioningInfo,
							new PresentationPositionChangedSubscription(presentationGroupViewModel, res))
					});
				}

			return res;
		}

		private Dictionary<Guid, PositioningInfoViewModel> CreatePositioningInfoViewModels(
			IPresentationSettings presentationSettings)
		{
			var res = new Dictionary<Guid, PositioningInfoViewModel>();

			if (presentationSettings?.Elements != null)
				foreach (var element in presentationSettings.Elements)
				{
					res.Add(element.RelatedMeasuringElementId, InitializePositioningInfo(element.PositioningInfo));
				}

			return res;
		}

		private PositioningInfoViewModel InitializePositioningInfo(IPositioningInfo positioningInfo,
			PresentationPositionChangedSubscription presentationPositionChangedSubscription = null)
		{

			return new PositioningInfoViewModel(positioningInfo.OffsetLeft, positioningInfo.OffsetTop,
				positioningInfo.SizeWidth, positioningInfo.SizeHeight, presentationPositionChangedSubscription);
		}
	}
}