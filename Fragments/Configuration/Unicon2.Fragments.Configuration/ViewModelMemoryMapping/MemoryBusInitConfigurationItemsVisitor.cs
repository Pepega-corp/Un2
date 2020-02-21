using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;
using Unicon2.Infrastructure.Functional;
using Unicon2.Presentation.Infrastructure.TreeGrid;

namespace Unicon2.Fragments.Configuration.ViewModelMemoryMapping
{
    public class MemoryBusInitConfigurationItemsVisitor : IConfigurationItemVisitor<Result>
    {
        private readonly MemoryBusDispatcher<Result> _memoryBusDispatcher;

        public MemoryBusInitConfigurationItemsVisitor(MemoryBusDispatcher<Result> memoryBusDispatcher)
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
            _memoryBusDispatcher.AddSubscription(property);
            return Result.Create(true);
        }

        public Result VisitComplexProperty(IComplexPropertyViewModel property)
        {
            throw new System.NotImplementedException();
        }

        public Result VisitMatrix(IAppointableMatrixViewModel appointableMatrixViewModel)
        {
            throw new System.NotImplementedException();
        }

        public Result VisitDependentProperty(IDependentPropertyViewModel dependentPropertyViewModel)
        {
            throw new System.NotImplementedException();
        }
    }
}