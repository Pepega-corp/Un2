using System;
using System.Collections.Generic;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Dependencies;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Configuration.ViewModel
{
    public class RuntimeFilterViewModel:ViewModelBase
    {
        private readonly Action _onActivatedChange;
        private bool _isActivated;

        public RuntimeFilterViewModel(string name,Action onActivatedChange, List<ICondition> conditions)
        {
            _onActivatedChange = onActivatedChange;
            Conditions = conditions;
            Name = name;
        }
        public string Name { get; }

        public bool IsActivated
        {
            get => _isActivated;
            set
            {
                _isActivated = value;
                RaisePropertyChanged();
                _onActivatedChange();
            }
        }
        public List<ICondition> Conditions { get; }
    }


}