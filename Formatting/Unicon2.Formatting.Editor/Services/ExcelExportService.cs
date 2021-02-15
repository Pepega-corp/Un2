using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OfficeOpenXml;
using Unicon2.Formatting.Editor.ViewModels;
using Unicon2.Formatting.Editor.Views;
using Unicon2.Formatting.Infrastructure.Services;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Services;

namespace Unicon2.Formatting.Editor.Services
{
    public class ExcelExportService:IExcelExportService
    {
        private readonly ISerializerService _serializerService;
        private readonly IApplicationGlobalCommands _applicationGlobalCommands;
        private readonly ILocalizerService _localizerService;

        public ExcelExportService(ISerializerService serializerService, IApplicationGlobalCommands applicationGlobalCommands, ILocalizerService localizerService)
        {
            _serializerService = serializerService;
            _applicationGlobalCommands = applicationGlobalCommands;
            _localizerService = localizerService;
        }

        private Result<ExcelWorksheet> GetWorksheet(ExcelPackage package)
        {
            if (package.Workbook.Worksheets.Count == 0)
            {
                return Result<ExcelWorksheet>.Create(false);
            }

            if (package.Workbook.Worksheets.Count > 1)
            {
                try
                {
                    var windowViewModel = new ExcelListSelectWindowViewModel(package.Workbook.Worksheets
                        .Select(excelWorksheet => excelWorksheet.Name.ToString()).ToList());

                    _applicationGlobalCommands.ShowWindowModal(() => new ExcelListSelectWindow(),
                        windowViewModel);
                    return package.Workbook.Worksheets.First(worksheet =>
                        worksheet.Name == windowViewModel.SelectedExcelList);
                }
                catch (Exception e)
                {
                    return Result<ExcelWorksheet>.CreateWithException(e);
                }
            }
            else
            {
                return package.Workbook.Worksheets[0];
            }
        }

        public Result<Dictionary<ushort, string>> GetDictionaryFromFile()
        {
            return _applicationGlobalCommands.SelectFileToOpen("", "")
                .OnSuccess(info =>
                {
                    try
                    {
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        using (ExcelPackage package = new ExcelPackage(info))
                        {
                            return GetWorksheet(package).OnSuccess(worksheet =>
                            {
                                if (string.IsNullOrWhiteSpace(worksheet.Cells[1, 3].Value.ToString()))
                                {
                                    return Result<Dictionary<ushort, string>>.Create(false);
                                }

                                try
                                {
                                    var dictionary = new Dictionary<ushort, string>();
                                    int counter = 2;
                                    while (worksheet.Cells[counter, 1].Value != null &&
                                           !string.IsNullOrWhiteSpace(worksheet.Cells[counter, 1].Value.ToString()))
                                    {
                                        dictionary[ushort.Parse(worksheet.Cells[counter, 2].Value.ToString())] =
                                            worksheet.Cells[counter, 3].Value.ToString();
                                        counter++;
                                    }

                                    return Result<Dictionary<ushort, string>>.Create(dictionary, true);
                                }
                                catch (Exception e)
                                {
                                    return Result<Dictionary<ushort, string>>.CreateWithException(
                                        new Exception(
                                            _localizerService.GetLocalizedString(ApplicationGlobalNames.StatusMessages
                                                .FORMAT_ERROR)));
                                }


                            });
                        }
                    }
                    catch (Exception e)
                    {
                        return Result<Dictionary<ushort, string>>.CreateWithException(e);
                    }
                });
        }

        public async Task SaveDictionaryToFile(Dictionary<ushort, string> dictionary, string name)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                //Add a new worksheet to the empty workbook
                var worksheet = package.Workbook.Worksheets.Add(name);
                //Add the headers
                worksheet.Cells[1, 1].Value = "№";
                worksheet.Cells[1, 2].Value = "Ключ";
                worksheet.Cells[1, 3].Value = "Значение";
                int counter = 0;
                foreach (var keyValue in dictionary)
                {
                    counter++;
                    worksheet.Cells[counter + 1, 1].Value = counter;
                    worksheet.Cells[counter + 1, 2].Value = keyValue.Key;
                    worksheet.Cells[counter + 1, 3].Value = keyValue.Value;

                }

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Set some document properties
                package.Workbook.Properties.Title = name;

                var filePath = _applicationGlobalCommands.SelectFilePathToSave("", ".xlsx", "", name + ".xlsx");
                FileInfo fileInfo = new FileInfo(filePath.GetFirstValue());

                // Save our new workbook in the output directory and we are done!
                await package.SaveAsAsync(fileInfo);
            }
        }
    }
}