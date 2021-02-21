using System;
using Unicon2.Infrastructure.Functional;

namespace Unicon2.Infrastructure.Interfaces.Excel
{
    public interface IExcelImporter
    {
        Result ImportFromExcel(Action<IExcelWorksheet> onImport);
    }

}