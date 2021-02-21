using System;
using System.Collections.Generic;
using System.Linq;
using Unicon2.Formatting.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Tree;
using Unicon2.Fragments.Configuration.Editor.ViewModels.Properties;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Interfaces.Excel;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.ViewModels;

namespace Unicon2.Fragments.Configuration.Editor.Helpers
{
    public class ImportPropertiesFromExcelTypeAHelper
    {
        private readonly IExcelImporter _excelImporter;
        private readonly ILocalizerService _localizerService;
        private readonly Func<ComplexPropertyEditorViewModel> _complexPropertyEditorViewModelFactory;
        private readonly Func<SubPropertyEditorViewModel> _subPropertyEditorViewModelFactory;
        private readonly Func<IFormatterParametersViewModel> _formatterParametersViewModelFactory;

        public ImportPropertiesFromExcelTypeAHelper(IExcelImporter excelImporter, ILocalizerService localizerService,
            Func<ComplexPropertyEditorViewModel> complexPropertyEditorViewModelFactory,  Func<SubPropertyEditorViewModel> subPropertyEditorViewModelFactory
            ,Func<IFormatterParametersViewModel> formatterParametersViewModelFactory)
        {
            _excelImporter = excelImporter;
            _localizerService = localizerService;
            _complexPropertyEditorViewModelFactory = complexPropertyEditorViewModelFactory;
            _subPropertyEditorViewModelFactory = subPropertyEditorViewModelFactory;
            _formatterParametersViewModelFactory = formatterParametersViewModelFactory;
        }

        public void ImportPropertiesToGroup(IConfigurationGroupEditorViewModel configurationGroupEditorViewModel)
        {
            _excelImporter.ImportFromExcel(worksheet =>
                OnImportFromExcel(worksheet, configurationGroupEditorViewModel));
        }

        private void OnImportFromExcel(IExcelWorksheet worksheet,
            IConfigurationGroupEditorViewModel configurationGroupEditorViewModel)
        {
            int rowCounter = 2;
            Result<string> propertyName = worksheet.GetCellValue(rowCounter, 3);


            List<ImportedComplexProperty> importedComplexProperties = new List<ImportedComplexProperty>();


            while (propertyName.IsSuccess &&
                   !string.IsNullOrEmpty(propertyName.Item))
            {
                if (!importedComplexProperties.Any() || importedComplexProperties.Last().SubpropertiesNames.Count == 16)
                {
                    importedComplexProperties.Add(new ImportedComplexProperty(
                        _localizerService.GetLocalizedString("Word") + " " + (importedComplexProperties.Count + 1)));
                }

                var importedComplexProperty = importedComplexProperties.Last();
                importedComplexProperty.SubpropertiesNames.Add(propertyName.Item);
                rowCounter++;
                propertyName = worksheet.GetCellValue(rowCounter, 3);
            }

            foreach (var importedComplexProperty in importedComplexProperties)
            {
                var complexProperty = _complexPropertyEditorViewModelFactory();
                complexProperty.IsGroupedProperty = false;
                complexProperty.Parent = configurationGroupEditorViewModel;
                configurationGroupEditorViewModel.ChildStructItemViewModels.Add(complexProperty);
                configurationGroupEditorViewModel.IsCheckable = true;
                complexProperty.Address = importedComplexProperties.IndexOf(importedComplexProperty).ToString();
                complexProperty.Name = importedComplexProperty.Name;
                
                foreach (var subpropertyName in importedComplexProperty.SubpropertiesNames)
                {
                    var subPropertyViewModel = _subPropertyEditorViewModelFactory();

                    subPropertyViewModel.BitNumbersInWord = complexProperty.MainBitNumbersInWordCollection;
                    var bitViewModel =
                        subPropertyViewModel.BitNumbersInWord[
                            15 - importedComplexProperty.SubpropertiesNames.IndexOf(subpropertyName)];
                    subPropertyViewModel.BitNumbersInWord.First(model => model.NumberOfBit == bitViewModel.NumberOfBit)
                        .ChangeValueByOwnerCommand.Execute(subPropertyViewModel);
                    subPropertyViewModel.Address = complexProperty.Address;
                    subPropertyViewModel.Parent = complexProperty;
                    subPropertyViewModel.Name = subpropertyName;
                    complexProperty.SubPropertyEditorViewModels.Add(subPropertyViewModel);
                    complexProperty.ChildStructItemViewModels.Add(subPropertyViewModel);
                    subPropertyViewModel.FormatterParametersViewModel = _formatterParametersViewModelFactory();
                    subPropertyViewModel.FormatterParametersViewModel.RelatedUshortsFormatterViewModel =
                        StaticContainer.Container.Resolve<IUshortsFormatterViewModel>(StringKeys.BOOL_FORMATTER + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
                    complexProperty.IsCheckable = true;
                }

            }

        }

        private class ImportedComplexProperty
        {
            public ImportedComplexProperty(string name)
            {
                Name = name;
                SubpropertiesNames = new List<string>();
            }

            public string Name { get; }
            public List<string> SubpropertiesNames { get; }
        }
    }

}