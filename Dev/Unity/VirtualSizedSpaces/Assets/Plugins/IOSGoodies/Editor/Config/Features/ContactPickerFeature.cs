using DeadMosquito.IosGoodies.Internal;
using UnityEditor;

namespace DeadMosquito.IosGoodies.Editor
{
	public class ContactPickerFeature : PluginFeature
	{
		protected override void OnDrawToggle()
		{
			var newIsEnabled = TitleToggle(IosGoodiesSettings.IsContactPickerEnabled);
			if (newIsEnabled == IosGoodiesSettings.IsContactPickerEnabled)
			{
				return;
			}

			IosGoodiesEditorUtils.ToggleFeature(newIsEnabled, FrameworkDependencies, IosGoodiesEditorUtils.ContactsUiMetaFiles, DefineSymbol);
			IosGoodiesSettings.IsContactPickerEnabled = newIsEnabled;
		}

		protected override bool Enabled
		{
			get { return IosGoodiesSettings.IsContactPickerEnabled; }
		}
	}
}