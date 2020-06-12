#if UNITY_IOS
namespace DeadMosquito.IosGoodies.Editor
{
	using System.Linq;
	using UnityEditor;
	using UnityEngine;

	public class IosGoodiesGettingStartedWindow : EditorWindow
	{
		static GUIContent _logo;

		const string DoNotShowIosGoodiesWelcomeScreenPref = "DoNotShowIosGoodiesWelcomeScreenPref";

		[MenuItem("Window/iOS Goodies/Getting Started", false, 1000)]
		static void Open()
		{
			ShowWindow();
		}

		static IosGoodiesGettingStartedWindow ShowWindow()
		{
			LoadResources();
			return GetWindowWithRect(typeof(IosGoodiesGettingStartedWindow), new Rect(100, 100, 570, 400), true, "iOS Goodies") as IosGoodiesGettingStartedWindow;
		}

		static void LoadResources()
		{
			if (_logo != null)
			{
				return;
			}

			_logo = new GUIContent(IosGoodiesEditorUtils.GoodiesIcon);
		}

		[UnityEditor.Callbacks.DidReloadScripts]
		static void OnScriptsReloaded()
		{
			var arguments = System.Environment.GetCommandLineArgs();
			if (arguments.ToList().Contains("-nographics"))
			{
				return;
			}

			if (EditorPrefs.HasKey(DoNotShowIosGoodiesWelcomeScreenPref))
			{
				return;
			}

			EditorPrefs.SetInt(DoNotShowIosGoodiesWelcomeScreenPref, 1);

			Open();
		}

		void OnGUI()
		{
			LoadResources();

			GUILayout.Space(10);
			GUI.Box(new Rect(13, 8, _logo.image.width / 2, _logo.image.height / 2), _logo, GUIStyle.none);

			GUILayout.Space(5);
			GUILayout.BeginHorizontal();
			GUILayout.Space(120);
			GUILayout.BeginVertical();

			GUILayout.Label("Welcome to iOS Goodies!", EditorStyles.boldLabel);
			GUILayout.Label("Thanks for using our product. To open this window go to Window -> iOS Goodies -> Getting Started. \n\nClick 'Edit Settings' button to see edit enabled features. You can disable features you don't use to remove unneeded dependencies from your XCode project. Please read the documentation carefully and check the examples inside the 'Example' folder before contacting the support.", "WordWrappedLabel");
			
			GUILayout.Space(20);
			GUILayout.Label("More:", "WordWrappedLabel");
			if (GUILayout.Button("Rate us on Asset Store", GUILayout.Width(200)))
			{
				Application.OpenURL("http://u3d.as/zMp");
			}
			
			if (GUILayout.Button("Edit settings", GUILayout.Width(200)))
			{
				IosGoodiesSettingsEditor.Edit();
			}
			
			if (GUILayout.Button("Read documentation", GUILayout.Width(200)))
			{
				Application.OpenURL("https://docs.ninevastudios.com/#/unity-plugins/ios-goodies");
			}
			
			if (GUILayout.Button("Ask a question on Discord", GUILayout.Width(200)))
			{
				Application.OpenURL("https://bit.ly/nineva_support_discord");
			}
			
			if (GUILayout.Button("Email support", GUILayout.Width(200)))
			{
				Application.OpenURL("mailto:support@ninevastudios.com");
			}

			GUILayout.Space(20);
			if (GUILayout.Button("Close", GUILayout.Width(200)))
			{
				Close();
			}

			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
		}
	}
}
#endif