namespace Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Dependencies.Conditions
{
    public interface IRegexMatchCondition: ICondition
    {
        string RegexPattern { get; set; }
        string ReferencedPropertyResourceName { get; set; }
    }
}