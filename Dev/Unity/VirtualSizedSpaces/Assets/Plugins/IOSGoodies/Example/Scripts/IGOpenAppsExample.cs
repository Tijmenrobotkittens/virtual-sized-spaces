using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

namespace DeadMosquito.IosGoodies.Example
{
	using JetBrains.Annotations;
	using UnityEngine;
	using UnityEngine.UI;

	public class IGOpenAppsExample : MonoBehaviour
	{
		public Image image;
		public Texture2D testImage;

		List<string> _lastPaths = new List<string>();
		string _lastPickedVideoPath;

#if UNITY_IOS
		[UsedImplicitly]
		public void OpenAppStoreApp()
		{
			const string facebookAppId = "284882215";
			IGApps.OpenAppOnAppStore(facebookAppId);
		}

		[UsedImplicitly]
		public void OnOpenYouTubeVideo()
		{
			const string videoId = "rZ2csdtP440";
			IGApps.OpenYoutubeVideo(videoId);
		}

		[UsedImplicitly]
		public void OnFaceTimeVideoCall()
		{
			IGApps.StartFaceTimeVideoCall("user@example.com");
		}

		[UsedImplicitly]
		public void OnFaceTimeAudioCall()
		{
			IGApps.StartFaceTimeAudioCall("user@example.com");
		}

		[UsedImplicitly]
		public void OnOpenDialer()
		{
			IGApps.OpenDialer("123456789");
		}

		[UsedImplicitly]
		public void OnOpenAppSettings()
		{
			IGApps.OpenAppSettings();
		}

		#region image_pickers

		[UsedImplicitly]
		public void PickImageFromCamera()
		{
#if !DISABLE_IMAGE_PICKERS
			const bool allowEditing = true;
			const float compressionQuality = 0.8f;
			const IGImagePicker.CameraType cameraType = IGImagePicker.CameraType.Front;
			const IGImagePicker.CameraFlashMode flashMode = IGImagePicker.CameraFlashMode.On;

			IGImagePicker.PickImageFromCamera(tex =>
				{
					Debug.Log("Successfully picked image from camera");
					image.sprite = SpriteFromTex2D(tex);
					// IMPORTANT! Call this method to clean memory if you are picking and discarding images
					Resources.UnloadUnusedAssets();
				},
				() => Debug.Log("Picking image from camera cancelled"),
				compressionQuality,
				allowEditing, cameraType, flashMode);
#else
			ExampleUtil.LogFeatureDisabled();
#endif
		}

		[UsedImplicitly]
		public void PickImageFromPhotoLibrary()
		{
#if !DISABLE_IMAGE_PICKERS
			const bool allowEditing = false;
			const float compressionQuality = 0.5f;
			var screenPosition = new Vector2(Screen.width, Screen.height); // On iPads ONLY you can choose screen position of popover

			IGImagePicker.PickImageFromPhotoLibrary(tex =>
				{
					Debug.Log("Successfully picked image from photo library");
					image.sprite = SpriteFromTex2D(tex);
					// IMPORTANT! Call this method to clean memory if you are picking and discarding images
					Resources.UnloadUnusedAssets();
				},
				() => Debug.Log("Picking image from photo library cancelled"),
				compressionQuality,
				allowEditing, screenPosition);
#else
			ExampleUtil.LogFeatureDisabled();
#endif
		}

		[UsedImplicitly]
		public void PickImageFromPhotosAlbum()
		{
#if !DISABLE_IMAGE_PICKERS
			const bool allowEditing = true;
			const float compressionQuality = 0.1f;
			var screenPosition = new Vector2(Screen.width / 2, Screen.height / 2);

			IGImagePicker.PickImageFromPhotosAlbum(tex =>
				{
					Debug.Log("Successfully picked image from photos album");
					image.sprite = SpriteFromTex2D(tex);
					// IMPORTANT! Call this method to clean memory if you are picking and discarding images
					Resources.UnloadUnusedAssets();
				},
				() => Debug.Log("Picking image from photos album cancelled"),
				compressionQuality,
				allowEditing, screenPosition);
#else
			ExampleUtil.LogFeatureDisabled();
#endif
		}

		[UsedImplicitly]
		public void PickVideoFromPhotosAlbum()
		{
#if !DISABLE_IMAGE_PICKERS
			const bool allowEditing = false;
			var screenPosition = new Vector2(Screen.width / 2, Screen.height / 2);

			IGImagePicker.PickVideoFromPhotoLibrary(path =>
				{
					_lastPickedVideoPath = path;
					Debug.Log("Successfully picked video from photos album: " + path);
					EditVid(path);
				},
				() => Debug.Log("Picking video from photos album cancelled"),
				allowEditing, screenPosition);
#else
			ExampleUtil.LogFeatureDisabled();
#endif
		}

		[UsedImplicitly]
		public void SaveImageToPhotoLibrary()
		{
#if !DISABLE_IMAGE_PICKERS
			IGImagePicker.SaveImageToGallery(testImage);
#else
			ExampleUtil.LogFeatureDisabled();
#endif
		}

		[UsedImplicitly]
		public void PickContact()
		{
#if !DISABLE_CONTACT_PICKER
			IGContactPicker.PickContact(Debug.Log, () => { Debug.Log("Picking contact was cancelled"); });
#else
			ExampleUtil.LogFeatureDisabled();
#endif
		}

		#endregion

		[UsedImplicitly]
		public void EditVideo()
		{
			if (_lastPickedVideoPath != null)
			{
				EditVid(_lastPickedVideoPath);
			}
			else
			{
				print("Pick a video first");
			}
		}
		
		

		[UsedImplicitly]
		public void PickFile()
		{
			IGFilePicker.Import(new[] {"public.data"},
				list =>
				{
					foreach (var path in list)
					{
						print(path);
						_lastPaths.Add(path);
					}

					IGDialogs.ShowOneBtnDialog("Success", "File was downloaded", "Ok", () => { });
				},
				() => { IGDialogs.ShowOneBtnDialog("Cancelled", "File pick was cancelled", "Ok", () => { }); }, true);
		}

		[UsedImplicitly]
		public void OpenFile()
		{
			IGFilePicker.Open(new[] {"public.image"},
				list =>
				{
					foreach (var path in list)
					{
						print(path);
						var bytes = File.ReadAllBytes(path);
						var tex = new Texture2D(2, 2);
						tex.LoadImage(bytes);
						image.sprite = SpriteFromTex2D(tex);
					}
				},
				() => { IGDialogs.ShowOneBtnDialog("Cancelled", "File pick was cancelled", "Ok", () => { }); }, true);
		}

		[UsedImplicitly]
		public void ExportFile()
		{
			if (_lastPaths.Count == 0)
			{
				IGDialogs.ShowOneBtnDialog("Paths list is empty", "Please, import files before exporting", "Ok", () => { });
				return;
			}
			
			IGFilePicker.ExportToService(list => { IGDialogs.ShowOneBtnDialog("Success", "File was exported", "Ok", () => { }); },
				() => { IGDialogs.ShowOneBtnDialog("Cancelled", "File pick was cancelled", "Ok", () => { }); }, _lastPaths);
		}

		[UsedImplicitly]
		public void MoveFile()
		{
			if (_lastPaths.Count == 0)
			{
				IGDialogs.ShowOneBtnDialog("Paths list is empty", "Please, import files before moving", "Ok", () => { });
				return;
			}
			
			IGFilePicker.MoveToService(list => { IGDialogs.ShowOneBtnDialog("Success", "File was moved", "Ok", () => { }); },
				() => { IGDialogs.ShowOneBtnDialog("Cancelled", "File pick was cancelled", "Ok", () => { }); }, _lastPaths);
			_lastPaths.Clear();
		}

		void EditVid(string path)
		{
			if (IGVideoEditor.CanEditVideoAtPath(path))
			{
				IGVideoEditor.EditVideo(path, newPath =>
				{
					_lastPickedVideoPath = newPath;
					print(newPath);
				}, IGVideoEditor.VideoQuality.Medium);
			}
			else
			{
				Debug.Log("Can't edit video at path: " + path);
			}
		}

		static Sprite SpriteFromTex2D(Texture2D texture)
		{
			return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
		}
#endif
	}
}