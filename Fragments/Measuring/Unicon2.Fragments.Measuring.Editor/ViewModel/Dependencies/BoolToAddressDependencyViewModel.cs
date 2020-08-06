using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;
using Unicon2.Fragments.Measuring.Infrastructure.ViewModel.Dependencies;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Measuring.Editor.ViewModel.Dependencies
{
	public class FormattableAdapter : IUshortFormattableEditorViewModel
	{
		private readonly string _name;
		private readonly Func<IFormatterParametersViewModel> _formatterParametersViewModelProvider;
		private readonly Action<IFormatterParametersViewModel> _formatterParametersViewModelSetter;

		public FormattableAdapter(string name,Func<IFormatterParametersViewModel> formatterParametersViewModelProvider,Action<IFormatterParametersViewModel> formatterParametersViewModelSetter)
		{
			_name = name;
			_formatterParametersViewModelProvider = formatterParametersViewModelProvider;
			_formatterParametersViewModelSetter = formatterParametersViewModelSetter;
		}
		public string Name
		{
			get => _name;
			set => throw new NotImplementedException();
		}

		public IFormatterParametersViewModel FormatterParametersViewModel
		{
			get => _formatterParametersViewModelProvider();
			set => _formatterParametersViewModelSetter(value);
		}
	}
	
	public class BoolToAddressDependencyViewModel : ViewModelBase, IDependencyViewModel
	{
		private readonly IFormatterEditorFactory _formatterEditorFactory;
		private string _relatedResourceName;
		private ushort _resultingAddressIfTrue;
		private ushort _resultingAddressIfFalse;
		private IFormatterParametersViewModel _formatterParametersIfTrueViewModel;
		private IFormatterParametersViewModel _formatterParametersIfFalseViewModel;


		public BoolToAddressDependencyViewModel(IFormatterEditorFactory formatterEditorFactory)
		{
			_formatterEditorFactory = formatterEditorFactory;
			ShowFormatterParametersIfTrueCommand = new RelayCommand(OnShowFormatterParametersIfTrueExecute);
			ShowFormatterParametersIfFalseCommand = new RelayCommand(OnShowFormatterParametersIfFalseExecute);

		}

		private void OnShowFormatterParametersIfFalseExecute()
		{
			_formatterEditorFactory.EditFormatterByUser(new FormattableAdapter(_relatedResourceName,
				() => FormatterParametersIfFalseViewModel, model => FormatterParametersIfFalseViewModel = model));
		}

		private void OnShowFormatterParametersIfTrueExecute()
		{
			_formatterEditorFactory.EditFormatterByUser(new FormattableAdapter(_relatedResourceName,
				() => FormatterParametersIfTrueViewModel, model => FormatterParametersIfTrueViewModel = model));
		}

		public string RelatedResourceName
		{
			get => _relatedResourceName;
			set
			{

				_relatedResourceName = value;
				RaisePropertyChanged();
			}
		}

		public ushort ResultingAddressIfTrue
		{
			get => _resultingAddressIfTrue;
			set
			{
				_resultingAddressIfTrue = value;
				RaisePropertyChanged();
			}
		}

		public ushort ResultingAddressIfFalse
		{
			get => _resultingAddressIfFalse;
			set
			{
				_resultingAddressIfFalse = value;
				RaisePropertyChanged();
			}
		}

		public ICommand ShowFormatterParametersIfTrueCommand{get;}
		public ICommand ShowFormatterParametersIfFalseCommand{get;}
		
		public IFormatterParametersViewModel FormatterParametersIfTrueViewModel
		{
			get => _formatterParametersIfTrueViewModel;
			set
			{
				_formatterParametersIfTrueViewModel = value;
				RaisePropertyChanged();
			}
		}
		
		public IFormatterParametersViewModel FormatterParametersIfFalseViewModel
		{
			get => _formatterParametersIfFalseViewModel;
			set
			{
				_formatterParametersIfFalseViewModel = value;
				RaisePropertyChanged();
			}
		}

		public IDependencyViewModel Clone()
		{
			return new BoolToAddressDependencyViewModel(_formatterEditorFactory)
			{
				ResultingAddressIfTrue = ResultingAddressIfTrue,
				ResultingAddressIfFalse = ResultingAddressIfFalse,
				RelatedResourceName = RelatedResourceName,
				FormatterParametersIfFalseViewModel = FormatterParametersIfFalseViewModel,
				FormatterParametersIfTrueViewModel = FormatterParametersIfTrueViewModel
			}; 
		}

		
	}
}