using Unicon2.Fragments.Configuration.Infrastructure.MemoryViewModelMapping;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;
using Unicon2.Infrastructure.Functional;
using Unicon2.Presentation.Infrastructure.TreeGrid;

namespace Unicon2.Fragments.Configuration.ViewModelMemoryMapping
{
    public class MemoryBusInitConfigurationItemsVisitor : IConfigurationItemVisitor<Result>
    {
        private readonly IMemoryBusDispatcher _memoryBusDispatcher;

        public MemoryBusInitConfigurationItemsVisitor(IMemoryBusDispatcher memoryBusDispatcher)
        {
            _memoryBusDispatcher = memoryBusDispatcher;
        }
        public Result VisitItemsGroup(IRuntimeItemGroupViewModel itemsGroup)
        {
            var res = Result.Create(true);
            foreach (var itemsGroupChildStructItemViewModel in itemsGroup.ChildStructItemViewModels)
            {
                if (itemsGroupChildStructItemViewModel is IRuntimeConfigurationItemViewModel runtimeItemsGroupChildStructItemViewModel)
                    res = Result.CreateMergeAnd(res, runtimeItemsGroupChildStructItemViewModel.Accept(this));
            }
            return res;
        }

        public Result VisitProperty(IRuntimePropertyViewModel property)
        {
            return _memoryBusDispatcher.AddSubscription(property);
        }

        public Result VisitComplexProperty(IRuntimeComplexPropertyViewModel property)
        {
            throw new System.NotImplementedException();
        }

        public Result VisitMatrix(IRuntimeAppointableMatrixViewModel appointableMatrixViewModel)
        {
            throw new System.NotImplementedException();
        }

        public Result VisitDependentProperty(IDependentPropertyViewModel dependentPropertyViewModel)
        {
            throw new System.NotImplementedException();
        }
    }
}