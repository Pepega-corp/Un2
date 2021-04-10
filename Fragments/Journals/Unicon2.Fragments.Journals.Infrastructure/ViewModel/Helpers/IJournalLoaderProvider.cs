using Unicon2.Fragments.Journals.Infrastructure.Model;
using Unicon2.Fragments.Journals.Infrastructure.Model.Loader;
using Unicon2.Presentation.Infrastructure.DeviceContext;

namespace Unicon2.Fragments.Journals.Infrastructure.ViewModel.Helpers
{
    public interface IJournalLoaderProvider
    {
        IJournalLoader GetJournalLoader(DeviceContext deviceContext, IUniconJournal uniconJournal, LoaderHooks loaderHooks);
    }
}