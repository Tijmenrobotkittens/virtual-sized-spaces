using UnityEngine;

namespace DeadMosquito.IosGoodies.Example
{
	public static class ExampleUtil
	{
		public static void LogFeatureDisabled()
		{
			Debug.Log("You have disabled this feature feature in settings. Go to Window -> iOS Goodies -> Edit Settings to enable it");
		}
	}
}