using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Matrix;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Properties;

namespace Unicon2.Fragments.Configuration.Infrastructure.ViewModel
{
    public interface IConfigurationItemVisitor<T>
    {
        T VisitItemsGroup(IItemsGroup itemsGroup);
        T VisitProperty(IProperty property);
        T VisitComplexProperty(IComplexProperty property);
        T VisitMatrix(IAppointableMatrix appointableMatrixViewModel);
        T VisitDependentProperty(IDependentProperty dependentPropertyViewModel);
        T VisitSubProperty(ISubProperty dependentPropertyViewModel);
    }     

}