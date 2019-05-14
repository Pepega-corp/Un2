using Unicon2.Infrastructure.Interfaces.Dependancy;

namespace Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.DependentProperty
{
   public class ConditionResultChangingEventArgs
    {
        public bool IsConditionTriggered { get; set; }
        public ConditionResultEnum ConditionResult { get; set; }
        public bool IsLocalValueTriggered { get; set; }

    }
}
