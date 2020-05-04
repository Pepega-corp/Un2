using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Factories;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Tree;
using Unicon2.Fragments.Configuration.Editor.View;
using Unicon2.Fragments.Configuration.Editor.Visitors;
using Unicon2.Fragments.Configuration.Infrastructure.Factories;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Unity.Commands;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Configuration.Editor.ViewModels.Properties
{
    public class ComplexPropertyEditorViewModel : PropertyEditorViewModel, IComplexPropertyEditorViewModel
    {
        private readonly IApplicationGlobalCommands _applicationGlobalCommands;
        private readonly Func<ISharedBitViewModel> _sharedBitViewModelGettingFunc;
        private bool _isGroupedProperty;

        public ComplexPropertyEditorViewModel(ITypesContainer container, IRangeViewModel rangeViewModel,
            ILocalizerService localizerService, IApplicationGlobalCommands applicationGlobalCommands,
            Func<ISharedBitViewModel> sharedBitViewModelGettingFunc) : base(container, rangeViewModel, localizerService)
        {
            _applicationGlobalCommands = applicationGlobalCommands;
            _sharedBitViewModelGettingFunc = sharedBitViewModelGettingFunc;
            SubPropertyEditorViewModels = new ObservableCollection<ISubPropertyEditorViewModel>();
            SubmitCommand = new RelayCommand<object>(OnSubmit);
            CancelCommand = new RelayCommand<object>(OnCancel);
            MainBitNumbersInWordCollection = new ObservableCollection<ISharedBitViewModel>();
            for (int i = 15; i >= 0; i--)
            {
                ISharedBitViewModel sharedBitViewModel = sharedBitViewModelGettingFunc();
                sharedBitViewModel.NumberOfBit = i;
                MainBitNumbersInWordCollection.Add(sharedBitViewModel);
            }
        }

        private void OnSubmit(object obj)
        {
            StopEditElement();
            (obj as Window)?.Close();
        }

        private void OnCancel(object obj)
        {
            //this.SetModel(this._model);
            StopEditElement();
            (obj as Window)?.Close();
        }

        public ObservableCollection<ISubPropertyEditorViewModel> SubPropertyEditorViewModels { get; set; }
        public ObservableCollection<ISharedBitViewModel> MainBitNumbersInWordCollection { get; set; }

        public bool IsGroupedProperty
        {
            get { return _isGroupedProperty; }
            set
            {
                _isGroupedProperty = value;
                RaisePropertyChanged();
            }
        }

        public ICommand SubmitCommand { get; set; }
        public ICommand CancelCommand { get; set; }


       
      

        public override string StrongName => ConfigurationKeys.COMPLEX_PROPERTY +
                                             ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL;

        public override void StartEditElement()
        {
            base.StartEditElement();
            _applicationGlobalCommands.ShowWindowModal(() => new ComplexPropertyEditorWindow(), this);
        }

        protected override string GetTypeName()
        {
            return ConfigurationKeys.COMPLEX_PROPERTY;
        }

        public bool GetIsSetElementPossible(IConfigurationItemViewModel element, bool isUp)
        {
            if (ChildStructItemViewModels.Contains(element))
            {
                int startIndex = ChildStructItemViewModels.IndexOf(element as IEditorConfigurationItemViewModel);
                return isUp ? startIndex > 0 : ChildStructItemViewModels.Count - 1 > startIndex;
            }

            return false;
        }


        public bool SetElement(IConfigurationItemViewModel element, bool isUp)
        {
			if (this.ChildStructItemViewModels.Contains(element))
			{
				int moveIndexFrom =
					this.ChildStructItemViewModels.IndexOf(element as IEditorConfigurationItemViewModel);
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
					this.SubPropertyEditorViewModels.Move(moveIndexFrom, moveIndexTo);
					return true;
				}
				else
				{
					throw new Exception("invalid data input");
				}
			}

			return false;
        }


		public IConfigurationItemViewModel AddSubProperty()
		{
			IEditorConfigurationItemViewModel subPropertyViewModel =
				ConfigurationItemEditorViewModelFactory.Create().WithParent(this).VisitSubProperty(null);
			(subPropertyViewModel as ISubPropertyEditorViewModel).BitNumbersInWord =
				this.MainBitNumbersInWordCollection;

			this.SubPropertyEditorViewModels.Add(subPropertyViewModel as ISubPropertyEditorViewModel);
			this.ChildStructItemViewModels.Add(subPropertyViewModel);
			return subPropertyViewModel;
		}


		public void RemoveChildItem(IEditorConfigurationItemViewModel configurationItemViewModelToRemove)
        {
			ISubPropertyEditorViewModel subPropertyEditorViewModelToRemove= configurationItemViewModelToRemove as ISubPropertyEditorViewModel;
			this.SubPropertyEditorViewModels.Remove(subPropertyEditorViewModelToRemove);
			this.ChildStructItemViewModels.Remove(subPropertyEditorViewModelToRemove);
		}

		public override object Clone()
		{
			var cloneEditorViewModel = new ComplexPropertyEditorViewModel(_container,
				_rangeViewModel.Clone() as IRangeViewModel, _localizerService,_applicationGlobalCommands,_sharedBitViewModelGettingFunc)
			{
				Address = Address,
				IsMeasureUnitEnabled = IsMeasureUnitEnabled,
				NumberOfPoints = NumberOfPoints,
				IsRangeEnabled = IsRangeEnabled,
				FormatterParametersViewModel = FormatterParametersViewModel?.Clone() as IFormatterParametersViewModel,
				Header = Header,
				Name = Name,
				MeasureUnit = MeasureUnit,
				IsGroupedProperty = IsGroupedProperty
			};

			foreach (var subPropertyEditorViewModel in SubPropertyEditorViewModels)
			{
				var subPropertyEditorViewModelClone = subPropertyEditorViewModel.Clone() as ISubPropertyEditorViewModel;
				subPropertyEditorViewModelClone.BitNumbersInWord = cloneEditorViewModel.MainBitNumbersInWordCollection;
				foreach (var bitViewModel in subPropertyEditorViewModel.BitNumbersInWord)
				{
					if (bitViewModel.Owner == subPropertyEditorViewModel && bitViewModel.Value)
					{
						subPropertyEditorViewModelClone.BitNumbersInWord.First(model => model.NumberOfBit==bitViewModel.NumberOfBit).ChangeValueByOwnerCommand.Execute(subPropertyEditorViewModelClone);
					}
				}

				subPropertyEditorViewModelClone.Parent = cloneEditorViewModel;
				cloneEditorViewModel.SubPropertyEditorViewModels.Add(subPropertyEditorViewModelClone);
				cloneEditorViewModel.ChildStructItemViewModels.Add(subPropertyEditorViewModelClone);
				cloneEditorViewModel.IsCheckable = true;
			}

			return cloneEditorViewModel;
		}

		public override T Accept<T>(IConfigurationItemViewModelVisitor<T> visitor)
        {
            return visitor.VisitComplexProperty(this);
        }
    }
}