using System.Linq;
using Unicon2.Fragments.Configuration.Infrastructure.MemoryViewModelMapping;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Dependencies;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Dependencies.Conditions;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Dependencies.Results;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.MemoryAccess.Subscriptions.ComplexProperty;
using Unicon2.Fragments.Configuration.MemoryAccess.Subscriptions.DependentProperty;
using Unicon2.Fragments.Configuration.ViewModelMemoryMapping;
using Unicon2.Fragments.Configuration.Visitors;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.Services.Formatting;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Presentation.Infrastructure.Visitors;

namespace Unicon2.Fragments.Configuration.MemoryAccess.Subscriptions
{
    public class LocalDataEditedSubscription : ILocalDataMemorySubscription
    {
        private readonly DeviceContext _deviceContext;
        private readonly IProperty _property;
        private readonly int _offset;

        public LocalDataEditedSubscription(IEditableValueViewModel editableValueViewModel,
            DeviceContext deviceContext, IProperty property, int offset)
        {
            _deviceContext = deviceContext;
            _property = property;
            _offset = offset;
            EditableValueViewModel = editableValueViewModel;
        }
        public int Priority { get; set; } = 2;

        public async void Execute()
        {
            if (_property.IsFromBits)
            {
                //var resultBitArray=new bool[16];
                var resultBitArray = _deviceContext.DeviceMemory.LocalMemoryValues[(ushort)(_property.Address + _offset)].GetBoolArrayFromUshort();


                IFormattingService formattingService = StaticContainer.Container.Resolve<IFormattingService>();
                IEditableValueFetchingFromViewModelVisitor fetchingFromViewModelVisitor =
                    StaticContainer.Container.Resolve<IEditableValueFetchingFromViewModelVisitor>();


                var formatterForDependentProperty = _property.UshortsFormatter;

                if (_property?.Dependencies?.Count > 0)
                {
                    formatterForDependentProperty =
                        DependentSubscriptionHelpers.GetFormatterConsideringDependencies(_property.Dependencies,
                            _deviceContext, formattingService, _property.UshortsFormatter, (ushort)_offset);
                }


                var ushorts = formattingService.FormatBack(formatterForDependentProperty,
                    EditableValueViewModel.Accept(fetchingFromViewModelVisitor),true); var ushortOfSubProperty = ushorts.First();
                    var boolArray = ushorts.GetBoolArrayFromUshortArray();
                    int counter = 0;
                    for (int i = 0; i < 16; i++)
                    {
                        if (_property.BitNumbers.Contains((ushort) i))
                        {
                            resultBitArray[i] = boolArray[counter];
                            counter++;
                        }


                    }

                

                resultBitArray = resultBitArray.ToArray();
                var resUshorts = new ushort[] { resultBitArray.BoolArrayToUshort() };
                MemoryAccessor.SetUshortsInMemory(_deviceContext.DeviceMemory, (ushort)(_property.Address + _offset), resUshorts, true);
                _deviceContext.DeviceEventsDispatcher.TriggerLocalAddressSubscription(
                    (ushort)(_property.Address + _offset), (ushort)resUshorts.Length);
            }
            else
            {


                IFormattingService formattingService = StaticContainer.Container.Resolve<IFormattingService>();
                IEditableValueFetchingFromViewModelVisitor fetchingFromViewModelVisitor =
                    StaticContainer.Container.Resolve<IEditableValueFetchingFromViewModelVisitor>();


                var formatterForDependentProperty = _property.UshortsFormatter;

                if (_property?.Dependencies?.Count > 0)
                {
                    formatterForDependentProperty =
                        DependentSubscriptionHelpers.GetFormatterConsideringDependencies(_property.Dependencies,
                            _deviceContext, formattingService, _property.UshortsFormatter, (ushort) _offset);
                }


                var ushorts =await formattingService.FormatBackAsync(formatterForDependentProperty,
                    EditableValueViewModel.Accept(fetchingFromViewModelVisitor),_deviceContext,true);


                if (!ushorts.IsSuccess)
                {
                    if (EditableValueViewModel is ValidatableBindableBase validatableBindableBase)
                    {
                        validatableBindableBase.AddError("NumValue",ushorts.Exception.Message);
                    }
                    
                }
                else
                {
                   MemoryAccessor.SetUshortsInMemory(_deviceContext.DeviceMemory, (ushort) (_property.Address + _offset),
                    ushorts.Item, true);
                _deviceContext.DeviceEventsDispatcher.TriggerLocalAddressSubscription(
                    (ushort) (_property.Address + _offset), (ushort) ushorts.Item.Length); 
                }
                
            }
        }

     



        public IEditableValueViewModel EditableValueViewModel { get; }
    }
}