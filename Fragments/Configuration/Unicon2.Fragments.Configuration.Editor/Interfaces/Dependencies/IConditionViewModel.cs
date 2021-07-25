using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.Configuration.Editor.Interfaces.Dependencies
{
    public interface IConditionViewModel : ICloneable<IConditionViewModel>, IStronglyNamed
    {

    }

    public interface IConditionWithResourceViewModel
    {
        string ReferencedResourcePropertyName { get; set; }
    }
}