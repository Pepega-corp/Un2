namespace Unicon2.Infrastructure.Progress
{
    public interface ITaskProgressReport
    {
        //current progress
        int CurrentProgressAmount { get; set; }
        //total progress
        int TotalProgressAmount { get; set; }
        //some message to pass to the UI of current progress
        string CurrentProgressMessage { get; set; }
    }
}