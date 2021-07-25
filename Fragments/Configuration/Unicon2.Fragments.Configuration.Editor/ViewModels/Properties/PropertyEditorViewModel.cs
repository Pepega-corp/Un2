﻿using System.Collections.ObjectModel;
using System.Linq;
using Unicon2.Formatting.Infrastructure.ViewModel;
using Unicon2.Fragments.Configuration.Editor.Interfaces.EditOperations;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Tree;
using Unicon2.Fragments.Configuration.Editor.ViewModels.Validators;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Presentation.Infrastructure.ViewModels.Dependencies;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Configuration.Editor.ViewModels.Properties
{
    public class PropertyEditorViewModel : EditorConfigurationItemViewModelBase, IPropertyEditorViewModel, ICanBeHidden
	{
		protected readonly ITypesContainer _container;
		protected readonly ILocalizerService _localizerService;
		protected bool _isInEditMode;
		private string _address;
		private string _numberOfPoints;
		private string _measureUnit;
		protected IRangeViewModel _rangeViewModel;
		private bool _isRangeEnabled;
		private bool _isMeasureUnitEnabled;
	    private IFormatterParametersViewModel _formatterParametersViewModel;
	    private ushort _numberOfWriteFunction;
        private bool _isHidden;

        public PropertyEditorViewModel(ITypesContainer container, IRangeViewModel rangeViewModel,
			ILocalizerService localizerService)
		{
			_container = container;
			_localizerService = localizerService;
			RangeViewModel = rangeViewModel;
			DependencyViewModels=new ObservableCollection<IDependencyViewModel>();
            NumberOfPoints = "1";
            BitNumbersInWord=new ObservableCollection<IBitViewModel>();

			for (int i = 15; i >= 0; i--)
            {
                IBitViewModel bitViewModel = new BitViewModel(i,true);
                BitNumbersInWord.Add(bitViewModel);
            }
		}



		public virtual string Address
		{
			get { return _address; }
			set
			{
				_address = value;
				RaisePropertyChanged();
				FireErrorsChanged();
			}
		}

		public virtual string NumberOfPoints
		{
			get { return _numberOfPoints; }
			set
			{
				_numberOfPoints = value;
				RaisePropertyChanged();
				FireErrorsChanged();
			}
		}


	    public ushort NumberOfWriteFunction
	    {
	        get => _numberOfWriteFunction;
	        set
	        {
	            _numberOfWriteFunction = value;
	            RaisePropertyChanged();

            }
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

		public virtual void StartEditElement()
		{
			IsInEditMode = true;
		}

		public virtual void StopEditElement()
		{
			IsInEditMode = false;
		}

		public void DeleteElement()
		{
			if (Parent != null)
			{
				if (Parent is IChildItemRemovable removable)
				{ 
					removable.RemoveChildItem(this);
				}
			}
		}

		protected override void OnDisposing()
		{
			base.OnDisposing();
		}

		public override string TypeName => GetTypeName();


		protected virtual string GetTypeName()
		{
			return ConfigurationKeys.DEFAULT_PROPERTY;
		}


		public override string StrongName => ConfigurationKeys.DEFAULT_PROPERTY +
		                                    ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL;





		public string MeasureUnit
		{
			get { return _measureUnit; }
			set
			{
				_measureUnit = value;
				RaisePropertyChanged();
			}
		}

		public bool IsMeasureUnitEnabled
		{
			get { return _isMeasureUnitEnabled; }
			set
			{
				_isMeasureUnitEnabled = value;
				RaisePropertyChanged();
			}
		}

		public bool IsRangeEnabled
		{
			get { return _isRangeEnabled; }
			set
			{
				_isRangeEnabled = value;
				RaisePropertyChanged();
			}
		}

		public IRangeViewModel RangeViewModel
		{
			get { return _rangeViewModel; }
			set
			{
				_rangeViewModel = value;
				RaisePropertyChanged();
			}
		}

		protected override void OnValidate()
		{
			FluentValidation.Results.ValidationResult res =
				(new PropertyEditorEditorViewModelValidator(_localizerService)).Validate(this);
			SetValidationErrors(res);
		}

		public override object Clone()
		{
			var cloneEditorViewModel = new PropertyEditorViewModel(_container,
				_rangeViewModel.Clone() as IRangeViewModel, _localizerService)
			{
				Address = Address,
				IsMeasureUnitEnabled = IsMeasureUnitEnabled,
				NumberOfPoints = NumberOfPoints,
				IsRangeEnabled = IsRangeEnabled,
				FormatterParametersViewModel = FormatterParametersViewModel?.Clone() as IFormatterParametersViewModel,
				Header = Header,
				Name = Name,
				MeasureUnit = MeasureUnit,
				NumberOfWriteFunction = NumberOfWriteFunction,
				IsFromBits = IsFromBits
			};

			foreach (var bitViewModel in BitNumbersInWord.Where(model => model.IsChecked).ToList())
            {
				cloneEditorViewModel.BitNumbersInWord.First(model =>model.BitNumber==bitViewModel.BitNumber ).IsChecked = true;
            }


			return cloneEditorViewModel;
		}

		public override T Accept<T>(IConfigurationItemViewModelVisitor<T> visitor)
		{
			return visitor.VisitProperty(this);
		}

		public void ChangeAddress(ushort addressOffset, bool isIncrease)
		{
			Address = isIncrease ? (ushort.Parse(Address) + addressOffset).ToString() : (ushort.Parse(Address) - addressOffset).ToString();
		}

	    public IFormatterParametersViewModel FormatterParametersViewModel
	    {
	        get => _formatterParametersViewModel;
	        set
	        {
	            _formatterParametersViewModel = value;
                RaisePropertyChanged();
	        }
	    }

	    public ObservableCollection<IDependencyViewModel> DependencyViewModels { get; }
        public bool IsFromBits { get; set; }
        public ObservableCollection<IBitViewModel> BitNumbersInWord { get; set; }

        public (ushort address, ushort numberOfPoints) GetAddressInfo()
        {

            var address = Address == null ? (ushort)0 : ushort.Parse(Address);
            return (address, ushort.Parse(NumberOfPoints));
        }

        public bool IsHidden
        {
            get => _isHidden;
            set
            {
                _isHidden = value;
				RaisePropertyChanged();
            }
        }
    }
}