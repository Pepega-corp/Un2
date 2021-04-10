using System.Collections.Generic;
using Unicon2.Fragments.Journals.Infrastructure.ViewModel;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Model;

namespace Unicon2.Fragments.Oscilliscope.Infrastructure.ViewModel
{
    public interface IOscilloscopeJournalViewModel:IUniconJournalViewModel
    {
        List<int> SelectedRows { get; set; }
        void SetParentOscilloscopeModel(IOscilloscopeModel oscilloscopeModel,
            IOscilloscopeViewModel oscilloscopeViewModel);
    }
}