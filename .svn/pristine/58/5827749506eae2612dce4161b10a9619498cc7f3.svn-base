using System.Windows.Input;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel.Elements
{
    public interface IAnalogMeasuringElementEditorViewModel: IMeasuringElementEditorViewModel,IMeasurable
    {
        ICommand ShowFormatterParametersCommand { get; }
        ushort Address { get; set; }
        ushort NumberOfPoints { get; set; }
        string FormatterString { get; }
    }
}