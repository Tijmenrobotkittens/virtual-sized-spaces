using System;
using DeadMosquito.IosGoodies.Internal;

#if UNITY_IOS
namespace DeadMosquito.IosGoodies
{
	using System.Runtime.InteropServices;
	using JetBrains.Annotations;

	/// <summary>
	/// Allows to opens system video editor
	/// </summary>
	[PublicAPI]
	public static class IGVideoEditor
	{
		[PublicAPI]
		public enum VideoQuality
		{
			High = 0,
			Medium = 1,
			Low = 2,
			VGA640x480 = 3,
			IFrame1280x720 = 4,
			IFrame960x540 = 5
		}
		/// <summary>
		/// Whether video at provided path can be edited
		/// </summary>
		/// <param name="videoPath">Path to the video to check whether it can be edited</param>
		public static bool CanEditVideoAtPath([NotNull] string videoPath)
		{
			if (IGUtils.IsIosCheck())
			{
				return false;
			}
			
			return _canEditVideoAtPath(videoPath);
		}

		/// <summary>
		/// Edits video at the provided path
		/// </summary>
		/// <param name="videoPath"> Path to the video </param>
		/// <param name="onEditFinished"> Action to perform with the new path to the edited version of the video </param>
		/// <param name="videoMaximumDuration"> Maximum duration of the edited video in seconds. 0 for unlimited </param>
		/// <param name="quality"> Resulting quality of the edited video. Will not be higher than quality of the source. </param>
		/// <exception cref="ArgumentNullException"> Thrown if the path is null </exception>
		public static void EditVideo([NotNull] string videoPath, Action<string> onEditFinished,
			VideoQuality quality = VideoQuality.High, int videoMaximumDuration = 0)
		{
			if (videoPath == null)
			{
				throw new ArgumentNullException("videoPath");
			}

			if (IGUtils.IsIosCheck())
			{
				return;
			}

			_editVideoAtPath(videoPath, IGUtils.ActionStringCallback, onEditFinished.GetPointer(), 
				videoMaximumDuration, (int) quality);
		}

		[DllImport("__Internal")]
		static extern void _editVideoAtPath(string path, IGUtils.ActionStringCallbackDelegate stringCallbackDelegate,
			IntPtr onFinishEditing, int maxDuration, int quality);
		
		[DllImport("__Internal")]
		static extern bool _canEditVideoAtPath(string path);
	}
}
#endif