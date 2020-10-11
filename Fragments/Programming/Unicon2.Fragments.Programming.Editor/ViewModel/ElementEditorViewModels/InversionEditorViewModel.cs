using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementEditorViewModels;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Programming.Editor.ViewModel.ElementEditorViewModels
{
    public class InversionEditorViewModel : ViewModelBase, IInversionEditorViewModel
    {
        public string StrongName { get; }
        public object Model { get; set; }
        public object Clone()
        {
            throw new NotImplementedException();
        }

        public string ElementName { get; }
        public string Description { get; }
    }
}
