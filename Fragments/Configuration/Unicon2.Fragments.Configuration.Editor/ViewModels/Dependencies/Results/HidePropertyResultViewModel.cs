using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Dependencies;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Configuration.Editor.ViewModels.Dependencies.Results
{
    public class HidePropertyResultViewModel : ViewModelBase, IResultViewModel
    {
        public IResultViewModel Clone()
        {
            return new HidePropertyResultViewModel();
        }

        public string StrongName => ConfigurationKeys.HIDE_PROPERTY_RESULT;
    }

}
