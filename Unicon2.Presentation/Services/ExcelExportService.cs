using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OfficeOpenXml;
using Unicon2.Formatting.Editor.Views;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Interfaces.Excel;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.ViewModels;

namespace Unicon2.Presentation.Services
{
    public class ExcelExportService : IExcelExporter, IExcelImporter
    {
        private readonly ISerializerService _serializerService;
        private readonly IApplicationGlobalCommands _applicationGlobalCommands;
        private readonly ILocalizerService _localizerService;

        public ExcelExportService(ISerializerService serializerService,
            IApplicationGlobalCommands applicationGlobalCommands, ILocalizerService localizerService)
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
                    if (windowViewModel.IsCancelled)
                    {
                        return ResultUtils.Nothing;
                    }
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


   
        public async Task<Result> ExportToExcel(Action<IExcelWorksheet> onFillWorksheet, string listName)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                //Add a new worksheet to the empty workbook
                var worksheet = package.Workbook.Worksheets.Add(listName);
                onFillWorksheet(new WorksheetAdapter(worksheet));

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Set some document properties
                package.Workbook.Properties.Title = listName;

                var filePath = _applicationGlobalCommands.SelectFilePathToSave("", ".xlsx", "", listName + ".xlsx");
                FileInfo fileInfo = new FileInfo(filePath.GetFirstValue());

                // Save our new workbook in the output directory and we are done!
                await package.SaveAsAsync(fileInfo);
            }
            return Result.Create(true);
        }

        public Result ImportFromExcel(Action<IExcelWorksheet> onImport)
        {
            return _applicationGlobalCommands.SelectFileToOpen("", "")
                .OnSuccess(info =>
                {
                    try
                    {
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        using (ExcelPackage package = new ExcelPackage(info))
                        {
                            GetWorksheet(package).OnSuccess(worksheet =>
                            {
                                onImport(new WorksheetAdapter(worksheet));
                            });
                        }

                        return Result.Create(true);
                    }
                    catch (Exception e)
                    {
                        return Result.Create(e);
                    }
                });
        }


        private class WorksheetAdapter : IExcelWorksheet
        {
            private readonly ExcelWorksheet _excelWorksheet;

            public WorksheetAdapter(ExcelWorksheet excelWorksheet)
            {
                _excelWorksheet = excelWorksheet;
            }

            public Result<string> GetCellValue(int rowNum, int columnNum)
            {
                try
                {
                    return _excelWorksheet.Cells[rowNum, columnNum].Value.ToString();
                }
                catch (Exception e)
                {
                    return Result<string>.CreateWithException(e);
                }
            }

            public Result SetCellValue(int rowNum, int columnNum, string value)
            {
                try
                {
                    _excelWorksheet.Cells[rowNum, columnNum].Value = value;
                    return Result.Create(true);
                }
                catch (Exception e)
                {
                    return Result.Create(e);
                }
            }
        }
    }
}