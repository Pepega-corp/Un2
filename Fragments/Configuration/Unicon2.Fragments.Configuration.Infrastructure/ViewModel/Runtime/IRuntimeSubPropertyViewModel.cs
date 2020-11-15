using System.Collections.Generic;

namespace Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime
{
    public interface IRuntimeSubPropertyViewModel : IRuntimePropertyViewModel
	{
        List<int> BitNumbersInWord { get; set; }
    }
}