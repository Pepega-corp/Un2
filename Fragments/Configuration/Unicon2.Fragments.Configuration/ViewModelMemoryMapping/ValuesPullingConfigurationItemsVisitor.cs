using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;
using Unicon2.Infrastructure.Functional;
using Unicon2.Presentation.Infrastructure.TreeGrid;

namespace Unicon2.Fragments.Configuration.ViewModelMemoryMapping
{
    public class ValuesPullingConfigurationItemsVisitor : IConfigurationItemVisitor<Result>
    {
        public ValuesPullingConfigurationItemsVisitor()
        {
            
        }
        public Result VisitItemsGroup(IRuntimeItemGroupViewModel itemsGroup)
        {
            throw new System.NotImplementedException();
        }

        public Result VisitProperty(IRuntimePropertyViewModel property)
        {
            throw new System.NotImplementedException();
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