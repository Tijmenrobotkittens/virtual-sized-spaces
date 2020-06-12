using JetBrains.Annotations;

#if UNITY_IOS
namespace DeadMosquito.IosGoodies
{
	[PublicAPI]
	public enum ImageFormat
	{
		PNG,
		JPEG
	}
}
#endif