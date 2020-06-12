using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using UnityEditor;
using UnityEngine;

namespace DeadMosquito.IosGoodies.Editor
{
	[InitializeOnLoad]
	public static class IosGoodiesEditorUtils
	{
		static readonly Texture2D PlistInfoIcon;
		static readonly Texture2D IosFrameworkIcon;

		public static readonly Texture2D GoodiesIcon;
		public static readonly Texture2D InfoIcon;
		public static readonly Texture2D FeaturesIcon;
		public static readonly Texture2D ContactsIcon;
		
		public static readonly string[] StoreKitMetaFiles;
		public static readonly string[] ContactsUiMetaFiles;
		public static readonly string[] LocalAuthMetaFiles;
		public static readonly string[] MessageUiMetaFiles;
		public static readonly string[] EventKitMetaFiles;
		
		public static readonly string[] ImagePickerMetaFiles;

		static GUIStyle _textAreaStyle;
		static string _editorPath;
		static string _editorGuiPath;

		static GUIStyle TextAreaStyle
		{
			get { return _textAreaStyle ?? (_textAreaStyle = new GUIStyle(EditorStyles.textArea) {wordWrap = true}); }
		}

		static IosGoodiesEditorUtils()
		{
			Initialize();
			GoodiesIcon = GetIcon("goodies_icon.png");
			InfoIcon = GetIcon("ic_help.png");
			FeaturesIcon = GetIcon("ic_features.png");
			IosFrameworkIcon = GetIcon("ios-framework.png");
			ContactsIcon = GetIcon(_editorGuiPath + "ic_contacts_black.png");
			PlistInfoIcon = GetIcon("info_plist.png");

			StoreKitMetaFiles = GetMetaFiles("StoreKit");
			ContactsUiMetaFiles = GetMetaFiles("ContactsUI");
			LocalAuthMetaFiles = GetMetaFiles("LocalAuthentication");
			MessageUiMetaFiles = GetMetaFiles("MessageUI");
			EventKitMetaFiles = GetMetaFiles("EventKit");
			ImagePickerMetaFiles = GetMetaFiles("ImagePickers");
		}

		static Texture2D GetIcon(string goodiesIconPng)
		{
			return (Texture2D) AssetDatabase.LoadAssetAtPath(Path.Combine(_editorGuiPath, goodiesIconPng), typeof(Texture2D));
		}

		static void Initialize()
		{
			var rootDir = new DirectoryInfo(Application.dataPath);
			var files = rootDir.GetFiles(typeof(IosGoodiesSettingsEditor).Name + ".cs", SearchOption.AllDirectories);
			_editorPath = Path.GetDirectoryName(files[0].FullName.Replace("\\", "/").Replace(Application.dataPath, "Assets"));
			_editorGuiPath = Path.Combine(_editorPath, "GUI");
		}

		static string[] GetMetaFiles(string framework)
		{
			var folderPath = Path.Combine(_editorPath, String.Format("iOS/Feature/{0}", framework));
			return Directory.GetFiles(folderPath, "*.meta");
		}

		static string GetImportSettings(List<string> frameworks)
		{
			return FrameworkDependencyTemplate.Replace("${framework}", frameworks.Count == 0 ? string.Empty : string.Join(";", frameworks.ToArray()));
		}

		const string FrameworkDependencyTemplate = @"PluginImporter:
  serializedVersion: 2
  iconMap: {}
  executionOrder: {}
  isPreloaded: 0
  isOverridable: 0
  platformData:
    data:
      first:
        '': Any
      second:
        enabled: 0
        settings:
          Exclude Android: 1
          Exclude Editor: 1
          Exclude Linux: 1
          Exclude Linux64: 1
          Exclude LinuxUniversal: 1
          Exclude OSXIntel: 1
          Exclude OSXIntel64: 1
          Exclude OSXUniversal: 1
          Exclude Win: 1
          Exclude Win64: 1
          Exclude iOS: 0
    data:
      first:
        '': Editor
      second:
        enabled: 0
        settings:
          CPU: AnyCPU
          OS: AnyOS
    data:
      first:
        Android: Android
      second:
        enabled: 0
        settings:
          CPU: ARMv7
    data:
      first:
        Any: 
      second:
        enabled: 0
        settings: {}
    data:
      first:
        Editor: Editor
      second:
        enabled: 0
        settings:
          CPU: AnyCPU
          DefaultValueInitialized: true
          OS: AnyOS
    data:
      first:
        Facebook: Win
      second:
        enabled: 0
        settings:
          CPU: AnyCPU
    data:
      first:
        Facebook: Win64
      second:
        enabled: 0
        settings:
          CPU: AnyCPU
    data:
      first:
        Standalone: Linux
      second:
        enabled: 0
        settings:
          CPU: x86
    data:
      first:
        Standalone: Linux64
      second:
        enabled: 0
        settings:
          CPU: x86_64
    data:
      first:
        Standalone: OSXIntel
      second:
        enabled: 0
        settings:
          CPU: AnyCPU
    data:
      first:
        Standalone: OSXIntel64
      second:
        enabled: 0
        settings:
          CPU: AnyCPU
    data:
      first:
        Standalone: Win
      second:
        enabled: 0
        settings:
          CPU: AnyCPU
    data:
      first:
        Standalone: Win64
      second:
        enabled: 0
        settings:
          CPU: AnyCPU
    data:
      first:
        iPhone: iOS
      second:
        enabled: 1
        settings:
          CompileFlags: 
          FrameworkDependencies: ${framework};
  userData: 
  assetBundleName: 
  assetBundleVariant: ";

		const string None = @"PluginImporter:
  serializedVersion: 2
  iconMap: {}
  executionOrder: {}
  isPreloaded: 0
  isOverridable: 0
  platformData:
    data:
      first:
        '': Any
      second:
        enabled: 0
        settings:
          Exclude Android: 1
          Exclude Editor: 1
          Exclude Linux: 1
          Exclude Linux64: 1
          Exclude LinuxUniversal: 1
          Exclude OSXIntel: 1
          Exclude OSXIntel64: 1
          Exclude OSXUniversal: 1
          Exclude Win: 1
          Exclude Win64: 1
          Exclude iOS: 1
    data:
      first:
        '': Editor
      second:
        enabled: 0
        settings:
          CPU: AnyCPU
          OS: AnyOS
    data:
      first:
        Android: Android
      second:
        enabled: 0
        settings:
          CPU: ARMv7
    data:
      first:
        Any: 
      second:
        enabled: 0
        settings: {}
    data:
      first:
        Editor: Editor
      second:
        enabled: 0
        settings:
          DefaultValueInitialized: true
    data:
      first:
        Facebook: Win
      second:
        enabled: 0
        settings:
          CPU: AnyCPU
    data:
      first:
        Facebook: Win64
      second:
        enabled: 0
        settings:
          CPU: AnyCPU
    data:
      first:
        Standalone: Linux
      second:
        enabled: 0
        settings:
          CPU: x86
    data:
      first:
        Standalone: Linux64
      second:
        enabled: 0
        settings:
          CPU: x86_64
    data:
      first:
        Standalone: OSXIntel
      second:
        enabled: 0
        settings:
          CPU: AnyCPU
    data:
      first:
        Standalone: OSXIntel64
      second:
        enabled: 0
        settings:
          CPU: AnyCPU
    data:
      first:
        Standalone: Win
      second:
        enabled: 0
        settings:
          CPU: AnyCPU
    data:
      first:
        Standalone: Win64
      second:
        enabled: 0
        settings:
          CPU: AnyCPU
    data:
      first:
        iPhone: iOS
      second:
        enabled: 0
        settings:
          CompileFlags: 
          FrameworkDependencies: 
  userData: 
  assetBundleName: 
  assetBundleVariant: ";

		static bool TryThisNTimes(Action action, int numberOfTries = 20, int sleepBetweenTries = 10)
		{
			Exception exx = null;

			while (numberOfTries-- > 0)
			{
				try
				{
					action();
					return true;
				}
				catch (Exception ex)
				{
					exx = ex;
					Thread.Sleep(sleepBetweenTries);
				}
			}

			Debug.LogException(exx);
			return false;
		}

		public static void SetPluginImportSettings(string metaFile, string pluginImportSettings)
		{
			var pluginImportSettingsLines = new List<string>();
			var tmpLines = pluginImportSettings.Split(new[] {Environment.NewLine}, StringSplitOptions.None);
			var foundImportSettings = false;

			foreach (var line in tmpLines)
			{
				if (foundImportSettings || line.StartsWith("PluginImporter:"))
				{
					pluginImportSettingsLines.Add(line);
					foundImportSettings = true;
				}
			}

			var currLines = new List<string>();
			TryThisNTimes(() =>
			{
				currLines.Clear();

				using (var stream = new FileStream(metaFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
				using (var reader = new StreamReader(stream, Encoding.UTF8))
				{
					while (reader.Peek() >= 0)
					{
						currLines.Add(reader.ReadLine());
					}
				}
			});

			var newLines = new List<string>();
			foreach (var line in currLines)
			{
				if (line.StartsWith("PluginImporter:"))
				{
					break;
				}

				newLines.Add(line);
			}

			newLines.AddRange(pluginImportSettingsLines);

			TryThisNTimes(() =>
			{
				File.Delete(metaFile); // In later Unity versions, editing a metafile throws an exception if we don't delete it first.

				using (var stream = new FileStream(metaFile, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
				using (var writer = new StreamWriter(stream, Encoding.UTF8))
				{
					foreach (var t in newLines)
					{
						writer.WriteLine(t);
					}
				}
			});

			AssetDatabase.Refresh();
		}

		public static void DrawFrameworkLabel(string framework)
		{
			EditorGUILayout.LabelField(new GUIContent(String.Format(" {0}.framework", framework), IosFrameworkIcon));
		}

		public static void DrawPlistUsageDescriptionLabel(string key)
		{
			EditorGUILayout.LabelField(new GUIContent(" " + key, PlistInfoIcon));
		}

		public static string DrawPlistUsageDescriptionText(string text)
		{
			return EditorGUILayout.TextArea(text, TextAreaStyle, GUILayout.MaxHeight(EditorGUIUtility.singleLineHeight * 3));
		}

		public static void GreyLine(int height = 1, int margin = 0)
		{
			GuiLine(Color.gray, height, margin);
		}

		public static void GuiLine(Color color, int height = 1, int margin = 0)
		{
			EditorGUILayout.Space();
//			GUILayout.Space(margin);
//			var rect = EditorGUILayout.GetControlRect(false, height);
////			rect.x = rect.x - 4;
////			rect.width = rect.width + 8;
//			rect.height = height;
//
//			EditorGUI.DrawRect(rect, color);
//			GUILayout.Space(margin);
		}

		public static void ToggleFeature(bool isEnabled, List<string> frameworkDependencies, string[] metaFiles, string defineSymbol)
		{
			if (defineSymbol == null)
			{
				return;
			}

			DefineUtils.ToggleDefineIos(defineSymbol, !isEnabled);
			EditorApplication.delayCall += () =>
			{
				var pluginImportSettings = isEnabled ? GetImportSettings(frameworkDependencies) : None;
				foreach (var storeKitMetaFile in metaFiles)
				{
					SetPluginImportSettings(storeKitMetaFile, pluginImportSettings);
				}
			};
		}
	}
}
