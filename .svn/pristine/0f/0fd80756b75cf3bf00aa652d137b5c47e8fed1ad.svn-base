using System.Collections.Generic;
using Unicon2.Fragments.Journals.Infrastructure.Model;
using Unicon2.Infrastructure.Values;

namespace Unicon2.Fragments.Oscilliscope.Infrastructure.Model.OscillogramLoadingParameters
{
    public interface IOscillogramLoadingParameters
    {
        List<IOscilloscopeTag> OscilloscopeTags { get; set; }
        ushort AddressOfOscillogram { get; set; }
        ushort MaxSizeOfRewritableOscillogramInMs { get; set; }
        bool IsFullPageLoading { get; set; }
        void Initialize(List<IFormattedValue> formattedValues, IRecordTemplate recordTemplate);
        int GetOscillogramCountingsNumber();
        int GetSizeOfCountingInWords();
        int GetPointOfStart();
        int GetSizeAfter();

        int GetReadyPointAddress();
        string GetDataTimeofAlarm();

        int GetBeginAddresInData();
        string GetDateTime();
        string GetAlarm();
        ushort GetReadyPointNumberOfPoints();
    }
}