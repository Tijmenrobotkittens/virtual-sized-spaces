using JetBrains.Annotations;
using UnityEngine;

#if UNITY_IOS
namespace DeadMosquito.IosGoodies
{
	using System;
	using System.Runtime.InteropServices;
	using Internal;

	/// <summary>
	///     Class to show https://developer.apple.com/reference/uikit/uiactionsheet
	/// </summary>
	public static class IGActionSheet
	{
		/// <summary>
		///     Displays action sheet with cancel button and provided options.
		/// </summary>
		/// <param name="title">Action sheet title</param>
		/// <param name="cancelBtnTitle">Action sheet cancel button title</param>
		/// <param name="onCancelClicked">Action sheet cancel button callback</param>
		/// <param name="otherButtonTitles">Titles of other buttons. You should provide at least one button title.</param>
		/// <param name="onOtherButtonClicked">
		/// 	Action sheet other button callback, index is the same as index array passed to
		/// 	method.
		/// </param>
		/// <param name="message"> Optional message to be displayed on the action sheet. </param>
		///  <param name="iPadScreenPosition">Position of the arrow on iPad screen. Positioning zero is at top left screen point. </param>
		public static void ShowActionSheet(string title,
			string cancelBtnTitle, Action onCancelClicked,
			string[] otherButtonTitles, Action<int> onOtherButtonClicked, string message = null, Vector2 iPadScreenPosition = default(Vector2))
		{
			if (IGUtils.IsIosCheck())
			{
				return;
			}

			if (message == null)
			{
				message = string.Empty;
			}
			
			Check.Argument.IsNotNull(otherButtonTitles, "OtherButtonTitles");
			
			var posX = IGUtils.ClampXScreenPos(iPadScreenPosition);
			var posY = IGUtils.ClampYScreenPos(iPadScreenPosition);

			_showActionSheet(title, message, cancelBtnTitle, string.Empty, string.Join("|", otherButtonTitles),
				posX, posY,
				IGUtils.ActionIntCallback, onOtherButtonClicked.GetPointer(),
				IGUtils.ActionVoidCallback, onCancelClicked.GetPointer(), IntPtr.Zero);
		}

		/// <summary>
		///     Displays action sheet with cancel and destructive button and provided options.
		/// </summary>
		/// <param name="title">Action sheet title</param>
		/// <param name="cancelBtnTitle">Action sheet cancel button title</param>
		/// <param name="onCancelClicked">Action sheet cancel button callback</param>
		/// <param name="destructiveButtonTitle">Action sheet destructive button title</param>
		/// <param name="onDestructiveButtonClicked">Action sheet destructive button callback</param>
		/// <param name="otherButtonTitles">Titles of other buttons</param>
		/// <param name="onOtherButtonClicked">
		///     Action sheet other button callback, index is the same as index array passed to
		///     method
		/// </param>
		/// <param name="message"> Optional message to be displayed on the action sheet. </param>
		///  <param name="iPadScreenPosition">Position of the arrow on iPad screen. Positioning zero is at top left screen point. </param>
		public static void ShowActionSheet(string title,
			string cancelBtnTitle, Action onCancelClicked,
			string destructiveButtonTitle, Action onDestructiveButtonClicked,
			[CanBeNull] string[] otherButtonTitles, Action<int> onOtherButtonClicked, string message = null, Vector2 iPadScreenPosition = default(Vector2))
		{
			if (IGUtils.IsIosCheck())
			{
				return;
			}

			if (message == null)
			{
				message = string.Empty;
			}

			if (otherButtonTitles == null)
			{
				otherButtonTitles = new string[] { };
			}
			
			var posX = IGUtils.ClampXScreenPos(iPadScreenPosition);
			var posY = IGUtils.ClampYScreenPos(iPadScreenPosition);

			_showActionSheet(title, message, cancelBtnTitle, destructiveButtonTitle, string.Join("|", otherButtonTitles), posX, posY,
				IGUtils.ActionIntCallback, onOtherButtonClicked.GetPointer(),
				IGUtils.ActionVoidCallback, onCancelClicked.GetPointer(), onDestructiveButtonClicked.GetPointer());
		}

		[DllImport("__Internal")]
		static extern void _showActionSheet(string title, string message, string cancelButtonTitle,
			string destructiveButtonTitle, string otherBtnTitles, int posX, int posY,
			IGUtils.ActionIntCallbackDelegate buttonCallback, IntPtr buttonPtr,
			IGUtils.ActionVoidCallbackDelegate callback, IntPtr cancelPtr, IntPtr destructivePtr);
	}
}
#endif