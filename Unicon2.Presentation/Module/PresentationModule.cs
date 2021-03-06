﻿using System;
using System.Linq;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Interfaces.Excel;
using Unicon2.Presentation.Connection;
using Unicon2.Presentation.Factories;
using Unicon2.Presentation.FragmentSettings;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.FragmentSettings;
using Unicon2.Presentation.Infrastructure.Keys;
using Unicon2.Presentation.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.Services.CommandStack;
using Unicon2.Presentation.Infrastructure.Services.Dependencies;
using Unicon2.Presentation.Infrastructure.Subscription;
using Unicon2.Presentation.Infrastructure.ViewModels.Connection;
using Unicon2.Presentation.Infrastructure.ViewModels.Device;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentSettings;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Presentation.Infrastructure.ViewModels.Windows;
using Unicon2.Presentation.Infrastructure.Visitors;
using Unicon2.Presentation.Services;
using Unicon2.Presentation.Subscription;
using Unicon2.Presentation.Values;
using Unicon2.Presentation.Values.Editable;
using Unicon2.Presentation.ViewModels;
using Unicon2.Presentation.ViewModels.Dependencies;
using Unicon2.Presentation.ViewModels.Device;
using Unicon2.Presentation.ViewModels.Fragment;
using Unicon2.Presentation.ViewModels.Fragment.FragmentOptions;
using Unicon2.Presentation.ViewModels.Windows;
using Unicon2.Presentation.Visitors;
using Unicon2.Unity.Commands;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Presentation.Module
{
    public class PresentationModule : IUnityModule
    {
        public void Initialize(ITypesContainer container)
        {
            container.Register<IFormattedValueViewModel, BoolValueViewModel>(nameof(BoolValueViewModel));
            container.Register<IFormattedValueViewModel, NumericValueViewModel>(nameof(NumericValueViewModel));
            container.Register<INumericValueViewModel, NumericValueViewModel>();
            container.Register<IFormattedValueViewModel, BitMaskValueViewModel>(
                PresentationKeys.BIT_MASK_VALUE + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
            container.Register<IDeviceEventsDispatcher, FragmentLevelEventsDispatcher>();

            container.Register<IFormattedValueViewModel, ChosenFromListValueViewModel>(
                PresentationKeys.CHOSEN_FROM_LIST_VALUE_KEY + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
            container.Register<IFormattedValueViewModel, StringValueViewModel>(
                PresentationKeys.STRING_VALUE_KEY + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
            container.Register<IFormattedValueViewModel, ErrorValueViewModel>(nameof(ErrorValueViewModel));
            container.Register<ToolBarViewModel>(true);
            container.Register<IFormattedValueViewModel, EditableChosenFromListValueViewModel>(
                ApplicationGlobalNames.CommonInjectionStrings.EDITABLE + PresentationKeys.CHOSEN_FROM_LIST_VALUE_KEY +
                ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
            container.Register(typeof(IFormattedValueViewModel), typeof(EditableNumericValueViewModel),
                ApplicationGlobalNames.CommonInjectionStrings.EDITABLE + PresentationKeys.NUMERIC_VALUE_KEY +
                ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
            container.Register(typeof(IFormattedValueViewModel), typeof(EditableBoolValueViewModel),
                ApplicationGlobalNames.CommonInjectionStrings.EDITABLE + PresentationKeys.BOOL_VALUE_KEY +
                ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
            container.Register(typeof(IStringValueViewModel), typeof(StringValueViewModel));
            container.Register(typeof(IDeviceEventsDispatcher), typeof(FragmentLevelEventsDispatcher));

            container.Register(typeof(IBoolValueViewModel), typeof(BoolValueViewModel));
            container.Register(typeof(IBitMaskValueViewModel), typeof(BitMaskValueViewModel));
            container.Register(typeof(IValueViewModelFactory), typeof(ValueViewModelFactory));
            container.Register(typeof(ICommandStackService), typeof(CommandStackService),true);

            container.Register(typeof(IFragmentOptionCommandViewModel), typeof(DefaultFragmentOptionCommandViewModel));
            container.Register(typeof(IFragmentOptionGroupViewModel), typeof(DefaultFragmentOptionGroupViewModel));
            container.Register(typeof(IFragmentOptionsViewModel), typeof(DefaultFragmentOptionsViewModel));

            container.Register(typeof(IFragmentSettingViewModel), typeof(QuickAccessMemorySettingViewModel),
                ApplicationGlobalNames.QUICK_ACCESS_MEMORY_CONFIGURATION_SETTING +
                ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);

            container.Register(typeof(IFragmentPaneViewModelFactory), typeof(FragmentPaneViewModelFactory));
            container.Register(typeof(IFragmentPaneViewModel), typeof(FragmentPaneViewModel));

            container.Register(typeof(IDeviceViewModelFactory), typeof(DeviceViewModelFactory));
            container.Register(typeof(IDeviceViewModel), typeof(DeviceViewModel));
            container.Register(typeof(IRangeViewModel), typeof(RangeViewModel));
            container.Register(typeof(IConnectionStateViewModel), typeof(ConnectionStateViewModel));
            container.Register(typeof(IDeviceLoggerViewModel), typeof(DeviceLoggerViewModel));
            container.Register(typeof(IDependenciesService), typeof(DependenciesService));
            container.Register<DependenciesViewModel>();
            container.Register(typeof(ILogServiceViewModel), typeof(LogServiceViewModel), true);
            container.Register(typeof(IProjectBrowserViewModel), typeof(ProjectBrowserViewModel), true);
            container.Register(typeof(ILoadAllService), typeof(LoadAllService), true);

            container.Register(typeof(IFragmentEditorViewModelFactory), typeof(FragmentEditorViewModelFactory));
            container.Register(typeof(IFragmentSettingsViewModel), typeof(FragmentSettingsViewModel));
            container.Register(typeof(IEditableValueFetchingFromViewModelVisitor),
                typeof(EditableValueFetchingFromViewModelVisitor));
            container.Register<IConnectionService, ConnectionService>();
            container.Register<IExcelImporter, ExcelExportService>();
            container.Register<IExcelExporter, ExcelExportService>();
        }
    }
}