using System.Threading.Tasks;

namespace Unicon2.Fragments.Journals.MemoryAccess
{
    public interface ISequenceLoader
    {
        bool GetIsNextRecordAvailable();
        Task<ushort[]> GetNextRecordUshorts();
    }

  
}