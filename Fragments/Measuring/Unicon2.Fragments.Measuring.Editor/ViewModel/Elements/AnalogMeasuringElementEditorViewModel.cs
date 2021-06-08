using System.Collections.Generic;
using System.Windows.Input;
using Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel.Elements;
using Unicon2.Fragments.Measuring.Editor.ViewModel.Dependencies;
using Unicon2.Fragments.Measuring.Infrastructure.Keys;
using Unicon2.Infrastructure;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.Services.Dependencies;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Presentation.Infrastructure.ViewModels.Dependencies;
using Unicon2.Unity.Commands;

namespace Unicon2.Fragments.Measuring.Editor.ViewModel.Elements
{
	public class AnalogMeasuringElementEditorViewModel : MeasuringElementEditorViewModelBase,
		IAnalogMeasuringElementEditorViewModel,IDependenciesViewModelContainer
	{
		private readonly IFormatterEditorFactory _formatterEditorFactory;
		private readonly IApplicationGlobalCommands _applicationGlobalCommands;
	    private readonly IDependenciesService _dependenciesService;
	    private readonly ISharedResourcesGlobalViewModel _sharedResourcesGlobalViewModel;
	    private string _measureUnit;
		private bool _isMeasureUnitEnabled;
		private ushort _address;
		private ushort _numberOfPoints;
		private IFormatterParametersViewModel _formatterParametersViewModel;

		public AnalogMeasuringElementEditorViewModel(IFormatterEditorFactory formatterEditorFactory,
			IApplicationGlobalCommands applicationGlobalCommands,
			IDependenciesService dependenciesService,ISharedResourcesGlobalViewModel sharedResourcesGlobalViewModel)
		{
			_formatterEditorFactory = formatterEditorFactory;
			_applicationGlobalCommands = applicationGlobalCommands;
		    _dependenciesService = dependenciesService;
		    _sharedResourcesGlobalViewModel = sharedResourcesGlobalViewModel;
		    ShowFormatterParametersCommand = new RelayCommand(OnShowFormatterParametersExecute);
			ShowDependenciesCommand = new RelayCommand(OnShowDependenciesExecute);
		}

	    private void OnShowDependenciesExecute()
	    {
	        _dependenciesService.EditDependencies(this,
	            new DependenciesConfiguration(("BoolToAddressDependency",
	                () => new BoolToAddressDependencyViewModel(_formatterEditorFactory,_sharedResourcesGlobalViewModel))));

	    }

	    private void OnShowFormatterParametersExecute()
		{
			_formatterEditorFactory.EditFormatterByUser(this,new List<IConfigurationItemViewModel>());
			// this.RaisePropertyChanged(nameof(this.FormatterString));
		}


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

		public ICommand ShowFormatterParametersCommand { get; }

		public ushort Address
		{
			get { return _address; }
			set
			{
				_address = value;
				RaisePropertyChanged();
			}
		}

		public ushort NumberOfPoints
		{
			get { return _numberOfPoints; }
			set
			{
				_numberOfPoints = value;
				RaisePropertyChanged();
			}
		}

		

		public override string StrongName => MeasuringKeys.ANALOG_MEASURING_ELEMENT +
		                                     ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL;



		public override string NameForUiKey => MeasuringKeys.ANALOG_MEASURING_ELEMENT;


		public ICommand ShowDependenciesCommand { get; }

		public IFormatterParametersViewModel FormatterParametersViewModel
		{
			get => _formatterParametersViewModel;
			set
			{
				_formatterParametersViewModel = value;
				RaisePropertyChanged();
			}
		}
	}
}