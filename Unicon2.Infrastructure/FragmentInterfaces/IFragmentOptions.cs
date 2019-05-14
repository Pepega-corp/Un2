using System.Collections.Generic;
using System.Windows.Input;

namespace Unicon2.Infrastructure.FragmentInterfaces
{
    public interface IFragmentOptions
    {
        List<ICommand> FragmentOptionCommands { get; }
    }
}