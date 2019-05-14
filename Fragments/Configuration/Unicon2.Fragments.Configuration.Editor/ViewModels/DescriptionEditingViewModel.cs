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
            this.SubmitCommand = new RelayCommand<object>(this.OnSubmitExecute);
        }

        private void OnSubmitExecute(object window)
        {
            (this.Item as IEditable)?.StopEditElement();
            (window as Window)?.Close();
        }

        public IConfigurationItemViewModel Item { get; set; }
        public ICommand SubmitCommand { get; set; }
    }
}
