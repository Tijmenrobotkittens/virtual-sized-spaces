#if UNITY_IOS && !DISABLE_BIOMETRIC_AUTH
namespace DeadMosquito.IosGoodies
{
	using System;
	using System.Runtime.InteropServices;
	using Internal;
	using JetBrains.Annotations;
	using AOT;
	using UnityEngine;

	/// <summary>
	/// Class for biometric authentication
	/// </summary>
	[PublicAPI]
	public class IGLocalAuthentication
	{
		/// <summary>
		/// Errors of local authentication
		/// </summary>
		[PublicAPI]
		public enum LocalAuthenticationError
		{
			/// <summary>
			/// Authentication was not successful, because user failed to provide valid credentials.
			/// </summary>
			AuthenticationFailed = -1,
			
			/// <summary>
			/// Authentication was canceled by user (e.g. tapped Cancel button).
			/// </summary>
			UserCancel = -2,
			
			/// <summary>
			/// Authentication was canceled, because the user tapped the fallback button (Enter Password).
			/// </summary>
			UserFallback = -3,
			
			/// <summary>
			/// Authentication was canceled by system (e.g. another application went to foreground).
			/// </summary>
			SystemCancel = -4,
			
			/// <summary>
			/// Authentication could not start, because passcode is not set on the device.
			/// </summary>
			PasscodeNotSet = -5,
			
			/// <summary>
			/// 
			/// </summary>
			TouchIdNotAvailable = -6,
			
			/// <summary>
			/// Authentication could not start, because biometry has no enrolled identities.
			/// </summary>
			TouchIdNotEnrolled = -7,
			
			/// <summary>
			/// Authentication was not successful, because there were too many failed biometry attempts and
			/// biometry is now locked. Passcode is required to unlock biometry, e.g. evaluating
			/// <see cref="Policy.DeviceOwnerAuthenticationWithBiometrics"/> will ask for passcode as a prerequisite.
			/// </summary>
			TouchIdLockout = -8,
			
			/// <summary>
			/// Authentication was canceled by application (e.g. invalidate was called while authentication was in progress)
			/// </summary>
			AppCancel = -9,
			
			/// <summary>
			/// Context passed to this call has been previously invalidated.
			/// </summary>
			InvalidContext = -10,
			
			/// <summary>
			/// Authentication could not start, because biometry is not available on the device.
			/// </summary>
			BiometryNotAvailableailable = TouchIdNotAvailable,
			
			/// <summary>
			/// Authentication could not start, because biometry has no enrolled identities.
			/// </summary>
			BiometryNotEnrolled = TouchIdNotEnrolled,
			
			/// <summary>
			/// Authentication was not successful, because there were too many failed biometry attempts and
			/// biometry is now locked. Passcode is required to unlock biometry, e.g. evaluating
			/// <see cref="Policy.DeviceOwnerAuthenticationWithBiometrics"/> will ask for passcode as a prerequisite.
			/// </summary>
			BiometryLockout = TouchIdLockout,
			
			/// <summary>
			/// Authentication failed, because it would require showing UI which has been forbidden by using interactionNotAllowed property.
			/// </summary>
			NotInteractive = -1004
		}

		/// <summary>
		/// Authentication policy
		/// </summary>
		[PublicAPI]
		public enum Policy
		{
			/// <summary>
			/// Device owner is going to be authenticated using a biometric method (Touch ID or Face ID).
			/// </summary>
			DeviceOwnerAuthenticationWithBiometrics = 0,

			/// <summary>
			/// Device owner is going to be authenticated by biometry or device passcode.
			/// </summary>
			DeviceOwnerAuthentication = 1
		}

		/// <summary>
		/// Check, whether biometric authentication is available on current device.
		/// </summary>
		public static bool IsLocalAuthenticationAvailable
		{
			get
			{
				if (IGUtils.IsIosCheck())
				{
					return false;
				}

				return _isLocalAuthenticationAvailable();
			}
		}

		/// <summary>
		/// Request biometric user authentication.
		/// Evaluating a policy may involve prompting the user for various kinds of interaction or authentication.
		/// The actual behavior is dependent on the evaluated policy and the device type.
		/// The behavior can also be affected by installed configuration profiles.
		/// </summary>
		/// <param name="reason">Description in the appearing authentication dialog.</param>
		/// <param name="policy">Authentication policy.</param>
		/// <param name="onSuccess">Action to perform after successful authentication.</param>
		/// <param name="onFailure">Action to perform using a string with error description.</param>
		public static void AuthenticateWithBiometrics(string reason, Policy policy, Action onSuccess, Action<LocalAuthenticationError> onFailure)
		{
			if (IGUtils.IsIosCheck())
			{
				return;
			}

			_authenticateWithBiometrics(reason, (int) policy,
				IGUtils.ActionVoidCallback,
				LocalAuthenticationErrorCallback, 
				onSuccess.GetPointer(), onFailure.GetPointer());
		}

		[MonoPInvokeCallback(typeof(IGUtils.ActionIntCallbackDelegate))]
		public static void LocalAuthenticationErrorCallback(IntPtr actionPtr, int data)
		{
			if (Debug.isDebugBuild)
			{
				Debug.Log("LocalAuthenticationErrorCallback: " + data);
			}

			if (actionPtr != IntPtr.Zero)
			{
				var action = actionPtr.Cast<Action<LocalAuthenticationError>>();
				action((LocalAuthenticationError) data);
			}
		}

		[DllImport("__Internal")]
		static extern bool _isLocalAuthenticationAvailable();

		[DllImport("__Internal")]
		static extern void _authenticateWithBiometrics(string reason, int policy,
			IGUtils.ActionVoidCallbackDelegate successCallBack,
			IGUtils.ActionIntCallbackDelegate failureCallBack,
			IntPtr successPtr, IntPtr failurePtr);
	}
}
#endif