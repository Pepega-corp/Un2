using System.Collections.Generic;
using System.Threading.Tasks;
using Unicon2.Formatting.Infrastructure.Model;
using Unicon2.Infrastructure.Functional;

namespace Unicon2.Formatting.Infrastructure.Services
{
    public interface IExcelExportService
    {
        Result<Dictionary<ushort, string>>GetDictionaryFromFile();
        Task SaveDictionaryToFile(Dictionary<ushort, string> dictionary, string name);
    }
}