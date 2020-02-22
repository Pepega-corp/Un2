using Unicon2.Fragments.Configuration.Infrastructure.MemoryViewModelMapping;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;
using Unicon2.Presentation.Infrastructure.TreeGrid;

namespace Unicon2.Fragments.Configuration.Infrastructure.Factories
{
    public interface IRuntimeConfigurationItemViewModelFactory:IConfigurationItemVisitor<IRuntimeConfigurationItemViewModel>
    {
        void Initialize(IMemoryBusDispatcher memoryBusDispatcher);
    }
}