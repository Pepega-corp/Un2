using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Infrastructure.Services;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;
using Unicon2.Fragments.Configuration.ViewModel.Properties;
using Unicon2.Infrastructure.Functional;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Presentation.Infrastructure.Visitors;

namespace Unicon2.Fragments.Configuration.Services
{
    public class ConfigurationTreeWalker : IConfigurationTreeWalker
    {
        private readonly IEditableValueCopyVisitorProvider _editableValueCopyVisitorProvider;

        public ConfigurationTreeWalker(IEditableValueCopyVisitorProvider editableValueCopyVisitorProvider)
        {
            _editableValueCopyVisitorProvider = editableValueCopyVisitorProvider;
        }

        private Result ProcessConfigurationItem(
            Result<Action<ILocalAndDeviceValueContainingViewModel, ILocalAndDeviceValueContainingViewModel>> callback,
            IConfigurationItemViewModel configurationItemViewModel1,
            IConfigurationItemViewModel configurationItemViewModel2)
        {
            if (configurationItemViewModel1 is RuntimePropertyViewModel propertyViewModel1 &&
                configurationItemViewModel2 is RuntimePropertyViewModel propertyViewModel2)
            {
                callback.OnSuccess(func => func(propertyViewModel1, propertyViewModel2));
            }

            if (configurationItemViewModel1 is IItemGroupViewModel itemGroupViewModel1 &&
                configurationItemViewModel2 is IItemGroupViewModel itemGroupViewModel2)
            {
                var count = itemGroupViewModel1.ChildStructItemViewModels.Count;
                if (count != itemGroupViewModel2.ChildStructItemViewModels.Count)
                {
                    return Result.Create(false);
                }

                for (int i = 0; i < count; i++)
                {
                    var res = ProcessConfigurationItem(callback, itemGroupViewModel1.ChildStructItemViewModels[i],
                        itemGroupViewModel2.ChildStructItemViewModels[i]);
                    if (!res.IsSuccess)
                    {
                        return res;
                    }
                }
            }

            return Result.Create(true);
        }



        public bool IsStructureSimilar(IConfigurationItemViewModel configurationItemViewModel1,
            IConfigurationItemViewModel configurationItemViewModel2)
        {
            var res = ProcessConfigurationItem(
                Result<Action<ILocalAndDeviceValueContainingViewModel, ILocalAndDeviceValueContainingViewModel>>
                    .Create(false), configurationItemViewModel1, configurationItemViewModel2);
            return res.IsSuccess;
        }

        public void CopyValuesToItem(IConfigurationItemViewModel configurationItemViewModelFrom,
            IConfigurationItemViewModel configurationItemViewModelTo)
        {
            var callback =
                Result<Action<ILocalAndDeviceValueContainingViewModel, ILocalAndDeviceValueContainingViewModel>>
                    .Create((viewModel1, viewModel2) =>
                    {
                        viewModel2.LocalValue.Accept(
                            _editableValueCopyVisitorProvider.GetValueViewModelCopyVisitor(viewModel1.LocalValue));
                    }, true);
            var res = ProcessConfigurationItem(callback, configurationItemViewModelFrom, configurationItemViewModelTo);
        }
    }
}