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
        private readonly Func<PropertyEditorViewModel> _propertyViewModelFactory;
        private readonly Func<IFormatterParametersViewModel> _formatterParametersViewModelFactory;

        public ImportPropertiesFromExcelTypeAHelper(IExcelImporter excelImporter, ILocalizerService localizerService,
            Func<ComplexPropertyEditorViewModel> complexPropertyEditorViewModelFactory,Func<IFormatterParametersViewModel> formatterParametersViewModelFactory, Func<PropertyEditorViewModel> propertyViewModelFactory)
        {
            _excelImporter = excelImporter;
            _localizerService = localizerService;
            _formatterParametersViewModelFactory = formatterParametersViewModelFactory;
            _propertyViewModelFactory = propertyViewModelFactory;
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


            List<ImportedPropertyFromBits> importedProperties = new List<ImportedPropertyFromBits>();


            while (propertyName.IsSuccess &&
                   !string.IsNullOrEmpty(propertyName.Item))
            {
                if (!importedProperties.Any() || importedProperties.Last().SubpropertiesNames.Count == 16)
                {
                    importedProperties.Add(new ImportedPropertyFromBits(
                        _localizerService.GetLocalizedString("Word") + " " + (importedProperties.Count + 1)));
                }

                var importedComplexProperty = importedProperties.Last();
                importedComplexProperty.SubpropertiesNames.Add(propertyName.Item);
                rowCounter++;
                propertyName = worksheet.GetCellValue(rowCounter, 3);
            }

            foreach (var importedProperty in importedProperties)
            {

                foreach (var subpropertyName in importedProperty.SubpropertiesNames)
                {
                    var subPropertyViewModel = _propertyViewModelFactory();

                    var bitViewModel =
                        subPropertyViewModel.BitNumbersInWord[
                            15 - importedProperty.SubpropertiesNames.IndexOf(subpropertyName)];
                    subPropertyViewModel.BitNumbersInWord.First(model => model.BitNumber == bitViewModel.BitNumber).IsChecked=true;
                    subPropertyViewModel.Address = importedProperties.IndexOf(importedProperty).ToString();
                    subPropertyViewModel.Parent = configurationGroupEditorViewModel;
                    subPropertyViewModel.Name = subpropertyName;
                    subPropertyViewModel.FormatterParametersViewModel = _formatterParametersViewModelFactory();
                    subPropertyViewModel.FormatterParametersViewModel.RelatedUshortsFormatterViewModel =
                        StaticContainer.Container.Resolve<IUshortsFormatterViewModel>(StringKeys.BOOL_FORMATTER + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
                    configurationGroupEditorViewModel.ChildStructItemViewModels.Add(subPropertyViewModel);
                    configurationGroupEditorViewModel.IsCheckable = true;
                }

            }

        }

        private class ImportedPropertyFromBits
        {
            public ImportedPropertyFromBits(string name)
            {
                Name = name;
                SubpropertiesNames = new List<string>();
            }

            public string Name { get; }
            public List<string> SubpropertiesNames { get; }
        }
    }

}