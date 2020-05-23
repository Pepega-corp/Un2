using System.Linq;
using System.Runtime.Remoting.Messaging;
using Unicon2.Fragments.Measuring.Commands;
using Unicon2.Fragments.Measuring.Infrastructure.Factories;
using Unicon2.Fragments.Measuring.Infrastructure.Model;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;
using Unicon2.Fragments.Measuring.Infrastructure.ViewModel;
using Unicon2.Fragments.Measuring.Infrastructure.ViewModel.Elements;
using Unicon2.Fragments.Measuring.Subscriptions;
using Unicon2.Fragments.Measuring.ViewModel;
using Unicon2.Fragments.Measuring.ViewModel.Presentation;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Unity.Commands;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Measuring.Factories
{
	public class MeasuringGroupViewModelFactory : IMeasuringGroupViewModelFactory
	{
		private readonly ITypesContainer _container;
		private readonly IMeasuringElementViewModelFactory _measuringElementViewModelFactory;
		private readonly MeasuringMemorySubscriptionFactory _measuringMemorySubscriptionFactory;

		public MeasuringGroupViewModelFactory(ITypesContainer container,
			IMeasuringElementViewModelFactory measuringElementViewModelFactory,
			MeasuringMemorySubscriptionFactory measuringMemorySubscriptionFactory)
		{
			_container = container;
			_measuringElementViewModelFactory = measuringElementViewModelFactory;
			_measuringMemorySubscriptionFactory = measuringMemorySubscriptionFactory;
		}


		public IMeasuringGroupViewModel CreateMeasuringGroupViewModel(IMeasuringGroup measuringGroup,
			MeasuringSubscriptionSet measuringSubscriptionSet,DeviceContext deviceContext)
		{
			MeasuringGroupViewModel measuringGroupViewModel =
				_container.Resolve<IMeasuringGroupViewModel>() as MeasuringGroupViewModel;

			measuringGroupViewModel.Header = measuringGroup.Name;
			measuringGroupViewModel.MeasuringElementViewModels.Clear();

			foreach (var groupOfElements in measuringGroup.PresentationSettings.GroupsOfElements)
			{
				measuringGroupViewModel.PresentationMeasuringElements.Add(
					new PresentationMeasuringElementViewModel(groupOfElements.PositioningInfo,
						new PresentationGroupViewModel(groupOfElements.Name)));
			}

			foreach (IMeasuringElement measuringElement in measuringGroup.MeasuringElements)
			{
				var elementViewModel =
					_measuringElementViewModelFactory.CreateMeasuringElementViewModel(measuringElement,
						measuringGroup.Name);
				var positioningInfo =
					measuringGroup.PresentationSettings.Elements.FirstOrDefault(info =>
						info.RelatedMeasuringElementId == measuringElement.Id);
				if (positioningInfo != null)
				{
					measuringGroupViewModel.PresentationMeasuringElements.Add(
						new PresentationMeasuringElementViewModel(positioningInfo.PositioningInfo, elementViewModel));
				}
				_measuringMemorySubscriptionFactory.AddSubscription(measuringSubscriptionSet, elementViewModel, measuringElement,measuringGroup.Name,deviceContext);

				measuringGroupViewModel.MeasuringElementViewModels.Add(elementViewModel);

			    if (measuringElement is IControlSignal controlSignal)
			    {
			        var vm = elementViewModel as IControlSignalViewModel;
                    vm.WriteValueCommand=new WriteDiscretCommand(deviceContext,controlSignal,vm);
			    }
			}

			return measuringGroupViewModel;
		}
	}
}