using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Matrix;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Properties;

namespace Unicon2.Fragments.Configuration.Infrastructure.ViewModel
{
    public interface IConfigurationItemVisitor<TOutPut>
    {
        TOutPut VisitItemsGroup(IItemsGroup itemsGroup);
        TOutPut VisitProperty(IProperty property);
        TOutPut VisitComplexProperty(IComplexProperty property);
        TOutPut VisitMatrix(IAppointableMatrix appointableMatrixViewModel);
        TOutPut VisitDependentProperty(IDependentProperty dependentPropertyViewModel);
        TOutPut VisitSubProperty(ISubProperty dependentPropertyViewModel);
    }   
}