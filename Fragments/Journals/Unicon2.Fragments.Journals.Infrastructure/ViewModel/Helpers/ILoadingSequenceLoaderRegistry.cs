using System;
using Unicon2.Fragments.Journals.Infrastructure.Model.Loader;
using Unicon2.Fragments.Journals.Infrastructure.Model.LoadingSequence;
using Unicon2.Presentation.Infrastructure.DeviceContext;

namespace Unicon2.Fragments.Journals.Infrastructure.ViewModel.Helpers
{
    public interface ILoadingSequenceLoaderRegistry
    {
        void AddLoader<T>(Func<DeviceContext, IJournalLoadingSequence, ISequenceLoader> loader);
        ISequenceLoader ResolveLoader(IJournalLoadingSequence loadingSequence, DeviceContext deviceContext);
    }
}