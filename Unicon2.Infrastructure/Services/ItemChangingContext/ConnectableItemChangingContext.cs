using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Infrastructure.Services.ItemChangingContext
{
  public class ConnectableItemChangingContext
    {

        public ConnectableItemChangingContext(IConnectable connectable, ItemModifyingTypeEnum itemModifyingType)
        {
            Connectable = connectable;
            ItemModifyingType = itemModifyingType;
        }
      public IConnectable Connectable { get; }
        public ItemModifyingTypeEnum ItemModifyingType { get; }
    }
}
