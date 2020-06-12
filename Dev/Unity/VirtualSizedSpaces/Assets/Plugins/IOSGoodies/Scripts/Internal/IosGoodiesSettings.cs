using System;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace DeadMosquito.IosGoodies.Internal
{
	public class IosGoodiesSettings : ScriptableObject
	{
		const string SettingsAssetName = "IosGoodiesSettings";
		const string SettingsAssetPath = "Resources/";

		static IosGoodiesSettings _instance;

		[SerializeField] bool _isRateDialogEnabled = true;
		[SerializeField] bool _isContactPickerEnabled = true;
		[SerializeField] bool _isBiometricAuthEnabled = true;
		[SerializeField] bool _isSocialSharingEnabled = true;

		[SerializeField] bool _isCalenderAndRemindersEnabled = true;
		
		[SerializeField] string _calendarUsageDescription = "Plist entry Added by IGProjectPostprocessor.cs script. Describe to the user why you want calendar access here.";
		[SerializeField] string _remindersUsageDescription = "Plist entry Added by IGProjectPostprocessor.cs script. Describe to the user why you want reminders access here.";
		[SerializeField] string _biometricsUsageDescription = "Plist entry Added by IGProjectPostprocessor.cs script. Describe to the user why you want Face Id authentication access here.";
		[SerializeField] string _imagePickerUsageDescription = "Plist entry Added by IGProjectPostprocessor.cs script. Describe to the user why you want to pick images from Photos/Library here.";
		[SerializeField] string _photoPickerUsageDescription = "Plist entry Added by IGProjectPostprocessor.cs script. Describe to the user why you want to access camera here.";
		[SerializeField] string _addPhotoToGalleryUsageDescription = "Plist entry Added by IGProjectPostprocessor.cs script. Describe to the user why you want to save images to Library here.";

		[SerializeField] bool _isImagePickerEnabled = true;

		public static bool IsRateDialogEnabled
		{
			get { return Instance._isRateDialogEnabled; }
			set
			{
				Instance._isRateDialogEnabled = value;
				MarkAssetDirty();
			}
		}

		public static bool IsContactPickerEnabled
		{
			get { return Instance._isContactPickerEnabled; }
			set
			{
				Instance._isContactPickerEnabled = value;
				MarkAssetDirty();
			}
		}

		public static bool IsBiometricAuthEnabled
		{
			get { return Instance._isBiometricAuthEnabled; }
			set
			{
				Instance._isBiometricAuthEnabled = value;
				MarkAssetDirty();
			}
		}
		
		public static string BiometricsUsageDescription
		{
			get { return Instance._biometricsUsageDescription; }
			set
			{
				Instance._biometricsUsageDescription = value;
				MarkAssetDirty();
			}
		}

		public static bool IsSocialSharingEnabled
		{
			get { return Instance._isSocialSharingEnabled; }
			set
			{
				Instance._isSocialSharingEnabled = value;
				MarkAssetDirty();
			}
		}

		public static string CalendarUsageDescription
		{
			get { return Instance._calendarUsageDescription; }
			set
			{
				Instance._calendarUsageDescription = value;
				MarkAssetDirty();
			}
		}

		public static string RemindersUsageDescription
		{
			get { return Instance._remindersUsageDescription; }
			set
			{
				Instance._remindersUsageDescription = value;
				MarkAssetDirty();
			}
		}


		public static bool IsCalenderAndRemindersEnabled
		{
			get { return Instance._isCalenderAndRemindersEnabled; }
			set
			{
				Instance._isCalenderAndRemindersEnabled = value;
				MarkAssetDirty();
			}
		}
		
		public static string AddPhotoToGalleryUsageDescription
		{
			get { return Instance._addPhotoToGalleryUsageDescription; }
			set
			{
				Instance._addPhotoToGalleryUsageDescription = value;
				MarkAssetDirty();
			}
		}
		
		public static string ImagePickerUsageDescription
		{
			get { return Instance._imagePickerUsageDescription; }
			set
			{
				Instance._imagePickerUsageDescription = value;
				MarkAssetDirty();
			}
		}
		
		public static string PhotoPickerUsageDescription
		{
			get { return Instance._photoPickerUsageDescription; }
			set
			{
				Instance._photoPickerUsageDescription = value;
				MarkAssetDirty();
			}
		}
		
		public static bool IsImagePickerEnabled
		{
			get { return Instance._isImagePickerEnabled; }
			set
			{
				Instance._isImagePickerEnabled = value;
				MarkAssetDirty();
			}
		}

		public static IosGoodiesSettings Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = Resources.Load(SettingsAssetName) as IosGoodiesSettings;
					if (_instance == null)
					{
						_instance = CreateInstance<IosGoodiesSettings>();

						SaveAsset(Path.Combine(GetPluginPath(), SettingsAssetPath), SettingsAssetName);
					}
				}

				return _instance;
			}
		}


		static void SaveAsset(string directory, string name)
		{
#if UNITY_EDITOR
			if (!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}

			AssetDatabase.CreateAsset(Instance, directory + name + ".asset");
			AssetDatabase.Refresh();
#endif
		}

		static string GetPluginPath()
		{
			return GetAbsolutePluginPath().Replace("\\", "/").Replace(Application.dataPath, "Assets");
		}

		static string GetAbsolutePluginPath()
		{
			return Path.GetDirectoryName(Path.GetDirectoryName(FindEditor(Application.dataPath)));
		}

		static string FindEditor(string path)
		{
			foreach (var d in Directory.GetDirectories(path))
			{
				foreach (var f in Directory.GetFiles(d))
				{
					if (f.Contains("IosGoodiesSettingsEditor.cs"))
					{
						return f;
					}
				}

				var rec = FindEditor(d);
				if (rec != null)
				{
					return rec;
				}
			}

			return null;
		}

		static void MarkAssetDirty()
		{
#if UNITY_EDITOR
			EditorUtility.SetDirty(Instance);
#endif
		}

		[Serializable]
		public class PlistUsageDescription
		{
			public string key;
			public string usageDescription;
		}
	}
}