using Unicon2.Formatting.Editor.ViewModels;
using Unicon2.Formatting.Editor.Visitors;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Formatting.Editor.Services
{
    public class SaveFormatterService : ISaveFormatterService
    {
        private readonly ITypesContainer _container;

        public SaveFormatterService(ITypesContainer container)
        {
            _container = container;
        }

        public IUshortsFormatter CreateUshortsParametersFormatter(IFormatterParametersViewModel formatterParametersViewModel)
        {
	        if (formatterParametersViewModel == null)
	        {
		        return null;
	        }
            if (_container.Resolve<ISharedResourcesGlobalViewModel>()
                .CheckDeviceSharedResourcesContainsViewModel(formatterParametersViewModel.Name))
            {
                return _container.Resolve<ISharedResourcesGlobalViewModel>()
                    .GetResourceByName(formatterParametersViewModel.Name) as IUshortsFormatter;
            }

            return CreateUshortsParametersFormatter(formatterParametersViewModel
                .RelatedUshortsFormatterViewModel);
        }

        public IUshortsFormatter CreateUshortsParametersFormatter(IUshortsFormatterViewModel ushortsFormatterViewModel)
        {
	        if (ushortsFormatterViewModel is UshortsFormatterViewModelBase ushortsFormatterViewModelBase)
	        {
		        return ushortsFormatterViewModelBase.Accept(new SaveFormatterViewModelVisitor());
	        }

	        return null;
        }
    }
}