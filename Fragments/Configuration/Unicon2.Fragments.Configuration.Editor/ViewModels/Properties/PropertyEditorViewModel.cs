using System;
using System.Windows.Input;
using Unicon2.Fragments.Configuration.Editor.Interfaces.EditOperations;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Tree;
using Unicon2.Fragments.Configuration.Editor.ViewModels.Validators;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.Factories;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Unity.Commands;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Configuration.Editor.ViewModels.Properties
{
    public class PropertyEditorViewModel : EditorConfigurationItemViewModelBase, IPropertyEditorViewModel
    {
        private readonly ITypesContainer _container;
        private readonly ILocalizerService _localizerService;
        protected bool _isInEditMode;
        private string _address;
        private string _numberOfPoints;
        private string _measureUnit;
        private IRangeViewModel _rangeViewModel;
        private bool _isRangeEnabled;
        private bool _isMeasureUnitEnabled;
        private ushort _addressIteratorValue;

        public PropertyEditorViewModel(ITypesContainer container, IRangeViewModel rangeViewModel, ILocalizerService localizerService)
        {
            this._container = container;
            this._localizerService = localizerService;
            this.ShowFormatterParameters = new RelayCommand(this.OnShowFormatterParametersExecute);
            this.RangeViewModel = rangeViewModel;
            this.IncreaseAddressCommand = new RelayCommand(() =>
             {
                 //((IProperty)this._model).Address += AddressIteratorValue;
                 //this.Address = ((IProperty)this._model).Address.ToString();
             });
            this.DecreaseAddressCommand = new RelayCommand(() =>
            {
                //((IProperty)this._model).Address -= AddressIteratorValue;
                //this.Address = ((IProperty)this._model).Address.ToString();
            });
        }


        private void OnShowFormatterParametersExecute()
        {
            this._container.Resolve<IFormatterEditorFactory>().EditFormatterByUser(this);
	        this.RaisePropertyChanged(nameof(this.SelectedUshortFormatterName));
        }
        
        public ICommand ShowFormatterParameters { get; set; }
        public IUshortsFormatterViewModel RelatedUshortsFormatterViewModel { get; set; }

        public string SelectedUshortFormatterName => RelatedUshortsFormatterViewModel?.StrongName;



        public virtual string Address
        {
            get { return this._address; }
            set
            {
                this._address = value;
                this.RaisePropertyChanged();
                this.FireErrorsChanged();
            }
        }

        public virtual string NumberOfPoints
        {
            get { return this._numberOfPoints; }
            set
            {
                this._numberOfPoints = value;
                this.RaisePropertyChanged();
                this.FireErrorsChanged();
            }
        }

        public  ushort AddressIteratorValue
        {
            get { return this._addressIteratorValue; }
            set
            {
                this._addressIteratorValue = value;
                this.RaisePropertyChanged();
                this.FireErrorsChanged();
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

        public virtual void StartEditElement()
        {
            this.IsInEditMode = true;
        }

        public virtual void StopEditElement()
        {
            this.IsInEditMode = false;
        }

        public void DeleteElement()
        {
            if (this.Parent != null)
            {
                if (this.Parent is IChildItemRemovable)
                {
                 //   (this.Parent as IChildItemRemovable).RemoveChildItem((this._model as IProperty));
                }
            }
        }

        protected override void OnDisposing()
        {
            base.OnDisposing();
        }

        public override string TypeName => this.GetTypeName();


        protected virtual string GetTypeName()
        {
            return ConfigurationKeys.DEFAULT_PROPERTY;
        }


        public virtual string StrongName => ConfigurationKeys.DEFAULT_PROPERTY +
                                             ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL;


      


        public string MeasureUnit
        {
            get { return this._measureUnit; }
            set
            {
                this._measureUnit = value;
                this.RaisePropertyChanged();
            }
        }

        public bool IsMeasureUnitEnabled
        {
            get { return this._isMeasureUnitEnabled; }
            set
            {
                this._isMeasureUnitEnabled = value;
                this.RaisePropertyChanged();
            }
        }

        public bool IsRangeEnabled
        {
            get { return this._isRangeEnabled; }
            set
            {
                this._isRangeEnabled = value;
                this.RaisePropertyChanged();
            }
        }

        public IRangeViewModel RangeViewModel
        {
            get { return this._rangeViewModel; }
            set
            {
                this._rangeViewModel = value;
                this.RaisePropertyChanged();
            }
        }

        protected override void OnValidate()
        {
            FluentValidation.Results.ValidationResult res = (new PropertyEditorEditorViewModelValidator(this._localizerService)).Validate(this);
            this.SetValidationErrors(res);
        }

        public override object Clone()
        {
            PropertyEditorViewModel cloneEditorViewModel = new PropertyEditorViewModel(this._container, this._rangeViewModel.Clone() as IRangeViewModel, this._localizerService);
         

            return cloneEditorViewModel;
        }

        public override T Accept<T>(IConfigurationItemViewModelVisitor<T> visitor)
        {
            return visitor.VisitProperty(this);
        }

        public ICommand IncreaseAddressCommand { get; }
        public ICommand DecreaseAddressCommand { get; }
    }
}