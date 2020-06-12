using DeadMosquito.IosGoodies.Internal;
using UnityEditor;

namespace DeadMosquito.IosGoodies.Editor
{
	public class ImagePicker : PluginFeature
	{
		protected override void OnDrawToggle()
		{
			var newIsEnabled = TitleToggle(IosGoodiesSettings.IsImagePickerEnabled);
			if (newIsEnabled == IosGoodiesSettings.IsImagePickerEnabled)
			{
				return;
			}

			IosGoodiesEditorUtils.ToggleFeature(newIsEnabled, FrameworkDependencies, IosGoodiesEditorUtils.ImagePickerMetaFiles, DefineSymbol);
			IosGoodiesSettings.IsImagePickerEnabled = newIsEnabled;
		}

		protected override bool Enabled
		{
			get { return IosGoodiesSettings.IsImagePickerEnabled; }
		}

		protected override bool HasPlistModifications
		{
			get { return true; }
		}

		protected override void OnDrawUsageDescriptions()
		{
			IosGoodiesEditorUtils.DrawPlistUsageDescriptionLabel(IGProjectPostprocessor.PhotoLibraryUsageDescription);
			IosGoodiesSettings.ImagePickerUsageDescription = IosGoodiesEditorUtils.DrawPlistUsageDescriptionText(IosGoodiesSettings.ImagePickerUsageDescription);
			
			EditorGUILayout.Space();
			
			IosGoodiesEditorUtils.DrawPlistUsageDescriptionLabel(IGProjectPostprocessor.CameraUsageDescription);
			IosGoodiesSettings.PhotoPickerUsageDescription = IosGoodiesEditorUtils.DrawPlistUsageDescriptionText(IosGoodiesSettings.PhotoPickerUsageDescription);
			
			EditorGUILayout.Space();
			
			IosGoodiesEditorUtils.DrawPlistUsageDescriptionLabel(IGProjectPostprocessor.PhotoLibraryAddUsageDescription);
			IosGoodiesSettings.AddPhotoToGalleryUsageDescription = IosGoodiesEditorUtils.DrawPlistUsageDescriptionText(IosGoodiesSettings.AddPhotoToGalleryUsageDescription);
		}
	}
}