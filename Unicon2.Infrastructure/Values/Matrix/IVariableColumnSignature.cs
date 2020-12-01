namespace Unicon2.Infrastructure.Values.Matrix
{
    public interface IVariableColumnSignature
    {
        string Signature { get; set; }
        bool IsMultipleAssignmentAllowed { get; set; }
    }
}