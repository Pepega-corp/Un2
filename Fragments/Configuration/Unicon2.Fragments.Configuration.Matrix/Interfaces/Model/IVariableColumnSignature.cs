namespace Unicon2.Fragments.Configuration.Matrix.Interfaces.Model
{
    public interface IVariableColumnSignature
    {
        string Signature { get; set; }
        bool IsMultipleAssignmentAllowed { get; set; }
    }
}