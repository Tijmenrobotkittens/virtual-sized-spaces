using DeadMosquito.IosGoodies.Internal;

namespace DeadMosquito.IosGoodies.Editor
{
	public class BiometricsAuthentication : PluginFeature
	{
		protected override void OnDrawToggle()
		{
			var newIsEnabled = TitleToggle(IosGoodiesSettings.IsBiometricAuthEnabled);
			if (newIsEnabled == IosGoodiesSettings.IsBiometricAuthEnabled)
			{
				return;
			}

			IosGoodiesEditorUtils.ToggleFeature(newIsEnabled, FrameworkDependencies, IosGoodiesEditorUtils.LocalAuthMetaFiles, DefineSymbol);

			IosGoodiesSettings.IsBiometricAuthEnabled = newIsEnabled;
		}

		protected override bool HasPlistModifications
		{
			get { return true; }
		}

		protected override bool Enabled
		{
			get { return IosGoodiesSettings.IsBiometricAuthEnabled; }
		}

		protected override void OnDrawUsageDescriptions()
		{
			IosGoodiesEditorUtils.DrawPlistUsageDescriptionLabel(IGProjectPostprocessor.FaceIdUsageDescription);
			IosGoodiesSettings.BiometricsUsageDescription = IosGoodiesEditorUtils.DrawPlistUsageDescriptionText(IosGoodiesSettings.BiometricsUsageDescription);
		}
	}
}