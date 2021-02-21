using System;
using System.Threading.Tasks;
using Unicon2.Infrastructure.Functional;

namespace Unicon2.Infrastructure.Interfaces.Excel
{
    public interface IExcelExporter
    {
        Task<Result> ExportToExcel(Action<IExcelWorksheet> onFillWorksheet, string listName);
    }
}