namespace Unicon2.Infrastructure.Interfaces.Excel
{
    public interface IExcelWorksheet
    {
        Functional.Result<string> GetCellValue(int rowNum, int columnNum);
        Functional.Result SetCellValue(int rowNum, int columnNum, string value);

    }
}