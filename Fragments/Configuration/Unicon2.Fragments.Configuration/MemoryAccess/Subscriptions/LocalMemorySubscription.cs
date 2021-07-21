using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Dependencies;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Dependencies.Conditions;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Dependencies.Results;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;
using Unicon2.Fragments.Configuration.MemoryAccess.Subscriptions.ComplexProperty;
using Unicon2.Fragments.Configuration.MemoryAccess.Subscriptions.DependentProperty;
using Unicon2.Fragments.Configuration.ViewModelMemoryMapping;
using Unicon2.Fragments.Configuration.Visitors;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.Services.Formatting;

namespace Unicon2.Fragments.Configuration.MemoryAccess.Subscriptions
{
    public class LocalMemorySubscription : IDeviceSubscription
    {
        private readonly IUshortsFormatter _ushortsFormatter;
        private readonly DeviceContext _deviceContext;
        private readonly IRuntimePropertyViewModel _runtimePropertyViewModel;
        private readonly IProperty _property;
        private readonly IFormattingService _formattingService;
        private ushort[] _prevUshorts;
        private IUshortsFormatter _prevUshortFormatter;
        private bool _prevIsInteractionBlocked = false;
        private int _offset;
        public int Priority { get; set; } = 1;
        private bool _prevIsHidden = false;

        public LocalMemorySubscription(IUshortsFormatter ushortsFormatter, DeviceContext deviceContext,
            IRuntimePropertyViewModel runtimePropertyViewModel,
            IProperty property, IFormattingService formattingService, int offset, ushort[] prevUshorts)
        {
            _ushortsFormatter = ushortsFormatter;
            _deviceContext = deviceContext;
            _runtimePropertyViewModel = runtimePropertyViewModel;
            _property = property;
            _formattingService = formattingService;
            _offset = offset;
            _prevUshorts = prevUshorts;
            _prevUshortFormatter = property.UshortsFormatter;
        }

        public async void Execute()
        {
            if (_property.IsFromBits)
            {

                if (!MemoryAccessor.IsMemoryContainsAddresses(_deviceContext.DeviceMemory,
                    (ushort) (_property.Address + _offset),
                    _property.NumberOfPoints, true))
                {
                    return;
                }

                var newUshorts = MemoryAccessor.GetUshortsFromMemory(_deviceContext.DeviceMemory,
                    (ushort) (_property.Address + _offset),
                    _property.NumberOfPoints, true);


                var boolArray = newUshorts.GetBoolArrayFromUshortArray();
                bool[] subPropertyBools = new bool[16];
                int counter = 0;
                for (ushort i = 0; i < 16; i++)
                {
                    if (_property.BitNumbers.Contains(i))
                    {
                        subPropertyBools[counter] = boolArray[i];
                        counter++;
                    }
                }

                var subPropertyUshort = subPropertyBools.BoolArrayToUshort();




                if (_property?.Dependencies?.Count > 0)
                {
                    bool isHidden = false;

                    bool isInteractionBlocked = false;
                    var formatterForDependentProperty = _property.UshortsFormatter;

                    foreach (var dependency in _property.Dependencies)
                    {
                        if (dependency is IConditionResultDependency conditionResultDependency)
                        {
                            if (conditionResultDependency.Condition is ICompareResourceCondition
                                compareResourceCondition)
                            {
                                var checkResult = await DependentSubscriptionHelpers.CheckConditionFromResource(
                                    compareResourceCondition,
                                    _deviceContext, _formattingService, true, (ushort) _offset);

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
                                            case IHidePropertyResult hidePropertyResult:
                                                isHidden = checkResult.Item;
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
                                            case IHidePropertyResult hidePropertyResult:
                                                isHidden = checkResult.Item;
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

                    if (_prevUshorts.IsEqual(newUshorts) && _prevIsHidden == isHidden &&
                        formatterForDependentProperty == _prevUshortFormatter &&
                        _prevIsInteractionBlocked == isInteractionBlocked)
                    {
                        return;
                    }

                    _prevIsHidden = isHidden;

                    _prevIsInteractionBlocked = isInteractionBlocked;
                    _prevUshorts = newUshorts;
                    _prevUshortFormatter = formatterForDependentProperty;
                    if (_runtimePropertyViewModel?.LocalValue != null)
                    {
                        _deviceContext.DeviceEventsDispatcher.RemoveSubscriptionById(_runtimePropertyViewModel
                            .LocalValue.Id);
                        _runtimePropertyViewModel.LocalValue.Dispose();
                    }


                    _runtimePropertyViewModel.IsHidden = _property.IsHidden || isHidden;

                    var localValue = await _formattingService.FormatValueAsync(formatterForDependentProperty,
                        subPropertyUshort.AsCollection(),
                        new FormattingContext(_runtimePropertyViewModel, _deviceContext, true));
                    var editableValue = StaticContainer.Container.Resolve<IValueViewModelFactory>()
                        .CreateEditableValueViewModel(new FormattedValueInfo(localValue, _property,
                            formatterForDependentProperty,
                            _property, !isInteractionBlocked, !_prevUshorts.IsEqual(subPropertyUshort.AsCollection())));
                    var editSubscription =
                        new LocalDataEditedSubscription(_runtimePropertyViewModel, editableValue, _deviceContext,
                            _property, _offset);
                    _runtimePropertyViewModel.LocalValue = editableValue;
                    editableValue?.InitDispatcher(_deviceContext.DeviceEventsDispatcher);
                    if (_runtimePropertyViewModel.LocalValue != null)
                        _deviceContext.DeviceEventsDispatcher.AddSubscriptionById(editSubscription
                            , _runtimePropertyViewModel.LocalValue.Id);




                    editableValue.InitDispatcher(_deviceContext.DeviceEventsDispatcher);

                }
                else
                {
                    if (!MemoryAccessor.IsMemoryContainsAddresses(_deviceContext.DeviceMemory,
                        (ushort) (_property.Address + _offset), _property.NumberOfPoints, true))
                    {
                        return;
                    }

                    if (!newUshorts.IsEqual(_prevUshorts))
                    {
                        _prevUshorts = newUshorts;

                        var subPropertyValue = await StaticContainer.Container.Resolve<IFormattingService>()
                            .FormatValueAsync(
                                _property.UshortsFormatter,
                                new[] {subPropertyUshort},
                                new FormattingContext(_runtimePropertyViewModel, _deviceContext, true));


                        _runtimePropertyViewModel.LocalValue.Accept(
                            new EditableValueSetFromLocalVisitor(subPropertyValue));
                    }
                }


            }
            else
            {
                var newUshorts = MemoryAccessor.GetUshortsFromMemorySafe(
                    _deviceContext.DeviceMemory,
                    (ushort) (_property.Address + _offset), _property.NumberOfPoints, true);

                if (!newUshorts.IsSuccess)
                {
                    return;
                }

                if (_property?.Dependencies?.Count > 0)
                {
                    bool isHidden = false;

                    bool isInteractionBlocked = false;
                    var formatterForDependentProperty = _property.UshortsFormatter;

                    foreach (var dependency in _property.Dependencies)
                    {
                        if (dependency is IConditionResultDependency conditionResultDependency)
                        {
                            if (conditionResultDependency.Condition is ICompareResourceCondition
                                compareResourceCondition)
                            {
                                var checkResult = await DependentSubscriptionHelpers.CheckConditionFromResource(
                                    compareResourceCondition,
                                    _deviceContext, _formattingService, true, (ushort) _offset);

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
                                            case IHidePropertyResult hidePropertyResult:
                                                isHidden = checkResult.Item;
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
                                            case IHidePropertyResult hidePropertyResult:
                                                isHidden = checkResult.Item;
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

                    if (_runtimePropertyViewModel.LocalValue.IsEditEnabled == !isInteractionBlocked &&
                        isHidden == _prevIsHidden &&
                        formatterForDependentProperty == _prevUshortFormatter)
                    {
                        await ProcessValueViewModel(newUshorts, _prevIsInteractionBlocked, formatterForDependentProperty);
                        return;
                    }

                    _prevIsHidden = isHidden;
                    _prevUshorts = newUshorts.Item;
                    _prevUshortFormatter = formatterForDependentProperty;
                    if (_runtimePropertyViewModel?.LocalValue != null)
                    {
                        _deviceContext.DeviceEventsDispatcher.RemoveSubscriptionById(_runtimePropertyViewModel
                            .LocalValue.Id);
                        _runtimePropertyViewModel.LocalValue.Dispose();
                    }

                    _runtimePropertyViewModel.IsHidden = _property.IsHidden || isHidden;


                    var localValue = await _formattingService.FormatValueAsync(formatterForDependentProperty,
                        newUshorts.Item, new FormattingContext(_runtimePropertyViewModel, _deviceContext, true));
                    var editableValue = StaticContainer.Container.Resolve<IValueViewModelFactory>()
                        .CreateEditableValueViewModel(new FormattedValueInfo(localValue, _property,
                            formatterForDependentProperty,
                            _property, !isInteractionBlocked, !_prevUshorts.IsEqual(newUshorts.Item)));
                    var editSubscription =
                        new LocalDataEditedSubscription(_runtimePropertyViewModel, editableValue, _deviceContext,
                            _property, _offset);
                    _runtimePropertyViewModel.LocalValue = editableValue;
                    editableValue?.InitDispatcher(_deviceContext.DeviceEventsDispatcher);
                    if (_runtimePropertyViewModel.LocalValue != null)
                        _deviceContext.DeviceEventsDispatcher.AddSubscriptionById(editSubscription
                            , _runtimePropertyViewModel.LocalValue.Id);
                }
                else
                {
                    if (!MemoryAccessor.IsMemoryContainsAddresses(_deviceContext.DeviceMemory,
                        (ushort) (_property.Address), _property.NumberOfPoints, true))
                    {
                        return;
                    }

                    await ProcessValueViewModel(newUshorts,_prevIsInteractionBlocked, _ushortsFormatter);
                }


            }
        }

        private async Task ProcessValueViewModel(Result<ushort[]> newUshorts, bool isInteractionBlocked,
            IUshortsFormatter formatterForDependentProperty)
        {
            if (!newUshorts.Item.IsEqual(_prevUshorts))
            {
                _prevUshorts = newUshorts.Item;
                var localValue = await StaticContainer.Container.Resolve<IFormattingService>().FormatValueAsync(
                    _ushortsFormatter,
                    _prevUshorts, new FormattingContext(_runtimePropertyViewModel, _deviceContext, true));
                _runtimePropertyViewModel.LocalValue.Accept(new EditableValueSetFromLocalVisitor(localValue));

            }
        }

    }
}