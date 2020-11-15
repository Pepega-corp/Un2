using System.Collections.Generic;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;

namespace Unicon2.Fragments.Configuration.ViewModel.Properties
{
    public class RuntimeSubPropertyViewModel : RuntimePropertyViewModel, IRuntimeSubPropertyViewModel
    {
        protected override string GetTypeName()
        {
            return ConfigurationKeys.SUB_PROPERTY;
        }

        public List<int> BitNumbersInWord { get; set; }
    }
}