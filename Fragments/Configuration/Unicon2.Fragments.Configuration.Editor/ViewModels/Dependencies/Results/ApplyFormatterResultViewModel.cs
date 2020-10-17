using System.Collections.Generic;
using System.Windows.Input;
using Unicon2.Fragments.Configuration.Editor.Interfaces;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Dependencies;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Configuration.Editor.ViewModels.Dependencies.Results
{
    public class ApplyFormatterResultViewModel : ViewModelBase, IResultViewModel, IUshortFormattableEditorViewModel
    {
        private readonly IFormatterEditorFactory _formatterEditorFactory;

        public ApplyFormatterResultViewModel(IFormatterEditorFactory formatterEditorFactory)
        {
            _formatterEditorFactory = formatterEditorFactory;

            ShowFormatterParameters = new RelayCommand(() => { _formatterEditorFactory.EditFormatterByUser(this); });
        }

        private IFormatterParametersViewModel _formatterParametersViewModel;
        public ICommand ShowFormatterParameters { get; }

        public string Name { get; set; }

        public IFormatterParametersViewModel FormatterParametersViewModel
        {
            get => _formatterParametersViewModel;
            set
            {
                _formatterParametersViewModel = value;
                RaisePropertyChanged();
            }
        }

        public IResultViewModel Clone()
        {
            return new ApplyFormatterResultViewModel(_formatterEditorFactory)
            {
                FormatterParametersViewModel = FormatterParametersViewModel.Clone()
            };
        }

        public string StrongName => ConfigurationKeys.APPLY_FORMATTER_RESULT;
    }
}