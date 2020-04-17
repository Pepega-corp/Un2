using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Tree;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Configuration.Editor.ViewModels
{
    public class GroupWithReiterationEditorViewModel : ViewModelBase
    {
        private readonly Action _closeWindow;
        private string _name;
        private int _addressStep;
        private StringWrapper _selectedSubGroupName;

        public GroupWithReiterationEditorViewModel(IConfigurationGroupEditorViewModel parent, Action closeWindow)
        {
            _closeWindow = closeWindow;
            Parent = parent;
            SubmitCommand = new RelayCommand(OnSubmit);
            CancelCommand = new RelayCommand(OnCancel);
            Name = Parent.Header;
            AddressStep = Parent.ReiterationStep;
            SubGroupNames =
                new ObservableCollection<StringWrapper>(
                    Parent.SubGroupNames.Select(info => new StringWrapper(info.StringValue)));
            AddSubGroupCommand = new RelayCommand(OnAddSubGroup);
            RemoveSubGroupCommand = new RelayCommand(OnRemoveSubGroup, CanExecuteRemoveSubGroup);

        }

        private bool CanExecuteRemoveSubGroup()
        {
            return SelectedSubGroupName != null;
        }

        private void OnRemoveSubGroup()
        {
            SubGroupNames.Remove(SelectedSubGroupName);
            SelectedSubGroupName = null;
        }

        private void OnAddSubGroup()
        {
            SubGroupNames.Add(new StringWrapper("new"));
        }

        private void OnCancel()
        {
            Parent.StopEditElement();
            _closeWindow();
        }

        private void OnSubmit()
        {
            Parent.Header = Name;
            Parent.ReiterationStep = AddressStep;
            Parent.SubGroupNames.Clear();
		
                SubGroupNames.ForEach(wrapper => Parent.SubGroupNames.Add(new StringWrapper(wrapper.StringValue)));
            Parent.StopEditElement();
            _closeWindow();
        }

        public IConfigurationGroupEditorViewModel Parent { get; }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                RaisePropertyChanged();
            }
        }

        public int AddressStep
        {
            get => _addressStep;
            set
            {
                _addressStep = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<StringWrapper> SubGroupNames { get; }

        public ICommand AddSubGroupCommand { get; }

        public ICommand RemoveSubGroupCommand { get; }

        public ICommand SubmitCommand { get; }

        public ICommand CancelCommand { get; }

        public StringWrapper SelectedSubGroupName
        {
            get => _selectedSubGroupName;
            set
            {
                _selectedSubGroupName = value;
                RaisePropertyChanged();
                (RemoveSubGroupCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }
    }


}
