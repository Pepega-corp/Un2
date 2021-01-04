using System.Collections.Generic;
using Unicon2.Fragments.Journals.Infrastructure.Model;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;

namespace Unicon2.Fragments.Oscilliscope.Infrastructure.Model.CountingTemplate
{
    public interface ICountingTemplate:IDeviceContextConsumer
    {
        IRecordTemplate RecordTemplate { get; set; }
        List<string> GetCountingNames();
        ushort[] GetCountingValuesUshorts();

        int GetNumberOfAnalogs();
        List<string> GetAnalogsNames();

        string GetMeasureUnit(int numberOfElement);
        int GetNumberOfDiscrets();
        List<string> GetDiscretsNames();

        int GetAllChannels();
        
    }
}