using System.Windows;
using System.Windows.Input;
using Unicon2.Infrastructure.Interfaces.EditOperations;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Unity.Commands;

namespace Unicon2.Fragments.Configuration.Editor.ViewModels
{
    public class DescriptionEditingViewModel
    {
        public DescriptionEditingViewModel()
        {
            SubmitCommand = new RelayCommand<object>(OnSubmitExecute);
        }

        private void OnSubmitExecute(object window)
        {
            (Item as IEditable)?.StopEditElement();
            (window as Window)?.Close();
        }

        public IConfigurationItemViewModel Item { get; set; }
        public ICommand SubmitCommand { get; set; }
    }
}
