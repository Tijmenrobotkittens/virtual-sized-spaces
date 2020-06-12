#if UNITY_IOS
namespace DeadMosquito.IosGoodies
{
	using System;
	using System.Runtime.InteropServices;
	using Internal;
	using JetBrains.Annotations;

	/// <summary>
	///     Class to present native iOS dialogs
	/// </summary>
	[PublicAPI]
	public static class IGDialogs
	{
		/// <summary>
		///     Displays message dialog with positive button only
		/// </summary>
		/// <param name="title">Dialog title</param>
		/// <param name="message">Dialog message</param>
		/// <param name="confirmButtonTitle">Text for positive button</param>
		/// <param name="onConfirmButtonClick">Positive button callback</param>
		public static void ShowOneBtnDialog(string title, string message, string confirmButtonTitle,
			Action onConfirmButtonClick)
		{
			if (IGUtils.IsIosCheck())
			{
				return;
			}

			_showConfirmationDialog(title, message, confirmButtonTitle,
				IGUtils.ActionVoidCallback, onConfirmButtonClick.GetPointer());
		}

		/// <summary>
		///     Shows the two button dialog.
		/// </summary>
		/// <param name="title">Dialog title</param>
		/// <param name="message">Dialog message</param>
		/// <param name="okButtonTitle">Ok button title.</param>
		/// <param name="onOkButtonClick">Ok button callback.</param>
		/// <param name="cancelButtonTitle">Cancel button title.</param>
		/// <param name="onCancelButtonClick">Cancel button callback.</param>
		public static void ShowTwoBtnDialog(
			string title, string message,
			string okButtonTitle, Action onOkButtonClick,
			string cancelButtonTitle, Action onCancelButtonClick)
		{
			if (IGUtils.IsIosCheck())
			{
				return;
			}

			_showQuestionDialog(title, message, okButtonTitle, cancelButtonTitle,
				IGUtils.ActionVoidCallback, onOkButtonClick.GetPointer(), onCancelButtonClick.GetPointer());
		}

		/// <summary>
		///     Displays message dialog with three buttons
		/// </summary>
		/// <param name="title">Dialog title</param>
		/// <param name="message">Dialog message</param>
		/// <param name="firstButtonTitle">Text for first button</param>
		/// <param name="onFirstButtonClick">First button callback</param>
		/// <param name="secondButtonTitle">Text for second button</param>
		/// <param name="onSecondButtonClick">Second button callback</param>
		/// <param name="cancelButtonTitle">Text for cancel button</param>
		/// <param name="cancelButtonClick">Cancel button callback</param>
		public static void ShowThreeBtnDialog(string title, string message,
			string firstButtonTitle, Action onFirstButtonClick,
			string secondButtonTitle, Action onSecondButtonClick,
			string cancelButtonTitle, Action cancelButtonClick)
		{
			if (IGUtils.IsIosCheck())
			{
				return;
			}

			_showOptionalDialog(title, message,
				firstButtonTitle,
				secondButtonTitle,
				cancelButtonTitle,
				IGUtils.ActionVoidCallback,
				onFirstButtonClick.GetPointer(),
				onSecondButtonClick.GetPointer(),
				cancelButtonClick.GetPointer());
		}

		/// <summary>
		///  Displays alert dialog with input field, confirmation and cancellation buttons.
		/// </summary>
		/// <param name="title">The title of alert dialog.</param>
		/// <param name="text">Text body of alert dialog.</param>
		/// <param name="inputPlaceHolder">Input field text place holder.</param>
		/// <param name="buttonOkTitle">Confirmation button title.</param>
		/// <param name="buttonCancelTitle">Cancellation button title.</param>
		/// <param name="onCancelClick">Action to be performed after user taps cancellation button.</param>
		/// <param name="onConfirmClick">Action to be performed after user taps confirmation button.</param>
		public static void ShowInputFieldDialog(string title, string text, string inputPlaceHolder,
			string buttonOkTitle, string buttonCancelTitle, Action onCancelClick, Action<string> onConfirmClick)
		{
			if (IGUtils.IsIosCheck())
			{
				return;
			}
			
			_showInputFieldDialog(title, text, inputPlaceHolder, buttonOkTitle, buttonCancelTitle,
				IGUtils.ActionVoidCallback, IGUtils.ActionStringCallback, 
				onCancelClick.GetPointer(), onConfirmClick.GetPointer());
		}

		[DllImport("__Internal")]
		static extern void _showConfirmationDialog(string title, string message, string buttonTitle,
			IGUtils.ActionVoidCallbackDelegate callback, IntPtr onSuccessPtr);

		[DllImport("__Internal")]
		static extern void _showQuestionDialog(
			string title, string message, string okButtonTitle, string cancelButtonTitle,
			IGUtils.ActionVoidCallbackDelegate callback,
			IntPtr okButtonClickPtr,
			IntPtr cancelButtonClickPtr
		);

		[DllImport("__Internal")]
		static extern void _showOptionalDialog(string title, string message,
			string firstButtonTitle,
			string secondButtonTitle,
			string cancelButtonTitle,
			IGUtils.ActionVoidCallbackDelegate callback,
			IntPtr firstButtonClickPtr,
			IntPtr secondButtonClickPtr,
			IntPtr cancelButtonClickPtr
		);

		[DllImport("__Internal")]
		static extern void _showInputFieldDialog(string title, string text, string inputPlaceHolder,
			string buttonOkTitle, string buttonCancelTitle, IGUtils.ActionVoidCallbackDelegate cancelCallback,
			IGUtils.ActionStringCallbackDelegate successCallback, IntPtr cancelPtr, IntPtr successPtr);
	}
}
#endif