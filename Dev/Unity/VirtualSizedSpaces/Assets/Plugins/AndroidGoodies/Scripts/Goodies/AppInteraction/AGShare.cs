// 
// Class Documentation: https://github.com/TarasOsiris/android-goodies-docs-PRO/wiki/AGShare.cs
//


using UnityEngine.Networking;

#if UNITY_ANDROID
namespace DeadMosquito.AndroidGoodies
{
	using System;
	using System.IO;
	using Internal;
	using JetBrains.Annotations;
	using UnityEngine;

	[PublicAPI]
	public static class AGShare
	{
		/// <summary>
		/// Shares the text using default Android intent.
		/// </summary>
		/// <param name="subject">Subject.</param>
		/// <param name="body">Body.</param>
		/// <param name="withChooser">If set to <c>true</c> with chooser.</param>
		/// <param name="chooserTitle">Chooser title.</param>
		[PublicAPI]
		public static void ShareText(string subject, string body, bool withChooser = true,
			string chooserTitle = "Share via...")
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			var intent = new AndroidIntent(AndroidIntent.ActionSend)
				.SetType(AndroidIntent.MIMETypeTextPlain);
			intent.PutExtra(AndroidIntent.ExtraSubject, subject);
			intent.PutExtra(AndroidIntent.ExtraText, body);

			if (withChooser)
			{
				AGUtils.StartActivityWithChooser(intent.AJO, chooserTitle);
			}
			else
			{
				AGUtils.StartActivity(intent.AJO);
			}
		}

		/// <summary>
		/// Shares the text with image using default Android intent.
		/// </summary>
		/// <param name="subject">Subject.</param>
		/// <param name="body">Body.</param>
		/// <param name="image">Image to send.</param>
		/// <param name="withChooser">If set to <c>true</c> with chooser.</param>
		/// <param name="chooserTitle">Chooser title.</param>
		[PublicAPI]
		public static void ShareTextWithImage(string subject, string body, Texture2D image, bool withChooser = true,
			string chooserTitle = "Share via...")
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			if (image == null)
			{
				throw new ArgumentNullException("image", "Image must not be null");
			}

			var intent = new AndroidIntent()
				.SetAction(AndroidIntent.ActionSend)
				.SetType(AndroidIntent.MIMETypeImageJpeg)
				.PutExtra(AndroidIntent.ExtraSubject, subject)
				.PutExtra(AndroidIntent.ExtraText, body);

			var imageUri = AndroidPersistanceUtilsInternal.SaveImageToCacheDirectory(image);
			intent.PutExtra(AndroidIntent.ExtraStream, imageUri);

			if (withChooser)
			{
				AGUtils.StartActivityWithChooser(intent.AJO, chooserTitle);
			}
			else
			{
				AGUtils.StartActivity(intent.AJO);
			}
		}

		/// <summary>
		/// Take the screenshot and share using default share intent.
		/// </summary>
		/// <param name="withChooser">If set to <c>true</c> with chooser.</param>
		/// <param name="chooserTitle">Chooser title.</param>
		[PublicAPI]
		public static void ShareScreenshot(bool withChooser = true, string chooserTitle = "Share via...")
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			GoodiesSceneHelper.Instance.SaveScreenshotToGallery(uri =>
			{
				var intent = new AndroidIntent()
					.SetAction(AndroidIntent.ActionSend)
					.SetType(AndroidIntent.MIMETypeImageJpeg);

				intent.PutExtra(AndroidIntent.ExtraStream, AndroidUri.Parse(uri));

				if (withChooser)
				{
					AGUtils.StartActivityWithChooser(intent.AJO, chooserTitle);
				}
				else
				{
					AGUtils.StartActivity(intent.AJO);
				}
			});
		}

		/// <summary>
		/// Checks if user has any app that can handle SMS intent
		/// </summary>
		/// <returns><c>true</c>, if user has any SMS app installed, <c>false</c> otherwise.</returns>
		[PublicAPI]
		public static bool UserHasSmsApp()
		{
			if (AGUtils.IsNotAndroid())
			{
				return false;
			}

			return CreateSmsIntent("123123123", "dummy").ResolveActivity();
		}

		const string SmsUriFormat = "sms:{0}";

		/// <summary>
		/// Sends the sms using Android intent.
		/// </summary>
		/// <param name="phoneNumber">Phone number.</param>
		/// <param name="message">Message.</param>
		/// <param name="withChooser">If set to <c>true</c> with chooser.</param>
		/// <param name="chooserTitle">Chooser title.</param>
		[PublicAPI]
		public static void SendSms(string phoneNumber, string message, bool withChooser = true,
			string chooserTitle = "Send SMS...")
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			var intent = CreateSmsIntent(phoneNumber, message);
			if (withChooser)
			{
				AGUtils.StartActivityWithChooser(intent.AJO, chooserTitle);
			}
			else
			{
				AGUtils.StartActivity(intent.AJO);
			}
		}
		
		/// <summary>
		/// Sends the sms silently. Requires permission "android.permission.SEND_SMS".
		/// </summary>
		/// <param name="phoneNumber">Phone number.</param>
		/// <param name="message">Message.</param>
		[PublicAPI]
		public static void SendSmsSilently(string phoneNumber, string message)
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			const int maxSmsLength = 140;

			var smsManager = new AndroidJavaClass(C.AndroidTelephonySmsManager).CallStaticAJO("getDefault");

			if (message.Length <= maxSmsLength)
			{
				smsManager.Call("sendTextMessage", phoneNumber, null, message, null, null);
			}
			else
			{
				var javaList = smsManager.CallAJO("divideMessage", message);
				smsManager.Call("sendMultipartTextMessage", phoneNumber, null, javaList, null, null);
			}
		}

		static AndroidIntent CreateSmsIntent(string phoneNumber, string message)
		{
			var intent = new AndroidIntent(AndroidIntent.ActionView);

			if (AGDeviceInfo.SDK_INT >= AGDeviceInfo.VersionCodes.KITKAT)
			{
				var uri = AndroidUri.Parse(string.Format(SmsUriFormat, phoneNumber));
				intent.SetData(uri);
			}
			else
			{
				intent.SetType("vnd.android-dir/mms-sms");
				intent.PutExtra("address", phoneNumber);
			}

			intent.PutExtra("sms_body", message);
			return intent;
		}

		/// <summary>
		/// Sends the MMS using Android intent.
		/// </summary>
		/// <param name="phoneNumber">Address to send.</param>
		/// <param name="message">Body of the message.</param>
		/// <param name="image">Optional image.</param>
		/// <param name="withChooser">If set to <c>true</c> with chooser.</param>
		/// <param name="chooserTitle">Chooser title.</param>
		[PublicAPI]
		public static void SendMms(string phoneNumber, string message, Texture2D image = null,
			bool withChooser = true,
			string chooserTitle = "Send MMS...")
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			var intent = new AndroidIntent(AndroidIntent.ActionSend);
			intent.PutExtra("address", phoneNumber);
			intent.PutExtra("subject", message);

			if (image != null)
			{
				var imageUri = AndroidPersistanceUtilsInternal.SaveImageToCacheDirectory(image);
				intent.PutExtra(AndroidIntent.ExtraStream, imageUri);
				intent.SetType(AndroidIntent.MIMETypeImagePng);
			}

			if (withChooser)
			{
				AGUtils.StartActivityWithChooser(intent.AJO, chooserTitle);
			}
			else
			{
				AGUtils.StartActivity(intent.AJO);
			}
		}

		/// <summary>
		/// Checks if the user has any email app installed.
		/// </summary>
		/// <returns><c>true</c>, if the user has any email app installed, <c>false</c> otherwise.</returns>
		[PublicAPI]
		public static bool UserHasEmailApp()
		{
			if (AGUtils.IsNotAndroid())
			{
				return false;
			}

			return CreateEmailIntent(new[] {"dummy@gmail.com"}, "dummy", "dummy").ResolveActivity();
		}

		/// <summary>
		/// Sends the email using Android intent.
		/// </summary>
		/// <param name="recipients">Recipient email addresses.</param>
		/// <param name="subject">Subject of email.</param>
		/// <param name="body">Body of email.</param>
		/// <param name="attachment">Image to send.</param>
		/// <param name="withChooser">If set to <c>true</c> with chooser.</param>
		/// <param name="chooserTitle">Chooser title.</param>
		/// <param name="cc">Cc recipients. Cc stands for "carbon copy."
		/// Anyone you add to the cc: field of a message receives a copy of that message when you send it.
		/// All other recipients of that message can see that person you designated as a cc
		/// </param>
		/// <param name="bcc">Bcc recipients. Bcc stands for "blind carbon copy."
		/// Anyone you add to the bcc: field of a message receives a copy of that message when you send it.
		/// But, bcc: recipients are invisible to all the other recipients of the message including other bcc: recipients.
		/// </param>
		[PublicAPI]
		public static void SendEmail(string[] recipients, string subject, string body,
			Texture2D attachment = null, bool withChooser = true, string chooserTitle = "Send mail...",
			string[] cc = null, string[] bcc = null)
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			var intent = CreateEmailIntent(recipients, subject, body, attachment, cc, bcc);
			if (withChooser)
			{
				AGUtils.StartActivityWithChooser(intent.AJO, chooserTitle);
			}
			else
			{
				AGUtils.StartActivity(intent.AJO);
			}
		}

		static AndroidIntent CreateEmailIntent(string[] recipients, string subject, string body,
			Texture2D attachment = null, string[] cc = null, string[] bcc = null)
		{
			var uri = AndroidUri.Parse("mailto:");
			var intent = new AndroidIntent()
				.SetAction(AndroidIntent.ActionSendTo)
				.SetData(uri)
				.PutExtra(AndroidIntent.ExtraEmail, recipients)
				.PutExtra(AndroidIntent.ExtraSubject, subject)
				.PutExtra(AndroidIntent.ExtraText, body);
			if (cc != null)
			{
				intent.PutExtra(AndroidIntent.ExtraCc, cc);
			}
			if (bcc != null)
			{
				intent.PutExtra(AndroidIntent.ExtraBcc, bcc);
			}

			if (attachment != null)
			{
				var imageUri = AndroidPersistanceUtilsInternal.SaveImageToCacheDirectory(attachment);
				intent.PutExtra(AndroidIntent.ExtraStream, imageUri);
			}

			return intent;
		}

		// TWITTER
		const string TwitterPackage = "com.twitter.android";

		#region twitter

		/// <summary>
		/// Determines if twitter is installed.
		/// </summary>
		/// <returns><c>true</c> if twitter is installed; otherwise, <c>false</c>.</returns>
		[PublicAPI]
		public static bool IsTwitterInstalled()
		{
			if (AGUtils.IsNotAndroid())
			{
				return false;
			}

			return AGDeviceInfo.IsPackageInstalled(TwitterPackage);
		}

		/// <summary>
		/// Tweet the specified text and image. Will fallback to browser if official twitter app is not installed.
		/// </summary>
		/// <param name="tweet">Text to tweet.</param>
		/// <param name="image">Image to tweet.</param>
		[PublicAPI]
		public static void Tweet(string tweet, Texture2D image = null)
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			if (IsTwitterInstalled())
			{
				var intent = new AndroidIntent(AndroidIntent.ActionSend)
					.SetType(AndroidIntent.MIMETypeTextPlain)
					.PutExtra(AndroidIntent.ExtraText, tweet)
					.SetPreferredActivity(TwitterPackage, "composer");

				if (image != null)
				{
					var imageUri = AndroidPersistanceUtilsInternal.SaveImageToCacheDirectory(image);
					intent.PutExtra(AndroidIntent.ExtraStream, imageUri);
				}

				AGUtils.StartActivity(intent.AJO);
			}
			else
			{
				Application.OpenURL("https://twitter.com/intent/tweet?text=" + UnityWebRequest.EscapeURL(tweet));
			}
		}

		#endregion


		#region fb_messenger

		const string FbMessengerPackage = "com.facebook.orca";

		/// <summary>
		/// Determines if Facebook Messenger is installed.
		/// </summary>
		/// <returns><c>true</c> if Facebook Messenger is installed; otherwise, <c>false</c>.</returns>
		[PublicAPI]
		public static bool IsFacebookMessengerInstalled()
		{
			if (AGUtils.IsNotAndroid())
			{
				return false;
			}

			return AGDeviceInfo.IsPackageInstalled(FbMessengerPackage);
		}

		/// <summary>
		/// Sends the Facebook message text. Does nothing if Facebook messenger is not installed. 
		/// </summary>
		/// <param name="text">Text to send.</param>
		[PublicAPI]
		public static void SendFacebookMessageText(string text)
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			SendTextMessageGeneric(text, IsFacebookMessengerInstalled, FbMessengerPackage);
		}

		/// <summary>
		/// Sends the image. Does nothing if Facebook messenger is not installed.
		/// </summary>
		/// <param name="image">Image to send.</param>
		[PublicAPI]
		public static void SendFacebookMessageImage(Texture2D image)
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			if (image == null)
			{
				throw new ArgumentNullException("image", "Image must not be null");
			}

			SendImageGeneric(image, IsFacebookMessengerInstalled, FbMessengerPackage);
		}

		#endregion

		#region whatsapp

		const string WhatsAppPackage = "com.whatsapp";

		/// <summary>
		/// Determines if WhatsApp is installed.
		/// </summary>
		/// <returns><c>true</c> if WhatsApp is installed; otherwise, <c>false</c>.</returns>
		[PublicAPI]
		public static bool IsWhatsAppInstalled()
		{
			if (AGUtils.IsNotAndroid())
			{
				return false;
			}

			return AGDeviceInfo.IsPackageInstalled(WhatsAppPackage);
		}

		/// <summary>
		/// Sends the WhatsApp message text. Does nothing if WhatsApp messenger is not installed. 
		/// </summary>
		/// <param name="text">Text to send.</param>
		[PublicAPI]
		public static void SendWhatsAppTextMessage(string text)
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			SendTextMessageGeneric(text, IsWhatsAppInstalled, WhatsAppPackage);
		}

		/// <summary>
		/// Sends the image. Does nothing if WhatsApp messenger is not installed.
		/// </summary>
		/// <param name="image">Image to send.</param>
		[PublicAPI]
		public static void SendWhatsAppImageMessage(Texture2D image)
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			if (image == null)
			{
				throw new ArgumentNullException("image", "Image must not be null");
			}

			SendImageGeneric(image, IsWhatsAppInstalled, WhatsAppPackage);
		}

		#endregion

		#region instagram

		const string InstagramPackage = "com.instagram.android";

		/// <summary>
		/// Determines if Instagram is installed.
		/// </summary>
		/// <returns><c>true</c> if Instagram is installed; otherwise, <c>false</c>.</returns>
		[PublicAPI]
		public static bool IsInstagramInstalled()
		{
			if (AGUtils.IsNotAndroid())
			{
				return false;
			}

			return AGDeviceInfo.IsPackageInstalled(InstagramPackage);
		}

		/// <summary>
		/// Shares the instagram photo. Does nothing if instagram is not installed.
		/// </summary>
		/// <param name="image">Image to share.</param>
		[PublicAPI]
		public static void ShareInstagramPhoto(Texture2D image)
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			if (image == null)
			{
				throw new ArgumentNullException("image", "Image must not be null");
			}

			SendImageGeneric(image, IsInstagramInstalled, InstagramPackage);
		}

		#endregion

		#region telegram

		const string TelegramChatPackage = "org.telegram.messenger";

		/// <summary>
		/// Determines if Telegram is installed.
		/// </summary>
		/// <returns><c>true</c> if Telegram is installed; otherwise, <c>false</c>.</returns>
		[PublicAPI]
		public static bool IsTelegramInstalled()
		{
			if (AGUtils.IsNotAndroid())
			{
				return false;
			}

			return AGDeviceInfo.IsPackageInstalled(TelegramChatPackage);
		}

		/// <summary>
		/// Sends the Telegram message text. Does nothing if Telegram messenger is not installed. 
		/// </summary>
		/// <param name="text">Text to send.</param>
		[PublicAPI]
		public static void SendTelegramTextMessage(string text)
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			SendTextMessageGeneric(text, IsTelegramInstalled, TelegramChatPackage);
		}

		/// <summary>
		/// Sends the image. Does nothing if Telegram messenger is not installed.
		/// </summary>
		/// <param name="image">Image to send.</param>
		[PublicAPI]
		public static void SendTelegramImageMessage(Texture2D image)
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			if (image == null)
			{
				throw new ArgumentNullException("image", "Image must not be null");
			}

			SendImageGeneric(image, IsTelegramInstalled, TelegramChatPackage);
		}

		#endregion

		#region viber

		const string ViberPackage = "com.viber.voip";

		/// <summary>
		/// Determines if Viber is installed.
		/// </summary>
		/// <returns><c>true</c> if Viber is installed; otherwise, <c>false</c>.</returns>
		[PublicAPI]
		public static bool IsViberInstalled()
		{
			if (AGUtils.IsNotAndroid())
			{
				return false;
			}

			return AGDeviceInfo.IsPackageInstalled(ViberPackage);
		}

		/// <summary>
		/// Sends the Viber message text. Does nothing if Viber messenger is not installed. 
		/// </summary>
		/// <param name="text">Text to send.</param>
		[PublicAPI]
		public static void SendViberTextMessage(string text)
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			SendTextMessageGeneric(text, IsViberInstalled, ViberPackage);
		}

		/// <summary>
		/// Sends the image. Does nothing if Viber messenger is not installed.
		/// </summary>
		/// <param name="image">Image to send.</param>
		[PublicAPI]
		public static void SendViberImageMessage(Texture2D image)
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			if (image == null)
			{
				throw new ArgumentNullException("image", "Image must not be null");
			}

			SendImageGeneric(image, IsViberInstalled, ViberPackage);
		}

		#endregion

		#region snapchat

		const string SnapChatPackage = "com.snapchat.android";

		/// <summary>
		/// Determines if SnapChat is installed.
		/// </summary>
		/// <returns><c>true</c> if SnapChat is installed; otherwise, <c>false</c>.</returns>
		[PublicAPI]
		public static bool IsSnapChatInstalled()
		{
			if (AGUtils.IsNotAndroid())
			{
				return false;
			}

			return AGDeviceInfo.IsPackageInstalled(SnapChatPackage);
		}

		/// <summary>
		/// Sends the SnapChat message text. Does nothing if SnapChat messenger is not installed. 
		/// </summary>
		/// <param name="text">Text to send.</param>
		[PublicAPI]
		static void SendSnapChatTextMessage(string text)
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			SendTextMessageGeneric(text, IsSnapChatInstalled, SnapChatPackage);
		}

		/// <summary>
		/// Sends the image. Does nothing if SnapChat messenger is not installed.
		/// </summary>
		/// <param name="image">Image to send.</param>
		[PublicAPI]
		static void SendSnapChatImageMessage(Texture2D image)
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			if (image == null)
			{
				throw new ArgumentNullException("image", "Image must not be null");
			}

			SendImageGeneric(image, IsSnapChatInstalled, SnapChatPackage);
		}

		#endregion

		/// <summary>
		/// Shares the video that is stored on file system.
		/// You must provide a correct video path for this method to work properly.
		/// </summary>
		/// <param name="videoPathOnDisc">Video path on disc. File MUST exist!</param>
		/// <param name="title">Title of the video.</param>
		/// <param name="chooserTitle">Chooser title.</param>
		[PublicAPI]
		public static void ShareVideo(string videoPathOnDisc, string title, string chooserTitle = "Share Video")
		{
			if (AGUtils.IsNotAndroid())
			{
				return;
			}

			if (!File.Exists(videoPathOnDisc))
			{
				Debug.LogError("File (video) does not exist to share: " + videoPathOnDisc);
				return;
			}

			AGUtils.RunOnUiThread(() =>
				AndroidPersistanceUtilsInternal.ScanFile(videoPathOnDisc, (path, uri) =>
				{
					var intent = new AndroidIntent(AndroidIntent.ActionSend);
					intent.SetType("video/*");
					intent.PutExtra(AndroidIntent.ExtraSubject, title);
					intent.PutExtra(AndroidIntent.ExtraStream, uri);
					AGUtils.StartActivityWithChooser(intent.AJO, chooserTitle);
				}));
		}

		#region helpers

		/// <summary>
		/// Sends the text message with application of provided package.
		/// </summary>
		/// <param name="text">Text to send.</param>
		/// <param name="isInstalled">Function to check if the package is installed.</param>
		/// <param name="package">Application to use.</param>
		[PublicAPI]
		public static void SendTextMessageGeneric(string text, Func<bool> isInstalled, string package)
		{
			if (isInstalled())
			{
				var intent = new AndroidIntent(AndroidIntent.ActionSend)
					.SetType(AndroidIntent.MIMETypeTextPlain)
					.PutExtra(AndroidIntent.ExtraText, text)
					.SetPackage(package);

				AGUtils.StartActivity(intent.AJO);
			}
			else
			{
				Debug.Log(string.Format("Can't send message because {0} is not installed", package));
			}
		}

		/// <summary>
		/// Sends image  with application of provided package.
		/// </summary>
		/// <param name="image">Image to send.</param>
		/// <param name="isInstalled">Function to check if the package is installed.</param>
		/// <param name="package">Application to use.</param>
		[PublicAPI]
		public static void SendImageGeneric(Texture2D image, Func<bool> isInstalled, string package)
		{
			if (isInstalled())
			{
				var intent = new AndroidIntent(AndroidIntent.ActionSend)
					.SetType(AndroidIntent.MIMETypeImageAll)
					.SetPackage(package);

				if (image != null)
				{
					var imageUri = AndroidPersistanceUtilsInternal.SaveImageToCacheDirectory(image);
					intent.PutExtra(AndroidIntent.ExtraStream, imageUri);
				}

				AGUtils.StartActivity(intent.AJO);
			}
			else
			{
				Debug.Log(string.Format("Can't send image because {0} is not installed", package));
			}
		}

		#endregion
	}
}

#endif