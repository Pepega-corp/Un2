using System;
using System.Threading.Tasks;
using Unicon2.Infrastructure.Functional;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;

namespace Unicon2.Fragments.Journals.ViewModel.Helpers
{
    public static class JournalLoadHelper
    {
        public static Func<IFragmentViewModel, Task<Result<object>>> GetJournalLoadingHelper()
        {
            return async (fragment) =>
            {
                if(fragment is UniconJournalViewModel journalViewModel)
                {                    
                    if (!journalViewModel.DeviceContext.DataProviderContainer.DataProvider.IsSuccess)
                    {
                        return Result<object>.Create(false);
                    }
                    await journalViewModel.LoadJournal();
                    return Result<object>.Create(journalViewModel.UniconJournal,true);
                }
                return Result<object>.Create(false);
            };

        }
    }
}