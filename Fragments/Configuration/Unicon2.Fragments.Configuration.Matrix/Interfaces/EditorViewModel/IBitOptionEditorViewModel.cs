using System.Collections.Generic;
using Unicon2.Infrastructure.ViewModel;

namespace Unicon2.Fragments.Configuration.Matrix.Interfaces.EditorViewModel
{
    public interface IBitOptionEditorViewModel : IViewModel
    {
        string FullSugnature { get; }
        List<int> NumbersOfAssotiatedBits { get; set; }
        bool IsEnabled { get; }
        void UpdateIsEnabled();
    }
}