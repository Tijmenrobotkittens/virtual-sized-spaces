using DeadMosquito.IosGoodies.Internal;
using UnityEditor;
using UnityEngine;

namespace DeadMosquito.IosGoodies.Editor
{
	public class RateAppDialogFeature : PluginFeature
	{
		protected override void OnDrawToggle()
		{
			var newIsEnabled = TitleToggle(IosGoodiesSettings.IsRateDialogEnabled);
			if (newIsEnabled == IosGoodiesSettings.IsRateDialogEnabled)
			{
				return;
			}

			IosGoodiesEditorUtils.ToggleFeature(newIsEnabled, FrameworkDependencies, IosGoodiesEditorUtils.StoreKitMetaFiles, DefineSymbol);

			IosGoodiesSettings.IsRateDialogEnabled = newIsEnabled;
		}

		protected override bool Enabled
		{
			get { return IosGoodiesSettings.IsRateDialogEnabled; }
		}
	}
}