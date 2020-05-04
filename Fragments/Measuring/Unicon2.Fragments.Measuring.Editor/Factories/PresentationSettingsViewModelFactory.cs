using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel;
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
					res.PresentationElementViewModels.Add(new PresentationElementViewModel(
						new PresentationGroupViewModel()
						{
							Header = groupsOfElement.Name
						})
					{
						PositioningInfoViewModel = InitializePositioningInfo(groupsOfElement.PositioningInfo)
					});
				}

			return res;
		}

		private Dictionary<Guid, PositioningInfoViewModel> CreatePositioningInfoViewModels(
			IPresentationSettings presentationSettings)
		{
			var res = new Dictionary<Guid, PositioningInfoViewModel>();

			if (presentationSettings.Elements != null)
				foreach (var element in presentationSettings.Elements)
				{
					res.Add(element.RelatedMeasuringElementId, InitializePositioningInfo(element.PositioningInfo));
				}

			return res;
		}

		private PositioningInfoViewModel InitializePositioningInfo(IPositioningInfo positioningInfo)
		{

			return new PositioningInfoViewModel()
			{
				OffsetLeft = positioningInfo.OffsetLeft,
				OffsetTop = positioningInfo.OffsetTop,
				SizeHeight = positioningInfo.SizeHeight,
				SizeWidth = positioningInfo.SizeWidth
			};
		}
	}
}