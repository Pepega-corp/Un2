namespace Unicon2.Fragments.Configuration.Matrix.Interfaces.Model
{
    public interface IVariableSignature
    {
        string Signature { get; set; }
        bool IsMultipleAssgnmentAllowed { get; set; }
    }
}