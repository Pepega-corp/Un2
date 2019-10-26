using System;
using Unicon2.Fragments.Configuration.Infrastructure.Factories;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Configuration.Factories
{
    public class ValueViewModelFactory : IPropertyValueViewModelFactory
    {
        private readonly ITypesContainer _container;


        public ValueViewModelFactory(ITypesContainer container)
        {
            this._container = container;
        }

        #region Implementation of IValueViewModelFactory

        public IFormattedValueViewModel CreateFormattedValueViewModel(IFormattedValue formattedValue,IMeasurable measurable=null)
        {
            try
            {
                IFormattedValueViewModel formattedValueViewModel =
                             this._container.Resolve<IFormattedValueViewModel>(formattedValue.StrongName +
                             ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
                formattedValueViewModel.InitFromValue(formattedValue);
                if (measurable != null)
                {
                    formattedValueViewModel.IsMeasureUnitEnabled = measurable.IsMeasureUnitEnabled;
                    formattedValueViewModel.MeasureUnit = measurable.MeasureUnit;
                }
                else
                {
                    formattedValueViewModel.IsMeasureUnitEnabled = false;
                }
                return formattedValueViewModel;
            }
            catch (Exception)
            {
                IFormattedValueViewModel formattedValueViewModel =
                    this._container.Resolve<IFormattedValueViewModel>("StringValue" +
                                                                 ApplicationGlobalNames.CommonInjectionStrings
                                                                     .VIEW_MODEL);
                (formattedValueViewModel as IStringValueViewModel).StringValue = formattedValue.AsString();
                return formattedValueViewModel;
            }
        }


        public IEditableValueViewModel CreateEditableFormattedValueViewModel(IFormattedValue formattedValue,
            IProperty property, IUshortsFormatter ushortsFormatter)
        {
            IFormattedValue formattedValueToEdit;
            if (formattedValue is IErrorValue)
            {
                formattedValueToEdit = property.UshortsFormatter.Format(new[] { (ushort)0 });

            }
            else
            {
                formattedValueToEdit = formattedValue;
            }

            IEditableValueViewModel editableValueViewModel;
            try
            {
                editableValueViewModel = this._container.Resolve<IFormattedValueViewModel>(
                    ApplicationGlobalNames.CommonInjectionStrings.EDITABLE +
                    formattedValueToEdit.StrongName +
                    ApplicationGlobalNames.CommonInjectionStrings
                        .VIEW_MODEL) as IEditableValueViewModel;
            }
            catch (Exception)
            {
                return null;
            }

            editableValueViewModel.SetUshortFormatter(ushortsFormatter);
            if (property.IsRangeEnabled)
            {
                editableValueViewModel.IsRangeEnabled = true;
                editableValueViewModel.Range = property.Range;
            }
            editableValueViewModel.InitFromValue(formattedValueToEdit);
            editableValueViewModel.ValueChangedAction += (ushortsNew) =>
            {
                property.LocalUshortsValue = ushortsNew;
                (property as ISubProperty)?.LocalValueChanged?.Invoke();
            };
            editableValueViewModel.IsMeasureUnitEnabled = property.IsMeasureUnitEnabled;
            editableValueViewModel.MeasureUnit = property.MeasureUnit;
            if (property.DeviceUshortsValue != null)
            {
                editableValueViewModel.SetBaseValueToCompare(property.DeviceUshortsValue);
            }

            return editableValueViewModel;
        }

        //public IFormattedValueViewModel CreateCollectionValueItemViewModel(ICollectionValue collectionValue,
        //    ObservableCollection<IRuntimeConfigurationItemViewModel> exitingRuntimeConfigurationItemViewModels)
        //{
        //    IFormattedValueViewModel formattedValueViewModel =
        //        _container.Resolve<IFormattedValueViewModel>(collectionValue.StrongName +
        //                                                     ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
        //    formattedValueViewModel.InitFromValue(collectionValue);
        //    foreach (var configurationItemViewModel in (formattedValueViewModel as
        //        CollectionValueToConfigurationItemsCollectionAdapter).ConfigurationViewModels)
        //    {
        //        var existingRuntimeConfigurationItemViewModel =
        //            exitingRuntimeConfigurationItemViewModels.FirstOrDefault(
        //                (model => model.Header == configurationItemViewModel.Header));
        //        if (existingRuntimeConfigurationItemViewModel != null)
        //        {
        //            IRuntimePropertyViewModel runtimePropertyViewModel = existingRuntimeConfigurationItemViewModel as IRuntimePropertyViewModel;
        //            if ((runtimePropertyViewModel != null) && (configurationItemViewModel is IRuntimePropertyViewModel))
        //            {

        //                runtimePropertyViewModel.DeviceValue =
        //                    (configurationItemViewModel as IRuntimePropertyViewModel).DeviceValue;
        //            }
        //        }
        //        else
        //        {
        //            exitingRuntimeConfigurationItemViewModels.Add(configurationItemViewModel);
        //        }
        //    }
        //    return formattedValueViewModel;
        //}

        //public IEditableValueViewModel CreateEditableCollectionValueItemViewModel(ICollectionValue collectionValue,
        //    ObservableCollection<IRuntimeConfigurationItemViewModel> exitingRuntimeConfigurationItemViewModels,IProperty relatedProperty,IUshortsFormatter ushortsFormatter)
        //{
        //    IEditableValueViewModel formattedValueViewModel =
        //        _container.Resolve<IFormattedValueViewModel>(ApplicationGlobalNames.CommonInjectionStrings.EDITABLE +
        //                                                     collectionValue.StrongName +
        //                                                     ApplicationGlobalNames.CommonInjectionStrings
        //                                                         .VIEW_MODEL) as IEditableValueViewModel;
        //   (formattedValueViewModel as CollectionValueToEditableConfigurationItemsCollectionAdapter).Initialize(relatedProperty,ushortsFormatter);
        //    formattedValueViewModel.InitFromValue(collectionValue);
        //    foreach (var configurationItemViewModel in (formattedValueViewModel as
        //        CollectionValueToEditableConfigurationItemsCollectionAdapter).ConfigurationViewModels)
        //    {
        //        var existingRuntimeConfigurationItemViewModel =
        //            exitingRuntimeConfigurationItemViewModels.FirstOrDefault(
        //                (model => model.Header == configurationItemViewModel.Header));
        //        if (existingRuntimeConfigurationItemViewModel != null)
        //        {
        //            IRuntimePropertyViewModel runtimePropertyViewModel= existingRuntimeConfigurationItemViewModel as IRuntimePropertyViewModel;
        //            if ((runtimePropertyViewModel != null)&&(configurationItemViewModel is IRuntimePropertyViewModel))
        //            {

        //                runtimePropertyViewModel.LocalValue =
        //                    (configurationItemViewModel as IRuntimePropertyViewModel).LocalValue;
        //            }
        //        }
        //        else
        //        {
        //            exitingRuntimeConfigurationItemViewModels.Add(configurationItemViewModel);
        //        }
        //    }
        //    return formattedValueViewModel;
        //}

        #endregion
    }
}