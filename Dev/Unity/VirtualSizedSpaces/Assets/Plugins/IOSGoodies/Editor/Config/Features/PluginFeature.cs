using System;
using System.Collections.Generic;
using DeadMosquito.IosGoodies.Internal;
using UnityEditor;
using UnityEngine;

namespace DeadMosquito.IosGoodies.Editor
{
	public abstract class PluginFeature
	{
		public string Title { get; set; }
		public string Description { get; set; }
		public string DefineSymbol { get; set; }
		public List<string> FrameworkDependencies { get; set; }

		bool HasFrameworkDependencies
		{
			get { return FrameworkDependencies.Count != 0; }
		}

		protected virtual bool HasPlistModifications
		{
			get { return false; }
		}

		protected abstract void OnDrawToggle();

		protected virtual void OnDrawUsageDescriptions()
		{
			// override if needed
		}

		protected abstract bool Enabled { get; }

		protected PluginFeature()
		{
			FrameworkDependencies = new List<string>();
		}

		public void Draw()
		{
			bool backup = EditorStyles.label.wordWrap;
			EditorStyles.label.wordWrap = true;

			using (new EditorGUILayout.HorizontalScope("box"))
			{
				using (new EditorGUILayout.VerticalScope())
				{
					OnDrawToggle();

					using (new EditorGUI.DisabledScope(!Enabled))
					{
						EditorGUILayout.LabelField(Description);

						DrawFrameworkDependencies();
						DrawPlistModifications();
						EditorGUILayout.Space();
					}
				}
			}

			EditorStyles.label.wordWrap = backup;
//			DrawIcon(IosGoodiesEditorUtils.ContactsIcon);
		}

		static void DrawIcon(Texture2D icon)
		{
			var lastRect = GUILayoutUtility.GetLastRect();
			lastRect.x += 6;
			lastRect.y += 20;
			lastRect.width = 24;
			lastRect.height = 24;
			GUI.DrawTexture(lastRect, icon, ScaleMode.ScaleToFit);
		}

		void DrawFrameworkDependencies()
		{
			if (!HasFrameworkDependencies)
			{
				return;
			}

			IosGoodiesEditorUtils.GuiLine(new Color(0.3f, 0.3f, 0.3f, 1f), margin: 3);
			EditorGUILayout.LabelField("Framework dependencies:", EditorStyles.boldLabel);
//			EditorGUILayout.LabelField("These frameworks will be linked in your XCode project if feature is enabled", EditorStyles.miniLabel);
			foreach (var framework in FrameworkDependencies)
			{
				IosGoodiesEditorUtils.DrawFrameworkLabel(framework);
			}
		}

		void DrawPlistModifications()
		{
			if (!HasPlistModifications)
			{
				return;
			}

			IosGoodiesEditorUtils.GreyLine(margin: 3);

			EditorGUILayout.LabelField("Plist.info modifications:", EditorStyles.boldLabel);
//			EditorGUILayout.LabelField("Additional post processing steps that will be performed on your project by Android goodies", EditorStyles.miniLabel);
			OnDrawUsageDescriptions();
		}

		protected bool TitleToggle(bool value)
		{
			return EditorGUILayout.ToggleLeft(" " + Title, value, EditorStyles.boldLabel);
		}
	}
}