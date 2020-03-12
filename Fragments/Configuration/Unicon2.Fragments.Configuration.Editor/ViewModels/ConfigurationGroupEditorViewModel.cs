using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Unicon2.Fragments.Configuration.Editor.Factories;
using Unicon2.Fragments.Configuration.Editor.Interfaces.EditOperations;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Factories;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Tree;
using Unicon2.Fragments.Configuration.Editor.Visitors;
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
        private bool _isInEditMode;
        private bool _isTableViewAllowed;
        private bool _isMain;
        private bool _isGroupWithReiteration;
		

        public bool IsInEditMode
        {
            get { return _isInEditMode; }
            set
            {
                _isInEditMode = value;
                RaisePropertyChanged();
            }
        }

        public void StartEditElement()
        {
            IsInEditMode = true;
            if (IsGroupWithReiteration)
            {
               // GroupWithReiterationEditorWindowFactory.StartEditingGroupWithReiteration(this, _configurationItemFactory);
            }
        }

        public void StopEditElement()
        {
            IsInEditMode = false;
        }



        public void DeleteElement()
        {
           // (this.Parent as IChildItemRemovable)?.RemoveChildItem(this.Model as IConfigurationItem);
            Dispose();
        }

        public IConfigurationItemViewModel AddChildElement()
        {
            IEditorConfigurationItemViewModel newConfigurationItemViewModel =
                ConfigurationItemEditorViewModelFactory.Create().WithParent(this).VisitProperty(null);
            ChildStructItemViewModels.Add(newConfigurationItemViewModel);
            return newConfigurationItemViewModel;
        }

        public IConfigurationItemViewModel AddChildGroupElement()
        {
            IEditorConfigurationItemViewModel newConfigurationItemViewModel =
                ConfigurationItemEditorViewModelFactory.Create().WithParent(this).VisitItemsGroup(null);
            ChildStructItemViewModels.Add(newConfigurationItemViewModel);
            return newConfigurationItemViewModel;
        }

        public IConfigurationItemViewModel AddDependentProperty()
        {
            IEditorConfigurationItemViewModel newConfigurationItemViewModel = ConfigurationItemEditorViewModelFactory
                .Create().WithParent(this).VisitDependentProperty(null);

            ChildStructItemViewModels.Add(newConfigurationItemViewModel);
            return newConfigurationItemViewModel;
        }

        public IConfigurationItemViewModel AddComplexProperty()
        {
            IEditorConfigurationItemViewModel newConfigurationItemViewModel =
                ConfigurationItemEditorViewModelFactory.Create().WithParent(this).VisitComplexProperty(null);
            ChildStructItemViewModels.Add(newConfigurationItemViewModel);
            return newConfigurationItemViewModel;
        }

        public IConfigurationItemViewModel AddMatrix()
        {
            IEditorConfigurationItemViewModel newConfigurationItemViewModel =
                ConfigurationItemEditorViewModelFactory.Create().WithParent(this).VisitMatrix(null);
            ChildStructItemViewModels.Add(newConfigurationItemViewModel);
            return newConfigurationItemViewModel;
        }

        public bool GetIsSetElementPossible(IConfigurationItemViewModel element, bool isUp)
        {
            if (ChildStructItemViewModels.Contains(element))
            {
                if (isUp)
                {
                    if (ChildStructItemViewModels.IndexOf(element as IEditorConfigurationItemViewModel) >
                        0) return true;

                }
                else
                {
                    if (ChildStructItemViewModels.IndexOf(element as IEditorConfigurationItemViewModel) <
                        ChildStructItemViewModels.Count - 1) return true;
                }
            }

            return false;
        }

        public bool SetElement(IConfigurationItemViewModel element, bool isUp)
        {

            if (ChildStructItemViewModels.Contains(element))
            {
                int moveIndexFrom = ChildStructItemViewModels.IndexOf(element as IEditorConfigurationItemViewModel);
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
                    valid = moveIndexFrom < ChildStructItemViewModels.Count - 1;
                }
                if (valid)
                {
                    ChildStructItemViewModels.Move(moveIndexFrom, moveIndexTo);
                    //List<IConfigurationItem> modelItems = (this._model as IItemsGroup).ConfigurationItemList.ToList();
                    //if (modelItems.Count == this.ChildStructItemViewModels.Count)
                    //{
                    //    modelItems.Clear();
                    //    foreach (IConfigurationItemViewModel propViewModel in this.ChildStructItemViewModels)
                    //    {
                    //        modelItems.Add(propViewModel.Model as IConfigurationItem);
                    //    }
                    //    (this._model as IItemsGroup).ConfigurationItemList = modelItems;
                    //    return true;
                    //}
                }
                else
                {
                    throw new Exception("invalid data input");
                }
            }

            return false;
        }


        public override string TypeName => IsGroupWithReiteration ? ConfigurationKeys.GROUP_WITH_REITERATION : ConfigurationKeys.DEFAULT_ITEM_GROUP;

        public virtual string StrongName => ConfigurationKeys.DEFAULT_ITEM_GROUP +
                                             ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL;


        
        public void PasteAsChild(object itemToPaste)
        {
            if (itemToPaste is IEditorConfigurationItemViewModel)
            {
                ChildStructItemViewModels.Add(itemToPaste as IEditorConfigurationItemViewModel);
            }
        }

        public override object Clone()
        {
            ConfigurationGroupEditorViewModel cloneEditorViewModel = new ConfigurationGroupEditorViewModel();
            //object buffModel = (this.Model as IItemsGroup).Clone();
            //cloneEditorViewModel.Model = buffModel;
            return cloneEditorViewModel;
        }

        public override T Accept<T>(IConfigurationItemViewModelVisitor<T> visitor)
        {
            return visitor.VisitItemsGroup(this);
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
                //if ((_model as IItemsGroup)?.GroupInfo is IGroupWithReiterationInfo groupWithReiterationInfo)
                //{
                //    groupWithReiterationInfo.IsReiterationEnabled = value;
                //}

            }
        }

        #endregion

        public void RemoveChildItem(IEditorConfigurationItemViewModel configurationItemViewModelToRemove)
        {
            ChildStructItemViewModels.Remove(
                ChildStructItemViewModels.First((model => model == configurationItemViewModelToRemove)));
        }

        public void ChangeAddress(ushort addressOffset, bool isIncrease)
        {
			foreach (IConfigurationItemViewModel childStructItemViewModel in ChildStructItemViewModels)
			{
				(childStructItemViewModel as IAddressChangeable)?.ChangeAddress(addressOffset, isIncrease);
			}
		}
    }
}