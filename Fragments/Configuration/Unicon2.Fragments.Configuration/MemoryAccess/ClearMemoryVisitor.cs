using System.Collections.Generic;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Fragments.Configuration.ViewModelMemoryMapping;
using Unicon2.Infrastructure.Functional;

namespace Unicon2.Fragments.Configuration.MemoryAccess
{
    public class ClearMemoryVisitor : IConfigurationItemVisitor<Result>
    {
        private readonly Dictionary<ushort, ushort> _memorySet;

        public ClearMemoryVisitor(Dictionary<ushort, ushort> memorySet)
        {
            _memorySet = memorySet;
        }
        public Result VisitItemsGroup(IItemsGroup itemsGroup)
        {
            foreach (var itemGroup in itemsGroup.ConfigurationItemList)
            {
                itemGroup.Accept(this);
            }

            return Result.Create(true);
        }

        public Result VisitProperty(IProperty property)
        {
            MemoryAccessor.ClearRange(property.Address,property.NumberOfPoints,_memorySet);
            return Result.Create(true);

        }

        public Result VisitComplexProperty(IComplexProperty property)
        {
            return VisitProperty(property);
        }

        public Result VisitMatrix(IAppointableMatrix appointableMatrixViewModel)
        {
            throw new System.NotImplementedException();
        }

        public Result VisitSubProperty(ISubProperty subProperty)
        {
            throw new System.NotImplementedException();
        }
    }
}