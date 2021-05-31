using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Dependencies;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Configuration.Editor.ViewModels.Dependencies.Conditions
{
    public class CurrentPropertyBitCheckConditionViewModel : ViewModelBase, IConditionViewModel
    {
        public ushort BitNumber { get; set; }
        public IConditionViewModel Clone()
        {
            return new CurrentPropertyBitCheckConditionViewModel()
            {
                BitNumber = BitNumber
            };
        }

        public string StrongName => ConfigurationKeys.CURRENT_PROPERTY_BIT_CHECK_CONDITION;
    }
}
