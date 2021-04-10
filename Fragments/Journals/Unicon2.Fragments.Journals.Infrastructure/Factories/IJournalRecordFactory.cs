using System.Collections.Generic;
using System.Threading.Tasks;
using Unicon2.Fragments.Journals.Infrastructure.Model;
using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.DeviceContext;

namespace Unicon2.Fragments.Journals.Infrastructure.Factories
{
    public interface IJournalRecordFactory
    {
       Task<IJournalRecord> CreateJournalRecord(ushort[] values, IRecordTemplate recordTemplate, DeviceContext deviceContext);

       Task<List<IFormattedValue>> GetValuesForRecord(IJournalParameter parameter,
           ushort[] recordUshorts, DeviceContext deviceContext);
    }
}