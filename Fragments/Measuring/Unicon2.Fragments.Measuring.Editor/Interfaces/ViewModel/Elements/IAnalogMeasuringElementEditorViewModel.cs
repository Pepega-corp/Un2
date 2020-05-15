using System.Collections.ObjectModel;
using System.Windows.Input;
using Unicon2.Fragments.Measuring.Infrastructure.ViewModel.Dependencies;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.ViewModels;

namespace Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel.Elements
{
    public interface IAnalogMeasuringElementEditorViewModel : IMeasuringElementEditorViewModel, IMeasurable,
        IUshortFormattableEditorViewModel
    {
        ushort Address { get; set; }
        ushort NumberOfPoints { get; set; }
    }
}