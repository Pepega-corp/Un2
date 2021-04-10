using System.Threading.Tasks;
using Unicon2.Infrastructure.Functional;

namespace Unicon2.Fragments.Journals.Infrastructure.Model.Loader
{
    public interface ISequenceLoader
    {
        bool GetIsNextRecordAvailable();
        Task<Result<ushort[]>> GetNextRecordUshorts();
    }

    public class SequenceLoaderContext
    {

    }

}