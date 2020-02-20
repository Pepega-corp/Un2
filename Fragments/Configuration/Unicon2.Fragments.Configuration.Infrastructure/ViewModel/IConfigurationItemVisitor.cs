using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;

namespace Unicon2.Presentation.Infrastructure.TreeGrid
{
    public interface IConfigurationItemVisitor<T>
    {
        T VisitItemsGroup(IRuntimeItemGroupViewModel itemsGroup);
        T VisitProperty(IRuntimePropertyViewModel property);
        T VisitComplexProperty(IComplexPropertyViewModel property);
        T VisitMatrix(IAppointableMatrixViewModel appointableMatrixViewModel);
        T VisitDependentProperty(IDependentPropertyViewModel dependentPropertyViewModel);
    }
}