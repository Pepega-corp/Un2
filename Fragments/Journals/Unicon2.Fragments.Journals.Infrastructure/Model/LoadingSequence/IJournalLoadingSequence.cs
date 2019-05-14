using System.Threading.Tasks;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.Journals.Infrastructure.Model.LoadingSequence
{
    public interface IJournalLoadingSequence : IDataProviderContaining, IStronglyNamed
    {
        void Initialize(IJournalSequenceInitializingParameters journalSequenceInitializingParameters);
        Task<bool> GetIsNextRecordAvailable();
        Task<ushort[]> GetNextRecordUshorts();
        void ResetSequence();
    }

}
