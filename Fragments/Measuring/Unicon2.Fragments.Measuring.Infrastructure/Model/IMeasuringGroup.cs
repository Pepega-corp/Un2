using System.Collections.Generic;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings.QuickMemoryAccess;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.DataOperations;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;

namespace Unicon2.Fragments.Measuring.Infrastructure.Model
{
   public interface IMeasuringGroup: INameable
    {
        List<IMeasuringElement> MeasuringElements { get; set; }
    }
}
