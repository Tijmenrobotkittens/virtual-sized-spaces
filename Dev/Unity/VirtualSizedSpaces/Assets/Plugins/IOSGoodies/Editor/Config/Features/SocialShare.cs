using DeadMosquito.IosGoodies.Internal;

namespace DeadMosquito.IosGoodies.Editor
{
	public class SocialShare : PluginFeature
	{
		protected override void OnDrawToggle()
		{
			var newIsEnabled = TitleToggle(IosGoodiesSettings.IsSocialSharingEnabled);
			if (newIsEnabled == IosGoodiesSettings.IsSocialSharingEnabled)
			{
				return;
			}

			IosGoodiesEditorUtils.ToggleFeature(newIsEnabled, FrameworkDependencies, IosGoodiesEditorUtils.MessageUiMetaFiles, DefineSymbol);

			IosGoodiesSettings.IsSocialSharingEnabled = newIsEnabled;
		}

		protected override bool Enabled
		{
			get { return IosGoodiesSettings.IsSocialSharingEnabled; }
		}
	}
}