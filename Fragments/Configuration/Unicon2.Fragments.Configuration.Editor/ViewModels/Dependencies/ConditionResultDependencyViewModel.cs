using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Editor.Interfaces;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Dependencies;
using Unicon2.Presentation.Infrastructure.ViewModels.Dependencies;
using Unicon2.Unity.Interfaces;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Configuration.Editor.ViewModels.Dependencies
{
    public class ConditionResultDependencyViewModel : ViewModelBase, IDependencyViewModel
    {
        private readonly ITypesContainer _typesContainer;
        private IResultViewModel _selectedResultViewModel;
        private IConditionViewModel _selectedConditionViewModel;

        public ConditionResultDependencyViewModel(ITypesContainer typesContainer)
        {
            _typesContainer = typesContainer;
            ConditionViewModels = typesContainer.ResolveAll<IConditionViewModel>().ToList();
            ResultViewModels = typesContainer.ResolveAll<IResultViewModel>().ToList();
        }

        public List<IResultViewModel> ResultViewModels { get; }
        public List<IConditionViewModel> ConditionViewModels { get; }

        public IConditionViewModel SelectedConditionViewModel
        {
            get => _selectedConditionViewModel;
            set
            {
                _selectedConditionViewModel = value;
                RaisePropertyChanged();
            }
        }

        public IResultViewModel SelectedResultViewModel
        {
            get => _selectedResultViewModel;
            set
            {
                _selectedResultViewModel = value;
                RaisePropertyChanged();
            }
        }

        public IDependencyViewModel Clone()
        {
            return new ConditionResultDependencyViewModel(_typesContainer)
            {
                SelectedConditionViewModel = this.SelectedConditionViewModel.Clone(),
                SelectedResultViewModel = SelectedResultViewModel.Clone()
            };
        }

        public string Name { get; }
    }
}