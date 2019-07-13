using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Matrix.EditorViewModel;
using Unicon2.Fragments.Configuration.Matrix.EditorViewModel.Factories;
using Unicon2.Fragments.Configuration.Matrix.EditorViewModel.OptionTemplates;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.EditorViewModel;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.EditorViewModel.Factories;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.EditorViewModel.OptionTemplates;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.Model;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.Model.Helpers;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.Model.OptionTemplates;
using Unicon2.Fragments.Configuration.Matrix.Keys;
using Unicon2.Fragments.Configuration.Matrix.Model;
using Unicon2.Fragments.Configuration.Matrix.Model.Helpers;
using Unicon2.Fragments.Configuration.Matrix.Model.OptionTemplates;
using Unicon2.Fragments.Configuration.Matrix.ViewModel;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Services;
using Unicon2.Infrastructure.ViewModel;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Configuration.Matrix.Module
{
    public class MatrixConfigurationModule : IUnityModule
    {
        public void Initialize(ITypesContainer container)
        {
            container.Register<IConfigurationItem, AppointableMatrix>(ConfigurationKeys.APPOINTABLE_MATRIX);
            container.Register<IMatrixTemplate, DefaultMatrixTemplate>();
            container.Register<IMatrixMemoryVariable, DefaultMatrixMemoryVariable>();
            container.Register<IVariableColumnSignature, DefaultVariableColumnSignature>();
            container.Register<IOptionPossibleValue, OptionPossibleValue>();
            container.Register<IPossibleValueCondition, PossibleValueCondition>();
            container.Register<IBitOptionUpdatingStrategy, DefaultBitOptionUpdatingStrategy>();

            container.Register<IMatrixVariableOptionTemplate, ListMatrixVariableOptionTemplate>(MatrixKeys.LIST_MATRIX_TEMPLATE);
            container.Register<IMatrixVariableOptionTemplate, BoolMatrixVariableOptionTemplate>(MatrixKeys.BOOL_MATRIX_TEMPLATE);
            container.Register<IBitOption, BoolMatrixBitOption>(MatrixKeys.BOOL_MATRIX_BIT_OPTION);
            container.Register<IBitOption, ListMatrixBitOption>(MatrixKeys.LIST_MATRIX_BIT_OPTION);

            container.Register(typeof(IViewModel), typeof(AppointableMatrixEditorViewModel),
                ConfigurationKeys.APPOINTABLE_MATRIX + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL);
            container.Register(typeof(IViewModel), typeof(BoolMatrixVariableOptionTemplateEditorViewModel),
                     MatrixKeys.BOOL_MATRIX_TEMPLATE + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL);
            container.Register(typeof(IViewModel), typeof(ListMatrixVariableOptionTemplateEditorViewModel),
                MatrixKeys.LIST_MATRIX_TEMPLATE + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL);



            container.Register(typeof(IConfigurationItemViewModel), typeof(RuntimeAppointableMatrixViewModel),
                ConfigurationKeys.RUNTIME + ConfigurationKeys.APPOINTABLE_MATRIX +
                ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);


            container.Register<IMatrixTemplateEditorViewModel, MatrixTemplateEditorViewModel>();

            container.Register<IFormattedValueViewModel, MatrixValueViewModel>(
                 MatrixKeys.MATRIX_VALUE +
                ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);


            container.Register<IFormattedValueViewModel, EditableMatrixValueViewModel>(
                ApplicationGlobalNames.CommonInjectionStrings.EDITABLE +
                MatrixKeys.MATRIX_VALUE +
                ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);


            container.Register<IMatrixTemplateEditorViewModel, MatrixTemplateEditorViewModel>();

            container.Register<IMatrixMemoryVariableEditorViewModel, MatrixMemoryVariableEditorViewModel>();
            container.Register<IMatrixMemoryVariableEditorViewModelFactory, MatrixMemoryVariableEditorViewModelFactory>();
            container.Register<IVariableSignatureEditorViewModel, VariableSignatureEditorViewModel>();
            container.Register<IVariableSignatureEditorViewModelFactory, VariableSignatureEditorViewModelFactory>();
            container.Register<IMatrixVariableOptionTemplateEditorViewModelFactory, MatrixVariableOptionTemplateEditorViewModelFactory>();

            container.Register<IOptionPossibleValueEditorViewModel, OptionPossibleValueEditorViewModel>();
            container.Register<IPossibleValueConditionEditorViewModel, PossibleValueConditionEditorViewModel>();
            container.Register<IBitOptionEditorViewModel, BitOptionEditorViewModel>();

            container.Register<IAssignedBitEditorViewModel, AssignedBitEditorViewModel>();
        
            ISerializerService serializerService = container.Resolve<ISerializerService>();
            serializerService.AddKnownTypeForSerialization(typeof(AppointableMatrix));
            serializerService.AddKnownTypeForSerialization(typeof(DefaultMatrixTemplate));
            serializerService.AddKnownTypeForSerialization(typeof(DefaultMatrixMemoryVariable));
            serializerService.AddKnownTypeForSerialization(typeof(DefaultVariableColumnSignature));
            serializerService.AddKnownTypeForSerialization(typeof(ListMatrixVariableOptionTemplate));
            serializerService.AddKnownTypeForSerialization(typeof(BoolMatrixVariableOptionTemplate));
            serializerService.AddKnownTypeForSerialization(typeof(OptionPossibleValue));
            serializerService.AddKnownTypeForSerialization(typeof(PossibleValueCondition));
            serializerService.AddKnownTypeForSerialization(typeof(BoolMatrixBitOption));
            serializerService.AddKnownTypeForSerialization(typeof(ListMatrixBitOption));
            serializerService.AddKnownTypeForSerialization(typeof(MatrixValueFormatter));

            serializerService.AddNamespaceAttribute("appointableMatrix", "AppointableMatrixNS");
            serializerService.AddNamespaceAttribute("matrixValueFormatter", "MatrixValueFormatterNS");


            //регистрация ресурсов
            container.Resolve<IXamlResourcesService>().AddResourceAsGlobal("Resources/MatrixDataTemplates.xaml", this.GetType().Assembly);

        }

    }
}
