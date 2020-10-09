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
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Unity.Commands;
using Unicon2.Unity.Common;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Configuration.Editor.ViewModels
{
    public class ConfigurationGroupEditorViewModel : EditorConfigurationItemViewModelBase, IConfigurationGroupEditorViewModel
    {
        private bool _isInEditMode;
        private bool _isTableViewAllowed;
        private bool _isMain;
        private bool _isGroupWithReiteration;
        private int _reiterationStep;

        public ConfigurationGroupEditorViewModel()
        {
	        SubGroupNames=new ObservableCollection<StringWrapper>();
        }

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
                GroupWithReiterationEditorWindowFactory.StartEditingGroupWithReiteration(this);
            }
        }

        public void StopEditElement()
        {
            IsInEditMode = false;
        }



        public void DeleteElement()
        {
            (this.Parent as IChildItemRemovable)?.RemoveChildItem(this);

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
					element.Checked?.Invoke(false);
                    ChildStructItemViewModels.Move(moveIndexFrom, moveIndexTo);
                    //}
					this.Checked?.Invoke(false);
					this.Checked?.Invoke(true);
					return true;
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


        
        public void PasteAsChild(object itemToPaste)
        {
            if (itemToPaste is IEditorConfigurationItemViewModel editorConfigurationItemViewModel)
            {
                ChildStructItemViewModels.Add(editorConfigurationItemViewModel);
                editorConfigurationItemViewModel.Parent = this;

            }
        }

        public override object Clone()
        {
            ConfigurationGroupEditorViewModel cloneEditorViewModel = new ConfigurationGroupEditorViewModel()
            {
				IsMain = IsMain,
	            IsTableViewAllowed = IsTableViewAllowed,
				Header = Header
            };
            cloneEditorViewModel.SetIsGroupWithReiteration(IsGroupWithReiteration);
            cloneEditorViewModel.ReiterationStep = ReiterationStep;
            cloneEditorViewModel.SubGroupNames.AddCollection(SubGroupNames.Select(wrapper => new StringWrapper(wrapper.StringValue)).ToList());
            ChildStructItemViewModels.ForEach(model =>
            {
	            var child = (model as ICloneable).Clone() as IConfigurationItemViewModel;
	            child.Parent = cloneEditorViewModel;
				cloneEditorViewModel.IsCheckable = true;
	            cloneEditorViewModel.ChildStructItemViewModels.Add(
		            child);
            });
        
            return cloneEditorViewModel;
        }

        public override T Accept<T>(IConfigurationItemViewModelVisitor<T> visitor)
        {
            return visitor.VisitItemsGroup(this);
        }


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

            }
        }

        public ObservableCollection<StringWrapper> SubGroupNames { get; }

        public int ReiterationStep
        {
            get => _reiterationStep;
            set
            {
                _reiterationStep = value; 
                RaisePropertyChanged();
            }
        }

        public void SetIsGroupWithReiteration(bool value)
        {
			SetProperty(ref _isGroupWithReiteration, value);
			RaisePropertyChanged(nameof(TypeName));

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