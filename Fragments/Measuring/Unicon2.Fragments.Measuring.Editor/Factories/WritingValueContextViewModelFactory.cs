using Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel.Address;
using Unicon2.Fragments.Measuring.Editor.ViewModel.Address;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Address;

namespace Unicon2.Fragments.Measuring.Editor.Factories
{
	public class WritingValueContextViewModelFactory
	{
		public IWritingValueContextViewModel CreateWritingValueContextViewModel(
			IWritingValueContext writingValueContext = null)
		{
			if (writingValueContext == null)
			{
				return new WritingValueContextViewModel();
			}
			return new WritingValueContextViewModel()
			{
				ValueToWrite = writingValueContext.ValueToWrite,
				NumberOfFunction = writingValueContext.NumberOfFunction,
				Address = writingValueContext.Address,
			};
		}
	}
}
