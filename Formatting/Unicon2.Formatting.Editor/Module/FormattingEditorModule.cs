﻿using Unicon2.Formatting.Editor.Factories;
using Unicon2.Formatting.Editor.Services;
using Unicon2.Formatting.Editor.ViewModels;
using Unicon2.Formatting.Editor.ViewModels.FormatterParameters;
using Unicon2.Formatting.Editor.ViewModels.InnerMembers;
using Unicon2.Formatting.Editor.Visitors;
using Unicon2.Formatting.Infrastructure.Keys;
using Unicon2.Formatting.Infrastructure.Services;
using Unicon2.Formatting.Infrastructure.ViewModel.InnerMembers;
using Unicon2.Formatting.Services;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Presentation.Infrastructure.Visitors;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Formatting.Editor.Module
{
    public class FormattingEditorModule : IUnityModule
    {
        public void Initialize(ITypesContainer container)
        {
            container.Register(typeof(IUshortsFormatterViewModel), typeof(StringFormatter1251ViewModel),
                StringKeys.STRING_FORMATTER1251 + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
            container.Register(typeof(IUshortsFormatterViewModel), typeof(CodeFormatterViewModel),
                StringKeys.CODE_FORMATTER + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
            container.Register(typeof(IUshortsFormatterViewModel), typeof(AsciiStringFormatterViewModel),
                StringKeys.ASCII_STRING_FORMATTER + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
            container.Register(typeof(IUshortsFormatterViewModel), typeof(DictionaryMatchingFormatterViewModel),
                StringKeys.DICTIONARY_MATCHING_FORMATTER + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
            container.Register(typeof(IUshortsFormatterViewModel), typeof(FormulaFormatterViewModel),
                StringKeys.FORMULA_FORMATTER + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
            container.Register(typeof(IUshortsFormatterViewModel), typeof(DirectFormatterViewModel),
                StringKeys.DIRECT_USHORT_FORMATTER + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
            //containerRegistry.Register(typeof(IUshortsFormatterViewModel), typeof(BitGroupFormatterViewModel), StringKeys.BITS_GROUP_FORMATTER + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
            container.Register(typeof(IUshortsFormatterViewModel), typeof(BoolFormatterViewModel),
                StringKeys.BOOL_FORMATTER + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
            container.Register(typeof(IUshortsFormatterViewModel), typeof(DefaultTimeFormatterViewModel),
                StringKeys.DEFAULT_TIME_FORMATTER + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
            container.Register(typeof(IUshortsFormatterViewModel), typeof(DefaultBitMaskFormatterViewModel),
                StringKeys.DEFAULT_BIT_MASK_FORMATTER + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);

            container.Register(typeof(IUshortsFormatterViewModel), typeof(UshortToIntegerFormatterViewModel),
                StringKeys.USHORT_TO_INTEGER_FORMATTER + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
            // container.Register(typeof(IViewModel), typeof(StringFormatter1251ViewModel), StringKeys.STRING_FORMATTER1251 + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);

            //container.Register(typeof(IViewModel), typeof(AsciiStringFormatterViewModel), StringKeys.ASCII_STRING_FORMATTER + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
            //container.Register(typeof(IViewModel), typeof(DictionaryMatchingFormatterViewModel), StringKeys.DICTIONARY_MATCHING_FORMATTER + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
            //container.Register(typeof(IViewModel), typeof(FormulaFormatterViewModel), StringKeys.FORMULA_FORMATTER + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
            //container.Register(typeof(IViewModel), typeof(DirectFormatterViewModel), StringKeys.DIRECT_USHORT_FORMATTER + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
            ////containerRegistry.Register(typeof(IViewModel), typeof(BitGroupFormatterViewModel), StringKeys.BITS_GROUP_FORMATTER + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
            //container.Register(typeof(IViewModel), typeof(BoolFormatterViewModel), StringKeys.BOOL_FORMATTER + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
            container.Register(typeof(ISharedBitViewModel), typeof(SharedBitViewModel));
            container.Register(typeof(IArgumentViewModel), typeof(ArgumentViewModel));
            container.Register(typeof(IFormatterEditorFactory), typeof(FormatterEditorFactory));
            container.Register(typeof(ISaveFormatterService), typeof(SaveFormatterService));
            container.Register(typeof(IFormatterViewModelFactory), typeof(FormatterViewModelFactory));
            container.Register<IFormatterParametersViewModel, FormatterParametersViewModel>();
            container.Register<IEditableValueCopyVisitorProvider, EditableValueCopyVisitorProvider>();
            container.Register<ICodeFormatterService, UniconEngineCodeFormatterService>(true);
            container.Register<IFormatterInfoService, FormatterInfoService>(true);

            IXamlResourcesService xamlResourcesService = container.Resolve<IXamlResourcesService>();
            xamlResourcesService.AddResourceAsGlobal("Resources/FormattersTemplates.xaml", GetType().Assembly);
            xamlResourcesService.AddResourceAsGlobal("Resources/InnerMembersTemplates.xaml", GetType().Assembly);
        }
    }
}
