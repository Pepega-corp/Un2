namespace Unicon2.Fragments.Measuring.Infrastructure.Model
{
	public interface IMeasuringPresentationElement
	{
		int OffsetLeft { get; set; }
		int OffsetTop { get; set; }
		int SizeWidth { get; set; }
		int SizeHeight { get; set; }
	}
}