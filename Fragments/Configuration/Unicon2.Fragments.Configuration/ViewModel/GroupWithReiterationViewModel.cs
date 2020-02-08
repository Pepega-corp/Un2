using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Base;
using Unicon2.Infrastructure;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Configuration.ViewModel
{
    public class GroupWithReiterationViewModel: ConfigurationItemViewModelBase
    {
        public GroupWithReiterationViewModel()
        {
            
        }
        public override string TypeName => ConfigurationKeys.GROUP_WITH_REITERATION;
        public override string StrongName => ConfigurationKeys.GROUP_WITH_REITERATION+ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;
    }
}