using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Dependencies;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Tree;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Configuration.Editor.ViewModels.Dependencies.Conditions
{
    public class RegexMatchConditionViewModel:ViewModelBase,IConditionViewModel,IConditionWithResourceViewModel
    {
        private string _regexPattern;
        public ICommand SelectPropertyFromResourceCommand { get; }

        public RegexMatchConditionViewModel(ISharedResourcesGlobalViewModel sharedResourcesGlobalViewModel)
        {
            _sharedResourcesGlobalViewModel = sharedResourcesGlobalViewModel;
            SelectPropertyFromResourceCommand = new RelayCommand(OnSelectPropertyFromResourceExecute);
        }
        private void OnSelectPropertyFromResourceExecute()
        {
            ReferencedResourcePropertyName = _sharedResourcesGlobalViewModel
                .OpenSharedResourcesForSelectingString<IPropertyEditorViewModel>();
        }

        public string RegexPattern
        {
            get => _regexPattern;
            set
            {
                _regexPattern = value;
                RaisePropertyChanged();
            }
        }

        private string _referencedResourcePropertyName;
        private ISharedResourcesGlobalViewModel _sharedResourcesGlobalViewModel;

        public string ReferencedResourcePropertyName
        {
            get => _referencedResourcePropertyName;
            set
            {
                _referencedResourcePropertyName = value;
                RaisePropertyChanged();
            }
        }

        public IConditionViewModel Clone()
        {
           return new RegexMatchConditionViewModel(_sharedResourcesGlobalViewModel)
           {
               ReferencedResourcePropertyName = ReferencedResourcePropertyName,
               RegexPattern = RegexPattern
           };
        }

        public string StrongName => ConfigurationKeys.REGEX_MATCH_CONDITION;
    }
}