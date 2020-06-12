#if UNITY_IOS && !DISABLE_SOCIAL_SHARE
namespace DeadMosquito.IosGoodies
{
	using System;
	using System.Runtime.InteropServices;
	using Internal;
	using UnityEngine;

	public static class IGShare
	{
		static readonly byte[] EmptyByteArray = { };

		///  <summary>
		/// 	 Share the specified text with optional image.
		///  </summary>
		///  <param name="onFinished">Callback invoked when sharing finished.</param>
		///  <param name="onError">Callback invoked when error happened while sharing.</param>
		///  <param name="text">Text to share.</param>
		///  <param name="image">Image to share.</param>
		///  <param name="iPadScreenPosition">Position of the arrow on iPad screen. Positioning zero is at top left screen point. </param>
		public static void Share(Action<string> onFinished, Action<string> onError, 
			string text, Texture2D image = null, Vector2 iPadScreenPosition = default(Vector2))
		{
			if (IGUtils.IsIosCheck())
			{
				return;
			}

			Check.Argument.IsNotNull(onFinished, "onFinished");
			Check.Argument.IsNotNull(onError, "onError");

			var posX = IGUtils.ClampXScreenPos(iPadScreenPosition);
			var posY = IGUtils.ClampYScreenPos(iPadScreenPosition);

			if (image != null)
			{
				var imageBuffer = image.EncodeToPNG();
				var handle = GCHandle.Alloc(imageBuffer, GCHandleType.Pinned);
				_showShareMessageWithImage(text, handle.AddrOfPinnedObject(), imageBuffer.Length,
					posX, posY,
					IGUtils.ActionStringCallback, onFinished.GetPointer(), onError.GetPointer());
				handle.Free();
				return;
			}
			// Just text
			_showShareMessageWithImage(text, EmptyByteArray.GetPointer(), EmptyByteArray.Length,
				posX, posY,
				IGUtils.ActionStringCallback, onFinished.GetPointer(), onError.GetPointer());
		}

		/// <summary>
		///	 Share the specified text with link.
		/// </summary>
		/// <param name="onFinished">Callback invoked when sharing finished.</param>
		/// <param name="onError">Callback invoked when error happened while sharing.</param>
		/// <param name="text">Text to share.</param>
		/// <param name="url">Link to share.</param>
		///  <param name="iPadScreenPosition">Position of the arrow on iPad screen. Positioning zero is at top left screen point. </param>
		public static void ShareTextWithLink(Action<string> onFinished, Action<string> onError,
			string text, string url, Vector2 iPadScreenPosition = default(Vector2))
		{
			if (IGUtils.IsIosCheck())
			{
				return;
			}

			Check.Argument.IsNotNull(onFinished, "onFinished");
			Check.Argument.IsNotNull(onError, "onError");
			Check.Argument.IsStrNotNullOrEmpty(text, "message");
			Check.Argument.IsStrNotNullOrEmpty(url, "url");
			
			var posX = IGUtils.ClampXScreenPos(iPadScreenPosition);
			var posY = IGUtils.ClampYScreenPos(iPadScreenPosition);

			_showShareMessageWithLink(text, url,
				posX, posY,
				IGUtils.ActionStringCallback, onFinished.GetPointer(), onError.GetPointer());
		}

		/// <summary>
		///	 Opens the sms app with provided phone number.
		///	 You can specify message body with this method but its not documented and NOT guaranteed to work.
		/// </summary>
		/// <param name="phoneNumber">Phone number.</param>
		/// <param name="body">Message body. NOT GUARANTEED TO WORK!</param>
		public static void SendSmsViaDefaultApp(string phoneNumber, string body = null)
		{
			if (IGUtils.IsIosCheck())
			{
				return;
			}

			var url = string.Format("sms:{0}", phoneNumber);
			if (!string.IsNullOrEmpty(body))
			{
				url += string.Format("&body={0}", body.Escape());
			}
			IGUtils._openUrl(url);
		}

		public static void SendSmsViaController(string phoneNumber, string body,
			Action onSuccess,
			Action onCancel,
			Action onFailure)
		{
			if (IGUtils.IsIosCheck())
			{
				return;
			}

			_sendSms(phoneNumber, body, IGUtils.ActionVoidCallback,
				onSuccess.GetPointer(), onCancel.GetPointer(), onFailure.GetPointer());
		}

		/// <summary>
		///	 Sends the email using default mail app.
		/// </summary>
		/// <param name="recipients">Recipient email addresses.</param>
		/// <param name="subject">Subject of email.</param>
		/// <param name="body">Body of email.</param>
		/// <param name="cc">
		///	 Cc recipients. Cc stands for "carbon copy."
		///	 Anyone you add to the cc: field of a message receives a copy of that message when you send it.
		///	 All other recipients of that message can see that person you designated as a cc
		/// </param>
		/// <param name="bcc">
		///	 Bcc recipients. Bcc stands for "blind carbon copy."
		///	 Anyone you add to the bcc: field of a message receives a copy of that message when you send it.
		///	 But, bcc: recipients are invisible to all the other recipients of the message including other bcc: recipients.
		/// </param>
		public static void SendEmail(string[] recipients, string subject, string body, 
			string[] cc = null, string[] bcc = null)
		{
			if (IGUtils.IsIosCheck())
			{
				return;
			}

			var url = string.Format("mailto:{0}?subject={1}&body={2}",
				recipients.JoinByComma(),
				subject.Escape(), body.Escape()
			);
			if (cc != null && cc.Length > 0)
			{
				url += string.Format("&cc={0}", cc.JoinByComma());
			}
			if (bcc != null && bcc.Length > 0)
			{
				url += string.Format("&bcc={0}", bcc.JoinByComma());
			}

			IGUtils._openUrl(url);
		}

		/// <summary>
		/// Sends the email using in-app view.
		/// </summary>
		/// <param name="recipients">Recipient email addresses.</param>
		/// <param name="cc">Cc recipients. Cc stands for "carbon copy".</param>
		/// <param name="bcc">Bcc recipients. Bcc stands for "blind carbon copy".</param>
		/// <param name="subject">Subject of an email.</param>
		/// <param name="body">Body of an email.</param>
		/// <param name="onSuccess">Action to be performed upon successful sending.</param>
		/// <param name="onCancel">Action to be performed upon cancellation.</param>
		/// <param name="onFailure">Action to be performed upon failed sending.</param>
		/// <param name="onSaved">Action to be performed if message was saved as draft.</param>
		/// <param name="texture">Image to send.</param>
		public static void SendEmailViaController(string[] recipients, string[] cc, string[] bcc, 
			string subject, string body, Action onSuccess, Action onCancel, Action onFailure, Action onSaved,
			Texture2D texture = null)
		{
			var recipientsStr = recipients.JoinByComma();
			var ccStr = cc.JoinByComma();
			var bccStr = bcc.JoinByComma();
			
			if (texture != null)
			{
				var imageBuffer = texture.EncodeToPNG();
				var handle = GCHandle.Alloc(imageBuffer, GCHandleType.Pinned);
				_sendEmail(subject, body, handle.AddrOfPinnedObject(), imageBuffer.Length, recipientsStr, ccStr,
					bccStr, IGUtils.ActionVoidCallback, onSuccess.GetPointer(), onCancel.GetPointer(), 
					onFailure.GetPointer(), onSaved.GetPointer());
				handle.Free();
				return;
			}
			_sendEmail(subject, body, EmptyByteArray.GetPointer(), EmptyByteArray.Length, recipientsStr, ccStr,
				bccStr, IGUtils.ActionVoidCallback, onSuccess.GetPointer(), onCancel.GetPointer(), 
				onFailure.GetPointer(), onSaved.GetPointer());
		}
		
		[DllImport("__Internal")]
		static extern void _showShareMessageWithImage(string message, IntPtr bufferPtr, int bufferLength,
			int posX, int posY,
			IGUtils.ActionStringCallbackDelegate callback, IntPtr successPtr, IntPtr errPtr);

		[DllImport("__Internal")]
		static extern void _showShareMessageWithLink(string message, string link, 
			int posX, int posY,
			IGUtils.ActionStringCallbackDelegate callback, IntPtr successPtr, IntPtr errPtr);

		[DllImport("__Internal")]
		static extern void _sendSms(string recipient, string body, IGUtils.ActionVoidCallbackDelegate callback,
			IntPtr successPtr, IntPtr cancelPtr, IntPtr failurePtr);
		
		[DllImport("__Internal")]
		static extern void _sendEmail(string subject, string body, IntPtr bufferPtr, int bufferLength, 
			string recipients, string cc, string bcc, IGUtils.ActionVoidCallbackDelegate callback, 
			IntPtr onSuccessActionPtr, IntPtr onCancelActionPtr, 
			IntPtr onFailureActionPtr, IntPtr onSavedActionPtr);
	}
}
#endif