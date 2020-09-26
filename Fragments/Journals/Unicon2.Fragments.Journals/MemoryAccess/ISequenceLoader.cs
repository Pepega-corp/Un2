using System.Threading.Tasks;
using Unicon2.Infrastructure.Functional;

namespace Unicon2.Fragments.Journals.MemoryAccess
{
    public interface ISequenceLoader
    {
        bool GetIsNextRecordAvailable();
        Task<Result<ushort[]>> GetNextRecordUshorts();
    }

  
}