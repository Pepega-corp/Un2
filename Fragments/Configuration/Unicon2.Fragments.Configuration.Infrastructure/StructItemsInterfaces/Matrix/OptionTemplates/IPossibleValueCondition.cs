namespace Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Matrix.OptionTemplates
{
    public interface IPossibleValueCondition
    {
        bool BoolConditionRule { get; set; }
        IOptionPossibleValue RelatedOptionPossibleValue { get; set; }
    }
}