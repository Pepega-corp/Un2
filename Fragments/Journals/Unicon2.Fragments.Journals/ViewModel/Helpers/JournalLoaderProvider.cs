using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Journals.Infrastructure.Model;
using Unicon2.Fragments.Journals.Infrastructure.Model.Loader;
using Unicon2.Fragments.Journals.Infrastructure.ViewModel;
using Unicon2.Fragments.Journals.Infrastructure.ViewModel.Helpers;
using Unicon2.Fragments.Journals.MemoryAccess;
using Unicon2.Presentation.Infrastructure.DeviceContext;

namespace Unicon2.Fragments.Journals.ViewModel.Helpers
{
    public class JournalLoaderProvider : IJournalLoaderProvider
    {
        public IJournalLoader GetJournalLoader(DeviceContext deviceContext,
            IUniconJournal uniconJournal, LoaderHooks loaderHooks)
        {
            return new JournalLoader(deviceContext, uniconJournal,loaderHooks);
        }
    }
}
