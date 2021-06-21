using Unicon2.Fragments.Configuration.Infrastructure.MemoryViewModelMapping;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Dependencies;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Dependencies.Conditions;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Dependencies.Results;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;
using Unicon2.Fragments.Configuration.MemoryAccess.Subscriptions.DependentProperty;
using Unicon2.Fragments.Configuration.ViewModelMemoryMapping;
using Unicon2.Fragments.Configuration.Visitors;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.Services.Formatting;

namespace Unicon2.Fragments.Configuration.MemoryAccess.Subscriptions.ComplexProperty
{
    public class LocalSubPropertySubscription : ILocalDataMemorySubscription
    {
        private readonly DeviceContext _deviceContext;
        private readonly IRuntimeSubPropertyViewModel _runtimeSubPropertyViewModel;
        private readonly IRuntimeComplexPropertyViewModel _runtimeComplexPropertyViewModel;
        private readonly IComplexProperty _complexProperty;

        private readonly ISubProperty _subProperty;
        private readonly IFormattingService _formattingService;
        private ushort[] _prevUshorts = new ushort[0];
        private bool _prevIsBlocked;
        private IUshortsFormatter _prevUshortFormatter;
        private int _offset;

        public LocalSubPropertySubscription(DeviceContext deviceContext,
            IRuntimeSubPropertyViewModel runtimeSubPropertyViewModel, ISubProperty subProperty,
            IFormattingService formattingService, int offset, IRuntimeComplexPropertyViewModel runtimeComplexPropertyViewModel, IComplexProperty complexProperty)
        {
            _deviceContext = deviceContext;
            _runtimeSubPropertyViewModel = runtimeSubPropertyViewModel;
            _subProperty = subProperty;
            _formattingService = formattingService;
            _offset = offset;
            _runtimeComplexPropertyViewModel = runtimeComplexPropertyViewModel;
            _complexProperty = complexProperty;
        }
        public int Priority { get; set; } = 1;

        public void Execute()
        {
            if (!MemoryAccessor.IsMemoryContainsAddresses(_deviceContext.DeviceMemory,
                (ushort)(_subProperty.Address + _offset),
                _subProperty.NumberOfPoints, true))
            {
                return;
            }

            var newUshorts = MemoryAccessor.GetUshortsFromMemory(_deviceContext.DeviceMemory,
                (ushort) (_subProperty.Address + _offset),
                _subProperty.NumberOfPoints, true);


            var boolArray = newUshorts.GetBoolArrayFromUshortArray();
            bool[] subPropertyBools = new bool[16];
            int counter = 0;
            for (int i = 0; i < 16; i++)
            {
                if (_subProperty.BitNumbersInWord.Contains(i))
                {
                    subPropertyBools[counter] = boolArray[i];
                    counter++;
                }
            }

            var subPropertyUshort = subPropertyBools.BoolArrayToUshort();




            if (_subProperty?.Dependencies?.Count > 0)
            {
                bool isInteractionBlocked = false;
                var formatterForDependentProperty = _subProperty.UshortsFormatter;

                foreach (var dependency in _subProperty.Dependencies)
                {
                    if (dependency is IConditionResultDependency conditionResultDependency)
                    {
                        if (conditionResultDependency.Condition is ICompareResourceCondition
                            compareResourceCondition)
                        {
                            var checkResult = DependentSubscriptionHelpers.CheckConditionFromResource(compareResourceCondition,
                                _deviceContext, _formattingService, true,(ushort)_offset);

                            if (checkResult.IsSuccess)
                            {
                                if (checkResult.Item)
                                {
                                    switch (conditionResultDependency.Result)
                                    {
                                        case IApplyFormatterResult applyFormatterResult:
                                            formatterForDependentProperty = applyFormatterResult.UshortsFormatter;
                                            break;
                                        case IBlockInteractionResult blockInteractionResult:
                                            isInteractionBlocked = checkResult.Item;
                                            break;
                                    }

                                }
                                else
                                {
                                    switch (conditionResultDependency.Result)
                                    {
                                        case IBlockInteractionResult blockInteractionResult:
                                            isInteractionBlocked = checkResult.Item;
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                return;
                            }
                        }

                    }
                }

                if (_prevUshorts.IsEqual(newUshorts) && _prevIsBlocked == isInteractionBlocked &&
                    formatterForDependentProperty == _prevUshortFormatter)
                {
                    return;
                }

                _prevUshorts = newUshorts;
                _prevIsBlocked = isInteractionBlocked;
                _prevUshortFormatter = formatterForDependentProperty;
                if (_runtimeSubPropertyViewModel?.LocalValue != null)
                {
                    _deviceContext.DeviceEventsDispatcher.RemoveSubscriptionById(_runtimeSubPropertyViewModel
                        .LocalValue.Id);
                    _runtimeSubPropertyViewModel.LocalValue.Dispose();
                }

                var localValue = _formattingService.FormatValue(formatterForDependentProperty,
                    subPropertyUshort.AsCollection(),true);


                var editableValue = StaticContainer.Container.Resolve<IValueViewModelFactory>()
                    .CreateEditableValueViewModel(new FormattedValueInfo(localValue, _subProperty,
                        formatterForDependentProperty,
                        _subProperty, !isInteractionBlocked));
                var editSubscription =
                    new LocalDataComplexPropertyEditedSubscription(_runtimeComplexPropertyViewModel, _deviceContext,_complexProperty, _offset);
                _runtimeSubPropertyViewModel.LocalValue = editableValue;
                editableValue.InitDispatcher(_deviceContext.DeviceEventsDispatcher);
                _deviceContext.DeviceEventsDispatcher.AddSubscriptionById(editSubscription
                    , _runtimeSubPropertyViewModel.LocalValue.Id);
            }
            else
            {
                if (!MemoryAccessor.IsMemoryContainsAddresses(_deviceContext.DeviceMemory,
                    (ushort) (_subProperty.Address+_offset), _subProperty.NumberOfPoints, true))
                {
                    return;
                }

                if (!newUshorts.IsEqual(_prevUshorts))
                {
                    _prevUshorts = newUshorts;

                    var subPropertyValue = StaticContainer.Container.Resolve<IFormattingService>().FormatValue(
                        _subProperty.UshortsFormatter,
                        new[] { subPropertyUshort },true);


                    _runtimeSubPropertyViewModel.LocalValue.Accept(
                        new EditableValueSetFromLocalVisitor(subPropertyValue));
                }
            }


        }
    }
}