using System;
using System.Linq;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Functional;
using Unicon2.Presentation.Infrastructure.TreeGrid;

namespace Unicon2.Fragments.Configuration.ViewModelMemoryMapping
{
    public class ValuesSeedingConfigurationItemsVisitor : IConfigurationItemVisitor<Result>
    {
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
            property.LocalValue=(property.Model as IProperty)?.UshortsFormatter.Format()
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