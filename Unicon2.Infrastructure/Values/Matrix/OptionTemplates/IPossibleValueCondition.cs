namespace Unicon2.Infrastructure.Values.Matrix.OptionTemplates
{
    public interface IPossibleValueCondition
    {
        bool BoolConditionRule { get; set; }
        IOptionPossibleValue RelatedOptionPossibleValue { get; set; }
    }
}