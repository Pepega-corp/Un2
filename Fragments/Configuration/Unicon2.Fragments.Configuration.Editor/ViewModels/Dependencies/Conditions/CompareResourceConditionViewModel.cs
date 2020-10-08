using System;
using System.Collections.Generic;
using System.Windows.Input;
using Unicon2.Fragments.Configuration.Editor.Interfaces;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Dependencies;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Tree;
using Unicon2.Infrastructure.Interfaces.Dependancy;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Configuration.Editor.ViewModels.Dependencies.Conditions
{
    public class CompareResourceConditionViewModel : ViewModelBase, IConditionViewModel
    {
        private string _selectedCondition;
        private ushort _ushortValueToCompare;
        private string _referencedResourcePropertyName;
        private readonly ISharedResourcesGlobalViewModel _sharedResourcesGlobalViewModel;

        public ICommand SelectPropertyFromResourceCommand { get; }


        public CompareResourceConditionViewModel(ISharedResourcesGlobalViewModel sharedResourcesGlobalViewModel)
        {
            _sharedResourcesGlobalViewModel = sharedResourcesGlobalViewModel;
            SelectPropertyFromResourceCommand = new RelayCommand(OnSelectPropertyFromResourceExecute);
            ConditionsList = new List<string>(Enum.GetNames(typeof(ConditionsEnum))); 
        }
        private void OnSelectPropertyFromResourceExecute()
        {
            ReferencedResourcePropertyName = _sharedResourcesGlobalViewModel.OpenSharedResourcesForSelectingString<IPropertyEditorViewModel>();
        }

        public string ReferencedResourcePropertyName
        {
            get => _referencedResourcePropertyName;
            set
            {
                _referencedResourcePropertyName = value;
                RaisePropertyChanged();
            }
        }


        public List<string> ConditionsList { get; set; }

        public string SelectedCondition
        {
            get { return _selectedCondition; }
            set
            {
                _selectedCondition = value;
                RaisePropertyChanged();
            }
        }

        public ushort UshortValueToCompare
        {
            get { return _ushortValueToCompare; }
            set
            {
                _ushortValueToCompare = value;
                RaisePropertyChanged();
            }
        }

        public IConditionViewModel Clone()
        {
            return new CompareResourceConditionViewModel(_sharedResourcesGlobalViewModel)
            {
                SelectedCondition = this.SelectedCondition,
                UshortValueToCompare = _ushortValueToCompare,
                ReferencedResourcePropertyName = _referencedResourcePropertyName
            };
        }
    }
}