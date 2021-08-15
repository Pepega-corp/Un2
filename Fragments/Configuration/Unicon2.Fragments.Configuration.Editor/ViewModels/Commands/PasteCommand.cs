using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Unity.Commands;

namespace Unicon2.Fragments.Configuration.Editor.ViewModels.Commands
{
    public class PasteCommand : RelayCommand
    {
        public PasteCommand(Action execute) : base(execute)
        {
        }

        public PasteCommand(Action execute, Func<bool> canExecute) : base(execute, canExecute)
        {

        }
    }
}
