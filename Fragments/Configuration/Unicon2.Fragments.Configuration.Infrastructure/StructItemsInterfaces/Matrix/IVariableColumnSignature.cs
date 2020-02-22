namespace Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Matrix
{
    public interface IVariableColumnSignature
    {
        string Signature { get; set; }
        bool IsMultipleAssignmentAllowed { get; set; }
    }
}