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
            IDataProviderContainer dataProviderContainer)
        {
            switch (journalLoadingSequence)
            {
                case IndexLoadingSequence indexLoadingSequence:
                    return new IndexLoadingSequenceLoader(indexLoadingSequence, dataProviderContainer);
                case OffsetLoadingSequence offsetLoadingSequence:
                    return new OffsetLoadingSequenceLoader(offsetLoadingSequence, dataProviderContainer);
            }

            throw new Exception();
        }
    }
}