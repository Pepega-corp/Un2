using System.Windows.Input;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.Configuration.Infrastructure.ViewModel.ElementAdding
{
    public interface IElementAddingCommand : INameable
    {
        ICommand AddingCommand { get; set; }
        bool IsSelected { get; set; }
    }
}