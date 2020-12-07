namespace Unicon2.Infrastructure.Interfaces
{
    public interface IStronglyNamed
    {
        string StrongName { get; }

    }  
    public interface IStronglyNamedDynamic
    {
        void SetStrongName(string name);

    }
}