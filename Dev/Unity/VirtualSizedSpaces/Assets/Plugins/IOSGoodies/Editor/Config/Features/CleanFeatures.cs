using UnityEngine;

namespace DeadMosquito.IosGoodies.Editor
{
	public class CleanFeatures : PluginFeature
	{
		protected override void OnDrawToggle()
		{
			GUI.enabled = false;
			TitleToggle(true);
			GUI.enabled = true;
		}

		protected override bool Enabled
		{
			get { return true; }
		}
	}
}