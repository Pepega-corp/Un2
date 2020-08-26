using System.Windows.Input;

namespace Unicon2.Presentation.Infrastructure.Commands
{
    public interface ICommandFactory
    {
        ICommand CreateCommand();
    }
}