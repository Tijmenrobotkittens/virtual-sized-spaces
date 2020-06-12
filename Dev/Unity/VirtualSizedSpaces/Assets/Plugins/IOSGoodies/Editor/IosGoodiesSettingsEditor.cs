using System.Collections.Generic;
using UnityEngine;

namespace DeadMosquito.IosGoodies.Editor
{
	using Internal;
	using UnityEditor;

	[CustomEditor(typeof(IosGoodiesSettings))]
	public class IosGoodiesSettingsEditor : Editor
	{
		static Dictionary<string, List<GUIContent>> FrameworkDependencies;

		static GUIContent FeaturesTitle;
		static GUIContent HelpTitle;

		int _tab;
		static bool _value;

		void OnEnable()
		{
			FeaturesTitle = new GUIContent(" Features", IosGoodiesEditorUtils.FeaturesIcon);
			HelpTitle = new GUIContent(" Help", IosGoodiesEditorUtils.InfoIcon);
		}

		[MenuItem("Window/iOS Goodies/Edit Settings", false, 1000)]
		public static void Edit()
		{
			Selection.activeObject = IosGoodiesSettings.Instance;
		}

		public override void OnInspectorGUI()
		{
			_tab = GUILayout.Toolbar(_tab, new[] {FeaturesTitle, HelpTitle});
			switch (_tab)
			{
				case 0:
					DrawFeaturesTabContent();
					break;
				case 1:
					DrawHelpTabContent();
					break;
			}
		}

		static void DrawHelpTabContent()
		{
			EditorGUILayout.HelpBox("On this page you can find all the help resources", MessageType.Info);
			if (GUILayout.Button("Read documentation"))
			{
				Application.OpenURL("https://docs.ninevastudios.com/#/unity-plugins/ios-goodies");
			}

			if (GUILayout.Button("Contact support via Discord"))
			{
				Application.OpenURL("http://bit.ly/nineva_discord");
			}

			if (GUILayout.Button("Email support"))
			{
				Application.OpenURL("mailto:support@ninevastudios.com");
			}
		}

		void DrawFeaturesTabContent()
		{
			EditorGUILayout.HelpBox(
				"This menu allows you to see all the available features and the impact that they have on the project. You can disable the features you don't plan to use here, so it removes the unneeded dependencies/postprocessing steps from XCode project",
				MessageType.Info);

			Features.PluginFeatures.ForEach(f => f.Draw());
		}
	}
}
