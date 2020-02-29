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
        private readonly Func<string, IReiterationSubGroupInfo> _reiterationSubGroupInfoFactory;
        private string _name;
        private int _addressStep;
        private readonly IItemsGroup _model;
        private IGroupWithReiterationInfo _groupWithReiterationInfo;
        private StringWrapper _selectedSubGroupName;

        public GroupWithReiterationEditorViewModel(IConfigurationGroupEditorViewModel parent, Action closeWindow,
            Func<IReiterationSubGroupInfo> reiterationSubGroupInfoFactory)
        {
            _closeWindow = closeWindow;
            _reiterationSubGroupInfoFactory = name =>
            {
                var res = reiterationSubGroupInfoFactory();
                res.Name = name;
                return res;
            };
           // _model = parent.Model as IItemsGroup;
            Parent = parent;
            SubmitCommand = new RelayCommand(OnSubmit);
            CancelCommand = new RelayCommand(OnCancel);
            Name = Parent.Header;
            _groupWithReiterationInfo = ((IGroupWithReiterationInfo) _model.GroupInfo);
            AddressStep = _groupWithReiterationInfo.ReiterationStep;
            SubGroupNames =
                new ObservableCollection<StringWrapper>(
                    _groupWithReiterationInfo.SubGroups.Select(info => new StringWrapper(info.Name)));
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
            _groupWithReiterationInfo.ReiterationStep = AddressStep;
            _groupWithReiterationInfo.SubGroups =
                SubGroupNames.Select((wrapper => _reiterationSubGroupInfoFactory(wrapper.StringValue))).ToList();
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
