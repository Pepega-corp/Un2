using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Unicon2.Fragments.Configuration.Editor.Factories;
using Unicon2.Fragments.Configuration.Editor.Interfaces.EditOperations;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Factories;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Tree;
using Unicon2.Fragments.Configuration.Infrastructure.Factories;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Properties;
using Unicon2.Infrastructure;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Unity.Commands;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Configuration.Editor.ViewModels
{
    public class ConfigurationGroupEditorViewModel : EditorConfigurationItemViewModelBase, IConfigurationGroupEditorViewModel
    {
        private readonly ITypesContainer _container;
        private readonly IConfigurationItemEditorViewModelFactory _configurationItemEditorViewModelFactory;
        private readonly IConfigurationItemFactory _configurationItemFactory;
        private ObservableCollection<IPropertyViewModel> _properties;
        private bool _isInEditMode;
        private ushort _addressIteratorValue = 1;
        private bool _isTableViewAllowed;
        private bool _isMain;
        private bool _isGroupWithReiteration;

        public ConfigurationGroupEditorViewModel(ITypesContainer container,
            IConfigurationItemEditorViewModelFactory configurationItemEditorViewModelFactory,
            IConfigurationItemFactory configurationItemFactory)
        {
            this._container = container;
            this._configurationItemEditorViewModelFactory = configurationItemEditorViewModelFactory;
            this._configurationItemFactory = configurationItemFactory;
            this.IncreaseAddressCommand = new RelayCommand(this.OnIncreaseAddressExecute);
            this.DecreaseAddressCommand = new RelayCommand(this.OnDecreaseAddressExecute);

        }

        private void OnDecreaseAddressExecute()
        {
            foreach (IConfigurationItemViewModel childStructItemViewModel in this.ChildStructItemViewModels)
            {
                (childStructItemViewModel as IAddressIncreaseableDecreaseable).AddressIteratorValue = AddressIteratorValue;
                (childStructItemViewModel as IAddressIncreaseableDecreaseable)?.DecreaseAddressCommand?.Execute(null);
            }
        }

        private void OnIncreaseAddressExecute()
        {
            foreach (IConfigurationItemViewModel childStructItemViewModel in this.ChildStructItemViewModels)
            {
                (childStructItemViewModel as IAddressIncreaseableDecreaseable).AddressIteratorValue = AddressIteratorValue;
                (childStructItemViewModel as IAddressIncreaseableDecreaseable)?.IncreaseAddressCommand?.Execute(null);
            }
        }

        public ushort AddressIteratorValue
        {
            get
            {
                return this._addressIteratorValue;
            }
            set
            {
                this._addressIteratorValue = value;
                this.RaisePropertyChanged();
            }
        }

        public bool IsInEditMode
        {
            get { return this._isInEditMode; }
            set
            {
                this._isInEditMode = value;
                this.RaisePropertyChanged();
            }
        }

        public void StartEditElement()
        {
            this.IsInEditMode = true;
            if (IsGroupWithReiteration)
            {
                GroupWithReiterationEditorWindowFactory.StartEditingGroupWithReiteration(this, _configurationItemFactory);
            }
        }

        public void StopEditElement()
        {
            this.SaveModel();
            this.IsInEditMode = false;
        }



        public void DeleteElement()
        {
            (this.Parent as IChildItemRemovable)?.RemoveChildItem(this.Model as IConfigurationItem);
            this.Dispose();
        }

        public IConfigurationItemViewModel AddChildElement()
        {
            IConfigurationItem newProperty = this._configurationItemFactory.ResolveConfigurationItem();
            IEditorConfigurationItemViewModel newConfigurationItemViewModel =
                this._configurationItemEditorViewModelFactory.ResolveConfigurationItemEditorViewModel(newProperty,
                    parent: this);
            (this._model as IItemsGroup).ConfigurationItemList.Add(newProperty);
            this.ChildStructItemViewModels.Add(newConfigurationItemViewModel);
            return newConfigurationItemViewModel;
        }

        public IConfigurationItemViewModel AddChildGroupElement()
        {
            IConfigurationItem newGroupConfigurationItem = this._configurationItemFactory.ResolveGroupConfigurationItem();
            IEditorConfigurationItemViewModel newConfigurationItemViewModel =
                this._configurationItemEditorViewModelFactory.ResolveConfigurationItemEditorViewModel(
                    newGroupConfigurationItem,
                    parent: this);
            (this._model as IItemsGroup).ConfigurationItemList.Add(newGroupConfigurationItem);
            this.ChildStructItemViewModels.Add(newConfigurationItemViewModel);
            return newConfigurationItemViewModel;
        }

        public IConfigurationItemViewModel AddDependentProperty()
        {
            IConfigurationItem newDependentConfigurationItem = this._configurationItemFactory.ResolveDependentConfigurationItem();
            IEditorConfigurationItemViewModel newConfigurationItemViewModel =
                this._configurationItemEditorViewModelFactory.ResolveConfigurationItemEditorViewModel(
                    newDependentConfigurationItem,
                    parent: this);
            (this._model as IItemsGroup).ConfigurationItemList.Add(newDependentConfigurationItem);
            this.ChildStructItemViewModels.Add(newConfigurationItemViewModel);
            return newConfigurationItemViewModel;
        }

        public IConfigurationItemViewModel AddComplexProperty()
        {
            IConfigurationItem complexProperty = this._configurationItemFactory.ResolveComplexPropertyItem();
            IEditorConfigurationItemViewModel newConfigurationItemViewModel =
                this._configurationItemEditorViewModelFactory.ResolveConfigurationItemEditorViewModel(
                    complexProperty,
                    parent: this);
            (this._model as IItemsGroup).ConfigurationItemList.Add(complexProperty);
            this.ChildStructItemViewModels.Add(newConfigurationItemViewModel);
            return newConfigurationItemViewModel;
        }

        public IConfigurationItemViewModel AddMatrix()
        {
            IConfigurationItem matrix = this._configurationItemFactory.ResolveAppointableMatrix();
            IEditorConfigurationItemViewModel newConfigurationItemViewModel =
                this._configurationItemEditorViewModelFactory.ResolveConfigurationItemEditorViewModel(
                    matrix,
                    parent: this);
            (this._model as IItemsGroup).ConfigurationItemList.Add(matrix);
            this.ChildStructItemViewModels.Add(newConfigurationItemViewModel);
            return newConfigurationItemViewModel;
        }

        public bool GetIsSetElementPossible(IConfigurationItemViewModel element, bool isUp)
        {
            if (this.ChildStructItemViewModels.Contains(element))
            {
                if (isUp)
                {
                    if (this.ChildStructItemViewModels.IndexOf(element as IEditorConfigurationItemViewModel) >
                        0) return true;

                }
                else
                {
                    if (this.ChildStructItemViewModels.IndexOf(element as IEditorConfigurationItemViewModel) <
                        this.ChildStructItemViewModels.Count - 1) return true;
                }
            }

            return false;
        }

        public bool SetElement(IConfigurationItemViewModel element, bool isUp)
        {

            if (this.ChildStructItemViewModels.Contains(element))
            {
                int moveIndexFrom = this.ChildStructItemViewModels.IndexOf(element as IEditorConfigurationItemViewModel);
                int moveIndexTo;
                bool valid = false;
                if (isUp)
                {
                    moveIndexTo = moveIndexFrom - 1;
                    valid = (moveIndexTo >= 0) && (moveIndexFrom > 0);

                }
                else
                {
                    moveIndexTo = moveIndexFrom + 1;
                    valid = moveIndexFrom < this.ChildStructItemViewModels.Count - 1;
                }
                if (valid)
                {
                    this.ChildStructItemViewModels.Move(moveIndexFrom, moveIndexTo);
                    List<IConfigurationItem> modelItems = (this._model as IItemsGroup).ConfigurationItemList.ToList();
                    if (modelItems.Count == this.ChildStructItemViewModels.Count)
                    {
                        modelItems.Clear();
                        foreach (IConfigurationItemViewModel propViewModel in this.ChildStructItemViewModels)
                        {
                            modelItems.Add(propViewModel.Model as IConfigurationItem);
                        }
                        (this._model as IItemsGroup).ConfigurationItemList = modelItems;
                        return true;
                    }
                }
                else
                {
                    throw new Exception("invalid data input");
                }
            }

            return false;
        }


        public override string TypeName => IsGroupWithReiteration ? ConfigurationKeys.GROUP_WITH_REITERATION : ConfigurationKeys.DEFAULT_ITEM_GROUP;

        public override string StrongName => ConfigurationKeys.DEFAULT_ITEM_GROUP +
                                             ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL;


        protected override void SetModel(object model)
        {
            this.ChildStructItemViewModels.Clear();
            var itemGroup = (model as IItemsGroup);
            foreach (IConfigurationItem configurationItem in itemGroup.ConfigurationItemList)
            {
                this.ChildStructItemViewModels.Add(this._configurationItemEditorViewModelFactory
                    .ResolveConfigurationItemEditorViewModel(configurationItem, this));
            };
            IsCheckable = true;
            IsTableViewAllowed = itemGroup.IsTableViewAllowed;
            IsMain = itemGroup.IsMain ?? true;
            if (itemGroup.GroupInfo is IGroupWithReiterationInfo groupWithReiterationInfo)
            {
                _isGroupWithReiteration= groupWithReiterationInfo.IsReiterationEnabled;
            }
            base.SetModel(model);
        }


        protected override void SaveModel()
        {
            (this._model as IItemsGroup).ConfigurationItemList.Clear();
            foreach (IConfigurationItemViewModel configurationItem in this.ChildStructItemViewModels)
            {
                (this._model as IItemsGroup).ConfigurationItemList.Add(configurationItem.Model as IConfigurationItem);
            }

            base.SaveModel();
        }


        protected override object GetModel()
        {
            var itemsGroup = (_model as IItemsGroup);
            itemsGroup.ConfigurationItemList.Clear();
            foreach (var childStructItemViewModel in ChildStructItemViewModels)
            {
                itemsGroup.ConfigurationItemList.Add(childStructItemViewModel.Model as IConfigurationItem);
            }
            itemsGroup.IsMain = IsMain;
            itemsGroup.IsTableViewAllowed = IsTableViewAllowed;
            return base.GetModel();
        }


        public void RemoveChildItem(IConfigurationItem configurationItemToRemove)
        {
            this.ChildStructItemViewModels.Remove(
                this.ChildStructItemViewModels.First((model => model.Model == configurationItemToRemove)));
            (this._model as IItemsGroup).ConfigurationItemList.Remove(configurationItemToRemove);
        }

        public void PasteAsChild(object itemToPaste)
        {
            if (itemToPaste is IEditorConfigurationItemViewModel)
            {
                this.ChildStructItemViewModels.Add(itemToPaste as IEditorConfigurationItemViewModel);
            }
        }

        public override object Clone()
        {
            ConfigurationGroupEditorViewModel cloneEditorViewModel = new ConfigurationGroupEditorViewModel(this._container, this._configurationItemEditorViewModelFactory, this._configurationItemFactory);
            object buffModel = (this.Model as IItemsGroup).Clone();
            cloneEditorViewModel.Model = buffModel;
            return cloneEditorViewModel;
        }

        public ICommand IncreaseAddressCommand { get; }
        public ICommand DecreaseAddressCommand { get; }


        public bool IsTableViewAllowed
        {
            get => _isTableViewAllowed;
            set => SetProperty(ref _isTableViewAllowed, value);
        }

        #region Implementation of IConfigurationGroupEditorViewModel

        public bool IsMain
        {
            get => _isMain;
            set => SetProperty(ref _isMain, value);
        }

        public bool IsGroupWithReiteration
        {
            get => _isGroupWithReiteration;
            set
            {
                SetProperty(ref _isGroupWithReiteration, value);
                RaisePropertyChanged(nameof(TypeName));
                if (value)
                {
                    StartEditElement();
                }
                if ((_model as IItemsGroup)?.GroupInfo is IGroupWithReiterationInfo groupWithReiterationInfo)
                {
                    groupWithReiterationInfo.IsReiterationEnabled = value;
                }

            }
        }

        #endregion
    }
}