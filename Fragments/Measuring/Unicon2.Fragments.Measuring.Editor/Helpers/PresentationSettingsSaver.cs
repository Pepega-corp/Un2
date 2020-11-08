using System.Linq;
using Unicon2.Fragments.Measuring.Editor.ViewModel;
using Unicon2.Fragments.Measuring.Editor.ViewModel.PresentationSettings;
using Unicon2.Fragments.Measuring.Infrastructure.Model.PresentationSettings;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.Measuring.Editor.Helpers
{
	public class PresentationSettingsSaver
	{
		public IPresentationSettings SavePresentationSettings(
			PresentationSettingsViewModel presentationSettingsViewModel)
		{
			var res = StaticContainer.Container.Resolve<IPresentationSettings>();
			res.GroupsOfElements = presentationSettingsViewModel.PresentationElementViewModels
				.Where(model => model.TemplatedViewModelToShowOnCanvas is PresentationGroupViewModel).Select(model =>
				{
					var group = StaticContainer.Container.Resolve<IMeasuringPresentationGroup>();
					group.PositioningInfo = SavePositioningInfo(model.PositioningInfoViewModel);
					SaveTemplatedItemOnCanvas(group, model.TemplatedViewModelToShowOnCanvas as PresentationGroupViewModel);
					
					return group;
				}).ToList();
			res.Elements = presentationSettingsViewModel.PresentationElementViewModels
				.Where(model => !(model.TemplatedViewModelToShowOnCanvas is PresentationGroupViewModel)).Select(model =>
				{
					var presentationInfo = StaticContainer.Container.Resolve<IMeasuringElementPresentationInfo>();
					presentationInfo.PositioningInfo = SavePositioningInfo(model.PositioningInfoViewModel);
					presentationInfo.RelatedMeasuringElementId =
						(model.TemplatedViewModelToShowOnCanvas as IUniqueId).Id;
					return presentationInfo;
				}).ToList();
			return res;
		}

		private void SaveTemplatedItemOnCanvas(IMeasuringPresentationGroup measuringPresentationGroup, PresentationGroupViewModel presentationGroupViewModel)
		{
			measuringPresentationGroup.Name = presentationGroupViewModel.Header;
		}
		private IPositioningInfo SavePositioningInfo(
			PositioningInfoViewModel positioningInfoViewModel)
		{
			var res = StaticContainer.Container.Resolve<IPositioningInfo>();
			res.SizeHeight = positioningInfoViewModel.SizeHeight;
			res.OffsetLeft = positioningInfoViewModel.OffsetLeft;
			res.OffsetTop = positioningInfoViewModel.OffsetTop;
			res.SizeWidth = positioningInfoViewModel.SizeWidth;

			return res;
		}
	}
}
