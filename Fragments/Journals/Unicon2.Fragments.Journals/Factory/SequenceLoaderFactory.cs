using System;
using Unicon2.Fragments.Journals.Infrastructure.Model.LoadingSequence;
using Unicon2.Fragments.Journals.MemoryAccess;
using Unicon2.Fragments.Journals.Model.JournalLoadingSequence;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.Journals.Factory
{
    public class SequenceLoaderFactory
    {
        public ISequenceLoader CreateSequenceLoader(IJournalLoadingSequence journalLoadingSequence,
            IDataProviderContaining dataProviderContaining)
        {
            switch (journalLoadingSequence)
            {
                case IndexLoadingSequence indexLoadingSequence:
                    return new IndexLoadingSequenceLoader(indexLoadingSequence, dataProviderContaining);
                case OffsetLoadingSequence offsetLoadingSequence:
                    return new OffsetLoadingSequenceLoader(offsetLoadingSequence, dataProviderContaining);
            }

            throw new Exception();
        }
    }
}