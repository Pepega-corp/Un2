using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Factories;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Tree;
using Unicon2.Fragments.Configuration.Editor.View;
using Unicon2.Fragments.Configuration.Infrastructure.Factories;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Unity.Commands;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Configuration.Editor.ViewModels.Properties
{
    public class ComplexPropertyEditorViewModel : PropertyEditorEditorViewModel, IComplexPropertyEditorViewModel
    {
        private readonly IConfigurationItemEditorViewModelFactory _configurationItemEditorViewModelFactory;
        private readonly IConfigurationItemFactory _configurationItemFactory;
        private readonly IApplicationGlobalCommands _applicationGlobalCommands;
        private bool _isGroupedProperty;

        public ComplexPropertyEditorViewModel(ITypesContainer container, IRangeViewModel rangeViewModel,
            ILocalizerService localizerService, IConfigurationItemEditorViewModelFactory configurationItemEditorViewModelFactory,
            IConfigurationItemFactory configurationItemFactory, IApplicationGlobalCommands applicationGlobalCommands,
            Func<ISharedBitViewModel> sharedBitViewModelGettingFunc) : base(container, rangeViewModel, localizerService)
        {
            this._configurationItemEditorViewModelFactory = configurationItemEditorViewModelFactory;
            this._configurationItemFactory = configurationItemFactory;
            this._applicationGlobalCommands = applicationGlobalCommands;
            this.SubPropertyEditorViewModels = new ObservableCollection<ISubPropertyEditorViewModel>();
            this.SubmitCommand = new RelayCommand<object>(this.OnSubmit);
            this.CancelCommand = new RelayCommand<object>(this.OnCancel);
            this.MainBitNumbersInWordCollection = new ObservableCollection<ISharedBitViewModel>();
            for (int i = 0; i < 16; i++)
            {
                ISharedBitViewModel sharedBitViewModel = sharedBitViewModelGettingFunc();
                sharedBitViewModel.NumberOfBit = i;
                this.MainBitNumbersInWordCollection.Add(sharedBitViewModel);
            }
        }

        private void OnSubmit(object obj)
        {
            this.StopEditElement();
            (obj as Window)?.Close();
        }

        private void OnCancel(object obj)
        {
            this.SetModel(this._model);
            this.StopEditElement();
            (obj as Window)?.Close();
        }

        #region Implementation of IComplexPropertyEditorViewModel

        public ObservableCollection<ISubPropertyEditorViewModel> SubPropertyEditorViewModels { get; set; }
        public ObservableCollection<ISharedBitViewModel> MainBitNumbersInWordCollection { get; set; }

        public bool IsGroupedProperty
        {
            get { return this._isGroupedProperty; }
            set
            {
                this._isGroupedProperty = value;
                this.RaisePropertyChanged();
            }
        }

        public ICommand SubmitCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        #endregion


        #region Overrides of PropertyEditorEditorViewModel

        protected override void SetModel(object model)
        {
            base.SetModel(model);
            if (model is IComplexProperty)
            {
                this.SubPropertyEditorViewModels.Clear();
                this.ChildStructItemViewModels.Clear();
                foreach (ISubProperty subProperty in (model as IComplexProperty).SubProperties)
                {
                    ISubPropertyEditorViewModel subPropertyEditorViewModel =
                        this._configurationItemEditorViewModelFactory.ResolveSubPropertyEditorViewModel(subProperty, this.MainBitNumbersInWordCollection, this) as ISubPropertyEditorViewModel;
                    this.SubPropertyEditorViewModels.Add(subPropertyEditorViewModel);
                    this.ChildStructItemViewModels.Add(subPropertyEditorViewModel);
                    this.IsCheckable = true;
                }
                this.IsGroupedProperty = (model as IComplexProperty).IsGroupedProperty;
            }
        }
        
        protected override void SaveModel()
        {
            IComplexProperty complexProperty = this._model as IComplexProperty;
            complexProperty.SubProperties.Clear();
            foreach (ISubPropertyEditorViewModel subPropertyEditorViewModel in this.SubPropertyEditorViewModels)
            {
                subPropertyEditorViewModel.StopEditElement();
                complexProperty.SubProperties.Add(subPropertyEditorViewModel.Model as ISubProperty);
            }
            complexProperty.IsGroupedProperty = this.IsGroupedProperty;
            base.SaveModel();
        }

        public override string StrongName => ConfigurationKeys.COMPLEX_PROPERTY +
                                             ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL;

        public override void StartEditElement()
        {
            base.StartEditElement();
            this._applicationGlobalCommands.ShowWindowModal(() => new ComplexPropertyEditorWindow(), this);
        }

        #region Overrides of PropertyEditorEditorViewModel

        protected override string GetTypeName()
        {
            return ConfigurationKeys.COMPLEX_PROPERTY;
        }

        #endregion

        #endregion
        
        #region Implementation of IChildPositionChangeable

        public bool GetIsSetElementPossible(IConfigurationItemViewModel element, bool isUp)
        {
            if (this.ChildStructItemViewModels.Contains(element))
            {
                int startIndex = this.ChildStructItemViewModels.IndexOf(element as IEditorConfigurationItemViewModel);
                return isUp ? startIndex > 0 : this.ChildStructItemViewModels.Count - 1 > startIndex;
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
                    List<ISubProperty> modelItems = new List<ISubProperty>();
                    foreach (IConfigurationItemViewModel propViewModel in this.ChildStructItemViewModels)
                    {
                        modelItems.Add(propViewModel.Model as ISubProperty);
                    }
                    (this._model as IComplexProperty).SubProperties = modelItems;
                    return true;
                }
                else
                {
                    throw new Exception("invalid data input");
                }
            }

            return false;
        }


        #endregion

        #region Implementation of ISubPropertyAddable

        public IConfigurationItemViewModel AddSubProperty()
        {
            IConfigurationItem subProperty = this._configurationItemFactory.ResolveSubPropertyItem();
            (this._model as IComplexProperty).SubProperties.Add(subProperty as ISubProperty);
            IEditorConfigurationItemViewModel subPropertyViewModel = this._configurationItemEditorViewModelFactory.ResolveConfigurationItemEditorViewModel(subProperty, this);
            (subPropertyViewModel as ISubPropertyEditorViewModel).BitNumbersInWord = this.MainBitNumbersInWordCollection;
            this.SubPropertyEditorViewModels.Add(subPropertyViewModel as ISubPropertyEditorViewModel);
            this.ChildStructItemViewModels.Add(subPropertyViewModel);
            return subPropertyViewModel;
        }


        #endregion

        #region Implementation of IChildItemRemovable

        public void RemoveChildItem(IConfigurationItem configurationItemToRemove)
        {
            ISubProperty subPropertyToRemove = configurationItemToRemove as ISubProperty;
            (this._model as IComplexProperty).SubProperties.Remove((this._model as IComplexProperty).SubProperties.First((property => property.Name == configurationItemToRemove.Name)));
            ISubPropertyEditorViewModel subPropertyEditorViewModelToRemove = this.SubPropertyEditorViewModels.First((model => (model.Model as ISubProperty).Name == configurationItemToRemove.Name));
            this.SubPropertyEditorViewModels.Remove(subPropertyEditorViewModelToRemove);
            this.ChildStructItemViewModels.Remove(subPropertyEditorViewModelToRemove);
        }

        #endregion
    }
}
