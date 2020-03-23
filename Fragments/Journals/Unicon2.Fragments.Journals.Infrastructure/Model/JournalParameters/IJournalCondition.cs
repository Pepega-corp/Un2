using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Interfaces.Dependancy;

namespace Unicon2.Fragments.Journals.Infrastructure.Model.JournalParameters
{
    public interface IJournalCondition : IUshortFormattable
    {
        ConditionsEnum ConditionsEnum { get; set; }
        ushort UshortValueToCompare { get; set; }
        IJournalParameter BaseJournalParameter { get; set; }
    }
}