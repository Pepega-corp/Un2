using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Configuration.Editor.ViewModels
{
    public class SetIsFromBitsToManyPropsViewModel : ViewModelBase
    {
        private readonly ConfigurationEditorViewModel _parent;
        private bool _isEnabled;

        public SetIsFromBitsToManyPropsViewModel(ConfigurationEditorViewModel parent)
        {
            _parent = parent;
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            private set
            {
                _isEnabled = value;
                RaisePropertyChanged();
            }
        }

        public void Update()
        {

        }
    }
}
