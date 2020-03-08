using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Unicon2.Fragments.Configuration.Editor.Factories;
using Unicon2.Fragments.Configuration.Editor.Interfaces;
using Unicon2.Fragments.Configuration.Editor.Interfaces.EditOperations;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Factories;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Tree;
using Unicon2.Fragments.Configuration.Editor.View;
using Unicon2.Fragments.Configuration.Editor.Visitors;
using Unicon2.Fragments.Configuration.Infrastructure.Factories;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.ElementAdding;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.DeviceInterfaces.SharedResources;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.EditOperations;
using Unicon2.Infrastructure.Interfaces.Factories;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentSettings;
using Unicon2.Unity.Commands;
using Unicon2.Unity.Interfaces;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Configuration.Editor.ViewModels
{
    public class ConfigurationEditorViewModel : ViewModelBase, IConfigurationEditorViewModel, IChildPositionChangeable
    {
        private readonly IApplicationGlobalCommands _applicationGlobalCommands;
        private ObservableCollection<IConfigurationItemViewModel> _allRows;
        private IEditorConfigurationItemViewModel _selectedRow;
        private IEditorConfigurationItemViewModel _bufferConfigurationItem;
        private string _deviceName;

        public ConfigurationEditorViewModel(
            IApplicationGlobalCommands applicationGlobalCommands,
            Func<IElementAddingCommand> elementAddingCommandAddingFunc,
            ISharedResourcesGlobalViewModel sharedResourcesGlobalViewModel)
        {
            this._allRows = new ObservableCollection<IConfigurationItemViewModel>();
            this._applicationGlobalCommands = applicationGlobalCommands;
            this.RootConfigurationItemViewModels = new ObservableCollection<IConfigurationItemViewModel>();
            this.ElementsAddingCommandCollection = new ObservableCollection<IElementAddingCommand>();
            this.AddRootElementCommand = new RelayCommand(this.OnAddRootElement);

            this.AddRootGroupElementCommand = new RelayCommand(this.OnAddRootGroupElementExecute);
            IElementAddingCommand command = elementAddingCommandAddingFunc();
            command.Name = "AddChildElement";
            command.AddingCommand = new RelayCommand(this.OnAddChildElementExecute, this.CanExecuteAddChildElement);
            this.ElementsAddingCommandCollection.Add(command);
            command = elementAddingCommandAddingFunc();
            command.Name = "AddChildGroup";
            command.AddingCommand = new RelayCommand(this.OnAddChildGroupElementExecute, this.CanExecuteAddChildGroupElement);
            this.ElementsAddingCommandCollection.Add(command);


            command = elementAddingCommandAddingFunc();
            command.Name = "AddDependentProperty";
            command.AddingCommand = new RelayCommand(this.OnAddDependentPropertyExecute, this.CanExecuteAddChildGroupElement);
            this.ElementsAddingCommandCollection.Add(command);

            command = elementAddingCommandAddingFunc();
            command.Name = "AddComplexProperty";
            command.AddingCommand = new RelayCommand(this.OnAddComplexPropertyExecute, this.CanExecuteAddChildGroupElement);
            this.ElementsAddingCommandCollection.Add(command);

            command = elementAddingCommandAddingFunc();
            command.Name = "AddSubProperty";
            command.AddingCommand = new RelayCommand(this.OnAddAddSubPropertyExecute, this.CanExecuteAddSubPropertyElement);
            this.ElementsAddingCommandCollection.Add(command);

            command = elementAddingCommandAddingFunc();
            command.Name = "AddMatrix";
            command.AddingCommand = new RelayCommand(this.OnAddMatrixExecute, this.CanExecuteAddChildGroupElement);
            this.ElementsAddingCommandCollection.Add(command);

            this.EditElementCommand = new RelayCommand(this.OnEditElementExecute, this.CanExecuteEditElement);
            this.DeleteElementCommand = new RelayCommand(this.OnDeleteElementExecute, this.CanExecuteDeleteElement);
            this.ShowFormatterParametersCommand =
                new RelayCommand(this.OnShowFormatterParametersExecute, this.CanExecuteShowFormatterParameters);
            this.SetElementDownCommand = new RelayCommand(this.OnSetElementDownExecute, this.CanExecuteSetElementDown);
            this.SetElementUpCommand = new RelayCommand(this.OnSetElementUpExecute, this.CanExecuteSetElementUp);
            this.OpenConfigurationSettingsCommand = new RelayCommand(this.OnOpenConfigurationSettingsExecute);
            this.CopyElementCommand = new RelayCommand(this.OnCopyElementExecute, this.CanExecuteCopyElement);
            this.PasteAsChildElementCommand =
                new RelayCommand(this.OnPasteAsChildElementExecute, this.CanPasteAsChildElementElement);

            this.AddSelectedElementAsResourceCommand = new RelayCommand(this.OnAddSelectedElementAsResourceExecute,
                this.CanExecuteAddSelectedElementAsResource);
            this.EditDescriptionCommand = new RelayCommand(this.OnEditDescriptionExecute, this.CanExecuteEditDescription);
        }

        private void OnAddMatrixExecute()
        {
            if (this.SelectedRow is IChildAddable)
            {
                IEditorConfigurationItemViewModel dependentProperty = (this.SelectedRow as IChildAddable).AddMatrix() as IEditorConfigurationItemViewModel;
                this.PrepareAdding();
                this.SelectedRow = dependentProperty;
                this.CompleteAdding();
            }
        }

        private void OnAddAddSubPropertyExecute()
        {
            if (this.SelectedRow is ISubPropertyAddable)
            {
                IEditorConfigurationItemViewModel dependentProperty = (this.SelectedRow as ISubPropertyAddable).AddSubProperty() as IEditorConfigurationItemViewModel;
                this.PrepareAdding();

                this.SelectedRow = dependentProperty;
                this.CompleteAdding();
            }
        }

        private bool CanExecuteAddSubPropertyElement()
        {
            return this.SelectedRow is ISubPropertyAddable;
        }

        private void OnAddComplexPropertyExecute()
        {
            if (this.SelectedRow is IChildAddable)
            {
                IEditorConfigurationItemViewModel dependentProperty = (this.SelectedRow as IChildAddable).AddComplexProperty() as IEditorConfigurationItemViewModel;
                this.PrepareAdding();
                this.SelectedRow = dependentProperty;
                this.CompleteAdding();
            }
        }

        private void OnEditDescriptionExecute()
        {
            this._applicationGlobalCommands.ShowWindowModal(() => new DescriptionEditingWindow(), new DescriptionEditingViewModel() { Item = SelectedRow });
        }

        private bool CanExecuteEditDescription()
        {
            return this.SelectedRow != null;
        }


        private void OnAddDependentPropertyExecute()
        {
            if (this.SelectedRow is IChildAddable)
            {
                IEditorConfigurationItemViewModel dependentProperty = (this.SelectedRow as IChildAddable).AddDependentProperty() as IEditorConfigurationItemViewModel;
                this.PrepareAdding();
                this.SelectedRow = dependentProperty;
            }
        }

        private bool CanExecuteAddSelectedElementAsResource()
        {
            return false;
            // return (this._selectedRow != null) && !this._deviceSharedResources.IsItemReferenced(this._selectedRow.);
        }

        private void OnAddSelectedElementAsResourceExecute()
        {
           // this._sharedResourcesViewModelFactory.AddSharedResource(this._selectedRow.Model as INameable);
        }

        private bool CanPasteAsChildElementElement()
        {
            return this.SelectedRow is IAsChildPasteable && this._bufferConfigurationItem != null;
        }

        private void OnPasteAsChildElementExecute()
        {
			if (this.SelectedRow is IAsChildPasteable)
			{

				IEditorConfigurationItemViewModel editorConfigurationItemViewModel =_bufferConfigurationItem.Clone() as IEditorConfigurationItemViewModel;
				(this.SelectedRow as IAsChildPasteable).PasteAsChild(editorConfigurationItemViewModel);

				this.PrepareAdding();
				this.SelectedRow = editorConfigurationItemViewModel;
				this.CompleteAdding();
			}
		}

        private bool CanExecuteCopyElement()
        {
            return this.SelectedRow is ICloneable;
        }

        private void OnCopyElementExecute()
        {
			if (this.SelectedRow is ICloneable)
			{
				this._bufferConfigurationItem = SelectedRow;
			}
		}

        private void OnOpenConfigurationSettingsExecute()
        {
            //IFragmentSettingsViewModel configurationSettingsViewModel =
            //    this._container.Resolve<IFragmentSettingsViewModel>();
            //if (this._deviceConfiguration.FragmentSettings == null)
            //{
            //    this._deviceConfiguration.FragmentSettings = this._container.Resolve<IFragmentSettings>();
            //}
            //configurationSettingsViewModel.Model = this._deviceConfiguration.FragmentSettings;
            //this._applicationGlobalCommands.ShowWindowModal(() => new ConfigurationSettingsView(), configurationSettingsViewModel);
        }

        private bool CanExecuteAddChildGroupElement()
        {
            return (this.SelectedRow is IChildAddable);
        }

        private void OnAddChildGroupElementExecute()
        {
            if (this.SelectedRow is IChildAddable)
            {
                IEditorConfigurationItemViewModel configurationItemViewModel = (this.SelectedRow as IChildAddable).AddChildGroupElement() as IEditorConfigurationItemViewModel;
                this.PrepareAdding();

                this.SelectedRow = configurationItemViewModel;
                this.CompleteAdding();
            }
        }

        private void OnAddRootGroupElementExecute()
        {
            IEditorConfigurationItemViewModel configurationItemViewModel =
                ConfigurationItemEditorViewModelFactory.Create().VisitItemsGroup(null);
            this.AllRows.Add(configurationItemViewModel);
            this.RootConfigurationItemViewModels.Add(configurationItemViewModel);
            this.SelectedRow = configurationItemViewModel;
            this.CompleteAdding();
        }

        private bool CanExecuteAddChildElement()
        {
            return (this.SelectedRow is IChildAddable);
        }

        private void OnAddChildElementExecute()
        {
            if (this.SelectedRow is IChildAddable)
            {

                IEditorConfigurationItemViewModel configurationEditorViewModel =
                    (this.SelectedRow as IChildAddable).AddChildElement() as IEditorConfigurationItemViewModel;
                this.PrepareAdding();
                this.SelectedRow = configurationEditorViewModel;
                this.CompleteAdding();
            }
        }

        private void OnAddRootElement()
        {
            IEditorConfigurationItemViewModel configurationItemViewModel =
                ConfigurationItemEditorViewModelFactory.Create().VisitProperty(null);
            this.AllRows.Add(configurationItemViewModel);
            this.RootConfigurationItemViewModels.Add(configurationItemViewModel);
            this.SelectedRow = configurationItemViewModel;
            this.CompleteAdding();
        }

        private void PrepareAdding()
        {
            this.SelectedRow.IsCheckable = true;
            this.SelectedRow?.Checked?.Invoke(true);
        }



        private void CompleteAdding()
        {
            this.OnEditElementExecute();
        }


        public IEditorConfigurationItemViewModel SelectedRow
        {
            get { return this._selectedRow; }
            set
            {
                if (this._selectedRow is IEditable)
                {
                    (this._selectedRow as IEditable).StopEditElement();
                }
                this._selectedRow = value;
                foreach (IElementAddingCommand elementAddingCommand in this.ElementsAddingCommandCollection)
                {
                    (elementAddingCommand.AddingCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
                (this.EditDescriptionCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (this.AddSelectedElementAsResourceCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (this.EditElementCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (this.DeleteElementCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (this.ShowFormatterParametersCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (this.SetElementDownCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (this.SetElementUpCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (this.CopyElementCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (this.PasteAsChildElementCommand as RelayCommand)?.RaiseCanExecuteChanged();
                this.RaisePropertyChanged();
            }
        }

        public ObservableCollection<IConfigurationItemViewModel> RootConfigurationItemViewModels { get; set; }

        public ObservableCollection<IConfigurationItemViewModel> AllRows
        {
            get { return this._allRows; }
            set
            {
                this._allRows = value;
                this.RaisePropertyChanged();
            }
        }

        public ICommand AddRootGroupElementCommand { get; set; }
        public ICommand AddRootElementCommand { get; set; }
        public ICommand EditElementCommand { get; set; }
        public ICommand DeleteElementCommand { get; set; }
        public ICommand ShowFormatterParametersCommand { get; set; }
        public ICommand SetElementUpCommand { get; set; }
        public ICommand SetElementDownCommand { get; set; }
        public ICommand OpenConfigurationSettingsCommand { get; set; }
        public ICommand CopyElementCommand { get; }
        public ICommand PasteAsChildElementCommand { get; }
        public ICommand AddSelectedElementAsResourceCommand { get; }
        public ICommand EditDescriptionCommand { get; }
        public ObservableCollection<IElementAddingCommand> ElementsAddingCommandCollection { get; set; }


        private bool CanExecuteShowFormatterParameters()
        {
            return (this.SelectedRow is IUshortFormattableEditorViewModel);
        }

        private void OnShowFormatterParametersExecute()
        {
	        (SelectedRow as IUshortFormattableEditorViewModel)?.FormatterParametersViewModel.ShowFormatterParameters();
        }

        private bool CanExecuteDeleteElement()
        {
            return (this.SelectedRow is IDeletable);
        }

        private bool CanExecuteEditElement()
        {

            return (this.SelectedRow is IEditable);
        }

        //private bool CanExecuteAddChildElement()
        //{
        //    return (SelectedRow is IChildAddable);
        //}


        private void OnDeleteElementExecute()
        {

            if (!(this.SelectedRow is IDeletable)) return;
            this.DeleteHeirarchicalRow(this.SelectedRow);
        }

        private void OnEditElementExecute()
        {
            if (this.SelectedRow is IEditable)
            {
                (this.SelectedRow as IEditable).StartEditElement();
            }
        }

        private bool CanExecuteSetElementUp()
        {
            if (this.SelectedRow == null) return false;
            if (this.RootConfigurationItemViewModels.Contains(this.SelectedRow))
            {
                return this.GetIsSetElementPossible(this.SelectedRow, true);
            }
            if (this.SelectedRow.Parent == null) return false;
            if (!(this.SelectedRow.Parent is IChildPositionChangeable)) return false;
            return (this.SelectedRow.Parent as IChildPositionChangeable).GetIsSetElementPossible(this.SelectedRow, true);
        }

        private bool CanExecuteSetElementDown()

        {
            if (this.SelectedRow == null) return false;
            if (this.RootConfigurationItemViewModels.Contains(this.SelectedRow))
            {
                return this.GetIsSetElementPossible(this.SelectedRow, false);
            }
            if (this.SelectedRow.Parent == null) return false;
            if (!(this.SelectedRow.Parent is IChildPositionChangeable)) return false;
            return (this.SelectedRow.Parent as IChildPositionChangeable).GetIsSetElementPossible(this.SelectedRow, false);
        }

        private void OnSetElementUpExecute()
        {
            if (this.SelectedRow == null) return;
            IEditorConfigurationItemViewModel selectedRowBuffer = this.SelectedRow;
            bool isElementSetted = false;
            if (this.RootConfigurationItemViewModels.Contains(this.SelectedRow))
            {
                this.SetElement(this.SelectedRow, true);
            }
            else
            {
                if (this.SelectedRow.Parent == null) return;
                if (!(this.SelectedRow.Parent is IChildPositionChangeable)) return;
                this.SelectedRow.Checked?.Invoke(false);
                isElementSetted = ((this.SelectedRow.Parent as IChildPositionChangeable).SetElement(this.SelectedRow, true));

                if (isElementSetted)
                {

                    this.SelectedRow.Parent.Checked?.Invoke(true);
                }
            }
            this.SelectedRow = selectedRowBuffer;

        }

        private void OnSetElementDownExecute()
        {
            if (this.SelectedRow == null) return;
            IEditorConfigurationItemViewModel selectedRowBuffer = this.SelectedRow;
            if (this.RootConfigurationItemViewModels.Contains(this.SelectedRow))
            {
                this.SetElement(this.SelectedRow, false);
            }
            else
            {
                if (this.SelectedRow.Parent == null) return;
                if (!(this.SelectedRow.Parent is IChildPositionChangeable)) return;
                this.SelectedRow.Checked?.Invoke(false);
                bool isElementSetted = (this.SelectedRow.Parent as IChildPositionChangeable).SetElement(this.SelectedRow, false);

                if (isElementSetted)
                {
                    this.SelectedRow.Parent.Checked?.Invoke(true);
                }
            }
            this.SelectedRow = selectedRowBuffer;
        }


        private void Save()
        {
            if (this.SelectedRow is IEditable)
            {
                if ((this.SelectedRow as IEditable).IsInEditMode)
                {
                    (this.SelectedRow as IEditable).StopEditElement();
                }
            }
        }

        private void DeleteHeirarchicalRow(IEditorConfigurationItemViewModel configurationItemViewModel)
        {
            if (configurationItemViewModel.ChildStructItemViewModels != null)
            {
                if (configurationItemViewModel.ChildStructItemViewModels is IEnumerable)
                {
                    List<IConfigurationItemViewModel> itemsToDelete = new List<IConfigurationItemViewModel>();
                    itemsToDelete.AddRange(configurationItemViewModel.ChildStructItemViewModels);
                    foreach (IConfigurationItemViewModel item in itemsToDelete)
                    {
                        if (item is IEditorConfigurationItemViewModel)
                        {
                            this.DeleteHeirarchicalRow(item as IEditorConfigurationItemViewModel);
                        }
                    }
                }
            }


            if (configurationItemViewModel is IDeletable)
            {
                //проверка на корневой элемент (у него нет родителя и он состоит в списке корневых элементов)
                if (configurationItemViewModel.Parent == null)
                {
                    if (this.RootConfigurationItemViewModels.Contains(configurationItemViewModel))
                    {
                        this.RootConfigurationItemViewModels.Remove(configurationItemViewModel);
                       // this._deviceConfiguration.RootConfigurationItemList.Remove(configurationItemViewModel.Model as IConfigurationItem);
                    }
                }
                else
                {
                    ((IDeletable)configurationItemViewModel).DeleteElement();
                }
            }
            this.AllRows.Remove(configurationItemViewModel);
        }


        public string StrongName => ApplicationGlobalNames.FragmentInjectcionStrings.CONFIGURATION + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL;



        public string NameForUiKey => ApplicationGlobalNames.FragmentInjectcionStrings.CONFIGURATION;
        public IDeviceFragment BuildDeviceFragment()
        {
	        return ConfigurationFragmentFactory.CreateConfiguration(this);
        }


        public IFragmentOptionsViewModel FragmentOptionsViewModel { get; set; }
        public void Initialize(IDeviceFragment deviceFragment)
        {
            if (deviceFragment is IDeviceConfiguration deviceConfiguration)
            {
                this.RootConfigurationItemViewModels.Clear();
                this.AllRows.Clear();
                foreach (IConfigurationItem member in deviceConfiguration.RootConfigurationItemList)
                {
                    IEditorConfigurationItemViewModel itemEditorViewModel =
                        member.Accept(ConfigurationItemEditorViewModelFactory.Create());
                    this.RootConfigurationItemViewModels.Add(itemEditorViewModel);
                    this.AllRows.Add(itemEditorViewModel);
                }
            }
        }

        public void SetResources(IDeviceSharedResources deviceSharedResources)
        {
           // this._deviceSharedResources = deviceSharedResources;
        }

        public bool GetIsSetElementPossible(IConfigurationItemViewModel element, bool isUp)
        {
            int indexOfElement = this.RootConfigurationItemViewModels.IndexOf(element);
            int itemsCount = this.RootConfigurationItemViewModels.Count;
            if (isUp)
            {
                return indexOfElement > 0;
            }
            else
            {
                return indexOfElement < itemsCount - 1;
            }
        }

        public bool SetElement(IConfigurationItemViewModel element, bool isUp)
        {
            int indexOfElement = this.RootConfigurationItemViewModels.IndexOf(element);
            int newIndexOfElement = isUp ? indexOfElement - 1 : indexOfElement + 1;
            if (element.IsChecked)
            {
                element.Checked?.Invoke(false);
            }
            IConfigurationItemViewModel replaceableElement = this.RootConfigurationItemViewModels[newIndexOfElement];
            if (replaceableElement.IsChecked)
            {
                replaceableElement.Checked?.Invoke(false);
            }

            this.RootConfigurationItemViewModels.Move(indexOfElement, newIndexOfElement);
            this.AllRows.Move(this.AllRows.IndexOf(replaceableElement), this.AllRows.IndexOf(element));
            return true;
        }

        public void RemoveChildItem(IEditorConfigurationItemViewModel configurationItemViewModelToRemove)
        {
            this.RootConfigurationItemViewModels.Remove(
                this.RootConfigurationItemViewModels.First((model => model == configurationItemViewModelToRemove)));
        }
    }
}