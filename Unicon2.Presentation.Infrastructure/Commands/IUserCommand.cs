namespace Unicon2.Presentation.Infrastructure.Commands
{
	public interface IUserCommand
	{
		void Do();
		void UnDo();
	}
}