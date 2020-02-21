using System;
using System.Linq;
using Unicon2.Fragments.Configuration.Infrastructure.Factories;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Functional;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.TreeGrid;

namespace Unicon2.Fragments.Configuration.ViewModelMemoryMapping
{
    public class ValuesSeedingConfigurationItemsVisitor : IConfigurationItemVisitor<Result>
    {
        private readonly IPropertyValueViewModelFactory _valueViewModelFactory;
        private readonly IConfigurationMemory _configurationMemory;
        private readonly bool _isLocal;

        public ValuesSeedingConfigurationItemsVisitor(IPropertyValueViewModelFactory valueViewModelFactory,
            IConfigurationMemory configurationMemory, bool isLocal)
        {
            _valueViewModelFactory = valueViewModelFactory;
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
                    property.LocalValue = _valueViewModelFactory.CreateEditableFormattedValueViewModel(value, propertyModel, propertyModel?.UshortsFormatter);
                }
                else
                {
                    property.DeviceValue = _valueViewModelFactory.CreateFormattedValueViewModel(value, propertyModel, propertyModel);
                }
                return Result.Create(true);
            }
            catch (Exception e)
            {
                return Result.Create(e);
            }
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