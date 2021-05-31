namespace Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Dependencies.Conditions
{
    public interface ICurrentPropertyBitCheckCondition : ICondition
    {
        ushort BitNumber { get; set; }
    }
}