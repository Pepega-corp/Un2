namespace Unicon2.Fragments.Measuring.Infrastructure.Model.PresentationSettings
{
	public interface IPositioningInfo
	{
		int OffsetLeft { get; set; }
		int OffsetTop { get; set; }
		int SizeWidth { get; set; }
		int SizeHeight { get; set; }
	}
}