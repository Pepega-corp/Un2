using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Dependencies;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Tree;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Configuration.Editor.ViewModels.Dependencies.Conditions
{
    public class CompareResourceConditionViewModel : ViewModelBase, IConditionViewModel, IConditionWithResourceViewModel
    {
        private string _selectedCondition;
        private ushort _ushortValueToCompare;
        private string _referencedResourcePropertyName;
        private readonly ISharedResourcesGlobalViewModel _sharedResourcesGlobalViewModel;

        public ICommand SelectPropertyFromResourceCommand { get; }


        public CompareResourceConditionViewModel(ISharedResourcesGlobalViewModel sharedResourcesGlobalViewModel,
            List<string> conditionsList)
        {
            _sharedResourcesGlobalViewModel = sharedResourcesGlobalViewModel;
            SelectPropertyFromResourceCommand = new RelayCommand(OnSelectPropertyFromResourceExecute);
            ConditionsList = conditionsList;
        }

        private void OnSelectPropertyFromResourceExecute()
        {
            ReferencedResourcePropertyName = _sharedResourcesGlobalViewModel
                .OpenSharedResourcesForSelectingString<IPropertyEditorViewModel>();
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


        public List<string> ConditionsList { get; }

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
            return new CompareResourceConditionViewModel(_sharedResourcesGlobalViewModel, ConditionsList.Select(s => s).ToList())
            {
                SelectedCondition = this.SelectedCondition,
                UshortValueToCompare = _ushortValueToCompare,
                ReferencedResourcePropertyName = _referencedResourcePropertyName
            };
        }

        public string StrongName => ConfigurationKeys.COMPARE_RESOURCE_CONDITION;
    }
}