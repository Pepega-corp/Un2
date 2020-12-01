using Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel.Address;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Address;
using Unicon2.Infrastructure.Common;

namespace Unicon2.Fragments.Measuring.Editor.Helpers
{
	public class WritingValueContextSaver
	{
		public IWritingValueContext CreateWritingValueContext(
			IWritingValueContextViewModel writingValueContextViewModel)
		{
			var res = StaticContainer.Container.Resolve<IWritingValueContext>();
			res.Address = writingValueContextViewModel.Address;
			res.NumberOfFunction = writingValueContextViewModel.NumberOfFunction;
			res.ValueToWrite = writingValueContextViewModel.ValueToWrite;
			return res;
		}
	}
}
