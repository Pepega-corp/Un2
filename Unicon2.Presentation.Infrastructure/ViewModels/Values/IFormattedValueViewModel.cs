using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Presentation.Infrastructure.ViewModels.Values
{
    public interface IFormattedValueViewModel : IRangeable, IMeasurable
    {
        string Header { get; set; }
        string AsString();
    }
}