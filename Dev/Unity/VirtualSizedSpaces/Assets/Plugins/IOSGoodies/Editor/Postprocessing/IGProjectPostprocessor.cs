using System;
using System.IO;
using DeadMosquito.IosGoodies.Internal;
using JetBrains.Annotations;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace DeadMosquito.IosGoodies.Editor
{
	using UnityEditor;
	using UnityEditor.iOS.Xcode;

	[UsedImplicitly]
	public class IGProjectPostprocessor : IPostprocessBuildWithReport
	{
		public const string CameraUsageDescription = "NSCameraUsageDescription";
		public const string PhotoLibraryUsageDescription = "NSPhotoLibraryUsageDescription";
		public const string PhotoLibraryAddUsageDescription = "NSPhotoLibraryAddUsageDescription";
		public const string FaceIdUsageDescription = "NSFaceIDUsageDescription";

		public const string CalendarsUsageDescription = "NSCalendarsUsageDescription";

		public const string RemindersUsageDescriptionKey = "NSRemindersUsageDescription";

		public void OnPostprocessBuild(BuildReport report)
		{
			if (report.summary.platform != BuildTarget.iOS)
			{
				return;
			}

			try
			{
				var plistInfoFile = new PlistDocument();

				var infoPlistPath = Path.Combine(report.summary.outputPath, "Info.plist");
				plistInfoFile.ReadFromString(File.ReadAllText(infoPlistPath));

				AddFeatureUsageDescriptions(plistInfoFile);

				File.WriteAllText(infoPlistPath, plistInfoFile.WriteToString());
			}
			catch (Exception e)
			{
				Debug.LogException(e);
			}
		}

		static void AddFeatureUsageDescriptions(PlistDocument plist)
		{
			SetIfRequired(plist, IosGoodiesSettings.IsImagePickerEnabled, CameraUsageDescription, IosGoodiesSettings.PhotoPickerUsageDescription);
			SetIfRequired(plist, IosGoodiesSettings.IsImagePickerEnabled, PhotoLibraryUsageDescription, IosGoodiesSettings.ImagePickerUsageDescription);
			SetIfRequired(plist, IosGoodiesSettings.IsImagePickerEnabled, PhotoLibraryAddUsageDescription, IosGoodiesSettings.AddPhotoToGalleryUsageDescription);

			SetIfRequired(plist, IosGoodiesSettings.IsBiometricAuthEnabled, FaceIdUsageDescription, IosGoodiesSettings.BiometricsUsageDescription);

			SetIfRequired(plist, IosGoodiesSettings.IsCalenderAndRemindersEnabled, CalendarsUsageDescription, IosGoodiesSettings.CalendarUsageDescription);
			SetIfRequired(plist, IosGoodiesSettings.IsCalenderAndRemindersEnabled, RemindersUsageDescriptionKey, IosGoodiesSettings.RemindersUsageDescription);
		}

		static void SetIfRequired(PlistDocument plist, bool isEnabled, string description, string usageDescription)
		{
			if (!isEnabled)
			{
				return;
			}

			if (!plist.root.values.ContainsKey(description))
			{
				plist.root.AsDict().SetString(description, usageDescription);
			}
		}

		public int callbackOrder
		{
			get { return 2000; }
		}
	}
}