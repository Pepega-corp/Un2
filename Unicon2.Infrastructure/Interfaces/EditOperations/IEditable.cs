namespace Unicon2.Infrastructure.Interfaces.EditOperations
{
    public interface IEditable
    {
        bool IsInEditMode { get; set; }
        void StartEditElement();
        void StopEditElement();
    }
}