using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Base;
using Unicon2.Infrastructure;

namespace Unicon2.Fragments.Configuration.ViewModel
{
    public class GroupWithReiterationViewModel : ConfigurationItemViewModelBase
    {
        public GroupWithReiterationViewModel()
        {

        }

        public override string TypeName => ConfigurationKeys.GROUP_WITH_REITERATION;

        public override string StrongName => ConfigurationKeys.GROUP_WITH_REITERATION +
                                            ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;
    }
}