using System.Collections.Generic;
using DeadMosquito.IosGoodies.Internal;
using UnityEngine;

namespace DeadMosquito.IosGoodies.Editor
{
	public static class Features
	{
		static readonly CleanFeatures AllCleanFeatures = new CleanFeatures
		{
			Title = "Dialogs, Action sheets, Date/Time/Timer Pickers, Device Info, Haptics, Flashlight etc.",
			Description = "All features that have no special dependencies and do not modify XCode project",
		};

		static readonly RateAppDialogFeature AppRatingDialog = new RateAppDialogFeature
		{
			Title = "App rating dialog",
			Description = "Display a dialog for the user to rate your app",
			DefineSymbol = "DISABLE_STOREKIT",
			FrameworkDependencies = new List<string>
			{
				"StoreKit"
			}
		};

		static readonly ContactPickerFeature ContactPicker = new ContactPickerFeature
		{
			Title = "Contact Picker",
			Description = "UI to pick contacts form device contacts",
			DefineSymbol = "DISABLE_CONTACT_PICKER",
			FrameworkDependencies = new List<string>
			{
				"ContactsUI"
			}
		};

		static readonly BiometricsAuthentication BiometricAuth = new BiometricsAuthentication
		{
			Title = "Fingerprint/Face Id Authentication",
			Description = "Biometric authentication using Face Id/Fingerprint Scanner",
			DefineSymbol = "DISABLE_BIOMETRIC_AUTH",
			FrameworkDependencies = new List<string>
			{
				"LocalAuthentication"
			}
		};

		static readonly SocialShare SocialShare = new SocialShare
		{
			Title = "Social Sharing / Email / SMS",
			Description = "Various social sharing of text, links and images, sending of SMS or email",
			DefineSymbol = "DISABLE_SOCIAL_SHARE",
			FrameworkDependencies = new List<string>
			{
				"MessageUI"
			}
		};

		static readonly CalendarAndReminders CalendarAndReminders = new CalendarAndReminders
		{
			Title = "Calendar/Reminders access",
			Description = "Creating calendar event and reminders in the Reminders app",
			DefineSymbol = "DISABLE_CALENDAR_AND_REMINDERS",
			FrameworkDependencies = new List<string>
			{
				"EventKit"
			}
		};

		static readonly ImagePicker ImagePicker = new ImagePicker
		{
			Title = "Image picker form Photos/Library/Camera. Saving image to gallery",
			Description = "Picking a photo as Texture2D from user Photos/Gallery/Camera. Saving image to gallery",
			DefineSymbol = "DISABLE_IMAGE_PICKERS",
			FrameworkDependencies = new List<string>
			{
				"MobileCoreServices"
			}
		};

		public static readonly List<PluginFeature> PluginFeatures = new List<PluginFeature>
		{
			AllCleanFeatures,
			SocialShare,
			AppRatingDialog,
			
			ImagePicker,
			
			ContactPicker,
			BiometricAuth,
			CalendarAndReminders
		};
	}
}