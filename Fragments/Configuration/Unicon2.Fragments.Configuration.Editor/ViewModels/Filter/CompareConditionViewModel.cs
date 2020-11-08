using System.Collections.Generic;
using System.Linq;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Dependencies;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Configuration.Editor.ViewModels.Filter
{
    public class CompareConditionViewModel:ViewModelBase,IConditionViewModel
    {
        private ushort _ushortValueToCompare;
        private string _selectedCondition;


        public CompareConditionViewModel(List<string> conditionsList)
        {
            ConditionsList = conditionsList;
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
            return new CompareConditionViewModel(ConditionsList.Select(s => s).ToList())
            {
                SelectedCondition = this.SelectedCondition,
                UshortValueToCompare = _ushortValueToCompare
            };
        }

        public string StrongName => "CompareConditionViewModel";
    }
}