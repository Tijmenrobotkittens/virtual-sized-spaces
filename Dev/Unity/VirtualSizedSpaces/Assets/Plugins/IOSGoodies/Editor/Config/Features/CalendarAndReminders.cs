using DeadMosquito.IosGoodies.Internal;
using UnityEditor;

namespace DeadMosquito.IosGoodies.Editor
{
	public class CalendarAndReminders : PluginFeature
	{
		
		protected override bool Enabled
		{
			get { return IosGoodiesSettings.IsCalenderAndRemindersEnabled; }
		}
		
		protected override bool HasPlistModifications
		{
			get { return true; }
		}
		
		protected override void OnDrawToggle()
		{
			var newIsEnabled = TitleToggle(IosGoodiesSettings.IsCalenderAndRemindersEnabled);
			if (newIsEnabled == IosGoodiesSettings.IsCalenderAndRemindersEnabled)
			{
				return;
			}

			IosGoodiesEditorUtils.ToggleFeature(newIsEnabled, FrameworkDependencies, IosGoodiesEditorUtils.EventKitMetaFiles, DefineSymbol);
			IosGoodiesSettings.IsCalenderAndRemindersEnabled = newIsEnabled;
		}
		

		protected override void OnDrawUsageDescriptions()
		{
			IosGoodiesEditorUtils.DrawPlistUsageDescriptionLabel(IGProjectPostprocessor.CalendarsUsageDescription);
			IosGoodiesSettings.CalendarUsageDescription = IosGoodiesEditorUtils.DrawPlistUsageDescriptionText(IosGoodiesSettings.CalendarUsageDescription);

			IosGoodiesEditorUtils.DrawPlistUsageDescriptionLabel(IGProjectPostprocessor.RemindersUsageDescriptionKey);
			IosGoodiesSettings.RemindersUsageDescription = IosGoodiesEditorUtils.DrawPlistUsageDescriptionText(IosGoodiesSettings.RemindersUsageDescription);
		}
	}
}