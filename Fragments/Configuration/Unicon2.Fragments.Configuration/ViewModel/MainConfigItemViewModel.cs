using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Presentation.Infrastructure.TreeGrid;

namespace Unicon2.Fragments.Configuration.ViewModel
{
   public class MainConfigItemViewModel
    {

        public MainConfigItemViewModel(IEnumerable<MainConfigItemViewModel> childConfigItemViewModels, IConfigurationItemViewModel relatedConfigurationItemViewModel)
        {
            ChildConfigItemViewModels = childConfigItemViewModels;
            RelatedConfigurationItemViewModel = relatedConfigurationItemViewModel;
        }

        public IEnumerable<MainConfigItemViewModel> ChildConfigItemViewModels { get; }

        public IConfigurationItemViewModel RelatedConfigurationItemViewModel { get; }
    }
}
