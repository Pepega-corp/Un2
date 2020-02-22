using System;
using System.Linq;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.TreeGrid;

namespace Unicon2.Fragments.Configuration.ViewModelMemoryMapping
{
    public class ValuesPullingConfigurationItemsVisitor : IConfigurationItemVisitor<Result>
    {
        private readonly IConfigurationMemory _configurationMemory;
        private readonly bool _isLocal;

        public ValuesPullingConfigurationItemsVisitor(IConfigurationMemory configurationMemory, bool isLocal)
        {
            _configurationMemory = configurationMemory;
            _isLocal = isLocal;
        }
        public Result VisitItemsGroup(IRuntimeItemGroupViewModel itemsGroup)
        {
            var result = Result.Create(true);
            foreach (var viewModel in itemsGroup.ChildStructItemViewModels.ToList())
            {
                if (viewModel is IRuntimeConfigurationItemViewModel runtimeConfigurationItemViewModel)
                {
                    result = Result.CreateMergeAnd(result, runtimeConfigurationItemViewModel.Accept(this));
                }
            }
            return result;
        }

        public Result VisitProperty(IRuntimePropertyViewModel property)
        {
            try
            {
                var propertyModel = property.Model as IProperty;
                var value = propertyModel?.UshortsFormatter.Format(MemoryAccessor.GetUshortsFromMemory(
                    _configurationMemory,
                    propertyModel.Address, propertyModel.NumberOfPoints, _isLocal));
                if (_isLocal)
                {
                    var memoryValue =
                        propertyModel?.UshortsFormatter.FormatBack(property.LocalValue.Model as IFormattedValue);
                    MemoryAccessor.GetUshortsInMemory(_configurationMemory,propertyModel.Address,memoryValue,_isLocal);
                }
                else
                {
                    var memoryValue =
                        propertyModel?.UshortsFormatter.FormatBack(property.DeviceValue.Model as IFormattedValue);
                    MemoryAccessor.GetUshortsInMemory(_configurationMemory, propertyModel.Address, memoryValue, _isLocal);
                }
                return Result.Create(true);
            }
            catch (Exception e)
            {
                return Result.Create(e);
            }
        }

        public Result VisitComplexProperty(IRuntimeComplexPropertyViewModel property)
        {
            throw new System.NotImplementedException();
        }

        public Result VisitMatrix(IRuntimeAppointableMatrixViewModel runtimeAppointableMatrixViewModel)
        {
            throw new System.NotImplementedException();
        }

        public Result VisitDependentProperty(IDependentPropertyViewModel dependentPropertyViewModel)
        {
            throw new System.NotImplementedException();
        }
    }
}