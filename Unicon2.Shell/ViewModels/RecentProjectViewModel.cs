namespace Unicon2.Shell.ViewModels
{
	public class RecentProjectViewModel
	{
		public RecentProjectViewModel(string projectTitle,string path)
		{
			ProjectTitle = projectTitle;
			Path = path;
		}
		public string ProjectTitle { get; }
		public string Path { get; }
	}

}
